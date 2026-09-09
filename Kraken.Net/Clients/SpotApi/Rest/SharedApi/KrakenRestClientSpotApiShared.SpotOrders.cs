using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Clients.SpotApi
{
    internal partial class KrakenRestClientSpotSharedApi
    {

        #region Place Spot Order

        public SharedFeeDeductionType SpotFeeDeductionType => SharedFeeDeductionType.DeductFromOutput;
        public SharedFeeAssetType SpotFeeAssetType => SharedFeeAssetType.QuoteAsset;
        public SharedOrderType[] SpotSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker };
        public SharedTimeInForce[] SpotSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };

        public SharedQuantitySupport SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAndQuoteAsset,
                SharedQuantityType.BaseAsset);

        public string GenerateClientOrderId() => ExchangeHelpers.RandomString(18);

        async Task<ICallResult<SharedId>> IPlaceSpotOrder.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
            => await PlaceSpotOrderAsync(request, ct).ConfigureAwait(false);

        public PlaceSpotOrderOptions PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions(_exchangeName);
        public async Task<HttpResult<SharedId>> PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = PlaceSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                GetPlaceOrderType(request.OrderType),
                request.Quantity?.QuantityInBaseAsset ?? request.Quantity?.QuantityInQuoteAsset ?? 0,
                request.Price,
                clientOrderId: request.ClientOrderId,
                orderFlags: GetOrderFlags(request.OrderType, request.Quantity?.QuantityInQuoteAsset),
                timeInForce: GetTimeInForce(request.TimeInForce),
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderIds.Single().ToString()));
        }

        #endregion

        #region Get Spot Order

        async Task<ICallResult<SharedSpotOrder>> IGetSpotOrder.GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
            => await GetSpotOrderAsync(request, ct).ConfigureAwait(false);

        public GetSpotOrderOptions GetSpotOrderOptions { get; } = new GetSpotOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedSpotOrder>> GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

            var order = await _api.Trading.GetOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedSpotOrder>(order);

            var orderData = order.Data.SingleOrDefault();
            if (orderData.Value == null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(order, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, orderData.Value.OrderDetails.Symbol),
                orderData.Value.OrderDetails.Symbol,
                orderData.Value.Id.ToString(),
                ParseOrderType(orderData.Value.OrderDetails.Type, orderData.Value.Oflags),
                orderData.Value.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseStatus(orderData.Value.Status),
                orderData.Value.CreateTime)
            {
                ClientOrderId = orderData.Value.ClientOrderId,
#pragma warning disable CS0618 // Type or member is obsolete
                Fee = orderData.Value.Fee,
#pragma warning restore CS0618 // Type or member is obsolete
                OrderPrice = (orderData.Value.OrderDetails.Price == 0 || orderData.Value.OrderDetails.Type == OrderType.TrailingStop) ? null : orderData.Value.OrderDetails.Price,
                OrderQuantity = new SharedOrderQuantity(orderData.Value.Oflags.Contains("viqc") ? null : orderData.Value.Quantity, orderData.Value.Oflags.Contains("viqc") ? orderData.Value.Quantity : null),
                QuantityFilled = new SharedOrderQuantity(orderData.Value.QuantityFilled, orderData.Value.QuoteQuantityFilled),
                AveragePrice = orderData.Value.AveragePrice == 0 ? null : orderData.Value.AveragePrice,
                UpdateTime = orderData.Value.CloseTime
            });
        }

        #endregion

        #region Get Open Spot Orders

        async Task<ICallResult<SharedSpotOrder[]>> IGetOpenSpotOrders.GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
            => await GetOpenSpotOrdersAsync(request, ct).ConfigureAwait(false);

        public GetOpenSpotOrdersOptions GetOpenSpotOrdersOptions { get; } = new GetOpenSpotOrdersOptions(_exchangeName, true)
        {
            ParameterRuleOverwrites = [
                RequestParameterRuleOverride<GetOpenOrdersRequest>.NotSupported(x => x.Symbol),
                ]
        };
        public async Task<HttpResult<SharedSpotOrder[]>> GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = GetOpenSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var order = await _api.Trading.GetOpenOrdersAsync(ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(order);

            IEnumerable<KrakenOrder> orders = order.Data.Open.Values.AsEnumerable();
            if (symbol != null)
                orders = orders.Where(x => x.OrderDetails.Symbol == symbol);

            return HttpResult.Ok(order, orders.Select(x => new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.OrderDetails.Symbol),
                x.OrderDetails.Symbol,
                x.Id.ToString(),
                ParseOrderType(x.OrderDetails.Type, x.Oflags),
                x.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
#pragma warning disable CS0618 // Type or member is obsolete
                Fee = x.Fee,
#pragma warning restore CS0618 // Type or member is obsolete
                OrderPrice = (x.OrderDetails.Price == 0 || x.OrderDetails.Type == OrderType.TrailingStop) ? null : x.OrderDetails.Price,
                OrderQuantity = new SharedOrderQuantity(x.Oflags.Contains("viqc") ? null : x.Quantity, x.Oflags.Contains("viqc") ? x.Quantity : null),
                QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.QuoteQuantityFilled),
                AveragePrice = x.AveragePrice == 0 ? null : x.AveragePrice,
                UpdateTime = x.CloseTime
            }).ToArray());
        }

        #endregion

        #region Get Closed Spot Orders

        async Task<ICallResult<SharedSpotOrder[]>> IGetClosedSpotOrders.GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetClosedSpotOrdersAsync(request, pageRequest, ct).ConfigureAwait(false);

        public GetSpotClosedOrdersOptions GetClosedSpotOrdersOptions { get; } = new GetSpotClosedOrdersOptions(_exchangeName, false, true, true, 50)
        {
            RequestNotes = "Request always returns up to 50 results"
        };
        public async Task<HttpResult<SharedSpotOrder[]>> GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetClosedSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            int limit = request.Limit ?? 50;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            var result = await _api.Trading.GetClosedOrdersAsync(
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                resultOffset: pageParams.Offset,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromOffset(pageParams, result.Data.Closed.Count),
                     result.Data.Closed.Count,
                     result.Data.Closed.Select(x => x.Value.CreateTime),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            var orders = result.Data.Closed.Values.Where(x => x.OrderDetails.Symbol == request.Symbol!.GetSymbol(FormatSymbol));
            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(orders, x => x.CreateTime, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedSpotOrder(
                            ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.OrderDetails.Symbol),
                            x.OrderDetails.Symbol,
                            x.Id.ToString(),
                            ParseOrderType(x.OrderDetails.Type, x.Oflags),
                            x.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            ParseStatus(x.Status),
                            x.CreateTime)
                        {
                            ClientOrderId = x.ClientOrderId,
#pragma warning disable CS0618 // Type or member is obsolete
                            Fee = x.Fee,
#pragma warning restore CS0618 // Type or member is obsolete
                            OrderPrice = (x.OrderDetails.Price == 0 || x.OrderDetails.Type == OrderType.TrailingStop) ? null : x.OrderDetails.Price,
                            OrderQuantity = new SharedOrderQuantity(x.Oflags.Contains("viqc") ? null : x.Quantity, x.Oflags.Contains("viqc") ? x.Quantity : null),
                            QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.QuoteQuantityFilled),
                            AveragePrice = x.AveragePrice == 0 ? null : x.AveragePrice,
                            UpdateTime = x.CloseTime
                        })
                    .ToArray(), nextPageRequest);
        }

        #endregion

        #region Get Spot Order Trades

        async Task<ICallResult<SharedUserTrade[]>> IGetSpotOrderTrades.GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
            => await GetSpotOrderTradesAsync(request, ct).ConfigureAwait(false);

        public GetSpotOrderTradesOptions GetSpotOrderTradesOptions { get; } = new GetSpotOrderTradesOptions(_exchangeName, true);
        public async Task<HttpResult<SharedUserTrade[]>> GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = GetSpotOrderTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            var order = await _api.Trading.GetOrderAsync(request.OrderId, trades: true).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedUserTrade[]>(order);

            var orderData = order.Data.SingleOrDefault();
            if (orderData.Value == null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            var trades = orderData.Value.TradeIds.ToList();
            if (!trades.Any())
                return HttpResult.Ok(order, new SharedUserTrade[] { });

            var tradeInfo = await _api.Trading.GetUserTradeDetailsAsync(trades.Take(20), ct: ct).ConfigureAwait(false);
            if (!tradeInfo.Success)
                return HttpResult.Fail<SharedUserTrade[]>(tradeInfo);

            return HttpResult.Ok(tradeInfo, tradeInfo.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Value.Symbol),
                x.Value.Symbol,
                x.Value.OrderId.ToString(),
                x.Value.Id.ToString(),
                x.Value.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                new SharedOrderQuantity(x.Value.Quantity, x.Value.QuoteQuantity),
                x.Value.Price,
                x.Value.Timestamp)
            {
                Fee = x.Value.Fee,
                Role = x.Value.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());
        }

        #endregion

        #region Get Spot User Trade History

        async Task<ICallResult<SharedUserTrade[]>> IGetSpotUserTradeHistory.GetSpotUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetSpotUserTradeHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        Task<HttpResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotUserTradesAsync(GetUserTradesRequest request, PageRequest? nextPageToken, CancellationToken ct)
            => GetSpotUserTradeHistoryAsync(request, nextPageToken, ct);
        GetSpotUserTradeHistoryOptions ISpotOrderRestClient.GetSpotUserTradesOptions => GetSpotUserTradeHistoryOptions;

        public GetSpotUserTradeHistoryOptions GetSpotUserTradeHistoryOptions { get; } = new GetSpotUserTradeHistoryOptions(_exchangeName, false, true, true, 50)
        {
            RequestNotes = "Request always returns up to 50 results"
        };
        public async Task<HttpResult<SharedUserTrade[]>> GetSpotUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetSpotUserTradeHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            int limit = request.Limit ?? 50;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await _api.Trading.GetUserTradesAsync(
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                resultOffset: pageParams.Offset,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedUserTrade[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromOffset(pageParams, result.Data.Trades.Count),
                     result.Data.Trades.Count,
                     result.Data.Trades.Select(x => x.Value.Timestamp),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Trades, x => x.Value.Timestamp, request.StartTime, request.EndTime, direction)
                    .Where(t => t.Value.Symbol == symbol)
                    .Select(x =>
                        new SharedUserTrade(
                            ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Value.Symbol),
                            x.Value.Symbol,
                            x.Value.OrderId.ToString(),
                            x.Value.Id.ToString(),
                            x.Value.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            new SharedOrderQuantity(x.Value.Quantity, x.Value.QuoteQuantity),
                            x.Value.Price,
                            x.Value.Timestamp)
                        {
                            Fee = x.Value.Fee,
                            Role = x.Value.Maker ? SharedRole.Maker : SharedRole.Taker
                        }).ToArray(), nextPageRequest);
        }

        #endregion

        #region Cancel Spot Order

        async Task<ICallResult<SharedId>> ICancelSpotOrder.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelSpotOrderAsync(request, ct).ConfigureAwait(false);

        public CancelSpotOrderOptions CancelSpotOrderOptions { get; } = new CancelSpotOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.Trading.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.ToString()));
        }

        #endregion

        private SharedOrderStatus ParseStatus(OrderStatus orderStatus)
        {
            if (orderStatus == OrderStatus.Open || orderStatus == OrderStatus.Pending) return SharedOrderStatus.Open;
            if (orderStatus == OrderStatus.Canceled || orderStatus == OrderStatus.Expired) return SharedOrderStatus.Canceled;
            if (orderStatus == OrderStatus.Closed) return SharedOrderStatus.Filled;

            return SharedOrderStatus.Unknown;
        }

        private SharedOrderType ParseOrderType(OrderType type, string? flags)
        {
            if (type == OrderType.Market) return SharedOrderType.Market;
            if (type == OrderType.Limit && flags?.Contains("post") == true) return SharedOrderType.LimitMaker;
            if (type == OrderType.Limit) return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }

        private OrderType GetPlaceOrderType(SharedOrderType type)
        {
            if (type == SharedOrderType.Market) return OrderType.Market;

            return OrderType.Limit;
        }

        private IEnumerable<OrderFlags>? GetOrderFlags(SharedOrderType type, decimal? quoteQuantity)
        {
            List<OrderFlags>? result = null;
            if (type == SharedOrderType.LimitMaker)
            {
                result ??= new List<OrderFlags>();
                result.Add(OrderFlags.PostOnly);
            }
            if (quoteQuantity > 0)
            {
                result ??= new List<OrderFlags>();
                result.Add(OrderFlags.OrderVolumeExpressedInQuoteAsset);
            }
            return result;
        }

        private TimeInForce? GetTimeInForce(SharedTimeInForce? tif)
        {
            if (tif == SharedTimeInForce.ImmediateOrCancel) return TimeInForce.IOC;
            if (tif == SharedTimeInForce.GoodTillCanceled) return TimeInForce.GTC;
            if (tif == SharedTimeInForce.FillOrKill) return TimeInForce.FOK;

            return null;
        }

        #region Get Spot Order By Client Order Id

        async Task<ICallResult<SharedSpotOrder>> IGetSpotOrderByClientOrderId.GetSpotOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
            => await GetSpotOrderByClientOrderIdAsync(request, ct).ConfigureAwait(false);

        public GetSpotOrderByClientOrderIdOptions GetSpotOrderByClientOrderIdOptions { get; } = new GetSpotOrderByClientOrderIdOptions(_exchangeName, true);
        public async Task<HttpResult<SharedSpotOrder>> GetSpotOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetSpotOrderByClientOrderIdOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

            var order = await _api.Trading.GetOpenOrdersAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedSpotOrder>(order);

            var orderData = order.Data.Open.SingleOrDefault().Value;
            if (orderData == null)
            {
                var pageOrder = await _api.Trading.GetClosedOrdersAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
                if (!pageOrder.Success)
                    return HttpResult.Fail<SharedSpotOrder>(pageOrder);

                orderData = pageOrder.Data.Closed.SingleOrDefault().Value;
            }

            if (orderData == null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(order, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, orderData.OrderDetails.Symbol),
                orderData.OrderDetails.Symbol,
                orderData.Id.ToString(),
                ParseOrderType(orderData.OrderDetails.Type, orderData.Oflags),
                orderData.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseStatus(orderData.Status),
                orderData.CreateTime)
            {
                ClientOrderId = orderData.ClientOrderId,
#pragma warning disable CS0618 // Type or member is obsolete
                Fee = orderData.Fee,
#pragma warning restore CS0618 // Type or member is obsolete
                OrderPrice = (orderData.OrderDetails.Price == 0 || orderData.OrderDetails.Type == OrderType.TrailingStop) ? null : orderData.OrderDetails.Price,
                OrderQuantity = new SharedOrderQuantity(orderData.Oflags.Contains("viqc") ? null : orderData.Quantity, orderData.Oflags.Contains("viqc") ? orderData.Quantity : null),
                QuantityFilled = new SharedOrderQuantity(orderData.QuantityFilled, orderData.QuoteQuantityFilled),
                AveragePrice = orderData.AveragePrice == 0 ? null : orderData.AveragePrice,
                UpdateTime = orderData.CloseTime
            });
        }

        #endregion

        #region Cancel Spot Order By Client Order Id

        async Task<ICallResult<SharedId>> ICancelSpotOrderByClientOrderId.CancelSpotOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelSpotOrderByClientOrderIdAsync(request, ct).ConfigureAwait(false);

        public CancelSpotOrderByClientOrderIdOptions CancelSpotOrderByClientOrderIdOptions { get; } = new CancelSpotOrderByClientOrderIdOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> CancelSpotOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelSpotOrderByClientOrderIdOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.Trading.CancelOrderAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.Pending.FirstOrDefault().ToString() ?? request.OrderId));
        }

        #endregion

    }
}
