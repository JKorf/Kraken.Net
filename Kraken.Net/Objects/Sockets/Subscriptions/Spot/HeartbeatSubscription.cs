using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class HeartbeatSubscription : SystemSubscription<KrakenEvent>
    {
        public override List<string> Identifiers => new List<string> { "heartbeat" };

        public HeartbeatSubscription(ILogger logger) : base(logger, false)
        {
        }

        public override Task<CallResult> HandleMessageAsync(SocketConnection connection, DataEvent<ParsedMessage<KrakenEvent>> message) => Task.FromResult(new CallResult(null));
    }
}
