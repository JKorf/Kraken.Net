using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Options;
using Kraken.Net.Objects.Sockets.Subscriptions.Spot;
using System.Collections.Concurrent;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal partial class KrakenSocketClientSpotApi : SocketApiClient, IKrakenSocketClientSpotApi
    {
        private static readonly MessagePath _idPath = MessagePath.Get().Property("req_id");
        private static readonly MessagePath _methodPath = MessagePath.Get().Property("method");
        private static readonly MessagePath _channelPath = MessagePath.Get().Property("channel");
        private static readonly MessagePath _symbolPath = MessagePath.Get().Property("data").Index(0).Property("symbol");

        private static readonly ConcurrentDictionary<string, CachedToken> _tokenCache = new();

        private static readonly HashSet<string> _channelsWithoutSymbol =
        [
            "heartbeat",
            "status",
            "instrument",
            "executions",
            "balances"
        ];

        #region fields
        private readonly string _privateBaseAddress;

        /// <inheritdoc />
        public new KrakenSocketOptions ClientOptions => (KrakenSocketOptions)base.ClientOptions;

        #endregion

        #region ctor

        internal KrakenSocketClientSpotApi(ILogger logger, KrakenSocketOptions options) :
            base(logger, options.Environment.SpotSocketPublicAddress, options, options.SpotOptions)
        {
            _privateBaseAddress = options.Environment.SpotSocketPrivateAddress;

            AddSystemSubscription(new HeartbeatSubscription(_logger));
            AddSystemSubscription(new SystemStatusSubscription(_logger));

            RateLimiter = KrakenExchange.RateLimiter.SpotSocket;
                        
            SetDedicatedConnection(_privateBaseAddress, false);
        }
        #endregion

        protected override IByteMessageAccessor CreateAccessor() => new SystemTextJsonByteMessageAccessor();

        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null) => $"{baseAsset.ToUpperInvariant()}/{quoteAsset.ToUpperInvariant()}";

        /// <inheritdoc />
        public IKrakenSocketClientSpotApiShared SharedClient => this;

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            var id = message.GetValue<string>(_idPath);
            if (id != null)
                return id;

            var channel = message.GetValue<string>(_channelPath);
            var method = message.GetValue<string?>(_methodPath);

            if (!_channelsWithoutSymbol.Contains(channel!))
            {
                var symbol = message.GetValue<string>(_symbolPath);
                return channel + method + "-" + symbol;
            }

            return channel + method;
        }

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new KrakenAuthenticationProvider(credentials, ClientOptions.NonceProvider ?? new KrakenNonceProvider());

        #region System Status

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSystemStatusUpdatesAsync(Action<DataEvent<KrakenStreamSystemStatus>> handler, CancellationToken ct = default)
        {
            var subscription = new KrakenSystemStatusSubscription(_logger, handler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Ticker

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenTickerUpdate>> handler, CancellationToken ct = default)
        {
            var subscription = new KrakenSubscription<IEnumerable<KrakenTickerUpdate>>(_logger, "ticker", [symbol], null, null, null,
                x => handler(x.As(x.Data.First()).WithSymbol(x.Data.First().Symbol))
                );
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenTickerUpdate>> handler, CancellationToken ct = default)
        {
            var subscription = new KrakenSubscription<IEnumerable<KrakenTickerUpdate>>(_logger, "ticker", symbols, null, null, null,
                x => handler(x.As(x.Data.First()).WithSymbol(x.Data.First().Symbol))
                );
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Kline

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<IEnumerable<KrakenKlineUpdate>>> handler, CancellationToken ct = default)
            => SubscribeToKlineUpdatesAsync([symbol], interval, handler, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<IEnumerable<KrakenKlineUpdate>>> handler, CancellationToken ct = default)
        {
            var subscription = new KrakenSubscription<IEnumerable<KrakenKlineUpdate>>(_logger, "ohlc", symbols.ToArray(), int.Parse(EnumConverter.GetString(interval)), null, null,
                x => handler(x.WithSymbol(x.Data.First().Symbol))
                );
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Trade

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<IEnumerable<KrakenTradeUpdate>>> handler, bool? snapshot = null, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync([symbol], handler, snapshot, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IEnumerable<KrakenTradeUpdate>>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            var subscription = new KrakenSubscription<IEnumerable<KrakenTradeUpdate>>(_logger, "trade", symbols.ToArray(), null, snapshot, null,
                x => handler(x.WithSymbol(x.Data.First().Symbol))
                );
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Aggregated Order Book

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToAggregatedOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
            => SubscribeToAggregatedOrderBookUpdatesAsync([symbol], depth, handler, snapshot, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToAggregatedOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<KrakenBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 10, 25, 100, 500, 1000);

            var subscription = new KrakenSubscription<IEnumerable<KrakenBookUpdate>>(_logger, "book", symbols.ToArray(), null, snapshot, null,
                x => handler(x.As(x.Data.First()).WithSymbol(x.Data.First().Symbol))
                );
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Order Book

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToInvidualOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenIndividualBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
            => SubscribeToInvidualOrderBookUpdatesAsync([symbol], depth, handler, snapshot, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToInvidualOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<KrakenIndividualBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 10, 100, 1000);

            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<UpdateSubscription>(token.Error!);

            var subscription = new KrakenSubscription<IEnumerable<KrakenIndividualBookUpdate>>(_logger, "level3", symbols.ToArray(), null, snapshot, token.Data,
                x => handler(x.As(x.Data.First()).WithSymbol(x.Data.First().Symbol))
                );
            return await SubscribeAsync(_privateBaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Instrument

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToInstrumentUpdatesAsync(Action<DataEvent<KrakenInstrumentUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            var subscription = new KrakenSubscription<KrakenInstrumentUpdate>(_logger, "instrument", null, null, snapshot, null, handler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Balance

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<IEnumerable<KrakenBalanceSnapshot>>>? snapshotHandler, Action<DataEvent<IEnumerable<KrakenBalanceUpdate>>> updateHandler, bool? snapshot = null, CancellationToken ct = default)
        {
            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<UpdateSubscription>(token.Error!);

            var subscription = new KrakenBalanceSubscription(_logger, snapshot, token.Data, snapshotHandler, updateHandler);
            return await SubscribeAsync(_privateBaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Order

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(
            Action<DataEvent<IEnumerable<KrakenOrderUpdate>>> updateHandler,
            bool? snapshotOrder = null,
            bool? snapshotTrades = null,
            CancellationToken ct = default)
        {
            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<UpdateSubscription>(token.Error!);

            var subscription = new KrakenOrderSubscription(_logger, snapshotOrder, snapshotTrades, token.Data, updateHandler);
            return await SubscribeAsync(_privateBaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        //      #region Order Updates

        //      /// <inheritdoc />
        //      public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string socketToken, Action<DataEvent<IEnumerable<KrakenStreamOrder>>> handler, CancellationToken ct = default)
        //      {
        //          var innerHandler = new Action<DataEvent<KrakenAuthSocketUpdate<IEnumerable<Dictionary<string, KrakenStreamOrder>>>>>(data =>
        //          {
        //              foreach (var item in data.Data.Data)
        //              {
        //                  foreach (var value in item)
        //                  {
        //                      value.Value.Id = value.Key;
        //                      value.Value.SequenceNumber = data.Data.Sequence.Sequence;
        //                  }
        //              }

        //              handler.Invoke(data.As<IEnumerable<KrakenStreamOrder>>(data.Data.Data.SelectMany(d => d.Values).ToList()));
        //          });

        //          var subscription = new KrakenAuthSubscription<IEnumerable<Dictionary<string, KrakenStreamOrder>>>(_logger, "openOrders", socketToken, innerHandler);
        //          return await SubscribeAsync(_privateBaseAddress, subscription, ct).ConfigureAwait(false);
        //      }

        //      #endregion

        //      #region User Trade

        //      /// <inheritdoc />
        //      public Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(string socketToken, Action<DataEvent<IEnumerable<KrakenStreamUserTrade>>> handler, CancellationToken ct = default)
        //          => SubscribeToUserTradeUpdatesAsync(socketToken, true, handler, ct);

        //      /// <inheritdoc />
        //      public async Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(string socketToken, bool snapshot, Action<DataEvent<IEnumerable<KrakenStreamUserTrade>>> handler, CancellationToken ct = default)
        //      {
        //          var innerHandler = new Action<DataEvent<KrakenAuthSocketUpdate<IEnumerable<Dictionary<string, KrakenStreamUserTrade>>>>>(data =>
        //          {
        //              foreach (var item in data.Data.Data)
        //              {
        //                  foreach (var value in item)
        //                  {
        //                      value.Value.Id = value.Key;
        //                      value.Value.SequenceNumber = data.Data.Sequence.Sequence;
        //                  }
        //              }
        //              handler.Invoke(data.As(data.Data.Data.SelectMany(d => d.Values)));
        //          });

        //          var subscription = new KrakenAuthSubscription<IEnumerable<Dictionary<string, KrakenStreamUserTrade>>>(_logger, "ownTrades", socketToken, innerHandler);
        //          return await SubscribeAsync(_privateBaseAddress, subscription, ct).ConfigureAwait(false);
        //      }

        //      #endregion

        //      #region Place Order

        //      /// <inheritdoc />
        //      public async Task<CallResult<KrakenStreamPlacedOrder>> PlaceOrderAsync(
        //          string websocketToken,
        //          string symbol,
        //          OrderType type,
        //          OrderSide side,
        //          decimal quantity,
        //          uint? clientOrderId = null,
        //          decimal? price = null,
        //          decimal? secondaryPrice = null,
        //          decimal? leverage = null,
        //          DateTime? startTime = null,
        //          DateTime? expireTime = null,
        //          bool? validateOnly = null,
        //          OrderType? closeOrderType = null,
        //          decimal? closePrice = null,
        //          decimal? secondaryClosePrice = null,
        //          IEnumerable<OrderFlags>? flags = null,            
        //          bool? reduceOnly = null,
        //          bool? margin = null,
        //          CancellationToken ct = default)
        //      {
        //          var request = new KrakenSocketPlaceOrderRequest
        //          {
        //              Event = "addOrder",
        //              Token = websocketToken,
        //              Symbol = symbol,
        //              Type = side,
        //              OrderType = type,
        //              Volume = quantity.ToString(CultureInfo.InvariantCulture),
        //              ClientOrderId = clientOrderId,
        //              Price = price?.ToString(CultureInfo.InvariantCulture),
        //              SecondaryPrice = secondaryPrice?.ToString(CultureInfo.InvariantCulture),
        //              Leverage = leverage?.ToString(CultureInfo.InvariantCulture),
        //              StartTime = (startTime.HasValue && startTime > DateTime.UtcNow) ? DateTimeConverter.ConvertToSeconds(startTime).ToString() : null,
        //              ExpireTime = expireTime.HasValue ? DateTimeConverter.ConvertToSeconds(expireTime).ToString() : null,
        //              ValidateOnly = validateOnly,
        //              CloseOrderType = closeOrderType,
        //              ClosePrice = closePrice?.ToString(CultureInfo.InvariantCulture),
        //              SecondaryClosePrice = secondaryClosePrice?.ToString(CultureInfo.InvariantCulture),
        //              ReduceOnly = reduceOnly,
        //              RequestId = ExchangeHelpers.NextId(),
        //              Margin = margin,
        //              Flags = flags == null ? null : string.Join(",", flags.Select(EnumConverter.GetString))
        //          };

        //          var query = new KrakenSpotQuery<KrakenStreamPlacedOrder>(request, false);
        //          return await QueryAsync(_privateBaseAddress, query, ct).ConfigureAwait(false);
        //      }

        //      #endregion

        //      #region Cancel Order

        //      /// <inheritdoc />
        //      public Task<CallResult<bool>> CancelOrderAsync(string websocketToken, string orderId, CancellationToken ct = default)
        //          => CancelOrdersAsync(websocketToken, new[] { orderId }, ct);

        //      /// <inheritdoc />
        //      public async Task<CallResult<bool>> CancelOrdersAsync(string websocketToken, IEnumerable<string> orderIds, CancellationToken ct = default)
        //      {
        //          var request = new KrakenSocketCancelOrdersRequest
        //          {
        //              Event = "cancelOrder",
        //              OrderIds = orderIds,
        //              Token = websocketToken,
        //              RequestId = ExchangeHelpers.NextId()
        //          };

        //          var query = new KrakenSpotQuery<KrakenQueryEvent>(request, false);
        //          var result = await QueryAsync(_privateBaseAddress, query, ct).ConfigureAwait(false);
        //          return result.As(result.Success);
        //      }

        //      #endregion

        //      #region Cancel All Orders

        //      /// <inheritdoc />
        //      public async Task<CallResult<KrakenStreamCancelAllResult>> CancelAllOrdersAsync(string websocketToken, CancellationToken ct = default)
        //      {
        //          var request = new KrakenSocketAuthRequest
        //          {
        //              Event = "cancelAll",
        //              Token = websocketToken,
        //              RequestId = ExchangeHelpers.NextId()
        //          };

        //          var query = new KrakenSpotQuery<KrakenStreamCancelAllResult>(request, false);
        //          return await QueryAsync(_privateBaseAddress, query, ct).ConfigureAwait(false);
        //      }

        //      #endregion

        //      #region Cancel All Orders After

        //      /// <inheritdoc />
        //      public async Task<CallResult<KrakenStreamCancelAfterResult>> CancelAllOrdersAfterAsync(string websocketToken, TimeSpan timeout, CancellationToken ct = default)
        //      {
        //          var request = new KrakenSocketCancelAfterRequest
        //          {
        //              Event = "cancelAllOrdersAfter",
        //              Token = websocketToken,
        //              Timeout = (int)Math.Round(timeout.TotalSeconds),
        //              RequestId = ExchangeHelpers.NextId()
        //          };

        //          var query = new KrakenSpotQuery<KrakenStreamCancelAfterResult>(request, false);
        //          return await QueryAsync(_privateBaseAddress, query, ct).ConfigureAwait(false);
        //      }

        //      #endregion

        ///// <inheritdoc />
        //protected override async Task<CallResult> RevitalizeRequestAsync(Subscription subscription)
        //{
        //    if (subscription is not KrakenAuthSubscription authSubscription)
        //        return new CallResult(null);

        //    var apiCredentials = ApiOptions.ApiCredentials ?? ClientOptions.ApiCredentials;
        //    var restClient = new KrakenRestClient(x =>
        //    {
        //        x.ApiCredentials = apiCredentials;
        //        x.Environment = ClientOptions.Environment;
        //    });

        //    var newToken = await restClient.SpotApi.Account.GetWebsocketTokenAsync().ConfigureAwait(false);
        //    if (!newToken.Success)
        //        return newToken.AsDataless();

        //    authSubscription.UpdateToken(newToken.Data.Token);
        //    return new CallResult(null);
        //}

        private async Task<CallResult<string>> GetTokenAsync()
        {
            var apiCredentials = ApiOptions.ApiCredentials ?? ClientOptions.ApiCredentials;
            if (apiCredentials == null)
                return new CallResult<string>(new NoApiCredentialsError());

            if (_tokenCache.TryGetValue(apiCredentials.Key, out var token) && token.Expire > DateTime.UtcNow)
                return new CallResult<string>(token.Token);

            _logger.LogDebug("Requesting websocket token");
            var restClient = new KrakenRestClient(x =>
            {
                x.ApiCredentials = apiCredentials;
                x.Environment = ClientOptions.Environment;
            });

            var result = await restClient.SpotApi.Account.GetWebsocketTokenAsync().ConfigureAwait(false);
            if (result)
                _tokenCache[apiCredentials.Key] = new CachedToken { Token = result.Data.Token, Expire = DateTime.UtcNow.AddSeconds(result.Data.Expires - 5) };

            return result.As<string>(result.Data?.Token);
        }

        private class CachedToken
        {
            public string Token { get; set; } = string.Empty;
            public DateTime Expire { get; set; }
        }
    }
}