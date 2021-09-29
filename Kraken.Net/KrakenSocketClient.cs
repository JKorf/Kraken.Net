using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Converters;
using Kraken.Net.Interfaces;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Socket;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kraken.Net
{
    /// <summary>
    /// Client for the Kraken websocket API
    /// </summary>
    public class KrakenSocketClient: SocketClient, IKrakenSocketClient
    {
        #region fields
        private static KrakenSocketClientOptions defaultOptions = new KrakenSocketClientOptions();
        private static KrakenSocketClientOptions DefaultOptions => defaultOptions.Copy();
                
        private readonly string _authBaseAddress;
        private readonly Dictionary<string, string> _symbolSynonyms;
        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of KrakenSocketClient using the default options
        /// </summary>
        public KrakenSocketClient() : this(DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of KrakenSocketClient using provided options
        /// </summary>
        /// <param name="options">The options to use for this client</param>
        public KrakenSocketClient(KrakenSocketClientOptions options) : base("Kraken", options, options.ApiCredentials == null ? null : new KrakenAuthenticationProvider(options.ApiCredentials, null))
        {
            AddGenericHandler("HeartBeat", (messageEvent) => { });
            AddGenericHandler("SystemStatus", (messageEvent) => { });
            _authBaseAddress = options.AuthBaseAddress;

            _symbolSynonyms = new Dictionary<string, string>
            {
                { "BTC", "XBT"},
                { "DOGE", "XDG" }
            };
        }
        #endregion

        #region methods
        /// <summary>
        /// Set the API key and secret
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        /// <param name="nonceProvider">Optional nonce provider. Careful providing a custom provider; once a nonce is sent to the server, every request after that needs a higher nonce than that</param>
        public void SetApiCredentials(string apiKey, string apiSecret, INonceProvider? nonceProvider = null)
        {
            SetAuthenticationProvider(new KrakenAuthenticationProvider(new ApiCredentials(apiKey, apiSecret), nonceProvider));
        }

        /// <summary>
        /// Set the default options to be used when creating new socket clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(KrakenSocketClientOptions options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Subscribe to system status updates. Gets fired when the socket is connected or when the system status changes. Note that if this is not the first subscription
        /// on the socket connection it does not fire the initial system status event
        /// </summary>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSystemStatusUpdatesAsync(Action<DataEvent<KrakenStreamSystemStatus>> handler)
        {
            return await SubscribeAsync(null, "SystemStatus", false, handler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to ticker updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamTick>> handler)
        {
            symbol.ValidateKrakenWebsocketSymbol();
            var subSymbol = SymbolToServer(symbol);
            var internalHandler = new Action<DataEvent<KrakenSocketEvent<KrakenStreamTick>>>(data =>
            {
                handler(data.As(data.Data.Data, symbol));
            });
            return await SubscribeAsync(new KrakenSubscribeRequest("ticker", NextId(), subSymbol), null, false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to ticker updates
        /// </summary>
        /// <param name="symbols">Symbols to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenStreamTick>> handler)
        {
            var symbolArray = symbols.ToArray();
            for (var i = 0; i< symbolArray.Length; i++)
            {
                symbolArray[i].ValidateKrakenWebsocketSymbol();
                symbolArray[i] = SymbolToServer(symbolArray[i]);
            }
            var internalHandler = new Action<DataEvent<KrakenSocketEvent<KrakenStreamTick>>>(data =>
            {
                handler(data.As(data.Data.Data, data.Data.Symbol));
            });

            return await SubscribeAsync(new KrakenSubscribeRequest("ticker", NextId(), symbolArray), null, false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to kline updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<KrakenStreamKline>> handler)
        {
            symbol.ValidateKrakenWebsocketSymbol();
            var subSymbol = SymbolToServer(symbol);

            var intervalMinutes = int.Parse(JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)));
            var internalHandler = new Action<DataEvent<KrakenSocketEvent<KrakenStreamKline>>>(data =>
            {
                handler(data.As(data.Data.Data, symbol));
            });

            return await SubscribeAsync(new KrakenSubscribeRequest("ohlc", NextId(), subSymbol) { Details = new KrakenOHLCSubscriptionDetails(intervalMinutes) }, null, false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to trade updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<IEnumerable<KrakenTrade>>> handler)
        {
            symbol.ValidateKrakenWebsocketSymbol();
            var subSymbol = SymbolToServer(symbol);

            var internalHandler = new Action<DataEvent<KrakenSocketEvent<IEnumerable<KrakenTrade>>>>(data =>
            {
                handler(data.As(data.Data.Data, symbol));
            });
            return await SubscribeAsync(new KrakenSubscribeRequest("trade", NextId(), subSymbol), null, false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to spread updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamSpread>> handler)
        {
            symbol.ValidateKrakenWebsocketSymbol();
            var subSymbol = SymbolToServer(symbol);
            var internalHandler = new Action<DataEvent<KrakenSocketEvent<KrakenStreamSpread>>>(data =>
            {
                handler(data.As(data.Data.Data, symbol));
            });
            return await SubscribeAsync(new KrakenSubscribeRequest("spread", NextId(), subSymbol), null, false, internalHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to depth updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="depth">Depth of the initial order book snapshot. 10, 25, 100, 500 or 1000</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenStreamOrderBook>> handler)
        {
            symbol.ValidateKrakenWebsocketSymbol();
            depth.ValidateIntValues(nameof(depth), 10, 25, 100, 500, 1000);
            var subSymbol = SymbolToServer(symbol);

            var innerHandler = new Action<DataEvent<string>>(data =>
            {
                var token = data.Data.ToJToken(log);
                if (token == null || token.Type != JTokenType.Array)
                {
                    log.Write(LogLevel.Warning, "Failed to deserialize stream order book");
                    return;
                }
                var evnt = StreamOrderBookConverter.Convert((JArray)token);
                if (evnt == null)
                {
                    log.Write(LogLevel.Warning, "Failed to deserialize stream order book");
                    return;
                }

                handler(data.As(evnt.Data, symbol));
            });

            return await SubscribeAsync(new KrakenSubscribeRequest("book", NextId(), subSymbol) { Details = new KrakenDepthSubscriptionDetails(depth)}, null, false, innerHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to open order updates
        /// </summary>
        /// <param name="socketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string socketToken, Action<DataEvent<KrakenStreamOrder>> handler)
        {
            var innerHandler = new Action<DataEvent<string>>(data =>
            {
                var token = data.Data.ToJToken(log);
                if (token != null && token.Count() > 2)
                {
                    var seq = token[2]!["sequence"];
                    if(seq == null)
                        log.Write(LogLevel.Warning, "Failed to deserialize stream order, no sequence");

                    var sequence = seq!.Value<int>();
                    if (token[0]!.Type == JTokenType.Array)
                    {
                        var dataArray = (JArray) token[0]!;
                        var deserialized = Deserialize<Dictionary<string, KrakenStreamOrder>[]>(dataArray);
                        if (deserialized)
                        {
                            foreach (var entry in deserialized.Data)
                            {
                                foreach(var dEntry in entry)
                                {
                                    dEntry.Value.OrderId = dEntry.Key;
                                    dEntry.Value.SequenceNumber = sequence;
                                    handler?.Invoke(data.As(dEntry.Value, dEntry.Value.OrderDetails?.Symbol));
                                }
                            }
                            return;
                        }
                    }
                }

                log.Write(LogLevel.Warning, "Failed to deserialize stream order");
            });

            return await SubscribeAsync(_authBaseAddress, new KrakenSubscribeRequest("openOrders", NextId())
            {
                Details = new KrakenOpenOrdersSubscriptionDetails(socketToken)
            }, null, false, innerHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to own trade updates
        /// </summary>
        /// <param name="socketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public Task<CallResult<UpdateSubscription>> SubscribeToOwnTradeUpdatesAsync(string socketToken, Action<DataEvent<KrakenStreamUserTrade>> handler)
            => SubscribeToOwnTradeUpdatesAsync(socketToken, true, handler);

        /// <summary>
        /// Subscribe to own trade updates
        /// </summary>
        /// <param name="socketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <param name="snapshot">Whether or not to receive a snapshot of the data upon subscribing</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToOwnTradeUpdatesAsync(string socketToken, bool snapshot, Action<DataEvent<KrakenStreamUserTrade>> handler)
        {
            var innerHandler = new Action<DataEvent<string>>(data =>
            {
                var token = data.Data.ToJToken(log);
                if (token != null && token.Count() > 2)
                {
                    var seq = token[2]!["sequence"];
                    if (seq == null)
                        log.Write(LogLevel.Warning, "Failed to deserialize stream order, no sequence");

                    var sequence = seq!.Value<int>();
                    if (token[0]!.Type == JTokenType.Array)
                    {
                        var dataArray = (JArray)token[0]!;
                        var deserialized = Deserialize<Dictionary<string, KrakenStreamUserTrade>[]>(dataArray);
                        if (deserialized)
                        {
                            foreach (var entry in deserialized.Data)
                            {
                                foreach(var item in entry)
                                {
                                    item.Value.TradeId = item.Key;
                                    item.Value.SequenceNumber = sequence;
                                    handler?.Invoke(data.As(item.Value, item.Value.Symbol));
                                }
                            }

                            return;
                        }
                    }
                }

                log.Write(LogLevel.Warning, "Failed to deserialize stream order");
            });

            return await SubscribeAsync(_authBaseAddress, new KrakenSubscribeRequest("ownTrades", NextId())
            {
                Details = new KrakenOwnTradesSubscriptionDetails(socketToken, snapshot)
            }, null, false, innerHandler).ConfigureAwait(false);
        }

        /// <summary>
        /// Place a new order
        /// </summary>
        /// <param name="websocketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <param name="symbol">The symbol the order is on</param>
        /// <param name="side">The side of the order</param>
        /// <param name="type">The type of the order</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="clientOrderId">A client id to reference the order by</param>
        /// <param name="price">Price of the order:
        /// Limit=limit price,
        /// StopLoss=stop loss price,
        /// TakeProfit=take profit price,
        /// StopLossProfit=stop loss price,
        /// StopLossProfitLimit=stop loss price,
        /// StopLossLimit=stop loss trigger price,
        /// TakeProfitLimit=take profit trigger price,
        /// TrailingStop=trailing stop offset,
        /// TrailingStopLimit=trailing stop offset,
        /// StopLossAndLimit=stop loss price,
        /// </param>
        /// <param name="secondaryPrice">Secondary price of an order:
        /// StopLossProfit/StopLossProfitLimit=take profit price,
        /// StopLossLimit/TakeProfitLimit=triggered limit price,
        /// TrailingStopLimit=triggered limit offset,
        /// StopLossAndLimit=limit price</param>
        /// <param name="leverage">Desired leverage</param>
        /// <param name="startTime">Scheduled start time</param>
        /// <param name="expireTime">Expiration time</param>
        /// <param name="validateOnly">Only validate inputs, don't actually place the order</param>
        /// <param name="closeOrderType">Close order type</param>
        /// <param name="closePrice">Close order price</param>
        /// <param name="secondaryClosePrice">Close order secondary price</param>
        /// <returns></returns>
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
            decimal? secondaryClosePrice = null)
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
                StartTime = startTime,
                ExpireTime = expireTime,
                ValidateOnly = validateOnly,
                CloseOrderType = closeOrderType,
                ClosePrice = closePrice?.ToString(CultureInfo.InvariantCulture),
                SecondaryClosePrice = secondaryClosePrice?.ToString(CultureInfo.InvariantCulture),
                RequestId = NextId()
            };

            return await QueryAsync<KrakenStreamPlacedOrder>(_authBaseAddress, request, false).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="websocketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <param name="orderId">Id of the order to cancel</param>
        /// <returns></returns>
        public Task<CallResult<bool>> CancelOrderAsync(string websocketToken, string orderId)
            => CancelOrdersAsync(websocketToken, new[] { orderId });

        /// <summary>
        /// Cancel multiple orders
        /// </summary>
        /// <param name="websocketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <param name="orderIds">Id of the orders to cancel</param>
        /// <returns></returns>
        public async Task<CallResult<bool>> CancelOrdersAsync(string websocketToken, IEnumerable<string> orderIds)
        {
            var request = new KrakenSocketCancelOrdersRequest
            {
                Event = "cancelOrder",
                OrderIds = orderIds,
                Token = websocketToken,
                RequestId = NextId()
            };
            var result = await QueryAsync<KrakenSocketResponseBase>(_authBaseAddress, request, false).ConfigureAwait(false);
            if (result)
                return new CallResult<bool>(true, null);
            return new CallResult<bool>(false, result.Error);
        }

        /// <summary>
        /// Cancel all open orders
        /// </summary>
        /// <param name="websocketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <returns></returns>
        public async Task<CallResult<KrakenStreamCancelAllResult>> CancelAllOrdersAsync(string websocketToken)
        {
            var request = new KrakenSocketRequestBase
            {
                Event = "cancelAll",
                Token = websocketToken,
                RequestId = NextId()
            };
            return await QueryAsync<KrakenStreamCancelAllResult>(_authBaseAddress, request, false).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancel all open orders after the timeout
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="websocketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <returns></returns>
        public async Task<CallResult<KrakenStreamCancelAfterResult>> CancelAllOrdersAfterAsync(string websocketToken, TimeSpan timeout)
        {
            var request = new KrakenSocketCancelAfterRequest
            {
                Event = "cancelAllOrdersAfter",
                Token = websocketToken,
                Timeout = (int)Math.Round(timeout.TotalSeconds),
                RequestId = NextId()
            };
            return await QueryAsync<KrakenStreamCancelAfterResult>(_authBaseAddress, request, false).ConfigureAwait(false);
        }
        #endregion

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
            if (!string.IsNullOrEmpty(error)) {
                callResult = new CallResult<T>(default, new ServerError(error!));
                return true;
            }

            var response = data.ToObject<T>();
            callResult = new CallResult<T>(response, null);
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
            var kRequest = (KrakenSubscribeRequest) request;
            if (requestId != kRequest.RequestId)
                return false;
            
            var response = message.ToObject<KrakenSubscriptionEvent>();
            if (response == null)
            {
                callResult = new CallResult<object>(response, new UnknownError("Failed to parse subscription response"));
                return true;
            }

            if(response.ChannelId != 0)
                kRequest.ChannelId = response.ChannelId;
            callResult = new CallResult<object>(response, response.Status == "subscribed" ? null: new ServerError(response.ErrorMessage ?? "-"));
            return true;
        }

        /// <inheritdoc />
        protected override bool MessageMatchesHandler(JToken message, object request)
        {
            if (message.Type != JTokenType.Array)
                return false;

            var kRequest = (KrakenSubscribeRequest)request;
            var arr = (JArray) message;

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
        protected override bool MessageMatchesHandler(JToken message, string identifier)
        {
            if (message.Type != JTokenType.Object)
                return false;

            if (identifier == "HeartBeat" && message["event"] != null && message["event"]!.ToString() == "heartbeat")
                return true;

            if (identifier == "SystemStatus" && message["event"] != null && message["event"]!.ToString() == "systemStatus")
                return true;

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
            var kRequest = ((KrakenSubscribeRequest)subscription.Request!);
            KrakenUnsubscribeRequest unsubRequest;
            if (!kRequest.ChannelId.HasValue)
            {
                if(kRequest.Details?.Topic == "ownTrades")
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
                    return true; // No channel id assigned, nothing to unsub
            }
            else
                unsubRequest = new KrakenUnsubscribeRequest(NextId(), kRequest.ChannelId.Value);
            var result = false;
            await connection.SendAndWaitAsync(unsubRequest, ResponseTimeout, data =>
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
