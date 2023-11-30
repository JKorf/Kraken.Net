using CryptoExchange.Net.Objects;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models.Futures;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kraken.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Kraken futures trading endpoints, placing and mananging orders.
    /// </summary>
    public interface IKrakenRestClientFuturesApiTrading
    {
        /// <summary>
        /// This endpoint provides a Dead Man's Switch mechanism to protect the user from network malfunctions. The user can send a request with a timeout in seconds which will trigger a countdown timer that will cancel all user orders when timeout expires. The user has to keep sending request to push back the timeout expiration or they can deactivate the mechanism by specifying a timeout of zero (0).
        /// The recommended mechanism usage is making a call every 15 to 20 seconds and provide a timeout of 60 seconds.This allows the user to keep the orders in place on a brief network failure, while keeping them safe in case of a network breakdown.
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-cancel-all-orders" /></para>
        /// </summary>
        /// <param name="cancelAfter">Cancel after this time. TimeSpan.Zero to cancel a previous cancel after.</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesCancelAfter>> CancelAllOrderAfterAsync(TimeSpan cancelAfter, CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-cancel-all-orders" /></para>
        /// </summary>
        /// <param name="symbol">Only cancel on this symbol</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesCancelledOrders>> CancelAllOrdersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel an order
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-cancel-order" /></para>
        /// </summary>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOrderResult>> CancelOrderAsync(string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Edit an existing order
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-edit-order" /></para>
        /// </summary>
        /// <param name="orderId">Order id of order to edit</param>
        /// <param name="clientOrderId">Client order id of order to edit</param>
        /// <param name="quantity">New quantity</param>
        /// <param name="price">New price</param>
        /// <param name="stopPrice">New stop price</param>
        /// <param name="trailingStopDeviationUnit">New trailing stop deviation unit</param>
        /// <param name="trailingStopMaxDeviation">New trailing stop max deviation</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOrderResult>> EditOrderAsync(string? orderId = null, string? clientOrderId = null, decimal? quantity = null, decimal? price = null, decimal? stopPrice = null, TrailingStopDeviationUnit? trailingStopDeviationUnit = null, decimal? trailingStopMaxDeviation = null, CancellationToken ct = default);

        /// <summary>
        /// Get execution events
        /// <para><a href="https://docs.futures.kraken.com/#http-api-history-account-history-get-execution-events" /></para>
        /// </summary>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="sort">Sort asc or desc</param>
        /// <param name="tradeable">If present events of other tradeables are filtered out.	</param>
        /// <param name="continuationToken">Opaque token from the Next-Continuation-Token header used to continue listing events. The sort parameter must be the same as in the previous request to continue listing in the same direction.</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesUserExecutionEvents>> GetExecutionEventsAsync(DateTime? startTime = null, DateTime? endTime = null, string? sort = null, string? tradeable = null, string? continuationToken = null, CancellationToken ct = default);

        /// <summary>
        /// Get the leverage settings
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-multi-collateral-get-the-leverage-setting-for-a-market" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenFuturesLeverage>>> GetLeverageAsync(CancellationToken ct = default);

        /// <summary>
        /// Get open orders
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-get-open-orders" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenFuturesOpenOrder>>> GetOpenOrdersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get open positions
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-account-information-get-open-positions" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenFuturesPosition>>> GetOpenPositionsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get orders by ids
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-get-the-current-status-for-specific-orders" /></para>
        /// </summary>
        /// <param name="orderIds">Order ids list</param>
        /// <param name="clientOrderIds">Client order ids list</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenFuturesOrderStatus>>> GetOrdersAsync(IEnumerable<string>? orderIds = null, IEnumerable<string>? clientOrderIds = null, CancellationToken ct = default);

        /// <summary>
        /// Get current self trade strategy
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-trading-settings-get-current-self-trade-strategy" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<SelfTradeStrategy>> GetSelfTradeStrategyAsync(CancellationToken ct = default);

        /// <summary>
        /// Get list of user trades
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-historical-data-get-your-fills" /></para>
        /// </summary>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenFuturesUserTrade>>> GetUserTradesAsync(DateTime? startTime = null, CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-send-order" /></para>
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="side">Order side</param>
        /// <param name="type">Order type</param>
        /// <param name="quantity">Order quantity</param>
        /// <param name="price">Limit price</param>
        /// <param name="stopPrice">Stop price</param>
        /// <param name="reduceOnly">Is reduce only</param>
        /// <param name="trailingStopDeviationUnit">Trailing stop deviation unit</param>
        /// <param name="trailingStopMaxDeviation">Trailing stop max deviation</param>
        /// <param name="triggerSignal">Trigger signal</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOrderResult>> PlaceOrderAsync(string symbol, OrderSide side, FuturesOrderType type, decimal quantity, decimal? price = null, decimal? stopPrice = null, bool? reduceOnly = null, TrailingStopDeviationUnit? trailingStopDeviationUnit = null, decimal? trailingStopMaxDeviation = null, TriggerSignal? triggerSignal = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Set max leverage for a symbol
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-multi-collateral-set-the-leverage-setting-for-a-market" /></para>
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="maxLeverage">Max leverage</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> SetLeverageAsync(string symbol, decimal maxLeverage, CancellationToken ct = default);

        /// <summary>
        /// Set self trading strategy
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-trading-settings-update-self-trade-strategy" /></para>
        /// </summary>
        /// <param name="strategy">The strategy</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<SelfTradeStrategy>> SetSelfTradeStrategyAsync(SelfTradeStrategy strategy, CancellationToken ct = default);
    }
}