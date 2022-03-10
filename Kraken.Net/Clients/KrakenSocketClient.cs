using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Clients.SpotApi;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kraken.Net.Clients
{
    /// <inheritdoc cref="IKrakenSocketClient" />
    public class KrakenSocketClient : BaseSocketClient, IKrakenSocketClient
    {
        #region Api clients

        /// <inheritdoc />
        public IKrakenSocketClientSpotStreams SpotStreams { get; }

        #endregion

        #region ctor
        /// <summary>
        /// Create a new instance of KrakenSocketClient using the default options
        /// </summary>
        public KrakenSocketClient() : this(KrakenSocketClientOptions.Default)
        {
        }

        /// <summary>
        /// Create a new instance of KrakenSocketClient using provided options
        /// </summary>
        /// <param name="options">The options to use for this client</param>
        public KrakenSocketClient(KrakenSocketClientOptions options) : base("Kraken", options)
        {
            AddGenericHandler("HeartBeat", (messageEvent) => { });
            AddGenericHandler("SystemStatus", (messageEvent) => { });
            AddGenericHandler("AdditionalSubResponses", (messageEvent) => { });

            SpotStreams = AddApiClient(new KrakenSocketClientSpotStreams(log, this, options));
        }
        #endregion

        #region methods

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="options">Options to use as default</param>
        public static void SetDefaultOptions(KrakenSocketClientOptions options)
        {
            KrakenSocketClientOptions.Default = options;
        }

        #endregion

        internal Task<CallResult<UpdateSubscription>> SubscribeInternalAsync<T>(SocketApiClient apiClient, object? request, string? identifier, bool authenticated, Action<DataEvent<T>> dataHandler, CancellationToken ct)
        {
            return SubscribeAsync(apiClient, request, identifier, authenticated, dataHandler, ct);
        }

        internal Task<CallResult<UpdateSubscription>> SubscribeInternalAsync<T>(SocketApiClient apiClient, string url, object? request, string? identifier, bool authenticated, Action<DataEvent<T>> dataHandler, CancellationToken ct)
        {
            return SubscribeAsync(apiClient, url, request, identifier, authenticated, dataHandler, ct);
        }

        internal Task<CallResult<T>> QueryInternalAsync<T>(SocketApiClient apiClient, string url, object request, bool authenticated)
            => QueryAsync<T>(apiClient, url, request, authenticated);

        internal static int NextIdInternal() => NextId();

        internal Task<CallResult<T>> QueryInternalAsync<T>(SocketApiClient apiClient, object request, bool authenticated)
            => QueryAsync<T>(apiClient, request, authenticated);

        internal CallResult<T> DeserializeInternal<T>(JToken obj, JsonSerializer? serializer = null, int? requestId = null)
            => Deserialize<T>(obj, serializer, requestId);

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
            callResult = response.Status == "subscribed" ? new CallResult<object>(response): new CallResult<object>(new ServerError(response.ErrorMessage ?? "-"));
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
                    return true; // No channel id assigned, nothing to unsub
            }
            else
                unsubRequest = new KrakenUnsubscribeRequest(NextId(), kRequest.ChannelId.Value);
            var result = false;
            await connection.SendAndWaitAsync(unsubRequest, ClientOptions.SocketResponseTimeout, data =>
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
