using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;

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
