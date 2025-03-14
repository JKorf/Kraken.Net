using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal abstract class KrakenSubscription : Subscription<KrakenSocketResponseV2<KrakenSocketSubResponse>, KrakenSocketResponseV2<KrakenSocketSubResponse>>
    {
        public string? Token { get; set; }
        public bool TokenRequired { get; set; }

        protected KrakenSubscription(ILogger logger, bool auth) : base(logger, false)
        {
            TokenRequired = auth;
        }
    }
}
