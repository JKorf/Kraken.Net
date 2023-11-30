using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Options;
using Kraken.Net.Objects.Sockets.Converters;
using Kraken.Net.Objects.Sockets.Queries;
using Kraken.Net.Objects.Sockets.Subscriptions.Futures;
using Kraken.Net.Objects.Sockets.Subscriptions.Spot;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class KrakenSocketClientFuturesApi : SocketApiClient, IKrakenSocketClientFuturesApi
    {
        #region fields                
        /// <inheritdoc />
        public new KrakenSocketOptions ClientOptions => (KrakenSocketOptions)base.ClientOptions;

        public override SocketConverter StreamConverter { get; } = new KrakenFuturesStreamConverter();
        #endregion

        #region ctor

        internal KrakenSocketClientFuturesApi(ILogger logger, KrakenSocketOptions options) :
            base(logger, options.Environment.FuturesSocketBaseAddress, options, options.FuturesOptions)
        {
            //AddGenericHandler("Info", (messageEvent) => { });
            //AddGenericHandler("AdditionalSubResponses", (messageEvent) => { });
        }
        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new KrakenFuturesAuthenticationProvider(credentials, ClientOptions.NonceProvider ?? new KrakenNonceProvider());

        public override async Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection socket) 
        {
            _logger.Log(LogLevel.Debug, $"Socket {socket.SocketId} Attempting to authenticate");
            var krakenAuthProvider = (KrakenFuturesAuthenticationProvider)AuthenticationProvider;
            var authRequest = new KrakenFuturesAuthQuery(krakenAuthProvider.GetApiKey());
            var result = await socket.SendAndWaitQueryAsync(authRequest).ConfigureAwait(false);

            if (!result)
            {
                _logger.Log(LogLevel.Warning, $"Socket {socket.SocketId} authentication failed");
                if (socket.Connected)
                    await socket.CloseAsync().ConfigureAwait(false);

                result.Error!.Message = "Authentication failed: " + result.Error.Message;
                return new CallResult<bool>(result.Error)!;
            }

            var sign = krakenAuthProvider.AuthenticateWebsocketChallenge(result.Data.Message);
            socket.Properties["OriginalChallenge"] = result.Data.Message;
            socket.Properties["SignedChallenge"] = sign;

            _logger.Log(LogLevel.Debug, $"Socket {socket.SocketId} authenticated");
            socket.Authenticated = true;
            return new CallResult<bool>(true);
        }

        //#region methods

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToHeartbeatUpdatesAsync(Action<DataEvent<KrakenFuturesHeartbeatUpdate>> handler, CancellationToken ct = default)
        {
            var subscription = new KrakenFuturesSubscription<KrakenFuturesHeartbeatUpdate>(_logger, "heartbeat", null, handler);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenFuturesTickerUpdate>> handler, CancellationToken ct = default)
            => SubscribeToTickerUpdatesAsync(new List<string> { symbol }, handler, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesTickerUpdate>> handler, CancellationToken ct = default)
        {
            var subscription = new KrakenFuturesSubscription<KrakenFuturesTickerUpdate>(_logger, "ticker", symbols.ToList(), handler);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
            IEnumerable<string> symbols,
            Action<DataEvent<IEnumerable<KrakenFuturesTradeUpdate>>> handler,
            CancellationToken ct = default)
        {
            var subscription = new KrakenFuturesTradesSubscription(_logger, symbols.ToList(), handler);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesMiniTickerUpdate>> handler, CancellationToken ct = default)
        {
            var subscription = new KrakenFuturesSubscription<KrakenFuturesMiniTickerUpdate>(_logger, "ticker_lite", symbols.ToList(), handler);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
            IEnumerable<string> symbols,
            Action<DataEvent<KrakenFuturesBookSnapshotUpdate>> snapshotHandler,
            Action<DataEvent<KrakenFuturesBookUpdate>> updateHandler,
            CancellationToken ct = default)
        {
            var subscription = new KrakenFuturesOrderbookSubscription(_logger, symbols.ToList(), snapshotHandler, updateHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOpenOrdersUpdatesAsync(
            bool verbose,
            Action<DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate>> snapshotHandler,
            Action<DataEvent<KrakenFuturesOpenOrdersUpdate>> updateHandler,
            CancellationToken ct = default)
        {
            var subscription = new KrakenFuturesOrdersSubscription(_logger, verbose, snapshotHandler, updateHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAccountLogUpdatesAsync(
            Action<DataEvent<KrakenFuturesAccountLogsSnapshotUpdate>> snapshotHandler,
            Action<DataEvent<KrakenFuturesAccountLogsUpdate>> updateHandler,
            CancellationToken ct = default)
        {
            var subscription = new KrakenFuturesAccountLogSubscription(_logger, snapshotHandler, updateHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOpenPositionUpdatesAsync(
            Action<DataEvent<KrakenFuturesOpenPositionUpdate>> handler,
            CancellationToken ct = default)
        {
            var subscription = new KrakenFuturesOpenPositionsSubscription(_logger, handler);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(
            Action<DataEvent<KrakenFuturesBalancesUpdate>> handler,
            CancellationToken ct = default)
        {
            var subscription = new KrakenFuturesBalanceSubscription(_logger, handler);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(
            Action<DataEvent<KrakenFuturesUserTradesUpdate>> handler,
            CancellationToken ct = default)
        {
            var subscription = new KrakenFuturesUserTradeSubscription(_logger, handler);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToNotificationUpdatesAsync(
            Action<DataEvent<KrakenFuturesNotificationUpdate>> handler,
            CancellationToken ct = default)
        {
            var subscription = new KrakenFuturesNotificationSubscription(_logger, handler);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), subscription, ct).ConfigureAwait(false);
        }




        ///// <inheritdoc />
        //protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        //    => throw new NotImplementedException();

        ///// <inheritdoc />
        //protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object>? callResult)
        //{
        //    callResult = null;
        //    if (message.Type != JTokenType.Object)
        //        return false;

        //    var evnt = message["event"]?.ToString();
        //    if (evnt == null || (evnt != "subscribed" && evnt != "error"))
        //        return false;

        //    var kRequest = (KrakenFuturesSubscribeMessage)request;

        //    var responseMessage = message.ToObject<KrakenFuturesSocketMessage>();
        //    if (responseMessage == null || !responseMessage.Feed.Equals(kRequest.Feed, StringComparison.InvariantCultureIgnoreCase))
        //        return false;

        //    callResult = responseMessage.Event == "subscribed" ? new CallResult<object>(responseMessage) : new CallResult<object>(new ServerError(responseMessage.Error ?? "-"));
        //    return true;
        //}

        ///// <inheritdoc />
        //protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, object request)
        //{
        //    if (message.Type != JTokenType.Object)
        //        return false;

        //    var kRequest = (KrakenFuturesSubscribeMessage)request;
        //    var responseMessage = message.ToObject<KrakenFuturesUpdateMessage>();
        //    if (responseMessage == null || 
        //        (!responseMessage.Feed.Equals(kRequest.Feed, StringComparison.InvariantCultureIgnoreCase) 
        //        && !responseMessage.Feed.Equals(kRequest.Feed + "_snapshot", StringComparison.InvariantCultureIgnoreCase)))
        //        return false;

        //    if (kRequest.Symbols == null)
        //        return true;

        //    return kRequest.Symbols.Any(s => s.Equals(responseMessage.Symbol, StringComparison.InvariantCultureIgnoreCase));
        //}

        ///// <inheritdoc />
        //protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, string identifier)
        //{
        //    if (message.Type != JTokenType.Object)
        //        return false;

        //    if (message["event"]?.ToString() == "info" && identifier == "Info")
        //        return true;

        //    if (identifier == "AdditionalSubResponses")
        //    {
        //        if (message.Type != JTokenType.Object)
        //            return false;

        //        var evnt = message["event"]?.ToString();
        //        if (evnt == null || (evnt != "subscribed" && evnt != "unsubscribed" && evnt != "error"))
        //            return false;

        //        var responseMessage = message.ToObject<KrakenFuturesSubscribeMessage>();
        //        if (responseMessage == null)
        //            return false;

        //        var subscription = socketConnection.GetSubscriptionByRequest(r =>
        //                    (r as KrakenFuturesSubscribeMessage)?.Feed.Equals(responseMessage.Feed, StringComparison.InvariantCultureIgnoreCase) == true
        //                    && (r as KrakenFuturesSubscribeMessage)?.Symbols?.Any(s => s.Equals(responseMessage.Symbols.First(), StringComparison.InvariantCultureIgnoreCase)) != false);

        //        if (subscription == null)
        //            return false;

        //        // This is another message for subscription we've already (un)subscribed
        //        return true;
        //    }

        //    return false;
        //}

        ///// <inheritdoc />
        //protected override Task<CallResult<bool>> SubscribeAndWaitAsync(SocketConnection socketConnection, object request, SocketSubscription subscription)
        //{
        //    if (request is KrakenFuturesSubscribeAuthMessage authMessage)
        //    {
        //        authMessage.ApiKey = ((KrakenFuturesAuthenticationProvider)AuthenticationProvider!).GetApiKey();
        //        authMessage.OriginalChallenge = (string)socketConnection.Properties["OriginalChallenge"];
        //        authMessage.SignedChallenge = (string)socketConnection.Properties["SignedChallenge"];
        //    }

        //    return base.SubscribeAndWaitAsync(socketConnection, request, subscription);
        //}

        ///// <inheritdoc />
        //protected override async Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection s)
        //{
        //    if (AuthenticationProvider == null)
        //        throw new InvalidOperationException("No credentials provided for private stream");

        //    var authProvider = (KrakenFuturesAuthenticationProvider)AuthenticationProvider!;
        //    string? challenge = null;
        //    await s.SendAndWaitAsync(new { @event = "challenge", api_key = authProvider.GetApiKey() }, TimeSpan.FromSeconds(5), null, 1, data =>
        //    {
        //        var evnt = data["event"]?.ToString();
        //        if (evnt != "challenge")
        //            return false;

        //        challenge = data["message"]!.ToString();
        //        return true;
        //    }).ConfigureAwait(false);

        //    if (challenge == null)
        //        return new CallResult<bool>(false);

        //    var sign = authProvider.AuthenticateWebsocketChallenge(challenge);
        //    s.Properties["OriginalChallenge"] = challenge;
        //    s.Properties["SignedChallenge"] = sign;
        //    return new CallResult<bool>(true);
        //}

        ///// <inheritdoc />
        //protected override async Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription subscription)
        //{
        //    var kRequest = (KrakenFuturesSubscribeMessage)subscription.Request!;

        //    var result = false;
        //    await connection.SendAndWaitAsync(new
        //    {
        //        @event = "unsubscribe",
        //        feed = kRequest.Feed,
        //        product_ids = kRequest.Symbols
        //    }, ClientOptions.RequestTimeout, null, 1, message =>
        //    {
        //        if (message.Type != JTokenType.Object)
        //            return false;

        //        var evnt = message["event"]?.ToString();
        //        if (evnt == null || (evnt != "unsubscribed" && evnt != "unsubscribed_failed"))
        //            return false;

        //        var responseMessage = message.ToObject<KrakenFuturesSocketMessage>();
        //        if (responseMessage == null || responseMessage.Feed != kRequest.Feed)
        //            return false;

        //        result = responseMessage.Event == "unsubscribed" ? new CallResult<object>(responseMessage) : new CallResult<object>(new ServerError(responseMessage.Error ?? "-"));
        //        return true;
        //    }).ConfigureAwait(false);
        //    return result;
        //}

        //private void HandleSnapshotUpdate<TSnapshot, TUpdate>(DataEvent<JToken> data, Action<DataEvent<TSnapshot>> snapshotHandler, Action<DataEvent<TUpdate>> updateHandler)
        //{
        //    if (data.Data["feed"]!.ToString().Contains("snapshot"))
        //    {
        //        var deserialized = Deserialize<TSnapshot>(data.Data);
        //        snapshotHandler.Invoke(data.As(deserialized.Data));
        //    }
        //    else
        //    {
        //        var deserialized = Deserialize<TUpdate>(data.Data);
        //        updateHandler.Invoke(data.As(deserialized.Data));
        //    }
        //}
        //#endregion
    }
}
