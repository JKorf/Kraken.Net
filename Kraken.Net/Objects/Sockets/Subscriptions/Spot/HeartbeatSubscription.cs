using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class HeartbeatSubscription : SystemSubscription
    {
        public HeartbeatSubscription(ILogger logger) : base(logger, false)
        {
            MessageMatcher = MessageMatcher.Create<KrakenEvent>("heartbeat");
            MessageRouter = MessageRouter.CreateWithoutHandler<KrakenEvent>("heartbeat");
        }
    }
}
