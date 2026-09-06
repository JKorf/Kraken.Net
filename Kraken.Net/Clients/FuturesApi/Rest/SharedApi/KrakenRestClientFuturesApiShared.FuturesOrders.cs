using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models.Futures;

namespace Kraken.Net.Clients.FuturesApi
{
	internal partial class KrakenRestClientFuturesSharedApi
    {

        #region Place Futures Order

        public SharedFeeDeductionType FuturesFeeDeductionType => SharedFeeDeductionType.AddToCost;
        public SharedFeeAssetType FuturesFeeAssetType => SharedFeeAssetType.QuoteAsset;

        public SharedOrderType[] FuturesSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market };
        public SharedTimeInForce[] FuturesSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel };
        public SharedQuantitySupport FuturesSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts);

        public string GenerateClientOrderId() => ExchangeHelpers.RandomString(32);

        async Task<ICallResult<SharedId>> IPlaceFuturesOrder.PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
            => await PlaceFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public PlaceFuturesOrderOptions PlaceFuturesOrderOptions { get; } = new PlaceFuturesOrderOptions(_exchangeName, false);
        public async Task<HttpResult<SharedId>> PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
        {
            var validationError = PlaceFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                GetOrderType(request.OrderType, request.TimeInForce),
                quantity: request.Quantity?.QuantityInContracts ?? 0,
                price: request.Price,
                reduceOnly: request.ReduceOnly,
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        }

        #endregion

        #region Get Futures Order

        async Task<ICallResult<SharedFuturesOrder>> IGetFuturesOrder.GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
            => await GetFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public GetFuturesOrderOptions GetFuturesOrderOptions { get; } = new GetFuturesOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedFuturesOrder>> GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, validationError);

            var order = await _api.Trading.GetOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedFuturesOrder>(order);

            return HttpResult.Ok(order, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, order.Data.Order.Symbol),
                order.Data.Order.Symbol,
                order.Data.Order.OrderId,
                SharedOrderType.Other,
                order.Data.Order.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Data.Status),
                order.Data.Order.Timestamp)
            {
                ClientOrderId = order.Data.Order.ClientOrderId,
                OrderPrice = order.Data.Order.Price == 0 ? null : order.Data.Order.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: order.Data.Order.Quantity == 0 ? null : order.Data.Order.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: order.Data.Order.QuantityFilled),
                UpdateTime = order.Data.Order.LastUpdateTime,
                ReduceOnly = order.Data.Order.ReduceOnly
            });
        }

        #endregion

        #region Get Open Futures Orders

        async Task<ICallResult<SharedFuturesOrder[]>> IGetOpenFuturesOrders.GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
            => await GetOpenFuturesOrdersAsync(request, ct).ConfigureAwait(false);

        public GetOpenFuturesOrdersOptions GetOpenFuturesOrdersOptions { get; } = new GetOpenFuturesOrdersOptions(_exchangeName, true);
        public async Task<HttpResult<SharedFuturesOrder[]>> GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = GetOpenFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await _api.Trading.GetOpenOrdersAsync(ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedFuturesOrder[]>(orders);

            IEnumerable<KrakenFuturesOpenOrder> data = orders.Data;
            if (symbol != null)
                data = data.Where(x => x.Symbol == symbol);

            return HttpResult.Ok(orders, data.Select(x => new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                x.Symbol,
                x.OrderId.ToString(),
                x.Type == FuturesOrderType.Limit ? SharedOrderType.Limit : SharedOrderType.Other,
                x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                SharedOrderStatus.Open,
                x.Timestamp)
            {
                ClientOrderId = x.ClientOrderId,
                OrderPrice = x.Price == 0 ? null : x.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                UpdateTime = x.LastUpdateTime,
                ReduceOnly = x.ReduceOnly
            }).ToArray());
        }

        #endregion

        public GetFuturesClosedOrdersOptions GetClosedFuturesOrdersOptions { get; } = new GetFuturesClosedOrdersOptions(_exchangeName, false, false, false, 1000)
        {
            Supported = false,
            RequestNotes = "There is no way to retrieve closed orders without specifying order ids, so this method can never return success"
        };
        public Task<HttpResult<SharedFuturesOrder[]>> GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return Task.FromResult(HttpResult.Fail<SharedFuturesOrder[]>(Exchange, new InvalidOperationError($"Method not available for {Exchange}")));
        }

        public GetFuturesOrderTradesOptions GetFuturesOrderTradesOptions { get; } = new GetFuturesOrderTradesOptions(_exchangeName, true)
        {
            Supported = false,
            RequestNotes = "There is no way to retrieve trades for a specific order, so this method can never return success"
        };
        public Task<HttpResult<SharedUserTrade[]>> GetFuturesOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            return Task.FromResult(HttpResult.Fail<SharedUserTrade[]>(Exchange, new InvalidOperationError($"Method not available for {Exchange}")));
        }

        #region Get Futures User Trade History

        async Task<ICallResult<SharedUserTrade[]>> IGetFuturesUserTradeHistory.GetFuturesUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetFuturesUserTradeHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        Task<HttpResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesUserTradesAsync(GetUserTradesRequest request, PageRequest? nextPageToken, CancellationToken ct)
            => GetFuturesUserTradeHistoryAsync(request, nextPageToken, ct);
        GetFuturesUserTradeHistoryOptions IFuturesOrderRestClient.GetFuturesUserTradesOptions => GetFuturesUserTradeHistoryOptions;

        public GetFuturesUserTradeHistoryOptions GetFuturesUserTradeHistoryOptions { get; } = new GetFuturesUserTradeHistoryOptions(_exchangeName, false, true, true, 100);
        public async Task<HttpResult<SharedUserTrade[]>> GetFuturesUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetFuturesUserTradeHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await _api.Trading.GetUserTradesAsync(
                startTime: request.EndTime,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedUserTrade[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromTime(pageParams, result.Data.Min(x => x.FillTime)),
                     result.Data.Length,
                     result.Data.Select(x => x.FillTime),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.FillTime, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedUserTrade(
                            ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                            x.Symbol,
                            x.OrderId.ToString(),
                            x.Id.ToString(),
                            x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            new SharedOrderQuantity(contractQuantity: x.Quantity),
                            x.Price,
                            x.FillTime)
                        {
                            ClientOrderId = x.ClientOrderId,
                            Price = x.Price,
                            Role = x.Type == TradeType.Maker ? SharedRole.Maker : SharedRole.Taker
                        }).ToArray(), nextPageRequest);
        }

        #endregion

        #region Cancel Futures Order

        async Task<ICallResult<SharedId>> ICancelFuturesOrder.CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelFuturesOrderAsync(request, ct).ConfigureAwait(false);

        public CancelFuturesOrderOptions CancelFuturesOrderOptions { get; } = new CancelFuturesOrderOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.Trading.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.OrderId.ToString()));
        }

        #endregion

        #region Get Positions

        async Task<ICallResult<SharedPosition[]>> IGetPositions.GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
            => await GetPositionsAsync(request, ct).ConfigureAwait(false);

        public GetPositionsOptions GetPositionsOptions { get; } = new GetPositionsOptions(_exchangeName, true);
        public async Task<HttpResult<SharedPosition[]>> GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
        {
            var validationError = GetPositionsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedPosition[]>(Exchange, validationError);

            var result = await _api.Trading.GetOpenPositionsAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedPosition[]>(result);

            IEnumerable<KrakenFuturesPosition> data = result.Data;
            if (request.Symbol != null)
                data = data.Where(x => x.Symbol == request.Symbol.GetSymbol(FormatSymbol));

            if (request.TradingMode.HasValue)
            {
                data = data.Where(x => request.TradingMode == TradingMode.PerpetualLinear ? x.Symbol.StartsWith("PF_") :
                    request.TradingMode == TradingMode.DeliveryLinear ? x.Symbol.StartsWith("FF_") :
                    request.TradingMode == TradingMode.PerpetualInverse ? x.Symbol.StartsWith("PI_") :
                    x.Symbol.StartsWith("FI_"));
            }

            var resultTypes = request.Symbol == null && request.TradingMode == null ? SupportedTradingModes : request.Symbol != null ? new[] { request.Symbol.TradingMode } : new[] { request.TradingMode!.Value };
            return HttpResult.Ok(result, data.Select(x =>
            new SharedPosition(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                x.Symbol,
                new SharedOrderQuantity(contractQuantity: Math.Abs(x.Quantity)),
                x.FillTime)
            {
                Leverage = x.MaxFixedLeverage,
                AverageOpenPrice = x.Price,
                PositionMode = SharedPositionMode.OneWay,
                PositionSide = x.Side == PositionSide.Short ? SharedPositionSide.Short : SharedPositionSide.Long,
                UnrealizedPnl = x.UnrealizedPnl
            }).ToArray());
        }

        #endregion

        #region Close Position

        async Task<ICallResult<SharedId>> IClosePosition.ClosePositionAsync(ClosePositionRequest request, CancellationToken ct)
            => await ClosePositionAsync(request, ct).ConfigureAwait(false);

        public ClosePositionOptions ClosePositionOptions { get; } = new ClosePositionOptions(_exchangeName, true)
        {
            RequiredRequestParameters = [
                RequestParameterRule<ClosePositionRequest>.Required(x => x.PositionSide, "The position side to close", SharedPositionSide.Long),
                RequestParameterRule<ClosePositionRequest>.Required(x => x.Quantity, "Quantity of the position to close", 1m)
                ]
        };
        public async Task<HttpResult<SharedId>> ClosePositionAsync(ClosePositionRequest request, CancellationToken ct)
        {
            var validationError = ClosePositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);

            var result = await _api.Trading.PlaceOrderAsync(
                symbol,
                request.PositionSide == SharedPositionSide.Long ? OrderSide.Sell : OrderSide.Buy,
                FuturesOrderType.Market,
                quantity: request.Quantity!.Value,
                reduceOnly: true,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        }

        #endregion

        private SharedOrderStatus ParseOrderStatus(KrakenFuturesOrderActiveStatus status)
        {
            if (status == KrakenFuturesOrderActiveStatus.EnteredBook) return SharedOrderStatus.Open;
            if (status == KrakenFuturesOrderActiveStatus.FullyExecuted) return SharedOrderStatus.Filled;
            if (status == KrakenFuturesOrderActiveStatus.Cancelled
                || status == KrakenFuturesOrderActiveStatus.Rejected
                || status == KrakenFuturesOrderActiveStatus.TriggerActivationFailure)
            {
                return SharedOrderStatus.Canceled;
            }

            return SharedOrderStatus.Unknown;
        }

        private FuturesOrderType GetOrderType(SharedOrderType orderType, SharedTimeInForce? timeInForce)
        {
            if (orderType == SharedOrderType.Market)
                return FuturesOrderType.Market;

            if (timeInForce == SharedTimeInForce.ImmediateOrCancel)
                return FuturesOrderType.ImmediateOrCancel;

            return FuturesOrderType.Limit;
        }

        #region Get Futures Order By Client Order Id

        async Task<ICallResult<SharedFuturesOrder>> IGetFuturesOrderByClientOrderId.GetFuturesOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
            => await GetFuturesOrderByClientOrderIdAsync(request, ct).ConfigureAwait(false);

        public GetFuturesOrderByClientOrderIdOptions GetFuturesOrderByClientOrderIdOptions { get; } = new GetFuturesOrderByClientOrderIdOptions(_exchangeName, true);
        public async Task<HttpResult<SharedFuturesOrder>> GetFuturesOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = GetFuturesOrderByClientOrderIdOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, validationError);

            var order = await _api.Trading.GetOrderAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedFuturesOrder>(order);

            return HttpResult.Ok(order, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, order.Data.Order.Symbol),
                order.Data.Order.Symbol,
                order.Data.Order.OrderId,
                SharedOrderType.Other,
                order.Data.Order.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Data.Status),
                order.Data.Order.Timestamp)
            {
                ClientOrderId = order.Data.Order.ClientOrderId,
                OrderPrice = order.Data.Order.Price == 0 ? null : order.Data.Order.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: order.Data.Order.Quantity == 0 ? null : order.Data.Order.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: order.Data.Order.QuantityFilled),
                UpdateTime = order.Data.Order.LastUpdateTime,
                ReduceOnly = order.Data.Order.ReduceOnly
            });
        }

        #endregion

        #region Cancel Futures Order By Client Order Id

        async Task<ICallResult<SharedId>> ICancelFuturesOrderByClientOrderId.CancelFuturesOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelFuturesOrderByClientOrderIdAsync(request, ct).ConfigureAwait(false);

        public CancelFuturesOrderByClientOrderIdOptions CancelFuturesOrderByClientOrderIdOptions { get; } = new CancelFuturesOrderByClientOrderIdOptions(_exchangeName, true);
        public async Task<HttpResult<SharedId>> CancelFuturesOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesOrderByClientOrderIdOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.Trading.CancelOrderAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.OrderId));
        }

        #endregion

    }
}
