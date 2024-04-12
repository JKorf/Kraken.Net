using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Kraken.Net.ExtensionMethods;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Objects.Options;
using Kraken.Net.Objects.Sockets;
using Kraken.Net.Objects.Sockets.Queries;
using Kraken.Net.Objects.Sockets.Subscriptions.Spot;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class KrakenSocketClientSpotApi : SocketApiClient, IKrakenSocketClientSpotApi
    {
        private static readonly MessagePath _idPath = MessagePath.Get().Property("reqid");
        private static readonly MessagePath _eventPath = MessagePath.Get().Property("event");
        private static readonly MessagePath _item1Path = MessagePath.Get().Index(1);
        private static readonly MessagePath _item2Path = MessagePath.Get().Index(2);
        private static readonly MessagePath _item3Path = MessagePath.Get().Index(3);
        private static readonly MessagePath _item4Path = MessagePath.Get().Index(4);

        #region fields                
        private readonly Dictionary<string, string> _symbolSynonyms;
        private readonly string _privateBaseAddress;

        /// <inheritdoc />
        public new KrakenSocketOptions ClientOptions => (KrakenSocketOptions)base.ClientOptions;

        #endregion

        #region ctor

        internal KrakenSocketClientSpotApi(ILogger logger, KrakenSocketOptions options) :
            base(logger, options.Environment.SpotSocketPublicAddress, options, options.SpotOptions)
        {
            _privateBaseAddress = options.Environment.SpotSocketPrivateAddress;
            _symbolSynonyms = new Dictionary<string, string>
            {
                { "BTC", "XBT"},
                { "DOGE", "XDG" }
            };

            AddSystemSubscription(new HeartbeatSubscription(_logger));
            AddSystemSubscription(new SystemStatusSubscription(_logger));

            RateLimiter = KrakenExchange.RateLimiters.SpotSocket;
        }
        #endregion

        /// <inheritdoc />
        public override string? GetListenerIdentifier(IMessageAccessor message)
        {
            var id = message.GetValue<string>(_idPath);
            if (id != null)
                return id;

            var evnt = message.GetValue<string>(_eventPath);
            if (evnt != null)
                return evnt;

            var arr3 = message.GetValue<string>(_item3Path);
            var arr4 = message.GetValue<string>(_item4Path);
            if (arr4 != null)
                return arr3!.ToLowerInvariant() + "-" + arr4.ToLowerInvariant();

            var arr2 = message.GetValue<string>(_item2Path);
            if (arr2 != null)
                return arr2.ToLowerInvariant() + "-" + arr3!.ToLowerInvariant();

            var arr1 = message.GetValue<string>(_item1Path);

            return arr1;
        }

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new KrakenAuthenticationProvider(credentials, ClientOptions.NonceProvider ?? new KrakenNonceProvider());

        #region methods

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSystemStatusUpdatesAsync(Action<DataEvent<KrakenStreamSystemStatus>> handler, CancellationToken ct = default)
        {
            var subscription = new KrakenSystemStatusSubscription(_logger, handler);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamTick>> handler, CancellationToken ct = default)
        {
            var subSymbol = SymbolToServer(symbol);
            var subscription = new KrakenSubscription<KrakenStreamTick>(_logger, "ticker", new[] { symbol }, null, null, handler);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenStreamTick>> handler, CancellationToken ct = default)
        {
            var symbolArray = symbols.ToArray();
            for (var i = 0; i < symbolArray.Length; i++)
            {
                symbolArray[i] = SymbolToServer(symbolArray[i]);
            }

            var subscription = new KrakenSubscription<KrakenStreamTick>(_logger, "ticker", symbols, null, null, handler);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<KrakenStreamKline>> handler, CancellationToken ct = default)
            => SubscribeToKlineUpdatesAsync(new[] { symbol }, interval, handler, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<KrakenStreamKline>> handler, CancellationToken ct = default)
        {
            var subSymbols = symbols.Select(SymbolToServer);

            var subscription = new KrakenSubscription<KrakenStreamKline>(_logger, "ohlc", subSymbols.ToArray(), int.Parse(JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))), null, handler);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<IEnumerable<KrakenTrade>>> handler, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync(new[] { symbol }, handler, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IEnumerable<KrakenTrade>>> handler, CancellationToken ct = default)
        {
            var subSymbols = symbols.Select(SymbolToServer);
            var subscription = new KrakenSubscription<IEnumerable<KrakenTrade>>(_logger, "trade", symbols.ToArray(), null, null, handler);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamSpread>> handler, CancellationToken ct = default)
            => SubscribeToSpreadUpdatesAsync(new[] { symbol }, handler, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenStreamSpread>> handler, CancellationToken ct = default)
        {
            var subscription = new KrakenSubscription<KrakenStreamSpread>(_logger, "spread", symbols.ToArray(), null, null, handler);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenStreamOrderBook>> handler, CancellationToken ct = default)
            => SubscribeToOrderBookUpdatesAsync(new[] { symbol }, depth, handler, ct);

        /// <inheritdoc />
		public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<KrakenStreamOrderBook>> handler, CancellationToken ct = default)
        {
            depth.ValidateIntValues(nameof(depth), 10, 25, 100, 500, 1000);
            var subSymbols = symbols.Select(SymbolToServer);

            var subscription = new KrakenBookSubscription(_logger, symbols.ToArray(), depth, handler);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string socketToken, Action<DataEvent<IEnumerable<KrakenStreamOrder>>> handler, CancellationToken ct = default)
        {
            var innerHandler = new Action<DataEvent<KrakenAuthSocketUpdate<IEnumerable<Dictionary<string, KrakenStreamOrder>>>>>(data =>
            {
                foreach (var item in data.Data.Data)
                {
                    foreach (var value in item)
                    {
                        value.Value.Id = value.Key;
                        value.Value.SequenceNumber = data.Data.Sequence.Sequence;
                    }
                }

                handler.Invoke(data.As<IEnumerable<KrakenStreamOrder>>(data.Data.Data.SelectMany(d => d.Values).ToList()));
            });

            var subscription = new KrakenAuthSubscription<IEnumerable<Dictionary<string, KrakenStreamOrder>>>(_logger, "openOrders", socketToken, innerHandler);
            return await SubscribeAsync(_privateBaseAddress, subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(string socketToken, Action<DataEvent<IEnumerable<KrakenStreamUserTrade>>> handler, CancellationToken ct = default)
            => SubscribeToUserTradeUpdatesAsync(socketToken, true, handler, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(string socketToken, bool snapshot, Action<DataEvent<IEnumerable<KrakenStreamUserTrade>>> handler, CancellationToken ct = default)
        {
            var innerHandler = new Action<DataEvent<KrakenAuthSocketUpdate<IEnumerable<Dictionary<string, KrakenStreamUserTrade>>>>>(data =>
            {
                foreach (var item in data.Data.Data)
                {
                    foreach (var value in item)
                    {
                        value.Value.Id = value.Key;
                        value.Value.SequenceNumber = data.Data.Sequence.Sequence;
                    }
                }
                handler.Invoke(data.As(data.Data.Data.SelectMany(d => d.Values)));
            });

            var subscription = new KrakenAuthSubscription<IEnumerable<Dictionary<string, KrakenStreamUserTrade>>>(_logger, "ownTrades", socketToken, innerHandler);
            return await SubscribeAsync(_privateBaseAddress, subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<KrakenStreamPlacedOrder>> PlaceOrderAsync(
            string websocketToken,
            string symbol,
            OrderType type,
            OrderSide side,
            decimal quantity,
            uint? clientOrderId = null,
            decimal? price = null,
            decimal? secondaryPrice = null,
            decimal? leverage = null,
            DateTime? startTime = null,
            DateTime? expireTime = null,
            bool? validateOnly = null,
            OrderType? closeOrderType = null,
            decimal? closePrice = null,
            decimal? secondaryClosePrice = null,
            IEnumerable<OrderFlags>? flags = null,
            bool? reduceOnly = null)
        {
            var request = new KrakenSocketPlaceOrderRequest
            {
                Event = "addOrder",
                Token = websocketToken,
                Symbol = symbol,
                Type = side,
                OrderType = type,
                Volume = quantity.ToString(CultureInfo.InvariantCulture),
                ClientOrderId = clientOrderId?.ToString(),
                Price = price?.ToString(CultureInfo.InvariantCulture),
                SecondaryPrice = secondaryPrice?.ToString(CultureInfo.InvariantCulture),
                Leverage = leverage?.ToString(CultureInfo.InvariantCulture),
                StartTime = (startTime.HasValue && startTime > DateTime.UtcNow) ? DateTimeConverter.ConvertToSeconds(startTime).ToString() : null,
                ExpireTime = expireTime.HasValue ? DateTimeConverter.ConvertToSeconds(expireTime).ToString() : null,
                ValidateOnly = validateOnly,
                CloseOrderType = closeOrderType,
                ClosePrice = closePrice?.ToString(CultureInfo.InvariantCulture),
                SecondaryClosePrice = secondaryClosePrice?.ToString(CultureInfo.InvariantCulture),
                ReduceOnly = reduceOnly,
                RequestId = ExchangeHelpers.NextId(),
                Flags = flags == null ? null : string.Join(",", flags.Select(f => JsonConvert.SerializeObject(f, new OrderFlagsConverter(false))))
            };

            var query = new KrakenSpotQuery<KrakenStreamPlacedOrder>(request, false);
            return await QueryAsync(_privateBaseAddress, query).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<bool>> CancelOrderAsync(string websocketToken, string orderId)
            => CancelOrdersAsync(websocketToken, new[] { orderId });

        /// <inheritdoc />
        public async Task<CallResult<bool>> CancelOrdersAsync(string websocketToken, IEnumerable<string> orderIds)
        {
            var request = new KrakenSocketCancelOrdersRequest
            {
                Event = "cancelOrder",
                OrderIds = orderIds,
                Token = websocketToken,
                RequestId = ExchangeHelpers.NextId()
            };

            var query = new KrakenSpotQuery<KrakenQueryEvent>(request, false);
            var result = await QueryAsync(_privateBaseAddress, query).ConfigureAwait(false);
            return result.As(result.Success);
        }

        /// <inheritdoc />
        public async Task<CallResult<KrakenStreamCancelAllResult>> CancelAllOrdersAsync(string websocketToken)
        {
            var request = new KrakenSocketAuthRequest
            {
                Event = "cancelAll",
                Token = websocketToken,
                RequestId = ExchangeHelpers.NextId()
            };

            var query = new KrakenSpotQuery<KrakenStreamCancelAllResult>(request, false);
            return await QueryAsync(_privateBaseAddress, query).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<KrakenStreamCancelAfterResult>> CancelAllOrdersAfterAsync(string websocketToken, TimeSpan timeout)
        {
            var request = new KrakenSocketCancelAfterRequest
            {
                Event = "cancelAllOrdersAfter",
                Token = websocketToken,
                Timeout = (int)Math.Round(timeout.TotalSeconds),
                RequestId = ExchangeHelpers.NextId()
            };

            var query = new KrakenSpotQuery<KrakenStreamCancelAfterResult>(request, false);
            return await QueryAsync(_privateBaseAddress, query).ConfigureAwait(false);
        }
        #endregion

        /// <summary>
        /// Maps an input symbol to a server symbol
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected string SymbolToServer(string input)
        {
            var split = input.Split('/');
            var baseAsset = split[0];
            var quoteAsset = split[1];
            if (_symbolSynonyms.TryGetValue(baseAsset.ToUpperInvariant(), out var baseOutput))
                baseAsset = baseOutput;
            if (_symbolSynonyms.TryGetValue(baseAsset.ToUpperInvariant(), out var quoteOutput))
                quoteAsset = quoteOutput;
            return baseAsset + "/" + quoteAsset;
        }

        /// <inheritdoc />
        protected override async Task<CallResult> RevitalizeRequestAsync(Subscription subscription)
        {
            if (!(subscription is KrakenAuthSubscription authSubscription))
                return new CallResult(null);

            var apiCredentials = ApiOptions.ApiCredentials ?? ClientOptions.ApiCredentials;
            var restClient = new KrakenRestClient(x =>
            {
                x.ApiCredentials = apiCredentials;
                x.Environment = ClientOptions.Environment;
            });

            var newToken = await restClient.SpotApi.Account.GetWebsocketTokenAsync().ConfigureAwait(false);
            if (!newToken.Success)
                return newToken.AsDataless();

            authSubscription.UpdateToken(newToken.Data.Token);
            return new CallResult(null);
        }
    }
}