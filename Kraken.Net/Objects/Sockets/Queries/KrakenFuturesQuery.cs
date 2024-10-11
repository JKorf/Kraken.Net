using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Queries
{
    internal class KrakenFuturesQuery<T> : Query<T> where T: KrakenFuturesResponse
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenFuturesQuery(KrakenFuturesRequest request, bool authenticated) : base(request, authenticated)
        {
            string evnt = request.Event;
            if (string.Equals(request.Event, "subscribe", StringComparison.Ordinal)
                || string.Equals(request.Event, "unsubscribe", StringComparison.Ordinal))
            {
                evnt += "d";
            }

            if (request.Symbols?.Any() == true)
            {
                ListenerIdentifiers = new HashSet<string>(request.Symbols.Select(s => evnt + "-" + request.Feed.ToLowerInvariant() + "-" + s.ToLowerInvariant()))
                {
                    "alert"
                };
            }
            else
            {
                ListenerIdentifiers = new HashSet<string> { evnt + "-" + request.Feed.ToLowerInvariant(), "alert" };
            }
        }

        public override CallResult<T> HandleMessage(SocketConnection connection, DataEvent<T> message)
        {
            if (string.Equals(message.Data.Event, "alert", StringComparison.Ordinal))
            {
                if (message.Data.Message == "Already subscribed to feed, re-requesting")
                    // Duplicate subscriptions are not an error
                    return message.ToCallResult();

                return new CallResult<T>(new ServerError(message.Data.Message!));
            }
            else
                return new CallResult<T>(message.Data!);
        }
    }
}
