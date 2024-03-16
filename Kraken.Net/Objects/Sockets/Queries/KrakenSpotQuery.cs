using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Queries
{
    internal class KrakenSpotQuery<T> : Query<T> where T : KrakenQueryEvent
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenSpotQuery(KrakenSocketRequest request, bool authenticated) : base(request, authenticated)
        {
            ListenerIdentifiers = new HashSet<string>() { request.RequestId.ToString() };
        }

        public override CallResult<T> HandleMessage(SocketConnection connection, DataEvent<T> message)
        {
            if (message.Data.Status != "error")
                return new CallResult<T>(message.Data!);
            else
                return new CallResult<T>(new ServerError(message.Data.ErrorMessage!));
        }
    }
}
