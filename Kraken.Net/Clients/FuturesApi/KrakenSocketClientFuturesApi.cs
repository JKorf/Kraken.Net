using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class KrakenSocketClientFuturesApi : SocketApiClient
    {
        #region fields                
        /// <inheritdoc />
        public new KrakenSocketOptions ClientOptions => (KrakenSocketOptions)base.ClientOptions;

        private readonly Dictionary<SocketConnection, (string, string)> _challenges = new Dictionary<SocketConnection, (string, string)>();
        #endregion

        #region ctor

        internal KrakenSocketClientFuturesApi(ILogger logger, KrakenSocketOptions options) :
            base(logger, options.Environment.FuturesSocketBaseAddress, options, options.FuturesOptions)
        {
            AddGenericHandler("Info", (messageEvent) => { });
            //AddGenericHandler("SystemStatus", (messageEvent) => { });
            AddGenericHandler("AdditionalSubResponses", (messageEvent) => { });
        }
        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new KrakenFuturesAuthenticationProvider(credentials, ClientOptions.NonceProvider ?? new KrakenNonceProvider());

        #region methods

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToHeartbeatUpdatesAsync(Action<DataEvent<KrakenFuturesHeartbeat>> handler, CancellationToken ct = default)
        {
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), new KrakenFuturesSubscribeMessage
            {
                Event = "subscribe",
                Feed = "heartbeat"
            }, null, false, handler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesTicker>> handler, CancellationToken ct = default)
        {
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), new KrakenFuturesSubscribeMessage
            {
                Event = "subscribe",
                Feed = "ticker",
                Symbols = symbols.ToList()
            }, null, false, handler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
            IEnumerable<string> symbols, 
            Action<DataEvent<KrakenFuturesSnapshotTrades>> snapshotHandler, 
            Action<DataEvent<KrakenFuturesTrade>> updateHandler,
            CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<JToken>>(data => HandleSnapshotUpdate(data, snapshotHandler, updateHandler));
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), new KrakenFuturesSubscribeMessage
            {
                Event = "subscribe",
                Feed = "trade",
                Symbols = symbols.ToList()
            }, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesMiniTicker>> handler, CancellationToken ct = default)
        {
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), new KrakenFuturesSubscribeMessage
            {
                Event = "subscribe",
                Feed = "ticker_lite",
                Symbols = symbols.ToList()
            }, null, false, handler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
            IEnumerable<string> symbols,
            Action<DataEvent<KrakenFuturesSnapshotBook>> snapshotHandler,
            Action<DataEvent<KrakenFuturesBookUpdate>> updateHandler,
            CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<JToken>>(data => HandleSnapshotUpdate(data, snapshotHandler, updateHandler));
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), new KrakenFuturesSubscribeMessage
            {
                Event = "subscribe",
                Feed = "book",
                Symbols = symbols.ToList()
            }, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOpenOrdersUpdatesAsync(
            Action<DataEvent<KrakenFuturesSnapshotOpenOrders>> snapshotHandler,
            Action<DataEvent<KrakenFuturesUpdateOpenOrders>> updateHandler,
            CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<JToken>>(data => HandleSnapshotUpdate(data, snapshotHandler, updateHandler));
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), new KrakenFuturesSubscribeAuthMessage
            {
                Event = "subscribe",
                Feed = "open_orders"
            }, null, true, internalHandler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(
            Action<DataEvent<KrakenFuturesBalances>> handler,
            CancellationToken ct = default)
        {
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), new KrakenFuturesSubscribeAuthMessage
            {
                Event = "subscribe",
                Feed = "balances"
            }, null, true, handler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(
            Action<DataEvent<KrakenFuturesUserTradesUpdate>> snapshotHandler,
            Action<DataEvent<KrakenFuturesUserTradesUpdate>> updateHandler,
            CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<JToken>>(data => HandleSnapshotUpdate(data, snapshotHandler, updateHandler));
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), new KrakenFuturesSubscribeAuthMessage
            {
                Event = "subscribe",
                Feed = "fills"
            }, null, true, internalHandler, ct).ConfigureAwait(false);
        }

        // TODO ================
        // Account log
        // Open positions
        // Open orders (verbose)
        // Test with actual trades
        // Interfaces
        // url doc

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToNotificationUpdatesAsync(
            Action<DataEvent<KrakenFuturesNotificationUpdate>> handler,
            CancellationToken ct = default)
        {
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), new KrakenFuturesSubscribeAuthMessage
            {
                Event = "subscribe",
                Feed = "notifications_auth"
            }, null, true, handler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
            => throw new NotImplementedException();

        /// <inheritdoc />
        protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object>? callResult)
        {
            callResult = null;
            if (message.Type != JTokenType.Object)
                return false;

            var evnt = message["event"]?.ToString();
            if (evnt == null || (evnt != "subscribed" && evnt != "error"))
                return false;

            var kRequest = (KrakenFuturesSubscribeMessage)request;

            var responseMessage = message.ToObject<KrakenFuturesSocketMessage>();
            if (responseMessage == null || responseMessage.Feed != kRequest.Feed)
                return false;

            callResult = responseMessage.Event == "subscribed" ? new CallResult<object>(responseMessage) : new CallResult<object>(new ServerError(responseMessage.Error ?? "-"));
            return true;
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, object request)
        {
            if (message.Type != JTokenType.Object)
                return false;

            var kRequest = (KrakenFuturesSubscribeMessage)request;
            var responseMessage = message.ToObject<KrakenFuturesUpdateMessage>();
            if (responseMessage == null || (responseMessage.Feed != kRequest.Feed && responseMessage.Feed != kRequest.Feed + "_snapshot"))
                return false;

            if (kRequest.Symbols == null)
                return true;

            return kRequest.Symbols.Contains(responseMessage.ProductId);
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, string identifier)
        {
            if (message.Type != JTokenType.Object)
                return false;

            if (message["event"]?.ToString() == "info" && identifier == "Info")
                return true;

            if (identifier == "AdditionalSubResponses")
            {
                if (message.Type != JTokenType.Object)
                    return false;

                var evnt = message["event"]?.ToString();
                if (evnt == null || (evnt != "subscribed" && evnt != "error"))
                    return false;

                var responseMessage = message.ToObject<KrakenFuturesSubscribeMessage>();
                if (responseMessage == null)
                    return false;

                var subscription = socketConnection.GetSubscriptionByRequest(r => 
                            (r as KrakenFuturesSubscribeMessage)?.Feed == responseMessage.Feed 
                            && (r as KrakenFuturesSubscribeMessage)?.Symbols?.Contains(responseMessage.Symbols.First()) != false);

                if (subscription == null)
                    return false;

                // This is another message for subscription we've already subscribed
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        protected override Task<CallResult<bool>> SubscribeAndWaitAsync(SocketConnection socketConnection, object request, SocketSubscription subscription)
        {
            if (request is KrakenFuturesSubscribeAuthMessage authMessage)
            {
                authMessage.ApiKey = ((KrakenFuturesAuthenticationProvider)AuthenticationProvider!).GetApiKey();
                authMessage.OriginalChallenge = _challenges[socketConnection].Item1;
                authMessage.SignedChallenge = _challenges[socketConnection].Item2;
            }

            return base.SubscribeAndWaitAsync(socketConnection, request, subscription);
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection s)
        {
            if (AuthenticationProvider == null)
                throw new InvalidOperationException("No credentials provided for private stream");

            var authProvider = (KrakenFuturesAuthenticationProvider)AuthenticationProvider!;
            string? challenge = null;
            await s.SendAndWaitAsync(new { @event = "challenge", api_key = authProvider.GetApiKey() }, TimeSpan.FromSeconds(5), null, data =>
            {
                var evnt = data["event"]?.ToString();
                if (evnt != "challenge")
                    return false;

                challenge = data["message"]!.ToString();
                return true;
            }).ConfigureAwait(false);

            if (challenge == null)
                return new CallResult<bool>(false);

            var sign = authProvider.AuthenticateWebsocketChallenge(challenge);
            _challenges[s] = (challenge!, sign);
            return new CallResult<bool>(true);
        }

        /// <inheritdoc />
        protected override async Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription subscription)
        {
            var kRequest = (KrakenSubscribeRequest)subscription.Request!;
            KrakenUnsubscribeRequest unsubRequest;
            if (!kRequest.ChannelId.HasValue)
            {
                if (kRequest.Details?.Topic == "ownTrades")
                {
                    unsubRequest = new KrakenUnsubscribeRequest(NextId(), new KrakenUnsubscribeSubscription
                    {
                        Name = "ownTrades",
                        Token = ((KrakenOwnTradesSubscriptionDetails)kRequest.Details).Token
                    });
                }
                else if (kRequest.Details?.Topic == "openOrders")
                {
                    unsubRequest = new KrakenUnsubscribeRequest(NextId(), new KrakenUnsubscribeSubscription
                    {
                        Name = "openOrders",
                        Token = ((KrakenOpenOrdersSubscriptionDetails)kRequest.Details).Token
                    });
                }
                else
                {
                    return true; // No channel id assigned, nothing to unsub
                }
            }
            else
            {
                unsubRequest = new KrakenUnsubscribeRequest(NextId(), kRequest.ChannelId.Value);
            }

            var result = false;
            await connection.SendAndWaitAsync(unsubRequest, ClientOptions.RequestTimeout, null, data =>
            {
                if (data.Type != JTokenType.Object)
                    return false;

                if (data["reqid"] == null)
                    return false;

                var requestId = data["reqid"]!.Value<int>();
                if (requestId != unsubRequest.RequestId)
                    return false;

                var response = data.ToObject<KrakenSubscriptionEvent>();
                result = response!.Status == "unsubscribed";
                return true;
            }).ConfigureAwait(false);
            return result;
        }

        private void HandleSnapshotUpdate<TSnapshot, TUpdate>(DataEvent<JToken> data, Action<DataEvent<TSnapshot>> snapshotHandler, Action<DataEvent<TUpdate>> updateHandler)
        {
            if (data.Data["feed"]!.ToString().Contains("snapshot"))
            {
                var deserialized = Deserialize<TSnapshot>(data.Data);
                snapshotHandler.Invoke(data.As(deserialized.Data));
            }
            else
            {
                var deserialized = Deserialize<TUpdate>(data.Data);
                updateHandler.Invoke(data.As(deserialized.Data));
            }
        }
        #endregion
    }
}
