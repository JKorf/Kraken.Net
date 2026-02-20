using Kraken.Net.Enums;
using Kraken.Net.Objects.Models.Futures;

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
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/cancel-all-orders-after" /></para>
        /// </summary>
        /// <param name="cancelAfter">Cancel after this time. TimeSpan.Zero to cancel a previous cancel after.</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesCancelAfter>> CancelAllOrderAfterAsync(TimeSpan cancelAfter, CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/cancel-all-orders" /></para>
        /// </summary>
        /// <param name="symbol">Only cancel on this symbol, for example `PF_ETHUSD`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesCancelledOrders>> CancelAllOrdersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel an order
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/cancel-order" /></para>
        /// </summary>
        /// <param name="orderId">Filter by order id</param>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOrderResult>> CancelOrderAsync(string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Edit an existing order
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/edit-order-spring" /></para>
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
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/history/get-execution-events" /></para>
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
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/get-leverage-setting" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesLeverage[]>> GetLeverageAsync(CancellationToken ct = default);

        /// <summary>
        /// Get open orders
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/get-open-orders" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOpenOrder[]>> GetOpenOrdersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get open positions
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/get-open-positions" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesPosition[]>> GetOpenPositionsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get order by id
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/get-order-status" /></para>
        /// </summary>
        /// <param name="orderId">Order id, either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">Client order id, either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOrderStatus>> GetOrderAsync(string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get orders by ids
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/get-order-status" /></para>
        /// </summary>
        /// <param name="orderIds">Order ids list</param>
        /// <param name="clientOrderIds">Client order ids list</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOrderStatus[]>> GetOrdersAsync(IEnumerable<string>? orderIds = null, IEnumerable<string>? clientOrderIds = null, CancellationToken ct = default);

        /// <summary>
        /// Get current self trade strategy
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/get-self-trade-strategy" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<SelfTradeStrategy>> GetSelfTradeStrategyAsync(CancellationToken ct = default);

        /// <summary>
        /// Get list of user trades
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/get-fills" /></para>
        /// </summary>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesUserTrade[]>> GetUserTradesAsync(DateTime? startTime = null, CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/send-order" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `PF_ETHUSD`</param>
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
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/set-leverage-setting" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `PF_ETHUSD`</param>
        /// <param name="maxLeverage">Max leverage</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> SetLeverageAsync(string symbol, decimal maxLeverage, CancellationToken ct = default);

        /// <summary>
        /// Set self trading strategy
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/set-self-trade-strategy" /></para>
        /// </summary>
        /// <param name="strategy">The strategy</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<SelfTradeStrategy>> SetSelfTradeStrategyAsync(SelfTradeStrategy strategy, CancellationToken ct = default);

        /// <summary>
        /// Get order history
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/history/get-order-events" /></para>
        /// </summary>
        /// <param name="open">Return open order events</param>
        /// <param name="close">Return close order events</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="ascending">Ascending order</param>
        /// <param name="continuationToken">Continuation token from previous request</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenOrderHistory[]>> GetOrderHistoryAsync(
            bool? open = null,
            bool? close = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            bool? ascending = null,
            string? continuationToken = null,
            int? limit = null,
            CancellationToken ct = default);
    }
}
