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
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/cancel-all-orders-after" /><br />
        /// Endpoint:<br />
        /// POST /derivatives/api/v3/cancelallordersafter
        /// </para>
        /// </summary>
        /// <param name="cancelAfter">["<c>timeout</c>"] Cancel after this time. TimeSpan.Zero to cancel a previous cancel after.</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesCancelAfter>> CancelAllOrderAfterAsync(TimeSpan cancelAfter, CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/cancel-all-orders" /><br />
        /// Endpoint:<br />
        /// POST /derivatives/api/v3/cancelallorders
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Only cancel on this symbol, for example `PF_ETHUSD`</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesCancelledOrders>> CancelAllOrdersAsync(string? symbol = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel an order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/cancel-order" /><br />
        /// Endpoint:<br />
        /// POST /derivatives/api/v3/cancelorder
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>order_id</c>"] Filter by order id</param>
        /// <param name="clientOrderId">["<c>cliOrdId</c>"] Filter by client order id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOrderResult>> CancelOrderAsync(string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Edit an existing order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/edit-order-spring" /><br />
        /// Endpoint:<br />
        /// POST /derivatives/api/v3/editorder
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>orderId</c>"] Order id of order to edit</param>
        /// <param name="clientOrderId">["<c>cliOrdId</c>"] Client order id of order to edit</param>
        /// <param name="quantity">["<c>size</c>"] New quantity</param>
        /// <param name="price">["<c>limitPrice</c>"] New price</param>
        /// <param name="stopPrice">["<c>stopPrice</c>"] New stop price</param>
        /// <param name="trailingStopDeviationUnit">["<c>trailingStopDeviationUnit</c>"] New trailing stop deviation unit</param>
        /// <param name="trailingStopMaxDeviation">["<c>trailingStopMaxDeviation</c>"] New trailing stop max deviation</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOrderResult>> EditOrderAsync(string? orderId = null, string? clientOrderId = null, decimal? quantity = null, decimal? price = null, decimal? stopPrice = null, TrailingStopDeviationUnit? trailingStopDeviationUnit = null, decimal? trailingStopMaxDeviation = null, CancellationToken ct = default);

        /// <summary>
        /// Get execution events
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/history/get-execution-events" /><br />
        /// Endpoint:<br />
        /// GET /api/history/v3/executions
        /// </para>
        /// </summary>
        /// <param name="startTime">["<c>since</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>before</c>"] Filter by end time</param>
        /// <param name="sort">["<c>sort</c>"] Sort asc or desc</param>
        /// <param name="tradeable">["<c>tradeable</c>"] If present events of other tradeables are filtered out.	</param>
        /// <param name="continuationToken">["<c>continuationToken</c>"] Opaque token from the Next-Continuation-Token header used to continue listing events. The sort parameter must be the same as in the previous request to continue listing in the same direction.</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesUserExecutionEvents>> GetExecutionEventsAsync(DateTime? startTime = null, DateTime? endTime = null, string? sort = null, string? tradeable = null, string? continuationToken = null, CancellationToken ct = default);

        /// <summary>
        /// Get the leverage settings
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-leverage-setting" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v3/leveragepreferences
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesLeverage[]>> GetLeverageAsync(CancellationToken ct = default);

        /// <summary>
        /// Get open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-open-orders" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v3/openorders
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOpenOrder[]>> GetOpenOrdersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get open positions
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-open-positions" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v3/openpositions
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesPosition[]>> GetOpenPositionsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get order by id
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-order-status" /><br />
        /// Endpoint:<br />
        /// POST /derivatives/api/v3/orders/status
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>orderIds</c>"] Order id, either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">["<c>cliOrdIds</c>"] Client order id, either this or orderId should be provided</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOrderStatus>> GetOrderAsync(string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Get orders by ids
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-order-status" /><br />
        /// Endpoint:<br />
        /// POST /derivatives/api/v3/orders/status
        /// </para>
        /// </summary>
        /// <param name="orderIds">["<c>orderIds</c>"] Order ids list</param>
        /// <param name="clientOrderIds">["<c>cliOrdIds</c>"] Client order ids list</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOrderStatus[]>> GetOrdersAsync(IEnumerable<string>? orderIds = null, IEnumerable<string>? clientOrderIds = null, CancellationToken ct = default);

        /// <summary>
        /// Get current self trade strategy
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-self-trade-strategy" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v3/self-trade-strategy
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<SelfTradeStrategy>> GetSelfTradeStrategyAsync(CancellationToken ct = default);

        /// <summary>
        /// Get list of user trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/get-fills" /><br />
        /// Endpoint:<br />
        /// GET /derivatives/api/v3/fills
        /// </para>
        /// </summary>
        /// <param name="startTime">["<c>lastFillTime</c>"] Filter by start time</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesUserTrade[]>> GetUserTradesAsync(DateTime? startTime = null, CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/send-order" /><br />
        /// Endpoint:<br />
        /// POST /derivatives/api/v3/sendorder
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `PF_ETHUSD`</param>
        /// <param name="side">["<c>side</c>"] Order side</param>
        /// <param name="type">["<c>orderType</c>"] Order type</param>
        /// <param name="quantity">["<c>size</c>"] Order quantity</param>
        /// <param name="price">["<c>limitPrice</c>"] Limit price</param>
        /// <param name="stopPrice">["<c>stopPrice</c>"] Stop price</param>
        /// <param name="reduceOnly">["<c>reduceOnly</c>"] Is reduce only</param>
        /// <param name="trailingStopDeviationUnit">["<c>trailingStopDeviationUnit</c>"] Trailing stop deviation unit</param>
        /// <param name="trailingStopMaxDeviation">["<c>trailingStopMaxDeviation</c>"] Trailing stop max deviation</param>
        /// <param name="triggerSignal">["<c>triggerSignal</c>"] Trigger signal</param>
        /// <param name="clientOrderId">["<c>cliOrdId</c>"] Client order id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesOrderResult>> PlaceOrderAsync(string symbol, OrderSide side, FuturesOrderType type, decimal quantity, decimal? price = null, decimal? stopPrice = null, bool? reduceOnly = null, TrailingStopDeviationUnit? trailingStopDeviationUnit = null, decimal? trailingStopMaxDeviation = null, TriggerSignal? triggerSignal = null, string? clientOrderId = null, CancellationToken ct = default);

        /// <summary>
        /// Set max leverage for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/set-leverage-setting" /><br />
        /// Endpoint:<br />
        /// PUT /derivatives/api/v3/leveragepreferences
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `PF_ETHUSD`</param>
        /// <param name="maxLeverage">["<c>maxLeverage</c>"] Max leverage</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> SetLeverageAsync(string symbol, decimal maxLeverage, CancellationToken ct = default);

        /// <summary>
        /// Set self trading strategy
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/trading/set-self-trade-strategy" /><br />
        /// Endpoint:<br />
        /// PUT /derivatives/api/v3/self-trade-strategy
        /// </para>
        /// </summary>
        /// <param name="strategy">["<c>strategy</c>"] The strategy</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<SelfTradeStrategy>> SetSelfTradeStrategyAsync(SelfTradeStrategy strategy, CancellationToken ct = default);

        /// <summary>
        /// Get order history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/history/get-order-events" /><br />
        /// Endpoint:<br />
        /// GET /api/history/v3/orders
        /// </para>
        /// </summary>
        /// <param name="open">["<c>opened</c>"] Return open order events</param>
        /// <param name="close">["<c>closed</c>"] Return close order events</param>
        /// <param name="startTime">["<c>since</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>before</c>"] Filter by end time</param>
        /// <param name="ascending">["<c>sort</c>"] Ascending order</param>
        /// <param name="continuationToken">["<c>continuation_token</c>"] Continuation token from previous request</param>
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
