using CryptoExchange.Net.Objects;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models.Futures;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kraken.Net.Interfaces.Clients.FuturesApi
{
    public interface IKrakenRestClientFuturesApiTrading
    {
        Task<WebCallResult<KrakenFuturesCancelAfter>> CancelAllOrderAfterAsync(TimeSpan cancelAfter, CancellationToken ct = default);
        Task<WebCallResult<KrakenFuturesCancelledOrders>> CancelAllOrdersAsync(string? symbol = null, CancellationToken ct = default);
        Task<WebCallResult<KrakenFuturesOrderResult>> CancelOrderAsync(string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);
        Task<WebCallResult<KrakenFuturesOrderResult>> EditOrderAsync(string? orderId = null, string? clientOrderId = null, int? quantity = null, decimal? price = null, decimal? stopPrice = null, TrailingStopDeviationUnit? trailingStopDeviationUnit = null, decimal? trailingStopMaxDeviation = null, CancellationToken ct = default);
        Task<WebCallResult<KrakenFuturesUserExecutionEvents>> GetExecutionEventsAsync(DateTime? startTime = null, DateTime? endTime = null, string? sort = null, string? tradeable = null, string? continuationToken = null, CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<KrakenFuturesLeverage>>> GetLeverageAsync(CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<KrakenFuturesOpenOrder>>> GetOpenOrdersAsync(CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<KrakenFuturesPosition>>> GetOpenPositionsAsync(CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<KrakenFuturesOrderStatus>>> GetOrdersAsync(IEnumerable<string>? orderIds = null, IEnumerable<string>? clientOrderIds = null, CancellationToken ct = default);
        Task<WebCallResult<SelfTradeStrategy>> GetSelfTradeStrategyAsync(CancellationToken ct = default);
        Task<WebCallResult<IEnumerable<KrakenFuturesUserTrade>>> GetUserTradesAsync(DateTime? startTime = null, CancellationToken ct = default);
        Task<WebCallResult<KrakenFuturesOrderResult>> PlaceOrderAsync(string symbol, OrderSide side, FuturesOrderType type, int quantity, decimal? price = null, decimal? stopPrice = null, bool? reduceOnly = null, TrailingStopDeviationUnit? trailingStopDeviationUnit = null, decimal? trailingStopMaxDeviation = null, TriggerSignal? triggerSignal = null, string? clientOrderId = null, CancellationToken ct = default);
        Task<WebCallResult> SetLeverageAsync(string symbol, decimal maxLeverage, CancellationToken ct = default);
        Task<WebCallResult<SelfTradeStrategy>> SetSelfTradeStrategyAsync(SelfTradeStrategy strategy, CancellationToken ct = default);
    }
}