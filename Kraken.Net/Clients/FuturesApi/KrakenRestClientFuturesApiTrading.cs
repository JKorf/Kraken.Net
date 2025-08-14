using Kraken.Net.Objects.Models.Futures;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using CryptoExchange.Net.Objects.Errors;

namespace Kraken.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class KrakenRestClientFuturesApiTrading : IKrakenRestClientFuturesApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly KrakenRestClientFuturesApi _baseClient;

        internal KrakenRestClientFuturesApiTrading(KrakenRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get User Trades

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesUserTrade[]>> GetUserTradesAsync(DateTime? startTime = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("lastFillTime", startTime?.ToString("o"));

            var weight = startTime == null ? 2 : 25;
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/fills", KrakenExchange.RateLimiter.FuturesApi, 1, true);
            return await _baseClient.SendAsync<KrakenFuturesUserTradeResult, KrakenFuturesUserTrade[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Self Trade Strategy

        /// <inheritdoc />
        public async Task<WebCallResult<SelfTradeStrategy>> GetSelfTradeStrategyAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/self-trade-strategy", KrakenExchange.RateLimiter.FuturesApi, 2, true);
            return await _baseClient.SendAsync<KrakenFuturesSelfTradeResult, SelfTradeStrategy>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Self Trade Strategy

        /// <inheritdoc />
        public async Task<WebCallResult<SelfTradeStrategy>> SetSelfTradeStrategyAsync(SelfTradeStrategy strategy, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "strategy", EnumConverter.GetString(strategy) }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Put, "derivatives/api/v3/self-trade-strategy", KrakenExchange.RateLimiter.FuturesApi, 2, true);
            return await _baseClient.SendAsync<KrakenFuturesSelfTradeResult, SelfTradeStrategy>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Open Positions

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesPosition[]>> GetOpenPositionsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/openpositions", KrakenExchange.RateLimiter.FuturesApi, 2, true);
            return await _baseClient.SendAsync<KrakenFuturesPositionResult, KrakenFuturesPosition[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Leverage

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesLeverage[]>> GetLeverageAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/leveragepreferences", KrakenExchange.RateLimiter.FuturesApi, 1, true);
            return await _baseClient.SendAsync<KrakenFuturesLeverageResult, KrakenFuturesLeverage[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Leverage

        /// <inheritdoc />
        public async Task<WebCallResult> SetLeverageAsync(string symbol, decimal maxLeverage, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol },
                { "maxLeverage", maxLeverage.ToString(CultureInfo.InvariantCulture) }
            };
            var request = _definitions.GetOrCreate(HttpMethod.Put, "derivatives/api/v3/leveragepreferences", KrakenExchange.RateLimiter.FuturesApi, 1, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Place Order

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesOrderResult>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            FuturesOrderType type,
            decimal quantity,
            decimal? price = null,
            decimal? stopPrice = null,
            bool? reduceOnly = null,
            TrailingStopDeviationUnit? trailingStopDeviationUnit = null,
            decimal? trailingStopMaxDeviation = null,
            TriggerSignal? triggerSignal = null,
            string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "symbol", symbol },
                { "orderType", EnumConverter.GetString(type) },
                { "side", EnumConverter.GetString(side) },
                { "size", quantity.ToString(CultureInfo.InvariantCulture) }
            };

            parameters.AddOptionalParameter("cliOrdId", clientOrderId);
            parameters.AddOptionalParameter("limitPrice", price?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("reduceOnly", reduceOnly);
            parameters.AddOptionalParameter("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("trailingStopDeviationUnit", EnumConverter.GetString(trailingStopDeviationUnit));
            parameters.AddOptionalParameter("trailingStopMaxDeviation", trailingStopMaxDeviation?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("triggerSignal", EnumConverter.GetString(triggerSignal));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "derivatives/api/v3/sendorder", KrakenExchange.RateLimiter.FuturesApi, 10, true);
            var result = await _baseClient.SendAsync<KrakenFuturesOrderPlaceResult, KrakenFuturesOrderResult>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result;

            var validStates = new [] {
                KrakenFuturesOrderActionStatus.Placed,
                KrakenFuturesOrderActionStatus.PartiallyFilled,
                KrakenFuturesOrderActionStatus.Filled,
                KrakenFuturesOrderActionStatus.Cancelled,
                KrakenFuturesOrderActionStatus.Edited};

            if (!validStates.Contains(result.Data.Status))
            {
                var errCode = EnumConverter.GetString(result.Data.Status);
                return result.AsErrorWithData(new ServerError(errCode, _baseClient.GetErrorInfo(errCode)), result.Data);
            }

            return result;
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesOpenOrder[]>> GetOpenOrdersAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/openorders", KrakenExchange.RateLimiter.FuturesApi, 2, true);
            return await _baseClient.SendAsync<KrakenFuturesOpenOrderResult, KrakenFuturesOpenOrder[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesOrderStatus>> GetOrderAsync(string? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("orderIds", orderId == null ? null : new object[] { orderId });
            parameters.AddOptional("cliOrdIds", clientOrderId == null ? null : new object[] { clientOrderId });
            var request = _definitions.GetOrCreate(HttpMethod.Post, "derivatives/api/v3/orders/status", KrakenExchange.RateLimiter.FuturesApi, 1, true);
            var result = await _baseClient.SendAsync<KrakenFuturesOrderStatusResult, KrakenFuturesOrderStatus[]>(request, parameters, ct).ConfigureAwait(false);
            if (!result)
                return result.As<KrakenFuturesOrderStatus>(default);

            if (result.Data == null)
                return result.AsError<KrakenFuturesOrderStatus>(new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return result.As<KrakenFuturesOrderStatus>(result.Data.Single());
        }

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesOrderStatus[]>> GetOrdersAsync(IEnumerable<string>? orderIds = null, IEnumerable<string>? clientOrderIds = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("orderIds", orderIds?.ToArray());
            parameters.AddOptionalParameter("cliOrdIds", clientOrderIds?.ToArray());
            var request = _definitions.GetOrCreate(HttpMethod.Post, "derivatives/api/v3/orders/status", KrakenExchange.RateLimiter.FuturesApi, 1, true);
            return await _baseClient.SendAsync<KrakenFuturesOrderStatusResult, KrakenFuturesOrderStatus[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesOrderResult>> EditOrderAsync(
            string? orderId = null,
            string? clientOrderId = null,
            decimal? quantity = null,
            decimal? price = null,
            decimal? stopPrice = null,
            TrailingStopDeviationUnit? trailingStopDeviationUnit = null,
            decimal? trailingStopMaxDeviation = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("orderId", orderId);
            parameters.AddOptionalParameter("cliOrdId", clientOrderId);
            parameters.AddOptionalParameter("size", quantity?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limitPrice", price?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("trailingStopDeviationUnit", EnumConverter.GetString(trailingStopDeviationUnit));
            parameters.AddOptionalParameter("trailingStopMaxDeviation", trailingStopMaxDeviation?.ToString(CultureInfo.InvariantCulture));

            var request = _definitions.GetOrCreate(HttpMethod.Post, "derivatives/api/v3/editorder", KrakenExchange.RateLimiter.FuturesApi, 10, true);
            return await _baseClient.SendAsync<KrakenFuturesOrderEditResult, KrakenFuturesOrderResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesOrderResult>> CancelOrderAsync(string? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("order_id", orderId);
            parameters.AddOptionalParameter("cliOrdId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "derivatives/api/v3/cancelorder", KrakenExchange.RateLimiter.FuturesApi, 10, true);
            return await _baseClient.SendAsync<KrakenFuturesOrderCancelResult, KrakenFuturesOrderResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesCancelledOrders>> CancelAllOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "derivatives/api/v3/cancelallorders", KrakenExchange.RateLimiter.FuturesApi, 25, true);
            return await _baseClient.SendAsync<KrakenFuturesCancelledOrdersResult, KrakenFuturesCancelledOrders>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel All Orders After

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesCancelAfter>> CancelAllOrderAfterAsync(TimeSpan cancelAfter, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("timeout", (int)Math.Floor(cancelAfter.TotalSeconds));
            var request = _definitions.GetOrCreate(HttpMethod.Post, "derivatives/api/v3/cancelallordersafter", KrakenExchange.RateLimiter.FuturesApi, 25, true);
            return await _baseClient.SendAsync<KrakenFuturesCancelAfterResult, KrakenFuturesCancelAfter>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Execution Events

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesUserExecutionEvents>> GetExecutionEventsAsync(DateTime? startTime = null, DateTime? endTime = null, string? sort = null, string? tradeable = null, string? continuationToken = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("before", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("since", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("sort", sort);
            parameters.AddOptionalParameter("tradeable", tradeable);
            parameters.AddOptionalParameter("continuationToken", continuationToken);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/history/v3/executions", KrakenExchange.RateLimiter.FuturesApi, 1, true);
            return await _baseClient.SendRawAsync<KrakenFuturesUserExecutionEvents>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
