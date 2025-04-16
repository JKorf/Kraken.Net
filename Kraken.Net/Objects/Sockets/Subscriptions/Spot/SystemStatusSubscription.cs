using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class SystemStatusSubscription : SystemSubscription<KrakenStreamSystemStatus>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; } = new HashSet<string> { "status" };

        public SystemStatusSubscription(ILogger logger) : base(logger, false)
        {
        }

        public override CallResult HandleMessage(SocketConnection connection, DataEvent<KrakenStreamSystemStatus> message) => CallResult.SuccessResult;
    }
}
