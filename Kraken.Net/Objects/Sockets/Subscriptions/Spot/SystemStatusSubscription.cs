using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class SystemStatusSubscription : SystemSubscription
    {
        public SystemStatusSubscription(ILogger logger) : base(logger, false)
        {
            MessageRouter = MessageRouter.CreateVoid<KrakenSocketUpdateV2<KrakenStreamSystemStatus[]>>("status");
        }
    }
}
