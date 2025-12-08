using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class SystemStatusSubscription : SystemSubscription
    {
        public SystemStatusSubscription(ILogger logger) : base(logger, false)
        {
            MessageMatcher = MessageMatcher.Create<KrakenStreamSystemStatus>("status");
            MessageRouter = MessageRouter.CreateWithoutHandler<KrakenStreamSystemStatus>("status");
        }
    }
}
