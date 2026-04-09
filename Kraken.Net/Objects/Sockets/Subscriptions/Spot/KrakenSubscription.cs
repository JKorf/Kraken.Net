using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal abstract class KrakenSubscription : Subscription
    {
        public string? Token { get; set; }
        public bool TokenRequired { get; set; }

        protected KrakenSubscription(ILogger logger, bool auth) : base(logger, false)
        {
            TokenRequired = auth;
        }
    }
}
