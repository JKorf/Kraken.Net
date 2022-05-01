using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Models.Socket;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class KrakenSocketClientSpotStreams : SocketApiClient, IKrakenSocketClientSpotStreams
    {
        #region fields                
        private readonly string _authBaseAddress;
        private readonly Dictionary<string, string> _symbolSynonyms;

        private readonly Log _log;
        private readonly KrakenSocketClientOptions _options;
        private readonly KrakenSocketClient _baseClient;
        #endregion

        #region ctor

        internal KrakenSocketClientSpotStreams(Log log, KrakenSocketClient baseClient, KrakenSocketClientOptions options) :
            base(options, options.SpotStreamsOptions)
        {
            _authBaseAddress = options.SpotStreamsOptions.BaseAddressAuthenticated;
            _log = log;
            _options = options;
            _baseClient = baseClient;

            _symbolSynonyms = new Dictionary<string, string>
            {
                { "BTC", "XBT"},
                { "DOGE", "XDG" }
            };
        }
        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new KrakenAuthenticationProvider(credentials, _options.NonceProvider ?? new KrakenNonceProvider());

        #region methods

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSystemStatusUpdatesAsync(Action<DataEvent<KrakenStreamSystemStatus>> handler, CancellationToken ct = default)
        {
            return await _baseClient.SubscribeInternalAsync(this, null, "SystemStatus", false, handler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamTick>> handler, CancellationToken ct = default)
        {
            symbol.ValidateKrakenWebsocketSymbol();
            var subSymbol = SymbolToServer(symbol);
            var internalHandler = new Action<DataEvent<KrakenSocketEvent<KrakenStreamTick>>>(data =>
            {
                handler(data.As(data.Data.Data, symbol));
            });
            return await _baseClient.SubscribeInternalAsync(this, new KrakenSubscribeRequest("ticker", KrakenSocketClient.NextIdInternal(), subSymbol), null, false, internalHandler, ct).ConfigureAwait(false);
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
            var internalHandler = new Action<DataEvent<KrakenSocketEvent<KrakenStreamTick>>>(data =>
            {
                handler(data.As(data.Data.Data, data.Data.Symbol));
            });

            return await _baseClient.SubscribeInternalAsync(this, new KrakenSubscribeRequest("ticker", KrakenSocketClient.NextIdInternal(), symbolArray), null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<KrakenStreamKline>> handler, CancellationToken ct = default)
        {
            symbol.ValidateKrakenWebsocketSymbol();
            var subSymbol = SymbolToServer(symbol);

            var intervalMinutes = int.Parse(JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)));
            var internalHandler = new Action<DataEvent<KrakenSocketEvent<KrakenStreamKline>>>(data =>
            {
                handler(data.As(data.Data.Data, symbol));
            });

            return await _baseClient.SubscribeInternalAsync(this, new KrakenSubscribeRequest("ohlc", KrakenSocketClient.NextIdInternal(), subSymbol) { Details = new KrakenOHLCSubscriptionDetails(intervalMinutes) }, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<IEnumerable<KrakenTrade>>> handler, CancellationToken ct = default)
        {
            symbol.ValidateKrakenWebsocketSymbol();
            var subSymbol = SymbolToServer(symbol);

            var internalHandler = new Action<DataEvent<KrakenSocketEvent<IEnumerable<KrakenTrade>>>>(data =>
            {
                handler(data.As(data.Data.Data, symbol));
            });
            return await _baseClient.SubscribeInternalAsync(this, new KrakenSubscribeRequest("trade", KrakenSocketClient.NextIdInternal(), subSymbol), null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamSpread>> handler, CancellationToken ct = default)
        {
            symbol.ValidateKrakenWebsocketSymbol();
            var subSymbol = SymbolToServer(symbol);
            var internalHandler = new Action<DataEvent<KrakenSocketEvent<KrakenStreamSpread>>>(data =>
            {
                handler(data.As(data.Data.Data, symbol));
            });
            return await _baseClient.SubscribeInternalAsync(this, new KrakenSubscribeRequest("spread", KrakenSocketClient.NextIdInternal(), subSymbol), null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenStreamOrderBook>> handler, CancellationToken ct = default)
        {
            symbol.ValidateKrakenWebsocketSymbol();
            depth.ValidateIntValues(nameof(depth), 10, 25, 100, 500, 1000);
            var subSymbol = SymbolToServer(symbol);

            var innerHandler = new Action<DataEvent<string>>(data =>
            {
                var token = data.Data.ToJToken(_log);
                if (token == null || token.Type != JTokenType.Array)
                {
                    _log.Write(LogLevel.Warning, "Failed to deserialize stream order book");
                    return;
                }
                var evnt = StreamOrderBookConverter.Convert((JArray)token);
                if (evnt == null)
                {
                    _log.Write(LogLevel.Warning, "Failed to deserialize stream order book");
                    return;
                }

                handler(data.As(evnt.Data, symbol));
            });

            return await _baseClient.SubscribeInternalAsync(this, new KrakenSubscribeRequest("book", KrakenSocketClient.NextIdInternal(), subSymbol) { Details = new KrakenDepthSubscriptionDetails(depth) }, null, false, innerHandler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string socketToken, Action<DataEvent<KrakenStreamOrder>> handler, CancellationToken ct = default)
        {
            var innerHandler = new Action<DataEvent<string>>(data =>
            {
                var token = data.Data.ToJToken(_log);
                if (token != null && token.Count() > 2)
                {
                    var seq = token[2]!["sequence"];
                    if (seq == null)
                        _log.Write(LogLevel.Warning, "Failed to deserialize stream order, no sequence");

                    var sequence = seq!.Value<int>();
                    if (token[0]!.Type == JTokenType.Array)
                    {
                        var dataArray = (JArray)token[0]!;
                        var deserialized = _baseClient.DeserializeInternal<Dictionary<string, KrakenStreamOrder>[]>(dataArray);
                        if (deserialized)
                        {
                            foreach (var entry in deserialized.Data)
                            {
                                foreach (var dEntry in entry)
                                {
                                    dEntry.Value.Id = dEntry.Key;
                                    dEntry.Value.SequenceNumber = sequence;
                                    handler?.Invoke(data.As(dEntry.Value, dEntry.Value.OrderDetails?.Symbol));
                                }
                            }
                            return;
                        }
                    }
                }

                _log.Write(LogLevel.Warning, "Failed to deserialize stream order");
            });

            return await _baseClient.SubscribeInternalAsync(this, _authBaseAddress, new KrakenSubscribeRequest("openOrders", KrakenSocketClient.NextIdInternal())
            {
                Details = new KrakenOpenOrdersSubscriptionDetails(socketToken)
            }, null, false, innerHandler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(string socketToken, Action<DataEvent<KrakenStreamUserTrade>> handler, CancellationToken ct = default)
            => SubscribeToUserTradeUpdatesAsync(socketToken, true, handler, ct);

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(string socketToken, bool snapshot, Action<DataEvent<KrakenStreamUserTrade>> handler, CancellationToken ct = default)
        {
            var innerHandler = new Action<DataEvent<string>>(data =>
            {
                var token = data.Data.ToJToken(_log);
                if (token != null && token.Count() > 2)
                {
                    var seq = token[2]!["sequence"];
                    if (seq == null)
                        _log.Write(LogLevel.Warning, "Failed to deserialize stream order, no sequence");

                    var sequence = seq!.Value<int>();
                    if (token[0]!.Type == JTokenType.Array)
                    {
                        var dataArray = (JArray)token[0]!;
                        var deserialized = _baseClient.DeserializeInternal<Dictionary<string, KrakenStreamUserTrade>[]>(dataArray);
                        if (deserialized)
                        {
                            foreach (var entry in deserialized.Data)
                            {
                                foreach (var item in entry)
                                {
                                    item.Value.Id = item.Key;
                                    item.Value.SequenceNumber = sequence;
                                    handler?.Invoke(data.As(item.Value, item.Value.Symbol));
                                }
                            }

                            return;
                        }
                    }
                }

                _log.Write(LogLevel.Warning, "Failed to deserialize stream order");
            });

            return await _baseClient.SubscribeInternalAsync(this, _authBaseAddress, new KrakenSubscribeRequest("ownTrades", KrakenSocketClient.NextIdInternal())
            {
                Details = new KrakenOwnTradesSubscriptionDetails(socketToken, snapshot)
            }, null, false, innerHandler, ct).ConfigureAwait(false);
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
            IEnumerable<OrderFlags>? flags = null)
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
                StartTime = (startTime.HasValue && startTime > DateTime.UtcNow) ? DateTimeConverter.ConvertToSeconds(startTime).ToString(): null,
                ExpireTime = expireTime.HasValue ? DateTimeConverter.ConvertToSeconds(expireTime).ToString() : null,
                ValidateOnly = validateOnly,
                CloseOrderType = closeOrderType,
                ClosePrice = closePrice?.ToString(CultureInfo.InvariantCulture),
                SecondaryClosePrice = secondaryClosePrice?.ToString(CultureInfo.InvariantCulture),
                RequestId = KrakenSocketClient.NextIdInternal(),
                Flags = flags == null ? null: string.Join(",", flags.Select(f => JsonConvert.SerializeObject(f, new OrderFlagsConverter(false))))
            };

            return await _baseClient.QueryInternalAsync<KrakenStreamPlacedOrder>(this, _authBaseAddress, request, false).ConfigureAwait(false);
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
                RequestId = KrakenSocketClient.NextIdInternal()
            };
            var result = await _baseClient.QueryInternalAsync<KrakenSocketResponseBase>(this, _authBaseAddress, request, false).ConfigureAwait(false);
            return result.As(result.Success);
        }

        /// <inheritdoc />
        public async Task<CallResult<KrakenStreamCancelAllResult>> CancelAllOrdersAsync(string websocketToken)
        {
            var request = new KrakenSocketRequestBase
            {
                Event = "cancelAll",
                Token = websocketToken,
                RequestId = KrakenSocketClient.NextIdInternal()
            };
            return await _baseClient.QueryInternalAsync<KrakenStreamCancelAllResult>(this, _authBaseAddress, request, false).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<KrakenStreamCancelAfterResult>> CancelAllOrdersAfterAsync(string websocketToken, TimeSpan timeout)
        {
            var request = new KrakenSocketCancelAfterRequest
            {
                Event = "cancelAllOrdersAfter",
                Token = websocketToken,
                Timeout = (int)Math.Round(timeout.TotalSeconds),
                RequestId = KrakenSocketClient.NextIdInternal()
            };
            return await _baseClient.QueryInternalAsync<KrakenStreamCancelAfterResult>(this, _authBaseAddress, request, false).ConfigureAwait(false);
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
    }
}
