using Kraken.Net;
using Kraken.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis.Interfaces;
using CryptoExchange.Net.SharedApis.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.SharedApis.Models.Rest;
using CryptoExchange.Net.SharedApis.Enums;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;
using CryptoExchange.Net.SharedApis.Models;
using CryptoExchange.Net.SharedApis.Interfaces.Rest.Spot;
using CryptoExchange.Net.SharedApis.Models.Options.Endpoints;
using CryptoExchange.Net.SharedApis.Interfaces.Rest;
using CryptoExchange.Net.SharedApis.Models.Options;

namespace Kraken.Net.Clients.SpotApi
{
    internal partial class KrakenRestClientSpotApi : IKrakenRestClientSpotApiShared
    {
        public string Exchange => KrakenExchange.ExchangeName;
        public TradingMode[] SupportedTradingModes { get; } = new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        #region Kline client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(SharedPaginationSupport.NotSupported, false)
        {
            MaxTotalDataPoints = 720
        };

        async Task<ExchangeWebResult<IEnumerable<SharedKline>>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new ExchangeWebResult<IEnumerable<SharedKline>>(Exchange, new ArgumentError("Interval not supported"));

            var validationError = ((IKlineRestClient)this).GetKlinesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedKline>>(Exchange, validationError);

            var apiLimit = 720;
            int limit = request.Limit ?? apiLimit;
            if (request.StartTime.HasValue == true)
                limit = (int)Math.Ceiling((DateTime.UtcNow - request.StartTime!.Value).TotalSeconds / (int)request.Interval);

            if (limit > apiLimit)
            {
                // Not available via the API
                var cutoff = DateTime.UtcNow.AddSeconds(-(int)request.Interval * apiLimit);
                return new ExchangeWebResult<IEnumerable<SharedKline>>(Exchange, new ArgumentError($"Time filter outside of supported range. Can only request the most recent {apiLimit} klines i.e. data later than {cutoff} at this interval"));
            }

            // Pagination not supported, no time filter available (always most recent)

            // Get data
            var result = await ExchangeData.GetKlinesAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                interval,
                DateTime.UtcNow.AddSeconds(-(int)request.Interval * limit),
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedKline>>(Exchange, null, default);

            // Filter the data based on requested timestamps
            var data = result.Data.Data;
            if (request.StartTime.HasValue == true)
                data = data.Where(d => d.OpenTime >= request.StartTime.Value);
            if (request.EndTime.HasValue == true)
                data = data.Where(d => d.OpenTime < request.EndTime.Value);
            if (request.Limit.HasValue == true)
                data = data.Take(request.Limit.Value);

            return result.AsExchangeResult<IEnumerable<SharedKline>>(Exchange, request.Symbol.TradingMode, data.Reverse().Select(x => new SharedKline(x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume)).ToArray());
        }

        #endregion

        #region Spot Symbol client

        EndpointOptions<GetSymbolsRequest> ISpotSymbolRestClient.GetSpotSymbolsOptions { get; } = new EndpointOptions<GetSymbolsRequest>(false);
        async Task<ExchangeWebResult<IEnumerable<SharedSpotSymbol>>> ISpotSymbolRestClient.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotSymbolRestClient)this).GetSpotSymbolsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedSpotSymbol>>(Exchange, validationError);

            var result = await ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedSpotSymbol>>(Exchange, null, default);

            return result.AsExchangeResult<IEnumerable<SharedSpotSymbol>>(Exchange, TradingMode.Spot, result.Data.Select(s =>
            {
                var assets = GetAssets(s.Value.WebsocketName);
                return new SharedSpotSymbol(assets.BaseAsset, assets.QuoteAsset, s.Key, s.Value.Status == SymbolStatus.Online)
                {
                    PriceDecimals = s.Value.PriceDecimals,
                    QuantityDecimals = s.Value.LotDecimals,
                    MinTradeQuantity = s.Value.OrderMin,
                    PriceStep = s.Value.TickSize,
                    MinNotionalValue = s.Value.MinValue
                };
            }).ToArray());
        }

        private (string BaseAsset, string QuoteAsset) GetAssets(string name)
        {
            var split = name.Split('/');
            return (split[0], split[1]);
        }

        #endregion

        #region Ticker client

        EndpointOptions<GetTickerRequest> ISpotTickerRestClient.GetSpotTickerOptions { get; } = new EndpointOptions<GetTickerRequest>(false);
        async Task<ExchangeWebResult<SharedSpotTicker>> ISpotTickerRestClient.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTickerRestClient)this).GetSpotTickerOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotTicker>(Exchange, validationError);

            var result = await ExchangeData.GetTickerAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotTicker>(Exchange, null, default);

            var ticker = result.Data.Single();
            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedSpotTicker(ticker.Value.Symbol, ticker.Value.LastTrade.Price, ticker.Value.High.Value24H, ticker.Value.Low.Value24H, ticker.Value.Volume.Value24H, ticker.Value.OpenPrice == 0 ? null : Math.Round(ticker.Value.LastTrade.Price / ticker.Value.OpenPrice * 100 - 100, 2)));
        }

        EndpointOptions<GetTickersRequest> ISpotTickerRestClient.GetSpotTickersOptions { get; } = new EndpointOptions<GetTickersRequest>(false);
        async Task<ExchangeWebResult<IEnumerable<SharedSpotTicker>>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTickerRestClient)this).GetSpotTickersOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedSpotTicker>>(Exchange, validationError);

            var result = await ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedSpotTicker>>(Exchange, null, default);

            return result.AsExchangeResult<IEnumerable<SharedSpotTicker>>(Exchange, TradingMode.Spot, result.Data.Select(x => new SharedSpotTicker(x.Value.Symbol, x.Value.LastTrade.Price, x.Value.High.Value24H, x.Value.Low.Value24H, x.Value.Volume.Value24H, x.Value.OpenPrice == 0 ? null : Math.Round(x.Value.LastTrade.Price / x.Value.OpenPrice * 100 - 100, 2))).ToArray());
        }

        #endregion

        #region Recent Trade client

        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(1000, false);

        async Task<ExchangeWebResult<IEnumerable<SharedTrade>>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = ((IRecentTradeRestClient)this).GetRecentTradesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedTrade>>(Exchange, validationError);

            var result = await ExchangeData.GetTradeHistoryAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedTrade>>(Exchange, null, default);

            return result.AsExchangeResult<IEnumerable<SharedTrade>>(Exchange, request.Symbol.TradingMode, result.Data.Data.Reverse().Select(x => new SharedTrade(x.Quantity, x.Price, x.Timestamp)).ToArray());
        }

        #endregion

        #region Balance client
        EndpointOptions<GetBalancesRequest> IBalanceRestClient.GetBalancesOptions { get; } = new EndpointOptions<GetBalancesRequest>(true);

        async Task<ExchangeWebResult<IEnumerable<SharedBalance>>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = ((IBalanceRestClient)this).GetBalancesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedBalance>>(Exchange, validationError);

            var result = await Account.GetAvailableBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<IEnumerable<SharedBalance>>(Exchange, null, default);

            return result.AsExchangeResult<IEnumerable<SharedBalance>>(Exchange, TradingMode.Spot, result.Data.Select(x => new SharedBalance(x.Key, x.Value.Available, x.Value.Total)).ToArray());
        }

        #endregion

        #region Spot Order client


        SharedFeeDeductionType ISpotOrderRestClient.SpotFeeDeductionType => SharedFeeDeductionType.DeductFromOutput;
        SharedFeeAssetType ISpotOrderRestClient.SpotFeeAssetType => SharedFeeAssetType.Variable;
        IEnumerable<SharedOrderType> ISpotOrderRestClient.SpotSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker };
        IEnumerable<SharedTimeInForce> ISpotOrderRestClient.SpotSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };

        SharedQuantitySupport ISpotOrderRestClient.SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAndQuoteAsset,
                SharedQuantityType.BaseAsset);

        PlaceSpotOrderOptions ISpotOrderRestClient.PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions();
        async Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).PlaceSpotOrderOptions.ValidateRequest(
                Exchange,
                request,
                request.Symbol.TradingMode,
                SupportedTradingModes,
                ((ISpotOrderRestClient)this).SpotSupportedOrderTypes,
                ((ISpotOrderRestClient)this).SpotSupportedTimeInForce,
                ((ISpotOrderRestClient)this).SpotSupportedOrderQuantity);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            uint cid = 0;
            if (request.ClientOrderId != null && !uint.TryParse(request.ClientOrderId, out cid))
                return new ExchangeWebResult<SharedId>(Exchange, new ArgumentError("Client order id needs to be a positive integer if specified"));

            var result = await Trading.PlaceOrderAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                GetPlaceOrderType(request.OrderType),
                request.Quantity ?? request.QuoteQuantity ?? 0,
                request.Price,
                clientOrderId: request.ClientOrderId != null ? cid : null,
                orderFlags: GetOrderFlags(request.OrderType, request.QuoteQuantity),
                timeInForce: GetTimeInForce(request.TimeInForce),
                ct: ct).ConfigureAwait(false);

            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedId(result.Data.OrderIds.Single().ToString()));
        }

        EndpointOptions<GetOrderRequest> ISpotOrderRestClient.GetSpotOrderOptions { get; } = new EndpointOptions<GetOrderRequest>(true);
        async Task<ExchangeWebResult<SharedSpotOrder>> ISpotOrderRestClient.GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotOrderOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotOrder>(Exchange, validationError);

            var order = await Trading.GetOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedSpotOrder>(Exchange, null, default);

            var orderData = order.Data.SingleOrDefault();
            if (orderData.Value == null)
                return new ExchangeWebResult<SharedSpotOrder>(Exchange, new ServerError("Order not found"));

            return order.AsExchangeResult(Exchange, TradingMode.Spot, new SharedSpotOrder(
                orderData.Value.OrderDetails.Symbol,
                orderData.Value.Id.ToString(),
                ParseOrderType(orderData.Value.OrderDetails.Type, orderData.Value.Oflags),
                orderData.Value.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(orderData.Value.Status),
                orderData.Value.CreateTime)
            {
                ClientOrderId = orderData.Value.ClientOrderId,
                Fee = orderData.Value.Fee,
                OrderPrice = orderData.Value.OrderDetails.Price,
                Quantity = orderData.Value.Oflags.Contains("viqc") ? null : orderData.Value.Quantity,
                QuantityFilled = orderData.Value.QuantityFilled,
                QuoteQuantity = orderData.Value.Oflags.Contains("viqc") ? orderData.Value.Quantity : null,
                QuoteQuantityFilled = orderData.Value.QuoteQuantityFilled,
                AveragePrice = orderData.Value.AveragePrice == 0 ? null : orderData.Value.AveragePrice,
                UpdateTime = orderData.Value.CloseTime
            });
        }

        EndpointOptions<GetOpenOrdersRequest> ISpotOrderRestClient.GetOpenSpotOrdersOptions { get; } = new EndpointOptions<GetOpenOrdersRequest>(true);
        async Task<ExchangeWebResult<IEnumerable<SharedSpotOrder>>> ISpotOrderRestClient.GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetOpenSpotOrdersOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedSpotOrder>>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var order = await Trading.GetOpenOrdersAsync(ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<IEnumerable<SharedSpotOrder>>(Exchange, null, default);

            IEnumerable<KrakenOrder> orders = order.Data.Open.Values.AsEnumerable();
            if (symbol != null)
                orders = orders.Where(x => x.OrderDetails.Symbol == symbol);

            return order.AsExchangeResult<IEnumerable<SharedSpotOrder>>(Exchange, TradingMode.Spot, orders.Select(x => new SharedSpotOrder(
                x.OrderDetails.Symbol,
                x.Id.ToString(),
                ParseOrderType(x.OrderDetails.Type, x.Oflags),
                x.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                Fee = x.Fee,
                OrderPrice = x.OrderDetails.Price,
                Quantity = x.Oflags.Contains("viqc") ? null : x.Quantity,
                QuantityFilled = x.QuantityFilled,
                QuoteQuantity = x.Oflags.Contains("viqc") ? x.Quantity : null,
                AveragePrice = x.AveragePrice == 0 ? null: x.AveragePrice,
                UpdateTime = x.CloseTime
            }).ToArray());
        }

        PaginatedEndpointOptions<GetClosedOrdersRequest> ISpotOrderRestClient.GetClosedSpotOrdersOptions { get; } = new PaginatedEndpointOptions<GetClosedOrdersRequest>(SharedPaginationSupport.Descending, true)
        {
            RequestNotes = "Request always returns up to 50 results"
        };
        async Task<ExchangeWebResult<IEnumerable<SharedSpotOrder>>> ISpotOrderRestClient.GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetClosedSpotOrdersOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedSpotOrder>>(Exchange, validationError);

            // Determine page token
            int? offset = null;
            if (pageToken is OffsetToken token)
                offset = token.Offset;

            var order = await Trading.GetClosedOrdersAsync(
                startTime: request.StartTime,
                endTime: request.EndTime,
                resultOffset: offset,
                ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<IEnumerable<SharedSpotOrder>>(Exchange, null, default);

            // Get next token
            OffsetToken? nextToken = null;
            if (order.Data.Count > (order.Data.Closed.Count + (offset ?? 0)))
                nextToken = new OffsetToken((offset ?? 0) + order.Data.Closed.Count);

            var orders = order.Data.Closed.Values.Where(x => x.OrderDetails.Symbol == request.Symbol.GetSymbol(FormatSymbol));
            return order.AsExchangeResult<IEnumerable<SharedSpotOrder>>(Exchange, TradingMode.Spot, orders.Select(x => new SharedSpotOrder(
                x.OrderDetails.Symbol,
                x.Id.ToString(),
                ParseOrderType(x.OrderDetails.Type, x.Oflags),
                x.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                Fee = x.Fee,
                OrderPrice = x.OrderDetails.Price,
                Quantity = x.Oflags.Contains("viqc") ? null : x.Quantity,
                QuantityFilled = x.QuantityFilled,
                QuoteQuantity = x.Oflags.Contains("viqc") ? x.Quantity : null,
                AveragePrice = x.AveragePrice == 0 ? null: x.AveragePrice,
                UpdateTime = x.CloseTime
            }).ToArray(), nextToken);
        }

        EndpointOptions<GetOrderTradesRequest> ISpotOrderRestClient.GetSpotOrderTradesOptions { get; } = new EndpointOptions<GetOrderTradesRequest>(true);
        async Task<ExchangeWebResult<IEnumerable<SharedUserTrade>>> ISpotOrderRestClient.GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotOrderTradesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedUserTrade>>(Exchange, validationError);

            var order = await Trading.GetOrderAsync(request.OrderId, trades: true).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, null, default);

            var orderData = order.Data.SingleOrDefault();
            if (orderData.Value == null)
                return new ExchangeWebResult<IEnumerable<SharedUserTrade>>(Exchange, new ServerError("Order not found"));

            var trades = orderData.Value.TradeIds.ToList();
            if (!trades.Any())
                return order.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, null, new SharedUserTrade[] { });

            var tradeInfo = await Trading.GetUserTradeDetailsAsync(trades.Take(20), ct: ct).ConfigureAwait(false);
            if (!tradeInfo)
                return tradeInfo.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, null, default);

            return tradeInfo.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, TradingMode.Spot, tradeInfo.Data.Select(x => new SharedUserTrade(
                x.Value.Symbol,
                x.Value.OrderId.ToString(),
                x.Value.Id.ToString(),
                x.Value.Quantity,
                x.Value.Price,
                x.Value.Timestamp)
            {
                Fee = x.Value.Fee,
                Role = x.Value.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());
        }

        PaginatedEndpointOptions<GetUserTradesRequest> ISpotOrderRestClient.GetSpotUserTradesOptions { get; } = new PaginatedEndpointOptions<GetUserTradesRequest>(SharedPaginationSupport.Descending, true)
        {
            RequestNotes = "Request always returns up to 50 results"
        };
        async Task<ExchangeWebResult<IEnumerable<SharedUserTrade>>> ISpotOrderRestClient.GetSpotUserTradesAsync(GetUserTradesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotUserTradesOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedUserTrade>>(Exchange, validationError);

            // Determine page token
            int? offset = null;
            if (pageToken is OffsetToken token)
                offset = token.Offset;

            // Get data
            var order = await Trading.GetUserTradesAsync(
                startTime: request.StartTime,
                endTime: request.EndTime,
                resultOffset: offset, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, null, default);

            // Get next token
            OffsetToken? nextToken = null;
            if (order.Data.Count > order.Data.Trades.Count)
                nextToken = new OffsetToken((offset ?? 0) + order.Data.Trades.Count);

            var symbol = request.Symbol.GetSymbol(FormatSymbol);
            return order.AsExchangeResult<IEnumerable<SharedUserTrade>>(Exchange, TradingMode.Spot, order.Data.Trades.Where(t => t.Value.Symbol == symbol).Select(x => new SharedUserTrade(
                x.Value.Symbol,
                x.Value.OrderId.ToString(),
                x.Value.Id.ToString(),
                x.Value.Quantity,
                x.Value.Price,
                x.Value.Timestamp)
            {
                Fee = x.Value.Fee,
                Role = x.Value.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray(), nextToken);
        }

        EndpointOptions<CancelOrderRequest> ISpotOrderRestClient.CancelSpotOrderOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).CancelSpotOrderOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedId(order.Data.ToString()));
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == OrderStatus.Open || status == OrderStatus.Pending) return SharedOrderStatus.Open;
            if (status == OrderStatus.Canceled || status == OrderStatus.Expired) return SharedOrderStatus.Canceled;
            return SharedOrderStatus.Filled;
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

            return null;
        }

        #endregion

        #region Asset client
        EndpointOptions<GetAssetRequest> IAssetsRestClient.GetAssetOptions { get; } = new EndpointOptions<GetAssetRequest>(false);
        async Task<ExchangeWebResult<SharedAsset>> IAssetsRestClient.GetAssetAsync(GetAssetRequest request, CancellationToken ct)
        {
            var validationError = ((IAssetsRestClient)this).GetAssetOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedAsset>(Exchange, validationError);

            var assets = await Account.GetWithdrawMethodsAsync(request.Asset, ct: ct).ConfigureAwait(false);
            if (!assets)
                return assets.AsExchangeResult<SharedAsset>(Exchange, null, default);

            if (!assets.Data.Any())
                return assets.AsExchangeError<SharedAsset>(Exchange, new ServerError("Asset not found"));

            return assets.AsExchangeResult(Exchange, TradingMode.Spot, new SharedAsset(request.Asset)
            {
                Networks = assets.Data.Select(x => new SharedAssetNetwork(x.Network)
                {
                    FullName = x.Method,
                    MinWithdrawQuantity = x.Minimum
                }).ToArray()
            });
        }

        EndpointOptions<GetAssetsRequest> IAssetsRestClient.GetAssetsOptions { get; } = new EndpointOptions<GetAssetsRequest>(true);
        async Task<ExchangeWebResult<IEnumerable<SharedAsset>>> IAssetsRestClient.GetAssetsAsync(GetAssetsRequest request, CancellationToken ct)
        {
            var validationError = ((IAssetsRestClient)this).GetAssetsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedAsset>>(Exchange, validationError);

            var assets = await Account.GetWithdrawMethodsAsync(ct: ct).ConfigureAwait(false);
            if (!assets)
                return assets.AsExchangeResult<IEnumerable<SharedAsset>>(Exchange, null, default);

            return assets.AsExchangeResult<IEnumerable<SharedAsset>>(Exchange, TradingMode.Spot, assets.Data.GroupBy(x => x.Asset).Select(x => new SharedAsset(x.Key)
            {
                Networks = x.Select(x => new SharedAssetNetwork(x.Network)
                {
                    FullName = x.Method,
                    MinWithdrawQuantity = x.Minimum
                }).ToArray()
            }).ToArray());
        }

        #endregion

        #region Deposit client

        EndpointOptions<GetDepositAddressesRequest> IDepositRestClient.GetDepositAddressesOptions { get; } = new EndpointOptions<GetDepositAddressesRequest>(true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(GetDepositAddressesRequest.Network), typeof(string), "The network the deposit address should be for", "Bitcoin")
            }
        };
        async Task<ExchangeWebResult<IEnumerable<SharedDepositAddress>>> IDepositRestClient.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            var validationError = ((IDepositRestClient)this).GetDepositAddressesOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedDepositAddress>>(Exchange, validationError);

            var depositAddresses = await Account.GetDepositAddressesAsync(request.Asset, request.Network!).ConfigureAwait(false);
            if (!depositAddresses)
                return depositAddresses.AsExchangeResult<IEnumerable<SharedDepositAddress>>(Exchange, null, default);

            return depositAddresses.AsExchangeResult<IEnumerable<SharedDepositAddress>>(Exchange, TradingMode.Spot, depositAddresses.Data.Select(x => new SharedDepositAddress(request.Asset, x.Address)
            {
                TagOrMemo = x.Tag
            }
            ).ToArray());
        }

        GetDepositsOptions IDepositRestClient.GetDepositsOptions { get; } = new GetDepositsOptions(SharedPaginationSupport.Descending, true);
        async Task<ExchangeWebResult<IEnumerable<SharedDeposit>>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IDepositRestClient)this).GetDepositsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedDeposit>>(Exchange, validationError);

            // Determine page token
            string? cursor = null;
            if (pageToken is CursorToken cursorToken)
                cursor = cursorToken.Cursor;

            // Get data
            var deposits = await Account.GetDepositHistoryAsync(
                asset: request.Asset,
                startTime: request.StartTime,
                endTime: request.EndTime,
                limit: request.Limit,
                cursor: cursor,
                ct: ct).ConfigureAwait(false);
            if (!deposits)
                return deposits.AsExchangeResult<IEnumerable<SharedDeposit>>(Exchange, null, default);

            // Determine next token
            CursorToken? nextToken = null;
            if (!string.IsNullOrEmpty(deposits.Data.NextCursor))
                nextToken = new CursorToken(deposits.Data.NextCursor!);

            return deposits.AsExchangeResult<IEnumerable<SharedDeposit>>(Exchange, TradingMode.Spot, deposits.Data.Items.Select(x => new SharedDeposit(x.Asset, x.Quantity, true, x.Timestamp)
            {
                Id = x.ReferenceId,
                TransactionId = x.TransactionId,
                Network = x.Method
            }).ToArray(), nextToken);
        }

        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(1, 500, false);
        async Task<ExchangeWebResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = ((IOrderBookRestClient)this).GetOrderBookOptions.ValidateRequest(Exchange, request, request.Symbol.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedOrderBook>(Exchange, validationError);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol.GetSymbol(FormatSymbol),
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedOrderBook>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedOrderBook(result.Data.Asks, result.Data.Bids));
        }

        #endregion

        #region Withdrawal client

        GetWithdrawalsOptions IWithdrawalRestClient.GetWithdrawalsOptions { get; } = new GetWithdrawalsOptions(SharedPaginationSupport.Descending, true);
        async Task<ExchangeWebResult<IEnumerable<SharedWithdrawal>>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IWithdrawalRestClient)this).GetWithdrawalsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<IEnumerable<SharedWithdrawal>>(Exchange, validationError);

            // Determine page token
            string? cursor = null;
            if (pageToken is CursorToken cursorToken)
                cursor = cursorToken.Cursor;

            // Get data
            var withdrawals = await Account.GetWithdrawalHistoryAsync(
                asset: request.Asset,
                startTime: request.StartTime,
                endTime: request.EndTime,
                limit: request.Limit,
                cursor: cursor,
                ct: ct).ConfigureAwait(false);
            if (!withdrawals)
                return withdrawals.AsExchangeResult<IEnumerable<SharedWithdrawal>>(Exchange, null, default);

            // Determine next token
            CursorToken? nextToken = null;
            if (!string.IsNullOrEmpty(withdrawals.Data.NextCursor))
                nextToken = new CursorToken(withdrawals.Data.NextCursor!);

            return withdrawals.AsExchangeResult<IEnumerable<SharedWithdrawal>>(Exchange, TradingMode.Spot, withdrawals.Data.Items.Select(x => new SharedWithdrawal(x.Asset, x.Key, x.Quantity, x.Status == "Success", x.Timestamp)
            {
                Id = x.ReferenceId,
                TransactionId = x.TransactionId,
                Network = x.Method,
                Fee = x.Fee
            }).ToArray(), nextToken);
        }

        #endregion

        #region Withdraw client

        WithdrawOptions IWithdrawRestClient.WithdrawOptions { get; } = new WithdrawOptions()
        {
            RequiredExchangeParameters = new List<ParameterDescription>
            {
                new ParameterDescription("key", typeof(string), "The name of the withdrawal address as defined in the web UI", "KucoinBitcoinAddress1")
            }
        };

        async Task<ExchangeWebResult<SharedId>> IWithdrawRestClient.WithdrawAsync(WithdrawRequest request, CancellationToken ct)
        {
            var validationError = ((IWithdrawRestClient)this).WithdrawOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var keyName = request.ExchangeParameters!.GetValue<string>(Exchange, "key");

            // Get data
            var withdrawal = await Account.WithdrawAsync(
                request.Asset,
                keyName!,
                request.Quantity,
                request.Address,
                ct: ct).ConfigureAwait(false);
            if (!withdrawal)
                return withdrawal.AsExchangeResult<SharedId>(Exchange, null, default);

            return withdrawal.AsExchangeResult(Exchange, TradingMode.Spot, new SharedId(withdrawal.Data.ReferenceId));
        }

        #endregion
    }
}
