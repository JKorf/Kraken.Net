using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;

namespace Kraken.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Futures API
    /// </summary>
    public interface IKrakenSocketClientFuturesApi : ISocketApiClient<KrakenCredentials>, IDisposable
    {
        /// <summary>
        /// Get the shared socket subscription client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        IKrakenSocketClientFuturesApiShared SharedClient { get; }

        /// <summary>
        /// Subscribe to account updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/account_log" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/account_log
        /// </para>
        /// </summary>
        /// <param name="snapshotHandler">Handler for the initial snapshot data received when (re)connecting the stream</param>
        /// <param name="updateHandler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAccountLogUpdatesAsync(Action<DataEvent<KrakenFuturesAccountLogsSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesAccountLogsUpdate>> updateHandler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to balance updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/balances" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/balances
        /// </para>
        /// </summary>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<KrakenFuturesBalancesUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to heartbeat updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/heartbeat" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/heartbeat
        /// </para>
        /// </summary>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToHeartbeatUpdatesAsync(Action<DataEvent<KrakenFuturesHeartbeatUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to mini ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/ticker_lite" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/ticker_lite
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenFuturesMiniTickerUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to mini ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/ticker_lite" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/ticker_lite
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesMiniTickerUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to notification updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/notifications" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/notifications
        /// </para>
        /// </summary>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToNotificationUpdatesAsync(Action<DataEvent<KrakenFuturesNotificationUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to open order updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/open_orders" /><br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/open_orders_verbose" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/open_orders<br />
        /// WS /ws/v1/open_orders_verbose
        /// </para>
        /// </summary>
        /// <param name="verbose">Whether to connect to the verbose stream or not. The verbose feed adds extra information about all the post-only orders that failed to cross the book.</param>
        /// <param name="snapshotHandler">Handler for the initial snapshot data received when (re)connecting the stream</param>
        /// <param name="updateHandler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOpenOrdersUpdatesAsync(bool verbose, Action<DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesOpenOrdersUpdate>> updateHandler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to open position updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/open_position" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/open_position
        /// </para>
        /// </summary>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOpenPositionUpdatesAsync(Action<DataEvent<KrakenFuturesOpenPositionUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/book" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/book
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="snapshotHandler">Handler for the initial snapshot data received when (re)connecting the stream</param>
        /// <param name="updateHandler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesBookSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesBookUpdate>> updateHandler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/book" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/book
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="snapshotHandler">Handler for the initial snapshot data received when (re)connecting the stream</param>
        /// <param name="updateHandler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<KrakenFuturesBookSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesBookUpdate>> updateHandler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/ticker" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/ticker
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenFuturesTickerUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/ticker" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/ticker
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesTickerUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to public trade updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/trade" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/trade
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="updateHandler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<KrakenFuturesTradeUpdate[]>> updateHandler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to public trade updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/trade" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/trade
        /// </para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="updateHandler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesTradeUpdate[]>> updateHandler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user trades updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/futures-api/websocket/fills" /><br />
        /// Endpoint:<br />
        /// WS /ws/v1/fills
        /// </para>
        /// </summary>
        /// <param name="handler">Handler for the initial snapshot data received when (re)connecting the stream</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(Action<DataEvent<KrakenFuturesUserTradesUpdate>> handler, CancellationToken ct = default);
    }
}
