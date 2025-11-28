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

                var routes = request.Symbols.Select(x => new MessageRoute<T>(evnt + "-" + request.Feed.ToLowerInvariant() + "-" + x.ToLowerInvariant(), (string?)null, HandleMessage)).ToList();
                routes.Add(new MessageRoute<T>("alert", (string?)null, HandleMessage));
                MessageRouter = MessageRouter.Create(routes.ToArray());

            }
            else
            {
                MessageMatcher = MessageMatcher.Create<T>([evnt + "-" + request.Feed.ToLowerInvariant(), "alert"], HandleMessage);
                MessageRouter = MessageRouter.Create<T>([evnt + "-" + request.Feed.ToLowerInvariant(), "alert"], HandleMessage);
            }
        }

        public CallResult<T> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, T message)
        {
            if (string.Equals(message.Event, "alert", StringComparison.Ordinal))
            {
                if (message.Message == "Already subscribed to feed, re-requesting")
                    // Duplicate subscriptions are not an error
                    return new CallResult<T>(message, originalData, null);

                return new CallResult<T>(new ServerError(message.Message!, _client.GetErrorInfo(message.Message!, message.Message!)), originalData);
            }
            else
            {
                return new CallResult<T>(message, originalData, null);
            }
        }
    }
}
