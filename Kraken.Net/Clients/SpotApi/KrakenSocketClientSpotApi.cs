using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Options;
using Kraken.Net.Objects.Sockets.Queries;
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

        #region Streams

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
            var subscription = new KrakenSubscriptionV2<IEnumerable<KrakenTickerUpdate>>(_logger, "ticker", [symbol], null, null, null,
                x => handler(x.As(x.Data.First()).WithSymbol(x.Data.First().Symbol))
                );
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenTickerUpdate>> handler, CancellationToken ct = default)
        {
            var subscription = new KrakenSubscriptionV2<IEnumerable<KrakenTickerUpdate>>(_logger, "ticker", symbols, null, null, null,
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
            var subscription = new KrakenSubscriptionV2<IEnumerable<KrakenKlineUpdate>>(_logger, "ohlc", symbols.ToArray(), int.Parse(EnumConverter.GetString(interval)), null, null,
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
            var subscription = new KrakenSubscriptionV2<IEnumerable<KrakenTradeUpdate>>(_logger, "trade", symbols.ToArray(), null, snapshot, null,
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

            var subscription = new KrakenSubscriptionV2<IEnumerable<KrakenBookUpdate>>(_logger, "book", symbols.ToArray(), null, snapshot, null,
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

            var subscription = new KrakenSubscriptionV2<IEnumerable<KrakenIndividualBookUpdate>>(_logger, "level3", symbols.ToArray(), null, snapshot, token.Data,
                x => handler(x.As(x.Data.First()).WithSymbol(x.Data.First().Symbol))
                );
            return await SubscribeAsync(_privateBaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Instrument

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToInstrumentUpdatesAsync(Action<DataEvent<KrakenInstrumentUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            var subscription = new KrakenSubscriptionV2<KrakenInstrumentUpdate>(_logger, "instrument", null, null, snapshot, null, handler);
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

        #endregion

        #region Queries

        #region Place Order

        /// <inheritdoc />
        public async Task<CallResult<KrakenOrderResult>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal quantity,
            string? clientOrderId = null,
            uint? userReference = null,
            decimal? limitPrice = null,
            PriceType? limitPriceType = null,
            TimeInForce? timeInForce = null,
            bool? reduceOnly = null,
            bool? margin = null,
            bool? postOnly = null,
            DateTime? startTime = null,
            DateTime? expireTime = null,
            DateTime? deadline = null,
            decimal? icebergQuantity = null,
            FeePreference? feePreference = null,
            bool? noMarketPriceProtection = null,
            SelfTradePreventionType? selfTradePreventionType = null,
            decimal? quoteQuantity = null,
            bool? validateOnly = null,

            Trigger? triggerPriceReference = null,
            decimal? triggerPrice = null,
            PriceType? triggerPriceType = null,

            OrderType? conditionalOrderType = null,
            decimal? conditionalLimitPrice = null,
            PriceType? conditionalLimitPriceType = null,
            decimal? conditionalTriggerPrice = null,
            PriceType? conditionalTriggerPriceType = null,

            CancellationToken ct = default)
        {

            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<KrakenOrderResult>(token.Error!);

            var request = new KrakenSocketPlaceOrderRequestV2
            {
                Token = token.Data,
                Symbol = symbol,
                Side = side,
                OrderType = type,
                Quantity = quantity,
                ClientOrderId = clientOrderId,
                UserReference = userReference,
                Price = limitPrice,
                LimitPriceType = limitPriceType,
                TimeInForce = timeInForce,
                ReduceOnly = reduceOnly,
                Margin = margin,
                PostOnly = postOnly,
                EffectiveTime = startTime?.ToRfc3339String(),
                ExpireTime = expireTime?.ToRfc3339String(),
                Deadline = deadline?.ToRfc3339String(),
                IcebergQuantity  = icebergQuantity,
                FeePreference = feePreference,
                NoMarketPriceProtection = noMarketPriceProtection,
                SelfTradePreventionType = selfTradePreventionType,
                QuoteQuantity = quoteQuantity,
                ValidateOnly = validateOnly,
            };

            if (triggerPrice != null)
            {
                request.Trigger = new KrakenSocketPlaceOrderRequestV2Trigger
                {
                    LimitPriceType = triggerPriceType,
                    Price = triggerPrice,
                    Reference = triggerPriceReference
                };
            }

            if (conditionalOrderType != null) 
            {
                request.Conditional = new KrakenSocketPlaceOrderRequestV2Condition
                {
                    LimitPriceType = conditionalLimitPriceType,
                    OrderType = conditionalOrderType.Value,
                    Price = conditionalLimitPrice,
                    TriggerPrice = conditionalTriggerPrice,
                    TriggerPriceType = conditionalTriggerPriceType
                };
            }

            var requestMessage = new KrakenSocketRequestV2<KrakenSocketPlaceOrderRequestV2>
            {
                Method = "add_order",
                RequestId = ExchangeHelpers.NextId(),
                Parameters = request
            };

            var query = new KrakenSpotQueryV2<KrakenOrderResult, KrakenSocketPlaceOrderRequestV2>(requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            return result.As<KrakenOrderResult>(result.Data?.Result);
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<CallResult<IEnumerable<KrakenOrderResult>>> PlaceMultipleOrdersAsync(
            string symbol,
            IEnumerable<KrakenSocketOrderRequest> orders,
            DateTime? deadline = null,
            bool? validateOnly = null,
            CancellationToken ct = default)
        {
            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<IEnumerable<KrakenOrderResult>>(token.Error!);

            var request = new KrakenSocketPlaceMultipleOrderRequestV2
            {
                Token = token.Data,
                Symbol = symbol,
                ValidateOnly = validateOnly,
                Deadline = deadline?.ToRfc3339String(),
                Orders = orders
            };

            var requestMessage = new KrakenSocketRequestV2<KrakenSocketPlaceMultipleOrderRequestV2>
            {
                Method = "batch_add",
                RequestId = ExchangeHelpers.NextId(),
                Parameters = request
            };

            var query = new KrakenSpotQueryV2<IEnumerable<KrakenOrderResult>, KrakenSocketPlaceMultipleOrderRequestV2>(requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<KrakenOrderResult>>(default);

            if (!result.Data.Success)
                return new CallResult<IEnumerable<KrakenOrderResult>>(result.Data.Result, result.OriginalData, new ServerError("Order placement failed"));

            return result.As<IEnumerable<KrakenOrderResult>>(result.Data?.Result);
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<CallResult<KrakenSocketAmendOrderResult>> EditOrderAsync(
            string? orderId = null,
            string? clientOrderId = null,
            decimal? limitPrice = null,
            PriceType? limitPriceType = null,
            decimal? quantity = null,
            decimal? icebergQuantity = null,
            bool? postOnly = null,
            decimal? triggerPrice = null,
            PriceType? triggerPriceType = null,
            DateTime? deadline = null,
            CancellationToken ct = default)
        {
            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<KrakenSocketAmendOrderResult>(token.Error!);

            var request = new KrakenSocketEditOrderRequest
            {
                Token = token.Data,
                ClientOrderId = clientOrderId,
                Deadline = deadline?.ToRfc3339String(),
                IcebergQuantity = icebergQuantity,
                LimitPriceType = limitPriceType,
                OrderId = orderId,
                PostOnly = postOnly,
                Price = limitPrice,
                Quantity = quantity,
                TriggerPrice = triggerPrice,
                TriggerPriceType = triggerPriceType
            };
            var requestMessage = new KrakenSocketRequestV2<KrakenSocketEditOrderRequest>
            {
                Method = "amend_order",
                RequestId = ExchangeHelpers.NextId(),
                Parameters = request
            };

            var query = new KrakenSpotQueryV2<KrakenSocketAmendOrderResult, KrakenSocketEditOrderRequest>(requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            return result.As<KrakenSocketAmendOrderResult>(result.Data?.Result);
        }

        #endregion

        #region Replace Order

        /// <inheritdoc />
        public async Task<CallResult<KrakenSocketReplaceOrderResult>> ReplaceOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            decimal? quantity = null,
            uint? userReference = null,
            decimal? limitPrice = null,
            bool? reduceOnly = null,
            bool? postOnly = null,
            DateTime? deadline = null,
            decimal? icebergQuantity = null,
            FeePreference? feePreference = null,
            bool? noMarketPriceProtection = null,
            bool? validateOnly = null,

            Trigger? triggerPriceReference = null,
            decimal? triggerPrice = null,
            PriceType? triggerPriceType = null,
            CancellationToken ct = default)
        {
            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<KrakenSocketReplaceOrderResult>(token.Error!);

            var request = new KrakenSocketReplaceOrderRequest
            {
                Token = token.Data,
                Deadline = deadline?.ToRfc3339String(),
                IcebergQuantity = icebergQuantity,
                OrderId = orderId,
                PostOnly = postOnly,
                Price = limitPrice,
                Quantity = quantity,
                FeePreference = feePreference,
                NoMarketPriceProtection = noMarketPriceProtection,
                ReduceOnly = reduceOnly,
                Symbol = symbol,
                UserReference = userReference,
                ValidateOnly = validateOnly,
            };

            if (triggerPrice != null)
            {
                request.Trigger = new KrakenSocketPlaceOrderRequestV2Trigger
                {
                    LimitPriceType = triggerPriceType,
                    Price = triggerPrice,
                    Reference = triggerPriceReference
                };
            }

            var requestMessage = new KrakenSocketRequestV2<KrakenSocketReplaceOrderRequest>
            {
                Method = "edit_order",
                RequestId = ExchangeHelpers.NextId(),
                Parameters = request
            };

            var query = new KrakenSpotQueryV2<KrakenSocketReplaceOrderResult, KrakenSocketReplaceOrderRequest>(requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            return result.As<KrakenSocketReplaceOrderResult>(result.Data?.Result);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public Task<CallResult<KrakenOrderResult>> CancelOrderAsync(string orderId, CancellationToken ct = default)
            => CancelOrdersAsync(new[] { orderId }, ct);

        /// <inheritdoc />
        public async Task<CallResult<KrakenOrderResult>> CancelOrdersAsync(IEnumerable<string> orderIds, CancellationToken ct = default)
        {
            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<KrakenOrderResult>(token.Error!);

            var request = new KrakenSocketCancelOrdersRequestV2
            {
                OrderIds = orderIds.ToArray(),
                Token = token.Data
            };
            var requestMessage = new KrakenSocketRequestV2<KrakenSocketCancelOrdersRequestV2>
            {
                Method = "cancel_order",
                RequestId = ExchangeHelpers.NextId(),
                Parameters = request
            };

            var query = new KrakenSpotQueryV2<KrakenOrderResult, KrakenSocketCancelOrdersRequestV2>(requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            return result.As<KrakenOrderResult>(result.Data?.Result);
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<CallResult<KrakenStreamCancelAllResult>> CancelAllOrdersAsync(CancellationToken ct = default)
        {
            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<KrakenStreamCancelAllResult>(token.Error!);

            var request = new KrakenSocketAuthRequestV2
            {
                Token = token.Data
            };
            var requestMessage = new KrakenSocketRequestV2<KrakenSocketAuthRequestV2>
            {
                Method = "cancel_all",
                RequestId = ExchangeHelpers.NextId(),
                Parameters = request
            };

            var query = new KrakenSpotQueryV2<KrakenStreamCancelAllResult, KrakenSocketAuthRequestV2>(requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            return result.As<KrakenStreamCancelAllResult>(result.Data?.Result);
        }

        #endregion

        #region Cancel All Orders After

        /// <inheritdoc />
        public async Task<CallResult<KrakenCancelAfterResult>> CancelAllOrdersAfterAsync(TimeSpan timeout, CancellationToken ct = default)
        {
            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<KrakenCancelAfterResult>(token.Error!);

            var request = new KrakenSocketCancelAfterRequest
            {
                Token = token.Data,
                Timeout = (int)timeout.TotalSeconds
            };
            var requestMessage = new KrakenSocketRequestV2<KrakenSocketCancelAfterRequest>
            {
                Method = "cancel_all_orders_after",
                RequestId = ExchangeHelpers.NextId(),
                Parameters = request
            };

            var query = new KrakenSpotQueryV2<KrakenCancelAfterResult, KrakenSocketCancelAfterRequest>(requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            return result.As<KrakenCancelAfterResult>(result.Data?.Result);
        }

        #endregion

        #endregion

        /// <inheritdoc />
        protected override async Task<CallResult> RevitalizeRequestAsync(Subscription subscription)
        {
            var krakenSubscription = (KrakenSubscription)subscription;

            if (!krakenSubscription.TokenRequired)
                return new CallResult(null);

            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return token.AsDataless();

            krakenSubscription.Token = token.Data;
            return new CallResult(null);
        }

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
            else
                _logger.LogWarning("Failed to retrieve websocket token: {Error}", result.Error);

            return result.As<string>(result.Data?.Token);
        }

        private class CachedToken
        {
            public string Token { get; set; } = string.Empty;
            public DateTime Expire { get; set; }
        }
    }
}