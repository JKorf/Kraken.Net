using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenFuturesInfoSubscription : SystemSubscription<KrakenInfoEvent>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; } = new HashSet<string> { "info" };

        public KrakenFuturesInfoSubscription(ILogger logger) : base(logger, false)
        {
        }

        public override CallResult HandleMessage(SocketConnection connection, DataEvent<KrakenInfoEvent> message) => new CallResult(null);
    }
}
