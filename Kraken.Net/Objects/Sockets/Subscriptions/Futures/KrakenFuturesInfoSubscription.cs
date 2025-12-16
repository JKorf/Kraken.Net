using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Kraken.Net.Objects.Internal;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenFuturesInfoSubscription : SystemSubscription
    {
        public KrakenFuturesInfoSubscription(ILogger logger) : base(logger, false)
        {
            MessageMatcher = MessageMatcher.Create<KrakenInfoEvent>("info");
            MessageRouter = MessageRouter.CreateWithoutHandler<KrakenInfoEvent>("info");
        }
    }
}
