using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Converters;
using Kraken.Net.Interfaces;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Socket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kraken.Net
{
    /// <summary>
    /// Client for the Kraken websocket API
    /// </summary>
    public class KrakenSocketClient: SocketClient, IKrakenSocketClient
    {
        #region fields
        private static KrakenSocketClientOptions defaultOptions = new KrakenSocketClientOptions();
        private static KrakenSocketClientOptions DefaultOptions => defaultOptions.Copy<KrakenSocketClientOptions>();
        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of KrakenSocketClient using the default options
        /// </summary>
        public KrakenSocketClient() : this(DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of KrakenSocketClient using provided options
        /// </summary>
        /// <param name="options">The options to use for this client</param>
        public KrakenSocketClient(KrakenSocketClientOptions options) : base("Kraken", options, options.ApiCredentials == null ? null : new KrakenAuthenticationProvider(options.ApiCredentials))
        {
            AddGenericHandler("Connection", (connection, token) => { });
            AddGenericHandler("HeartBeat", (connection, token) => { });
        }
        #endregion

        #region methods
        /// <summary>
        /// Set the default options to be used when creating new socket clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(KrakenSocketClientOptions options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Subscribe to ticker updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToTickerUpdates(string symbol, Action<KrakenSocketEvent<KrakenStreamTick>> handler) => SubscribeToTickerUpdatesAsync(symbol, handler).Result;
        /// <summary>
        /// Subscribe to ticker updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<KrakenSocketEvent<KrakenStreamTick>> handler)
        {
            symbol.ValidateKrakenWebsocketSymbol();

            return await Subscribe(new KrakenSubscribeRequest("ticker", NextId(), symbol), null, false, handler).ConfigureAwait(false);
        }

        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string[] symbols, Action<KrakenSocketEvent<KrakenStreamTick>> handler)
        {
            foreach(var symbol in symbols)
                symbol.ValidateKrakenWebsocketSymbol();

            return await Subscribe(new KrakenSubscribeRequest("ticker", NextId(), symbols), null, false, handler).ConfigureAwait(false);
        }


        /// <summary>
        /// Subscribe to kline updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToKlineUpdates(string symbol, KlineInterval interval, Action<KrakenSocketEvent<KrakenStreamKline>> handler) => SubscribeToKlineUpdatesAsync(symbol, interval, handler).Result;
        /// <summary>
        /// Subscribe to kline updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<KrakenSocketEvent<KrakenStreamKline>> handler)
        {
            symbol.ValidateKrakenWebsocketSymbol();

            var intervalMinutes = int.Parse(JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)));
            return await Subscribe(new KrakenSubscribeRequest("ohlc", NextId(), symbol) { Details = new KrakenOHLCSubscriptionDetails(intervalMinutes) }, null, false, handler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to trade updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToTradeUpdates(string symbol, Action<KrakenSocketEvent<IEnumerable<KrakenTrade>>> handler) => SubscribeToTradeUpdatesAsync(symbol, handler).Result;
        /// <summary>
        /// Subscribe to trade updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<KrakenSocketEvent<IEnumerable<KrakenTrade>>> handler)
        {
            symbol.ValidateKrakenWebsocketSymbol();

            return await Subscribe(new KrakenSubscribeRequest("trade", NextId(), symbol), null, false, handler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to spread updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToSpreadUpdates(string symbol, Action<KrakenSocketEvent<KrakenStreamSpread>> handler) => SubscribeToSpreadUpdatesAsync(symbol, handler).Result;
        /// <summary>
        /// Subscribe to spread updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(string symbol, Action<KrakenSocketEvent<KrakenStreamSpread>> handler)
        {
            symbol.ValidateKrakenWebsocketSymbol();

            return await Subscribe(new KrakenSubscribeRequest("spread", NextId(), symbol), null, false, handler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to depth updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="depth">Depth of the initial order book snapshot</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public CallResult<UpdateSubscription> SubscribeToDepthUpdates(string symbol, int depth, Action<KrakenSocketEvent<KrakenStreamOrderBook>> handler) => SubscribeToDepthUpdatesAsync(symbol, depth, handler).Result;

        /// <summary>
        /// Subscribe to depth updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="depth">Depth of the initial order book snapshot</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToDepthUpdatesAsync(string symbol, int depth, Action<KrakenSocketEvent<KrakenStreamOrderBook>> handler)
        {
            symbol.ValidateKrakenWebsocketSymbol();

            var innerHandler = new Action<string>(data =>
            {
                var token = data.ToJToken(log);
                if (token == null || token.Type != JTokenType.Array)
                {
                    log.Write(LogVerbosity.Warning, "Failed to deserialize stream order book");
                    return;
                }
                handler(StreamOrderBookConverter.Convert((JArray) token));
            });

            return await Subscribe(new KrakenSubscribeRequest("book", NextId(), symbol) { Details = new KrakenDepthSubscriptionDetails(depth)}, null, false, innerHandler).ConfigureAwait(false);
        }
        #endregion

        /// <inheritdoc />
        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object>? callResult)
        {
            callResult = null;
            if (message.Type != JTokenType.Object)
                return false;

            if (message["reqid"] == null)
                return false;

            var requestId = (int) message["reqid"];
            var kRequest = (KrakenSubscribeRequest) request;
            if (requestId != kRequest.RequestId)
                return false;
            
            var response = message.ToObject<KrakenSubscriptionEvent>();
            kRequest.ChannelId = response.ChannelId;
            callResult = new CallResult<object>(response, response.Status == "subscribed" ? null: new ServerError(response.ErrorMessage ?? "-"));
            return true;
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(JToken message, object request)
        {
            if (message.Type != JTokenType.Array)
                return false;

            var kRequest = (KrakenSubscribeRequest) request;
            var arr = (JArray) message;
            if (!int.TryParse(arr[0].ToString(), out var channelId))
                return false;

            return kRequest.ChannelId == channelId;
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(JToken message, string identifier)
        {
            if (message.Type != JTokenType.Object)
                return false;

            if (identifier == "HeartBeat" && message["event"] != null && (string)message["event"] == "heartbeat")
                return true;

            if (identifier == "Connection" && message["event"] != null && (string)message["event"] == "systemStatus")
                return true;

            return false;
        }

        /// <inheritdoc />
        protected override Task<CallResult<bool>> AuthenticateSocket(SocketConnection s)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override async Task<bool> Unsubscribe(SocketConnection connection, SocketSubscription subscription)
        {
            var channelId = ((KrakenSubscribeRequest)subscription.Request!).ChannelId;
            if (!channelId.HasValue)
                return true; // No channel id assigned, nothing to unsub

            var unsub = new KrakenUnsubscribeRequest(NextId(), channelId.Value);
            var result = false;
            await connection.SendAndWait(unsub, ResponseTimeout, data =>
            {
                if (data.Type != JTokenType.Object)
                    return false;

                if (data["reqid"] == null)
                    return false;

                var requestId = (int)data["reqid"];
                if (requestId != unsub.RequestId)
                    return false;

                var response = data.ToObject<KrakenSubscriptionEvent>();
                result = response.Status == "unsubscribed";
                return true;
            }).ConfigureAwait(false);
            return result;
        }
    }
}
