using Kraken.Net.Objects.Models.Futures;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using Kraken.Net.Enums;
using System.Globalization;
using System.Linq;
using Kraken.Net.Interfaces.Clients.FuturesApi;

namespace Kraken.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    public class KrakenRestClientFuturesApiTrading : IKrakenRestClientFuturesApiTrading
    {
        private readonly KrakenRestClientFuturesApi _baseClient;

        internal KrakenRestClientFuturesApiTrading(KrakenRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesUserTrade>>> GetUserTradesAsync(DateTime? startTime = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("lastFillTime", startTime?.ToString("u").Replace(" ", "T"));
            return await _baseClient.Execute<KrakenFuturesUserTradeResult, IEnumerable<KrakenFuturesUserTrade>>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/fills")), HttpMethod.Get, ct, parameters, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<SelfTradeStrategy>> GetSelfTradeStrategyAsync(CancellationToken ct = default)
        {
            return await _baseClient.Execute<KrakenFuturesSelfTradeResult, SelfTradeStrategy>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/self-trade-strategy")), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<SelfTradeStrategy>> SetSelfTradeStrategyAsync(SelfTradeStrategy strategy, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "strategy", EnumConverter.GetString(strategy) }
            };
            return await _baseClient.Execute<KrakenFuturesSelfTradeResult, SelfTradeStrategy>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/self-trade-strategy")), HttpMethod.Put, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesPosition>>> GetOpenPositionsAsync(CancellationToken ct = default)
        {
            return await _baseClient.Execute<KrakenFuturesPositionResult, IEnumerable<KrakenFuturesPosition>>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/openpositions")), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesLeverage>>> GetLeverageAsync(CancellationToken ct = default)
        {
            return await _baseClient.Execute<KrakenFuturesLeverageResult, IEnumerable<KrakenFuturesLeverage>>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/leveragepreferences")), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult> SetLeverageAsync(string symbol, decimal maxLeverage, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol },
                { "maxLeverage", maxLeverage.ToString(CultureInfo.InvariantCulture) }
            };
            return await _baseClient.Execute(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/leveragepreferences")), HttpMethod.Put, ct, parameters, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesOrderResult>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            FuturesOrderType type,
            int quantity,
            decimal? price = null,
            decimal? stopPrice = null,
            bool? reduceOnly = null,
            TrailingStopDeviationUnit? trailingStopDeviationUnit = null,
            decimal? trailingStopMaxDeviation = null,
            TriggerSignal? triggerSignal = null,
            string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>()
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

            return await _baseClient.Execute<KrakenFuturesOrderPlaceResult, KrakenFuturesOrderResult>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/sendorder")), HttpMethod.Post, ct, parameters, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesOpenOrder>>> GetOpenOrdersAsync(CancellationToken ct = default)
        {
            return await _baseClient.Execute<KrakenFuturesOpenOrderResult, IEnumerable<KrakenFuturesOpenOrder>>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/openorders")), HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenFuturesOrderStatus>>> GetOrdersAsync(IEnumerable<string>? orderIds = null, IEnumerable<string>? clientOrderIds = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("orderIds", orderIds?.ToArray());
            parameters.AddOptionalParameter("cliOrdIds", clientOrderIds?.ToArray());
            return await _baseClient.Execute<KrakenFuturesOrderStatusResult, IEnumerable<KrakenFuturesOrderStatus>>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/orders/status")), HttpMethod.Post, ct, parameters, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesOrderResult>> EditOrderAsync(
            string? orderId = null,
            string? clientOrderId = null,
            int? quantity = null,
            decimal? price = null,
            decimal? stopPrice = null,
            TrailingStopDeviationUnit? trailingStopDeviationUnit = null,
            decimal? trailingStopMaxDeviation = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("orderId", orderId);
            parameters.AddOptionalParameter("cliOrdId", clientOrderId);
            parameters.AddOptionalParameter("size", quantity?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("limitPrice", price?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("trailingStopDeviationUnit", EnumConverter.GetString(trailingStopDeviationUnit));
            parameters.AddOptionalParameter("trailingStopMaxDeviation", trailingStopMaxDeviation?.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.Execute<KrakenFuturesOrderEditResult, KrakenFuturesOrderResult>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/editorder")), HttpMethod.Post, ct, parameters, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesOrderResult>> CancelOrderAsync(string? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("order_id", orderId);
            parameters.AddOptionalParameter("cliOrdId", clientOrderId);
            return await _baseClient.Execute<KrakenFuturesOrderCancelResult, KrakenFuturesOrderResult>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/cancelorder")), HttpMethod.Post, ct, parameters, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesCancelledOrders>> CancelAllOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("symbol", symbol);
            return await _baseClient.Execute<KrakenFuturesCancelledOrdersResult, KrakenFuturesCancelledOrders>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/cancelallorders")), HttpMethod.Post, ct, parameters, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesCancelAfter>> CancelAllOrderAfterAsync(TimeSpan cancelAfter, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("timeout", (int)Math.Floor(cancelAfter.TotalSeconds));
            return await _baseClient.Execute<KrakenFuturesCancelAfterResult, KrakenFuturesCancelAfter>(new Uri(_baseClient.BaseAddress.AppendPath("derivatives/api/v3/cancelallordersafter")), HttpMethod.Post, ct, parameters, signed: true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesUserExecutionEvents>> GetExecutionEventsAsync(DateTime? startTime = null, DateTime? endTime = null, string? sort = null, string? tradeable = null, string? continuationToken = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("before", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("since", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("sort", sort);
            parameters.AddOptionalParameter("tradeable", tradeable);
            parameters.AddOptionalParameter("continuationToken", continuationToken);

            return await _baseClient.ExecuteBase<KrakenFuturesUserExecutionEvents>(new Uri(_baseClient.BaseAddress.AppendPath("api/history/v3/executions")), HttpMethod.Get, ct, parameters, signed: true).ConfigureAwait(false);
        }
    }
}
