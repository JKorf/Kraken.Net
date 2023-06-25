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
using CryptoExchange.Net.Sockets;
using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Objects.Options;
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
        #endregion

        #region ctor

        internal KrakenSocketClientSpotApi(ILogger logger, KrakenSocketOptions options) :
            base(logger, options.Environment.SpotSocketPublicAddress, options, options.SpotOptions)
        {
            _privateBaseAddress = ((KrakenEnvironment)options.Environment).SpotSocketPrivateAddress;
            _symbolSynonyms = new Dictionary<string, string>
            {
                { "BTC", "XBT"},
                { "DOGE", "XDG" }
            };

            AddGenericHandler("HeartBeat", (messageEvent) => { });
            AddGenericHandler("SystemStatus", (messageEvent) => { });
            AddGenericHandler("AdditionalSubResponses", (messageEvent) => { });
        }
        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new KrakenAuthenticationProvider(credentials, ClientOptions.NonceProvider ?? new KrakenNonceProvider());

        #region methods

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToSystemStatusUpdatesAsync(Action<DataEvent<KrakenStreamSystemStatus>> handler, CancellationToken ct = default)
        {
            return await SubscribeAsync(null, "SystemStatus", false, handler, ct).ConfigureAwait(false);
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
            return await SubscribeAsync(new KrakenSubscribeRequest("ticker", NextId(), subSymbol), null, false, internalHandler, ct).ConfigureAwait(false);
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

            return await SubscribeAsync(new KrakenSubscribeRequest("ticker", NextId(), symbolArray), null, false, internalHandler, ct).ConfigureAwait(false);
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
            var internalHandler = new Action<DataEvent<KrakenSocketEvent<KrakenStreamKline>>>(data =>
            {
                handler(data.As(data.Data.Data, data.Data.Symbol));
            });

            return await SubscribeAsync(new KrakenSubscribeRequest("ohlc", NextId(), subSymbols.ToArray()) { Details = new KrakenOHLCSubscriptionDetails(intervalMinutes) }, null, false, internalHandler, ct).ConfigureAwait(false);
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
			var internalHandler = new Action<DataEvent<KrakenSocketEvent<IEnumerable<KrakenTrade>>>>(data =>
            {
                handler(data.As(data.Data.Data, data.Data.Symbol));
            });
            return await SubscribeAsync(new KrakenSubscribeRequest("trade", NextId(), subSymbols.ToArray()), null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamSpread>> handler, CancellationToken ct = default)
            => SubscribeToSpreadUpdatesAsync(new[] { symbol }, handler, ct);

		/// <inheritdoc />
		public async Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenStreamSpread>> handler, CancellationToken ct = default)
        {
            foreach(var symbol in symbols)
                symbol.ValidateKrakenWebsocketSymbol();

            var subSymbols = symbols.Select(SymbolToServer);
            var internalHandler = new Action<DataEvent<KrakenSocketEvent<KrakenStreamSpread>>>(data =>
            {
                handler(data.As(data.Data.Data, data.Data.Symbol));
            });
            return await SubscribeAsync(new KrakenSubscribeRequest("spread", NextId(), subSymbols.ToArray()), null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenStreamOrderBook>> handler, CancellationToken ct = default)
            => SubscribeToOrderBookUpdatesAsync(new[] { symbol }, depth, handler, ct);

        /// <inheritdoc />
		public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<KrakenStreamOrderBook>> handler, CancellationToken ct = default)
        {
            foreach(var symbol in symbols)
                symbol.ValidateKrakenWebsocketSymbol();

            depth.ValidateIntValues(nameof(depth), 10, 25, 100, 500, 1000);
            var subSymbols = symbols.Select(SymbolToServer);

            var innerHandler = new Action<DataEvent<string>>(data =>
            {
                var token = data.Data.ToJToken(_logger);
                if (token == null || token.Type != JTokenType.Array)
                {
                    _logger.Log(LogLevel.Warning, "Failed to deserialize stream order book");
                    return;
                }
                var evnt = StreamOrderBookConverter.Convert((JArray)token);
                if (evnt == null)
                {
                    _logger.Log(LogLevel.Warning, "Failed to deserialize stream order book");
                    return;
                }

                handler(data.As(evnt.Data, evnt.Symbol));
            });

            return await SubscribeAsync(new KrakenSubscribeRequest("book", NextId(), subSymbols.ToArray()) { Details = new KrakenDepthSubscriptionDetails(depth) }, null, false, innerHandler, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string socketToken, Action<DataEvent<KrakenStreamOrder>> handler, CancellationToken ct = default)
        {
            var innerHandler = new Action<DataEvent<string>>(data =>
            {
                var token = data.Data.ToJToken(_logger);
                if (token != null && token.Count() > 2)
                {
                    var seq = token[2]!["sequence"];
                    if (seq == null)
                        _logger.Log(LogLevel.Warning, "Failed to deserialize stream order, no sequence");

                    var sequence = seq!.Value<int>();
                    if (token[0]!.Type == JTokenType.Array)
                    {
                        var dataArray = (JArray)token[0]!;
                        var deserialized = Deserialize<Dictionary<string, KrakenStreamOrder>[]>(dataArray);
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

                _logger.Log(LogLevel.Warning, "Failed to deserialize stream order");
            });

            return await SubscribeAsync(_privateBaseAddress, new KrakenSubscribeRequest("openOrders", NextId())
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
                var token = data.Data.ToJToken(_logger);
                if (token != null && token.Count() > 2)
                {
                    var seq = token[2]!["sequence"];
                    if (seq == null)
                        _logger.Log(LogLevel.Warning, "Failed to deserialize stream order, no sequence");

                    var sequence = seq!.Value<int>();
                    if (token[0]!.Type == JTokenType.Array)
                    {
                        var dataArray = (JArray)token[0]!;
                        var deserialized = Deserialize<Dictionary<string, KrakenStreamUserTrade>[]>(dataArray);
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

                _logger.Log(LogLevel.Warning, "Failed to deserialize stream order");
            });

            return await SubscribeAsync(_privateBaseAddress, new KrakenSubscribeRequest("ownTrades", NextId())
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
                StartTime = (startTime.HasValue && startTime > DateTime.UtcNow) ? DateTimeConverter.ConvertToSeconds(startTime).ToString(): null,
                ExpireTime = expireTime.HasValue ? DateTimeConverter.ConvertToSeconds(expireTime).ToString() : null,
                ValidateOnly = validateOnly,
                CloseOrderType = closeOrderType,
                ClosePrice = closePrice?.ToString(CultureInfo.InvariantCulture),
                SecondaryClosePrice = secondaryClosePrice?.ToString(CultureInfo.InvariantCulture),
                ReduceOnly = reduceOnly,
                RequestId = NextId(),
                Flags = flags == null ? null: string.Join(",", flags.Select(f => JsonConvert.SerializeObject(f, new OrderFlagsConverter(false))))
            };

            return await QueryAsync<KrakenStreamPlacedOrder>(_privateBaseAddress, request, false).ConfigureAwait(false);
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
                RequestId = NextId()
            };
            var result = await QueryAsync<KrakenSocketResponseBase>(_privateBaseAddress, request, false).ConfigureAwait(false);
            return result.As(result.Success);
        }

        /// <inheritdoc />
        public async Task<CallResult<KrakenStreamCancelAllResult>> CancelAllOrdersAsync(string websocketToken)
        {
            var request = new KrakenSocketRequestBase
            {
                Event = "cancelAll",
                Token = websocketToken,
                RequestId = NextId()
            };
            return await QueryAsync<KrakenStreamCancelAllResult>(_privateBaseAddress, request, false).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CallResult<KrakenStreamCancelAfterResult>> CancelAllOrdersAfterAsync(string websocketToken, TimeSpan timeout)
        {
            var request = new KrakenSocketCancelAfterRequest
            {
                Event = "cancelAllOrdersAfter",
                Token = websocketToken,
                Timeout = (int)Math.Round(timeout.TotalSeconds),
                RequestId = NextId()
            };
            return await QueryAsync<KrakenStreamCancelAfterResult>(_privateBaseAddress, request, false).ConfigureAwait(false);
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
        public override async Task<CallResult<object>> RevitalizeRequestAsync(object request)
        {
            var kRequest = (KrakenSubscribeRequest)request;
            var payloadType = kRequest.Details.GetType();
            if(payloadType == typeof(KrakenOpenOrdersSubscriptionDetails)
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

        /// <inheritdoc />
        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            callResult = null!;

            if (data.Type != JTokenType.Object)
                return false;

            var kRequest = (KrakenSocketRequestBase)request;
            var responseId = data["reqid"];
            if (responseId == null)
                return false;

            if (kRequest.RequestId != int.Parse(responseId.ToString()))
                return false;

            var error = data["errorMessage"]?.ToString();
            if (!string.IsNullOrEmpty(error))
            {
                callResult = new CallResult<T>(new ServerError(error!));
                return true;
            }

            var response = data.ToObject<T>();
            callResult = new CallResult<T>(response!);
            return true;
        }

        /// <inheritdoc />
        protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object>? callResult)
        {
            callResult = null;
            if (message.Type != JTokenType.Object)
                return false;

            if (message["reqid"] == null)
                return false;

            var requestId = message["reqid"]!.Value<int>();
            var kRequest = (KrakenSubscribeRequest)request;
            if (requestId != kRequest.RequestId)
                return false;

            var response = message.ToObject<KrakenSubscriptionEvent>();
            if (response == null)
            {
                callResult = new CallResult<object>(new UnknownError("Failed to parse subscription response"));
                return true;
            }

            if (response.ChannelId != 0)
                kRequest.ChannelId = response.ChannelId;
            callResult = response.Status == "subscribed" ? new CallResult<object>(response) : new CallResult<object>(new ServerError(response.ErrorMessage ?? "-"));
            return true;
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, object request)
        {
            if (message.Type != JTokenType.Array)
                return false;

            var kRequest = (KrakenSubscribeRequest)request;
            var arr = (JArray)message;

            string channel;
            string symbol;
            if (arr.Count == 5)
            {
                channel = arr[3].ToString();
                symbol = arr[4].ToString();
            }
            else if (arr.Count == 4)
            {
                // Public update
                channel = arr[2].ToString();
                symbol = arr[3].ToString();
            }
            else
            {
                // Private update
                var topic = arr[1].ToString();
                return topic == kRequest.Details.Topic;
            }

            if (kRequest.Details.ChannelName != channel)
                return false;

            if (kRequest.Symbols == null)
                return false;

            foreach (var subSymbol in kRequest.Symbols)
            {
                if (subSymbol == symbol)
                    return true;
            }
            return false;
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, string identifier)
        {
            if (message.Type != JTokenType.Object)
                return false;

            if (identifier == "HeartBeat" && message["event"] != null && message["event"]!.ToString() == "heartbeat")
                return true;

            if (identifier == "SystemStatus" && message["event"] != null && message["event"]!.ToString() == "systemStatus")
                return true;

            if (identifier == "AdditionalSubResponses")
            {
                if (message.Type != JTokenType.Object)
                    return false;

                if (message["reqid"] == null)
                    return false;

                var requestId = message["reqid"]!.Value<int>();
                var subscription = socketConnection.GetSubscriptionByRequest(r => (r as KrakenSubscribeRequest)?.RequestId == requestId);
                if (subscription == null)
                    return false;

                // This is another message for subscription we've already subscribed
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        protected override Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection s)
        {
            throw new NotImplementedException();
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
    }
}
