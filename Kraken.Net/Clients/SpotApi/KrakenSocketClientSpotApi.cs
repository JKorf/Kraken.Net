using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets.Default;
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
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal partial class KrakenSocketClientSpotApi : SocketApiClient, IKrakenSocketClientSpotApi
    {
        private static readonly MessagePath _idPath = MessagePath.Get().Property("req_id");
        private static readonly MessagePath _methodPath = MessagePath.Get().Property("method");
        private static readonly MessagePath _channelPath = MessagePath.Get().Property("channel");
        private static readonly MessagePath _typePath = MessagePath.Get().Property("type");
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

        protected override ErrorMapping ErrorMapping => KrakenErrors.SpotMapping;

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
        }
        #endregion

        protected override IByteMessageAccessor CreateAccessor(WebSocketMessageType type) => new SystemTextJsonByteMessageAccessor(SerializerOptions.WithConverters(KrakenExchange._serializerContext));

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
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            var id = message.GetValue<string>(_idPath);
            if (id != null)
                return id;

            var channel = message.GetValue<string>(_channelPath);
            if (channel!.Equals("balances", StringComparison.Ordinal))
            {
                var type = message.GetValue<string?>(_typePath);
                if (type?.Equals("snapshot", StringComparison.Ordinal) == true)
                    return channel + type;

                return channel;
            }

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
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenTickerUpdate>> handler, TriggerEvent? eventTrigger = null, bool? snapshot = null, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenTickerUpdate[]>>((receiveTime, originalData, data) =>
            {
                handler.Invoke(
                    new DataEvent<KrakenTickerUpdate>(KrakenExchange.ExchangeName, data.Data.First(), receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithSymbol(data.Data.First().Symbol)
                        .WithDataTimestamp(data.Timestamp)
                    );
            });

            var subscription = new KrakenSubscriptionV2<KrakenTickerUpdate[]>(_logger, this, "ticker", [symbol], null, snapshot, null, eventTrigger, null, internalHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenTickerUpdate>> handler, TriggerEvent? eventTrigger = null, bool? snapshot = null, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenTickerUpdate[]>>((receiveTime, originalData, data) =>
            {
                handler.Invoke(
                    new DataEvent<KrakenTickerUpdate>(KrakenExchange.ExchangeName, data.Data.First(), receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithSymbol(data.Data.First().Symbol)
                        .WithDataTimestamp(data.Timestamp)
                    );
            });

            var subscription = new KrakenSubscriptionV2<KrakenTickerUpdate[]>(_logger, this, "ticker", symbols.ToArray(), null, snapshot, null, eventTrigger, null, internalHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Kline

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<KrakenKlineUpdate[]>> handler, bool? snapshot = null, CancellationToken ct = default)
            => SubscribeToKlineUpdatesAsync([symbol], interval, handler, snapshot, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<KrakenKlineUpdate[]>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenKlineUpdate[]>>((receiveTime, originalData, data) =>
            {
                handler.Invoke(
                    new DataEvent<KrakenKlineUpdate[]>(KrakenExchange.ExchangeName, data.Data, receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithSymbol(data.Data.First().Symbol)
                        .WithDataTimestamp(data.Timestamp)
                    );
            });

            var subscription = new KrakenSubscriptionV2<KrakenKlineUpdate[]>(_logger, this, "ohlc", symbols.ToArray(), interval, snapshot, null, null, null, internalHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Trade

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<KrakenTradeUpdate[]>> handler, bool? snapshot = null, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync([symbol], handler, snapshot, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenTradeUpdate[]>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenTradeUpdate[]>>((receiveTime, originalData, data) =>
            {
                handler.Invoke(
                    new DataEvent<KrakenTradeUpdate[]>(KrakenExchange.ExchangeName, data.Data, receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithSymbol(data.Data.First().Symbol)
                        .WithDataTimestamp(data.Timestamp)
                    );
            });

            var subscription = new KrakenSubscriptionV2<KrakenTradeUpdate[]>(_logger, this, "trade", symbols.ToArray(), null, snapshot, null, null, null, internalHandler);
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

            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenBookUpdate[]>>((receiveTime, originalData, data) =>
            {
                handler.Invoke(
                    new DataEvent<KrakenBookUpdate>(KrakenExchange.ExchangeName, data.Data.First(), receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithSymbol(data.Data.First().Symbol)
                        .WithDataTimestamp(data.Data.First().Timestamp)
                    );
            });
            var subscription = new KrakenSubscriptionV2<KrakenBookUpdate[]>(_logger, this, "book", symbols.ToArray(), null, snapshot, depth, null, null, internalHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Order Book

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToIndividualOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenIndividualBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
            => SubscribeToIndividualOrderBookUpdatesAsync([symbol], depth, handler, snapshot, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToIndividualOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<KrakenIndividualBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 10, 100, 1000);

            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<UpdateSubscription>(token.Error!);

            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenIndividualBookUpdate[]>>((receiveTime, originalData, data) =>
            {
                handler.Invoke(
                    new DataEvent<KrakenIndividualBookUpdate>(KrakenExchange.ExchangeName, data.Data.First(), receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithSymbol(data.Data.First().Symbol)
                        .WithDataTimestamp(data.Timestamp)
                    );
            });
            var subscription = new KrakenSubscriptionV2<KrakenIndividualBookUpdate[]>(_logger, this, "level3", symbols.ToArray(), null, snapshot, depth, null, token.Data, internalHandler);
            return await SubscribeAsync(_privateBaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Instrument

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToInstrumentUpdatesAsync(Action<DataEvent<KrakenInstrumentUpdate>> handler, bool? snapshot = null, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, KrakenSocketUpdateV2<KrakenInstrumentUpdate>>((receiveTime, originalData, data) =>
            {
                handler.Invoke(
                    new DataEvent<KrakenInstrumentUpdate>(KrakenExchange.ExchangeName, data.Data, receiveTime, originalData)
                        .WithStreamId(data.Channel)
                        .WithUpdateType(data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                        .WithDataTimestamp(data.Timestamp)
                    );
            });

            var subscription = new KrakenSubscriptionV2<KrakenInstrumentUpdate>(_logger, this, "instrument", null, null, snapshot, null, null, null, internalHandler);
            return await SubscribeAsync(BaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Balance

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<KrakenBalanceSnapshot[]>>? snapshotHandler, Action<DataEvent<KrakenBalanceUpdate[]>> updateHandler, bool? snapshot = null, CancellationToken ct = default)
        {
            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<UpdateSubscription>(token.Error!);

            var subscription = new KrakenBalanceSubscription(_logger, this, snapshot, token.Data, snapshotHandler, updateHandler);
            return await SubscribeAsync(_privateBaseAddress.AppendPath("v2"), subscription, ct).ConfigureAwait(false);
        }

        #endregion

        #region Order

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(
            Action<DataEvent<KrakenOrderUpdate[]>> updateHandler,
            bool? snapshotOrder = null,
            bool? snapshotTrades = null,
            CancellationToken ct = default)
        {
            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<UpdateSubscription>(token.Error!);

            var subscription = new KrakenOrderSubscription(_logger, this, snapshotOrder, snapshotTrades, token.Data, updateHandler);
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
            return result.As<KrakenOrderResult>(result.Data?.Result);
        }

        #endregion

        #region Place Multiple Orders

        /// <inheritdoc />
        public async Task<CallResult<CallResult<KrakenOrderResult>[]>> PlaceMultipleOrdersAsync(
            string symbol,
            IEnumerable<KrakenSocketOrderRequest> orders,
            DateTime? deadline = null,
            bool? validateOnly = null,
            CancellationToken ct = default)
        {
            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return new CallResult<CallResult<KrakenOrderResult>[]>(token.Error!);

            var request = new KrakenSocketPlaceMultipleOrderRequestV2
            {
                Token = token.Data,
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
            if (!resultData)
                return resultData.As<CallResult<KrakenOrderResult>[]>(default);

            if (!resultData.Data.Success && resultData.Data.Result.Any() != true)
                return resultData.AsError<CallResult<KrakenOrderResult>[]>(new ServerError(resultData.Data.Error!, GetErrorInfo(resultData.Data.Error!, resultData.Data.Error)));

            var result = new List<CallResult<KrakenOrderResult>>();
            foreach (var item in resultData.Data.Result)
            {
                if (!string.IsNullOrEmpty(item.Error) || !string.IsNullOrEmpty(item.Status))
                {
                    var error = item.Error ?? item.Status!;
                    result.Add(new CallResult<KrakenOrderResult>(item, null, new ServerError(error, GetErrorInfo(error, error))));
                }
                else {
                    result.Add(new CallResult<KrakenOrderResult>(item));
                }
            }

            if (result.All(x => !x.Success))
                return resultData.AsErrorWithData(new ServerError(new ErrorInfo(ErrorType.AllOrdersFailed, "All orders failed")), result.ToArray());

            return resultData.As(result.ToArray());
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

            var query = new KrakenSpotQueryV2<KrakenOrderResult, KrakenSocketCancelOrdersRequestV2>(this, requestMessage, false);
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

            var query = new KrakenSpotQueryV2<KrakenStreamCancelAllResult, KrakenSocketAuthRequestV2>(this, requestMessage, false);
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

            var query = new KrakenSpotQueryV2<KrakenCancelAfterResult, KrakenSocketCancelAfterRequest>(this, requestMessage, false);
            var result = await QueryAsync(_privateBaseAddress.AppendPath("v2"), query, ct).ConfigureAwait(false);
            return result.As<KrakenCancelAfterResult>(result.Data?.Result);
        }

        #endregion

        #endregion

        /// <inheritdoc />
        protected override async Task<CallResult> RevitalizeRequestAsync(Subscription subscription)
        {
            if (subscription is not KrakenSubscription krakenSubscription)
            {
                // Heartbeat subscription
                return CallResult.SuccessResult;
            }

            if (!krakenSubscription.TokenRequired)
                return CallResult.SuccessResult;

            var token = await GetTokenAsync().ConfigureAwait(false);
            if (!token)
                return token.AsDataless();

            krakenSubscription.Token = token.Data;
            return CallResult.SuccessResult;
        }

        private async Task<CallResult<string>> GetTokenAsync()
        {
            if (ApiCredentials == null)
                return new CallResult<string>(new NoApiCredentialsError());

            if (_tokenCache.TryGetValue(ApiCredentials.Key, out var token) && token.Expire > DateTime.UtcNow)
                return new CallResult<string>(token.Token);

            if (ClientOptions.Environment.Name == "UnitTest")
                return new CallResult<string>("123");

            _logger.LogDebug("Requesting websocket token");
            var restClient = new KrakenRestClient(x =>
            {
                x.ApiCredentials = ApiCredentials;
                x.Environment = ClientOptions.Environment;
                x.NonceProvider = ClientOptions.NonceProvider;
                x.Proxy = ClientOptions.Proxy;
                x.RequestTimeout = ClientOptions.RequestTimeout;
            });

            var result = await restClient.SpotApi.Account.GetWebsocketTokenAsync().ConfigureAwait(false);
            if (result)
                _tokenCache[ApiCredentials.Key] = new CachedToken { Token = result.Data.Token, Expire = DateTime.UtcNow.AddSeconds(result.Data.Expires - 5) };
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
