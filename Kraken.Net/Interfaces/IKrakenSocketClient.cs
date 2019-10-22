using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Socket;

namespace Kraken.Net.Interfaces
{
    /// <summary>
    /// Interface for the Kraken socket client
    /// </summary>
    public interface IKrakenSocketClient: ISocketClient
    {
        /// <summary>
        /// Subscribe to ticker updates
        /// </summary>
        /// <param name="market">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToTickerUpdates(string market, Action<KrakenSocketEvent<KrakenStreamTick>> handler);

        /// <summary>
        /// Subscribe to ticker updates
        /// </summary>
        /// <param name="market">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string market, Action<KrakenSocketEvent<KrakenStreamTick>> handler);

        /// <summary>
        /// Subscribe to kline updates
        /// </summary>
        /// <param name="market">Symbol to subscribe to</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToKlineUpdates(string market, KlineInterval interval, Action<KrakenSocketEvent<KrakenStreamKline>> handler);

        /// <summary>
        /// Subscribe to kline updates
        /// </summary>
        /// <param name="market">Symbol to subscribe to</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string market, KlineInterval interval, Action<KrakenSocketEvent<KrakenStreamKline>> handler);

        /// <summary>
        /// Subscribe to trade updates
        /// </summary>
        /// <param name="market">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToTradeUpdates(string market, Action<KrakenSocketEvent<IEnumerable<KrakenTrade>>> handler);

        /// <summary>
        /// Subscribe to trade updates
        /// </summary>
        /// <param name="market">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string market, Action<KrakenSocketEvent<IEnumerable<KrakenTrade>>> handler);

        /// <summary>
        /// Subscribe to spread updates
        /// </summary>
        /// <param name="market">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToSpreadUpdates(string market, Action<KrakenSocketEvent<KrakenStreamSpread>> handler);

        /// <summary>
        /// Subscribe to spread updates
        /// </summary>
        /// <param name="market">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(string market, Action<KrakenSocketEvent<KrakenStreamSpread>> handler);

        /// <summary>
        /// Subscribe to depth updates
        /// </summary>
        /// <param name="market">Symbol to subscribe to</param>
        /// <param name="depth">Depth of the initial order book snapshot</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        CallResult<UpdateSubscription> SubscribeToDepthUpdates(string market, int depth, Action<KrakenSocketEvent<KrakenStreamOrderBook>> handler);

        /// <summary>
        /// Subscribe to depth updates
        /// </summary>
        /// <param name="market">Symbol to subscribe to</param>
        /// <param name="depth">Depth of the initial order book snapshot</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToDepthUpdatesAsync(string market, int depth, Action<KrakenSocketEvent<KrakenStreamOrderBook>> handler);
    }
}