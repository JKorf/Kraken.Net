using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.TokenManagement;
using Kraken.Net.Clients.MessageHandlers;
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
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal partial class KrakenSocketClientSpotApi : SocketApiClient<KrakenEnvironment, KrakenAuthenticationProvider, KrakenCredentials>, IKrakenSocketClientSpotApi
    {
        private static readonly HashSet<string> _channelsWithoutSymbol =
        [
            "heartbeat",
            "status",
            "instrument",
            "executions",
            "balances"
        ];

        private readonly ILoggerFactory? _loggerFactory;
        private KrakenRestClient? _tokenClient;
        internal TokenManager TokenManager { get; }
        private KrakenRestClient TokenClient
        {
            get
            {
                if (_tokenClient == null)
                {
                    _tokenClient = new KrakenRestClient(null, _loggerFactory, Options.Create(new KrakenRestOptions
                    {
                        ApiCredentials = ApiCredentials,
                        Environment = ClientOptions.Environment,
                        Proxy = ClientOptions.Proxy,
                        NonceProvider = ClientOptions.NonceProvider,
                        OutputOriginalData = ClientOptions.OutputOriginalData
                    }));
                }

                return _tokenClient;
            }
        }
        protected override ErrorMapping ErrorMapping => KrakenErrors.SpotMapping;

        #region fields
        private readonly string _privateBaseAddress;

        /// <inheritdoc />
        public new KrakenSocketOptions ClientOptions => (KrakenSocketOptions)base.ClientOptions;

        #endregion

        #region ctor

        internal KrakenSocketClientSpotApi(ILoggerFactory? loggerFactory, KrakenSocketOptions options) :
            base(loggerFactory, KrakenExchange.Metadata.Id, options.Environment.SpotSocketPublicAddress, options, options.SpotOptions)
        {
            _loggerFactory = loggerFactory;
            _privateBaseAddress = options.Environment.SpotSocketPrivateAddress;

            AddSystemSubscription(new HeartbeatSubscription(_logger));
            AddSystemSubscription(new SystemStatusSubscription(_logger));

            RateLimiter = KrakenExchange.RateLimiter.SpotSocket;
                        
            SetDedicatedConnection(_privateBaseAddress, false);

            RegisterPeriodicQuery(
                "Ping",
                TimeSpan.FromSeconds(30),
                x => new KrakenSpotPingQuery(),
                (connection, result) =>
                {
                    if (result.Error?.ErrorType == ErrorType.Timeout)
                    {
                        // Ping timeout, reconnect
                        _logger.LogWarning("[Sckt {SocketId}] Ping response timeout, reconnecting", connection.SocketId);
                        _ = connection.TriggerReconnectAsync();
                    }
                });

            TokenManager = new TokenManager(
                KrakenExchange.Metadata.Id,
                loggerFactory,
                TimeSpan.FromMinutes(15),
                TimeSpan.FromMinutes(14),
                startToken: StartListenKeyAsync,
                retentionPolicy: TokenRetentionPolicy.RetainUntilExpired);
        }
        #endregion

        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(KrakenExchange._serializerContext));

        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new KrakenSocketSpotMessageHandler();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            if (quoteAsset.Equals(SharedSymbol.UsdOrStable, StringComparison.InvariantCultureIgnoreCase))
                quoteAsset = "USD";

            // Note; assets not mapped to exchange name, websocket API expects BTC (common name) instead of XBT
            return $"{baseAsset.ToUpperInvariant()}/{quoteAsset.ToUpperInvariant()}";
        }

        /// <inheritdoc />
        public IKrakenSocketClientSpotApiShared SharedClient => this;

        /// <inheritdoc />
        protected override KrakenAuthenticationProvider CreateAuthenticationProvider(KrakenCredentials credentials)
            => new KrakenAuthenticationProvider(credentials, ClientOptions.NonceProvider ?? new KrakenNonceProvider());

        public override KrakenAuthenticationProvider? AuthenticationProvider
        {
            get
            {
                if (!_authProviderInitialized)
                {
                    if (ApiCredentials?.Spot != null)
                        _authenticationProvider = CreateAuthenticationProvider(ApiCredentials);

                    _authProviderInitialized = true;
                }

                return _authenticationProvider;
            }
            protected set => base.AuthenticationProvider = value;
        }

        #region Streams

        #region System Status

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToSystemStatusUpdatesAsync(Action<DataEvent<KrakenStreamSystemStatus>> handler, CancellationToken ct = default)
        {
            var subscription = new KrakenSystemStatusSubscription(_logger, this, handler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Ticker

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenTickerUpdate>> handler, TriggerEvent? eventTrigger = null, bool? snapshot = null, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenTickerUpdate[]>>((receiveTime, originalData, data) =>
            {
                if (data.Timestamp != null)
                    UpdateTimeOffset(data.Timestamp.Value);

                handler.Invoke(
                    new DataEvent<KrakenTickerUpdate>(KrakenExchange.ExchangeName, data.Data.First(), receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithSymbol(data.Data.First().Symbol)
                        .WithDataTimestamp(data.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new KrakenSubscriptionV2<KrakenTickerUpdate[]>(_logger, this, "ticker", [symbol], null, snapshot, null, eventTrigger, false, internalHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenTickerUpdate>> handler, TriggerEvent? eventTrigger = null, bool? snapshot = null, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenTickerUpdate[]>>((receiveTime, originalData, data) =>
            {
                if (data.Timestamp != null)
                    UpdateTimeOffset(data.Timestamp.Value);

                handler.Invoke(
                    new DataEvent<KrakenTickerUpdate>(KrakenExchange.ExchangeName, data.Data.First(), receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithSymbol(data.Data.First().Symbol)
                        .WithDataTimestamp(data.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new KrakenSubscriptionV2<KrakenTickerUpdate[]>(_logger, this, "ticker", symbols.ToArray(), null, snapshot, null, eventTrigger, false, internalHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Kline

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<KrakenKlineUpdate[]>> handler, bool? snapshot = null, CancellationToken ct = default)
            => SubscribeToKlineUpdatesAsync([symbol], interval, handler, snapshot, ct);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<KrakenKlineUpdate[]>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenKlineUpdate[]>>((receiveTime, originalData, data) =>
            {
                if (data.Timestamp != null)
                    UpdateTimeOffset(data.Timestamp.Value);

                handler.Invoke(
                    new DataEvent<KrakenKlineUpdate[]>(KrakenExchange.ExchangeName, data.Data, receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithSymbol(data.Data.First().Symbol)
                        .WithDataTimestamp(data.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new KrakenSubscriptionV2<KrakenKlineUpdate[]>(_logger, this, "ohlc", symbols.ToArray(), interval, snapshot, null, null, false, internalHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Trade

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<KrakenTradeUpdate[]>> handler, bool? snapshot = null, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync([symbol], handler, snapshot, ct);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenTradeUpdate[]>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenTradeUpdate[]>>((receiveTime, originalData, data) =>
            {
                if (data.Timestamp != null)
                    UpdateTimeOffset(data.Timestamp.Value);

                handler.Invoke(
                    new DataEvent<KrakenTradeUpdate[]>(KrakenExchange.ExchangeName, data.Data, receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithSymbol(data.Data.First().Symbol)
                        .WithDataTimestamp(data.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new KrakenSubscriptionV2<KrakenTradeUpdate[]>(_logger, this, "trade", symbols.ToArray(), null, snapshot, null, null, false, internalHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Aggregated Order Book

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToAggregatedOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
            => SubscribeToAggregatedOrderBookUpdatesAsync([symbol], depth, handler, snapshot, ct);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToAggregatedOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<KrakenBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 10, 25, 100, 500, 1000);

            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenBookUpdate[]>>((receiveTime, originalData, data) =>
            {
                var item = data.Data.First();
                UpdateTimeOffset(item.Timestamp);

                handler.Invoke(
                    new DataEvent<KrakenBookUpdate>(KrakenExchange.ExchangeName, item, receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithSymbol(item.Symbol)
                        .WithDataTimestamp(item.Timestamp, GetTimeOffset())
                    );
            });
            var subscription = new KrakenSubscriptionV2<KrakenBookUpdate[]>(_logger, this, "book", symbols.ToArray(), null, snapshot, depth, null, false, internalHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Order Book

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToIndividualOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenIndividualBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
            => SubscribeToIndividualOrderBookUpdatesAsync([symbol], depth, handler, snapshot, ct);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToIndividualOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<KrakenIndividualBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 10, 100, 1000);

            var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    KrakenExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Spot!.Key), ct).ConfigureAwait(false);
            if (!leaseResult.Success)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, leaseResult.Error);

            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenIndividualBookUpdate[]>>((receiveTime, originalData, data) =>
            {
                var item = data.Data.First();
                if (data.Timestamp != null)
                    UpdateTimeOffset(data.Timestamp.Value);

                handler.Invoke(
                    new DataEvent<KrakenIndividualBookUpdate>(KrakenExchange.ExchangeName, item, receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithSymbol(item.Symbol)
                        .WithDataTimestamp(data.Timestamp, GetTimeOffset())
                    );
            });
            var subscription = new KrakenSubscriptionV2<KrakenIndividualBookUpdate[]>(_logger, this, "level3", symbols.ToArray(), null, snapshot, depth, null, true, internalHandler)
            {
                TokenLease = leaseResult.Data
            };
            return await SubscribeAsync(_privateBaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Instrument

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToInstrumentUpdatesAsync(Action<DataEvent<KrakenInstrumentUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenInstrumentUpdate>>((receiveTime, originalData, data) =>
            {
                var type = data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update;
                if (data.Timestamp != null && type != SocketUpdateType.Snapshot)
                    UpdateTimeOffset(data.Timestamp.Value);

                handler.Invoke(
                    new DataEvent<KrakenInstrumentUpdate>(KrakenExchange.ExchangeName, data.Data, receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(type)
                        .WithDataTimestamp(data.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new KrakenSubscriptionV2<KrakenInstrumentUpdate>(_logger, this, "instrument", null, null, snapshot, null, null, false, internalHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Balance

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<KrakenBalanceSnapshot[]>>? snapshotHandler, Action<DataEvent<KrakenBalanceUpdate[]>> updateHandler, bool? snapshot = null, CancellationToken ct = default)
        {
            var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    KrakenExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Spot!.Key), ct).ConfigureAwait(false);
            if (!leaseResult.Success)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, leaseResult.Error);

            var subscription = new KrakenBalanceSubscription(_logger, this, snapshot, snapshotHandler, updateHandler)
            {
                TokenLease = leaseResult.Data
            };
            return await SubscribeAsync(_privateBaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Order

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(
            Action<DataEvent<KrakenOrderUpdate[]>> updateHandler,
            bool? snapshotOrder = null,
            bool? snapshotTrades = null,
            CancellationToken ct = default)
        {
            var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    KrakenExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Spot!.Key), ct).ConfigureAwait(false);
            if (!leaseResult.Success)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, leaseResult.Error);

            var subscription = new KrakenOrderSubscription(_logger, this, snapshotOrder, snapshotTrades, updateHandler)
            {
                TokenLease = leaseResult.Data
            };
            return await SubscribeAsync(_privateBaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #endregion

        #region Queries

        #region Place Order

        /// <inheritdoc />
        public async Task<QueryResult<KrakenOrderResult>> PlaceOrderAsync(
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
            var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    KrakenExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Spot!.Key), ct).ConfigureAwait(false);
            if (!leaseResult.Success)
                return QueryResult.Fail<KrakenOrderResult>(Exchange, leaseResult.Error);

            var request = new KrakenSocketPlaceOrderRequestV2
            {
                Token = leaseResult.Data.Token.Token,
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
                EffectiveTime = startTime?.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                ExpireTime = expireTime?.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                Deadline = deadline?.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
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

            var query = new KrakenSpotQueryV2<KrakenOrderResult, KrakenSocketPlaceOrderRequestV2>(this, requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);

            _ = leaseResult.Data.ReleaseAsync();
            if (!result.Success)
                return QueryResult.Fail<KrakenOrderResult>(result);

            return QueryResult.Ok(result, result.Data.Result);
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<QueryResult<CallResult<KrakenOrderResult>[]>> PlaceMultipleOrdersAsync(
            string symbol,
            IEnumerable<KrakenSocketOrderRequest> orders,
            DateTime? deadline = null,
            bool? validateOnly = null,
            CancellationToken ct = default)
        {
            var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    KrakenExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Spot!.Key), ct).ConfigureAwait(false);
            if (!leaseResult.Success)
                return QueryResult.Fail<CallResult<KrakenOrderResult>[]> (Exchange, leaseResult.Error);

            var request = new KrakenSocketPlaceMultipleOrderRequestV2
            {
                Token = leaseResult.Data.Token.Token,
                Symbol = symbol,
                ValidateOnly = validateOnly,
                Deadline = deadline?.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                Orders = orders.ToArray()
            };

            var requestMessage = new KrakenSocketRequestV2<KrakenSocketPlaceMultipleOrderRequestV2>
            {
                Method = "batch_add",
                RequestId = ExchangeHelpers.NextId(),
                Parameters = request
            };

            var query = new KrakenSpotQueryV2<KrakenOrderResult[], KrakenSocketPlaceMultipleOrderRequestV2>(this, requestMessage, false);
            var resultData = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            _ = leaseResult.Data.ReleaseAsync();
            if (!resultData.Success)
                return QueryResult.Fail<CallResult<KrakenOrderResult>[]>(resultData);

            if (!resultData.Data.Success && resultData.Data.Result.Any() != true)
                return QueryResult.Fail<CallResult<KrakenOrderResult>[]>(resultData, new ServerError(resultData.Data.Error!, GetErrorInfo(resultData.Data.Error!, resultData.Data.Error)));

            var result = new List<CallResult<KrakenOrderResult>>();
            foreach (var item in resultData.Data.Result)
            {
                if (!string.IsNullOrEmpty(item.Error) || !string.IsNullOrEmpty(item.Status))
                {
                    var error = item.Error ?? item.Status!;
                    result.Add(CallResult.Fail<KrakenOrderResult>(new ServerError(error, GetErrorInfo(error, error))));
                }
                else {
                    result.Add(CallResult.Ok(item));
                }
            }

            if (result.All(x => !x.Success))
                return QueryResult.Fail<CallResult<KrakenOrderResult>[]>(resultData, new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, "All orders failed")), result.ToArray());

            return QueryResult.Ok(resultData, result.ToArray());
        }

        #endregion

        #region Edit Order

        /// <inheritdoc />
        public async Task<QueryResult<KrakenSocketAmendOrderResult>> EditOrderAsync(
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
            var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    KrakenExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Spot!.Key), ct).ConfigureAwait(false);
            if (!leaseResult.Success)
                return QueryResult.Fail<KrakenSocketAmendOrderResult>(Exchange, leaseResult.Error);

            var request = new KrakenSocketEditOrderRequest
            {
                Token = leaseResult.Data.Token.Token,
                ClientOrderId = clientOrderId,
                Deadline = deadline?.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
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

            var query = new KrakenSpotQueryV2<KrakenSocketAmendOrderResult, KrakenSocketEditOrderRequest>(this, requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            _ = leaseResult.Data.ReleaseAsync();
            if (!result.Success)
                return QueryResult.Fail<KrakenSocketAmendOrderResult>(result);

            return QueryResult.Ok(result, result.Data.Result);
        }

        #endregion

        #region Replace Order

        /// <inheritdoc />
        public async Task<QueryResult<KrakenSocketReplaceOrderResult>> ReplaceOrderAsync(
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
            var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    KrakenExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Spot!.Key), ct).ConfigureAwait(false);
            if (!leaseResult.Success)
                return QueryResult.Fail<KrakenSocketReplaceOrderResult>(Exchange, leaseResult.Error);

            var request = new KrakenSocketReplaceOrderRequest
            {
                Token = leaseResult.Data.Token.Token,
                Deadline = deadline?.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
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

            var query = new KrakenSpotQueryV2<KrakenSocketReplaceOrderResult, KrakenSocketReplaceOrderRequest>(this, requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            _ = leaseResult.Data.ReleaseAsync();
            if (!result.Success)
                return QueryResult.Fail<KrakenSocketReplaceOrderResult>(result);

            return QueryResult.Ok(result, result.Data.Result);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public Task<QueryResult<KrakenOrderResult>> CancelOrderAsync(string orderId, CancellationToken ct = default)
            => CancelOrdersAsync(new[] { orderId }, ct);

        /// <inheritdoc />
        public async Task<QueryResult<KrakenOrderResult>> CancelOrdersAsync(IEnumerable<string> orderIds, CancellationToken ct = default)
        {
            var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    KrakenExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Spot!.Key), ct).ConfigureAwait(false);
            if (!leaseResult.Success)
                return QueryResult.Fail<KrakenOrderResult>(Exchange, leaseResult.Error);

            var request = new KrakenSocketCancelOrdersRequestV2
            {
                OrderIds = orderIds.ToArray(),
                Token = leaseResult.Data.Token.Token,
            };
            var requestMessage = new KrakenSocketRequestV2<KrakenSocketCancelOrdersRequestV2>
            {
                Method = "cancel_order",
                RequestId = ExchangeHelpers.NextId(),
                Parameters = request
            };

            var query = new KrakenSpotQueryV2<KrakenOrderResult, KrakenSocketCancelOrdersRequestV2>(this, requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            _ = leaseResult.Data.ReleaseAsync();
            if (!result.Success)
                return QueryResult.Fail<KrakenOrderResult>(result);

            return QueryResult.Ok(result, result.Data.Result);
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<QueryResult<KrakenStreamCancelAllResult>> CancelAllOrdersAsync(CancellationToken ct = default)
        {
            var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    KrakenExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Spot!.Key), ct).ConfigureAwait(false);
            if (!leaseResult.Success)
                return QueryResult.Fail<KrakenStreamCancelAllResult>(Exchange, leaseResult.Error);

            var request = new KrakenSocketAuthRequestV2
            {
                Token = leaseResult.Data.Token.Token,
            };
            var requestMessage = new KrakenSocketRequestV2<KrakenSocketAuthRequestV2>
            {
                Method = "cancel_all",
                RequestId = ExchangeHelpers.NextId(),
                Parameters = request
            };

            var query = new KrakenSpotQueryV2<KrakenStreamCancelAllResult, KrakenSocketAuthRequestV2>(this, requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            _ = leaseResult.Data.ReleaseAsync();
            if (!result.Success)
                return QueryResult.Fail<KrakenStreamCancelAllResult>(result);

            return QueryResult.Ok(result, result.Data.Result);
        }

        #endregion

        #region Cancel All Orders After

        /// <inheritdoc />
        public async Task<QueryResult<KrakenCancelAfterResult>> CancelAllOrdersAfterAsync(TimeSpan timeout, CancellationToken ct = default)
        {
            var leaseResult = await TokenManager.AcquireAsync(new TokenScope(
                    KrakenExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Spot!.Key), ct).ConfigureAwait(false);
            if (!leaseResult.Success)
                return QueryResult.Fail<KrakenCancelAfterResult>(Exchange, leaseResult.Error);

            var request = new KrakenSocketCancelAfterRequest
            {
                Token = leaseResult.Data.Token.Token,
                Timeout = (int)timeout.TotalSeconds
            };
            var requestMessage = new KrakenSocketRequestV2<KrakenSocketCancelAfterRequest>
            {
                Method = "cancel_all_orders_after",
                RequestId = ExchangeHelpers.NextId(),
                Parameters = request
            };

            var query = new KrakenSpotQueryV2<KrakenCancelAfterResult, KrakenSocketCancelAfterRequest>(this, requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            _ = leaseResult.Data.ReleaseAsync();
            if (!result.Success)
                return QueryResult.Fail<KrakenCancelAfterResult>(result);

            return QueryResult.Ok(result, result.Data.Result);
        }

        #endregion

        #endregion

        /// <inheritdoc />
        protected override async Task<CallResult> RevitalizeRequestAsync(Subscription subscription)
        {
            if (subscription.TokenLease == null)
                return CallResult.Ok(); // Not an authenticated subscription, no need to revitalize

            var scope = new TokenScope(
                    KrakenExchange.Metadata.Id,
                    EnvironmentName,
                    "Spot",
                    ApiCredentials!.Spot!.Key);

            return await TokenManager.AcquireAndReplaceAsync(subscription, scope).ConfigureAwait(false);
        }

        private async Task<CallResult<string>> StartListenKeyAsync(TokenScope tokenScope, CancellationToken ct)
        {
            if (EnvironmentName == "UnitTest")
                return CallResult.Ok("123");

            var result = await TokenClient.SpotApi.Account.GetWebsocketTokenAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return CallResult.Fail<string>(result.Error);

            return CallResult.Ok(result.Data.Token);
        }
    }
}
