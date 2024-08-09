using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Options;
using Kraken.Net.Objects.Sockets.Queries;
using Kraken.Net.Objects.Sockets.Subscriptions.Futures;
using Kraken.Net.Objects.Sockets.Subscriptions.Spot;
using Microsoft.Extensions.Logging;

namespace Kraken.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class KrakenSocketClientFuturesApi : SocketApiClient, IKrakenSocketClientFuturesApi
    {
        private static readonly MessagePath _eventPath = MessagePath.Get().Property("event");
        private static readonly MessagePath _feedPath = MessagePath.Get().Property("feed");
        private static readonly MessagePath _productIdsPath = MessagePath.Get().Property("product_ids");
        private static readonly MessagePath _productIdPath = MessagePath.Get().Property("product_id");

        #region fields                
        /// <inheritdoc />
        public new KrakenSocketOptions ClientOptions => (KrakenSocketOptions)base.ClientOptions;

        #endregion

        #region ctor

        internal KrakenSocketClientFuturesApi(ILogger logger, KrakenSocketOptions options) :
            base(logger, options.Environment.FuturesSocketBaseAddress, options, options.FuturesOptions)
        {
            RateLimiter = KrakenExchange.RateLimiter.FuturesSocket;

            AddSystemSubscription(new KrakenFuturesInfoSubscription(_logger));
        }
        #endregion

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, FuturesType? futuresType = null)
        {
            return $"{(futuresType == FuturesType.Linear ? "PF" : "PI")}_{baseAsset.ToUpperInvariant()}{quoteAsset.ToUpperInvariant()}";
        }

        /// <inheritdoc />
        public override string GetListenerIdentifier(IMessageAccessor message)
        {
            var evnt = message.GetValue<string>(_eventPath);
            var feed = message.GetValue<string>(_feedPath);
            if (feed == null)
                return evnt!;

            if (evnt != null)
            {
                var result = evnt + "-" + feed;
                var productIds = message.GetValues<string>(_productIdsPath);
                if (productIds != null)
                    result += "-" + string.Join("-", productIds.Select(i => i!.ToLowerInvariant()));
                return result;
            }

            var productId = message.GetValue<string>(_productIdPath);
            if (productId == null)
                return feed;

            return feed + "-" + productId.ToLowerInvariant();
        }

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new KrakenFuturesAuthenticationProvider(credentials, ClientOptions.NonceProvider ?? new KrakenNonceProvider());

        /// <inheritdoc />
        protected override Query? GetAuthenticationRequest(SocketConnection connection) => new KrakenFuturesAuthQuery(((KrakenFuturesAuthenticationProvider)AuthenticationProvider!).GetApiKey());

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
        public Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<IEnumerable<KrakenFuturesTradeUpdate>>> handler, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync(new List<string> { symbol }, handler, ct);

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
        public Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenFuturesMiniTickerUpdate>> handler, CancellationToken ct = default)
            => SubscribeToMiniTickerUpdatesAsync(new List<string> { symbol }, handler, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesMiniTickerUpdate>> handler, CancellationToken ct = default)
        {
            var subscription = new KrakenFuturesSubscription<KrakenFuturesMiniTickerUpdate>(_logger, "ticker_lite", symbols.ToList(), handler);
            return await SubscribeAsync(BaseAddress.AppendPath("ws/v1"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
            string symbol,
            Action<DataEvent<KrakenFuturesBookSnapshotUpdate>> snapshotHandler,
            Action<DataEvent<KrakenFuturesBookUpdate>> updateHandler,
            CancellationToken ct = default)
            => SubscribeToOrderBookUpdatesAsync(new[] { symbol }, snapshotHandler, updateHandler, ct);

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
    }
}
