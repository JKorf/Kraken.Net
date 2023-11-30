using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Objects.Options;
using Kraken.Net.Objects.Sockets;
using Kraken.Net.Objects.Sockets.Converters;
using Kraken.Net.Objects.Sockets.Queries;
using Kraken.Net.Objects.Sockets.Subscriptions;
using Kraken.Net.Objects.Sockets.Subscriptions.Spot;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class KrakenSocketClientSpotApi : SocketApiClient, IKrakenSocketClientSpotApi
    {
        #region fields                
        private readonly Dictionary<string, string> _symbolSynonyms;
        private readonly string _privateBaseAddress;

        /// <inheritdoc />
        public new KrakenSocketOptions ClientOptions => (KrakenSocketOptions)base.ClientOptions;

        public override SocketConverter StreamConverter { get; } = new KrakenSpotStreamConverter();
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
            //AddGenericHandler("AdditionalSubResponses", (messageEvent) => { });
        }
        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new KrakenAuthenticationProvider(credentials, ClientOptions.NonceProvider ?? new KrakenNonceProvider());

        #region methods

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSystemStatusUpdatesAsync(Action<DataEvent<KrakenStreamSystemStatus>> handler, CancellationToken ct = default)
        {
            var subscription = new KrakenListenSubscription<KrakenStreamSystemStatus>(_logger, handler);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamTick>> handler, CancellationToken ct = default)
        {
            symbol.ValidateKrakenWebsocketSymbol();
            var subSymbol = SymbolToServer(symbol);
            return await SubscribeInternalAsync(new KrakenSubscribeRequest("ticker", ExchangeHelpers.NextId(), subSymbol), handler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenStreamTick>> handler, CancellationToken ct = default)
        {
            var symbolArray = symbols.ToArray();
            for (var i = 0; i < symbolArray.Length; i++)
            {
                symbolArray[i].ValidateKrakenWebsocketSymbol();
                symbolArray[i] = SymbolToServer(symbolArray[i]);
            }
            return await SubscribeInternalAsync(new KrakenSubscribeRequest("ticker", ExchangeHelpers.NextId(), symbolArray), handler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<KrakenStreamKline>> handler, CancellationToken ct = default)
            => SubscribeToKlineUpdatesAsync(new[] { symbol }, interval, handler, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<KrakenStreamKline>> handler, CancellationToken ct = default)
        {
            foreach (var symbol in symbols)
                symbol.ValidateKrakenWebsocketSymbol();

            var subSymbols = symbols.Select(SymbolToServer);

            var intervalMinutes = int.Parse(JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)));
            return await SubscribeInternalAsync(new KrakenSubscribeRequest("ohlc", ExchangeHelpers.NextId(), subSymbols.ToArray()) { Details = new KrakenOHLCSubscriptionDetails(intervalMinutes) }, handler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<IEnumerable<KrakenTrade>>> handler, CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync(new[] { symbol }, handler, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IEnumerable<KrakenTrade>>> handler, CancellationToken ct = default)
        {
            foreach (var symbol in symbols)
                symbol.ValidateKrakenWebsocketSymbol();

            var subSymbols = symbols.Select(SymbolToServer);
            return await SubscribeInternalAsync(new KrakenSubscribeRequest("trade", ExchangeHelpers.NextId(), subSymbols.ToArray()), handler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamSpread>> handler, CancellationToken ct = default)
            => SubscribeToSpreadUpdatesAsync(new[] { symbol }, handler, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenStreamSpread>> handler, CancellationToken ct = default)
        {
            foreach (var symbol in symbols)
                symbol.ValidateKrakenWebsocketSymbol();

            var subSymbols = symbols.Select(SymbolToServer);
            return await SubscribeInternalAsync(new KrakenSubscribeRequest("spread", ExchangeHelpers.NextId(), subSymbols.ToArray()), handler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenStreamOrderBook>> handler, CancellationToken ct = default)
            => SubscribeToOrderBookUpdatesAsync(new[] { symbol }, depth, handler, ct);

        /// <inheritdoc />
		public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<KrakenStreamOrderBook>> handler, CancellationToken ct = default)
        {
            foreach (var symbol in symbols)
                symbol.ValidateKrakenWebsocketSymbol();

            depth.ValidateIntValues(nameof(depth), 10, 25, 100, 500, 1000);
            var subSymbols = symbols.Select(SymbolToServer);
            return await SubscribeInternalAsync(new KrakenSubscribeRequest("book", ExchangeHelpers.NextId(), subSymbols.ToArray()) { Details = new KrakenDepthSubscriptionDetails(depth) }, handler, ct).ConfigureAwait(false);
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

            return await SubscribeInternalAuthAsync(new KrakenSubscribeRequest("openOrders", ExchangeHelpers.NextId())
            {
                Details = new KrakenOpenOrdersSubscriptionDetails(socketToken)
            }, innerHandler, ct).ConfigureAwait(false);
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

            return await SubscribeInternalAuthAsync(new KrakenSubscribeRequest("ownTrades", ExchangeHelpers.NextId())
            {
                Details = new KrakenOwnTradesSubscriptionDetails(socketToken, snapshot)
            }, innerHandler, ct).ConfigureAwait(false);
        }

        private async Task<CallResult<UpdateSubscription>> SubscribeInternalAsync<T>(
            KrakenSubscribeRequest request,
            Action<DataEvent<T>> handler,
            CancellationToken ct)
        {
            var subscription = new KrakenSubscription<T>(_logger, request, handler);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        private async Task<CallResult<UpdateSubscription>> SubscribeInternalAuthAsync<T>(
            KrakenSubscribeRequest request,
            Action<DataEvent<T>> handler,
            CancellationToken ct)
        {
            var subscription = new KrakenAuthSubscription<T>(_logger, request, handler);
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
        protected override async Task<CallResult<object>> RevitalizeRequestAsync(object request)
        {
            var kRequest = (KrakenSubscribeRequest)request;
            var payloadType = kRequest.Details.GetType();
            if (payloadType == typeof(KrakenOpenOrdersSubscriptionDetails)
                || payloadType == typeof(KrakenOwnTradesSubscriptionDetails))
            {
                var apiCredentials = ApiOptions.ApiCredentials ?? ClientOptions.ApiCredentials;
                var restClient = new KrakenRestClient(x =>
                {
                    x.ApiCredentials = apiCredentials;
                    x.Environment = ClientOptions.Environment;
                });

                var newToken = await restClient.SpotApi.Account.GetWebsocketTokenAsync().ConfigureAwait(false);
                if (!newToken.Success)
                    return newToken.As<object>(null);

                if (payloadType == typeof(KrakenOpenOrdersSubscriptionDetails))
                    ((KrakenOpenOrdersSubscriptionDetails)kRequest.Details).Token = newToken.Data.Token;

                else if (payloadType == typeof(KrakenOwnTradesSubscriptionDetails))
                    ((KrakenOwnTradesSubscriptionDetails)kRequest.Details).Token = newToken.Data.Token;
            }

            return new CallResult<object>(kRequest);
        }
    }
}