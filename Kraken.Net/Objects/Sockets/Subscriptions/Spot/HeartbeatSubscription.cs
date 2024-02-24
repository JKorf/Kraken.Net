using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class HeartbeatSubscription : SystemSubscription<KrakenEvent>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; } = new HashSet<string> { "heartbeat" };

        public HeartbeatSubscription(ILogger logger) : base(logger, false)
        {
        }

        public override Task<CallResult> HandleMessageAsync(SocketConnection connection, DataEvent<KrakenEvent> message) => Task.FromResult(new CallResult(null));
    }
}
