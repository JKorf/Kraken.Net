using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class HeartbeatSubscription : SystemSubscription
    {
        public HeartbeatSubscription(ILogger logger) : base(logger, false)
        {
            MessageRouter = MessageRouter.CreateWithoutHandler<KrakenEvent>("heartbeat");
        }
    }
}
