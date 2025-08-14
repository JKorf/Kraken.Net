using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;

namespace Kraken.Net.Objects.Sockets.Queries
{
    internal class KrakenFuturesQuery<T> : Query<T> where T: KrakenFuturesResponse
    {
        private readonly SocketApiClient _client;

        public KrakenFuturesQuery(SocketApiClient client, KrakenFuturesRequest request, bool authenticated) : base(request, authenticated)
        {
            _client = client;
            string evnt = request.Event;
            if (string.Equals(request.Event, "subscribe", StringComparison.Ordinal)
                || string.Equals(request.Event, "unsubscribe", StringComparison.Ordinal))
            {
                evnt += "d";
            }

            if (request.Symbols?.Any() == true)
            {
                var checkers = request.Symbols.Select(x => new MessageHandlerLink<T>(evnt + "-" + request.Feed.ToLowerInvariant() + "-" + x.ToLowerInvariant(), HandleMessage)).ToList();
                checkers.Add(new MessageHandlerLink<T>("alert", HandleMessage));
                MessageMatcher = MessageMatcher.Create(checkers.ToArray());

            }
            else
            {
                MessageMatcher = MessageMatcher.Create<T>([evnt + "-" + request.Feed.ToLowerInvariant(), "alert"], HandleMessage);
            }
        }

        public CallResult<T> HandleMessage(SocketConnection connection, DataEvent<T> message)
        {
            if (string.Equals(message.Data.Event, "alert", StringComparison.Ordinal))
            {
                if (message.Data.Message == "Already subscribed to feed, re-requesting")
                    // Duplicate subscriptions are not an error
                    return message.ToCallResult();

                return new CallResult<T>(new ServerError(message.Data.Message!, _client.GetErrorInfo(message.Data.Message!, message.Data.Message!)));
            }
            else
            {
                return new CallResult<T>(message.Data!);
            }
        }
    }
}
