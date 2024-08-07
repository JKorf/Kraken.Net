using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kraken.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Futures API
    /// </summary>
    public interface IKrakenSocketClientFuturesApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Subscribe to account updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-private-feeds-account-log" /></para>
        /// </summary>
        /// <param name="snapshotHandler">Handler for the initial snapshot data received when (re)connecting the stream</param>
        /// <param name="updateHandler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAccountLogUpdatesAsync(Action<DataEvent<KrakenFuturesAccountLogsSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesAccountLogsUpdate>> updateHandler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to balance updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-private-feeds-balances" /></para>
        /// </summary>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<KrakenFuturesBalancesUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to heartbeat updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-public-feeds-heartbeat" /></para>
        /// </summary>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToHeartbeatUpdatesAsync(Action<DataEvent<KrakenFuturesHeartbeatUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to mini ticker updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-public-feeds-ticker-lite" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenFuturesMiniTickerUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to mini ticker updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-public-feeds-ticker-lite" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesMiniTickerUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to notification updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-private-feeds-notifications" /></para>
        /// </summary>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToNotificationUpdatesAsync(Action<DataEvent<KrakenFuturesNotificationUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to open order updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-private-feeds-open-orders" /></para>
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-private-feeds-open-orders-verbose" /></para>
        /// </summary>
        /// <param name="verbose">Whether to connect to the verbose stream or not. The verbose feed adds extra information about all the post-only orders that failed to cross the book.</param>
        /// <param name="snapshotHandler">Handler for the initial snapshot data received when (re)connecting the stream</param>
        /// <param name="updateHandler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOpenOrdersUpdatesAsync(bool verbose, Action<DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesOpenOrdersUpdate>> updateHandler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to open position updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-private-feeds-open-positions" /></para>
        /// </summary>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOpenPositionUpdatesAsync(Action<DataEvent<KrakenFuturesOpenPositionUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-public-feeds-book" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="snapshotHandler">Handler for the initial snapshot data received when (re)connecting the stream</param>
        /// <param name="updateHandler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesBookSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesBookUpdate>> updateHandler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-public-feeds-book" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="snapshotHandler">Handler for the initial snapshot data received when (re)connecting the stream</param>
        /// <param name="updateHandler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, Action<DataEvent<KrakenFuturesBookSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesBookUpdate>> updateHandler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to ticker updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-public-feeds-ticker" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenFuturesTickerUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to ticker updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-public-feeds-ticker" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="handler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesTickerUpdate>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to public trade updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-public-feeds-trade" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="updateHandler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<IEnumerable<KrakenFuturesTradeUpdate>>> updateHandler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to public trade updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-public-feeds-trade" /></para>
        /// </summary>
        /// <param name="symbols">The symbols to subscribe, for example `PF_ETHUSD`</param>
        /// <param name="updateHandler">Update handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IEnumerable<KrakenFuturesTradeUpdate>>> updateHandler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user trades updates
        /// <para><a href="https://docs.futures.kraken.com/#websocket-api-private-feeds-fills" /></para>
        /// </summary>
        /// <param name="handler">Handler for the initial snapshot data received when (re)connecting the stream</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(Action<DataEvent<KrakenFuturesUserTradesUpdate>> handler, CancellationToken ct = default);
    }
}