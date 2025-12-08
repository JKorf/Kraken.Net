using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Objects.Sockets.Queries
{
    internal class KrakenSpotQueryV2<TResponse, TRequest> : Query<KrakenSocketResponseV2<TResponse>>
    {
        private readonly SocketApiClient _client;

        public KrakenSpotQueryV2(SocketApiClient client, KrakenSocketRequestV2<TRequest> request, bool authenticated) : base(request, authenticated)
        {
            _client = client;
            MessageMatcher = MessageMatcher.Create<KrakenSocketResponseV2<TResponse>>(request.RequestId.ToString(), HandleMessage);
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<KrakenSocketResponseV2<TResponse>>(request.RequestId.ToString(), HandleMessage);
        }

        public CallResult<KrakenSocketResponseV2<TResponse>> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenSocketResponseV2<TResponse> message)
        {
            if (message.Success)
            {
                return new CallResult<KrakenSocketResponseV2<TResponse>>(message, originalData, null);
            }
            else if (message is KrakenSocketResponseV2<KrakenOrderResult[]> response // We'll want to return the actual response data, so return as no error and handle it in the method itself
                || message.Error == "Already subscribed") // Duplicate subscription shouldn't be treated as an error
            {
                return new CallResult<KrakenSocketResponseV2<TResponse>>(message, originalData, null);
            }
            else
            {
                return new CallResult<KrakenSocketResponseV2<TResponse>>(new ServerError(message.Error!, _client.GetErrorInfo("Subscription", message.Error!)));
            }
        }
    }
}
