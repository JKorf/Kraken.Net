using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenFuturesInfoSubscription : SystemSubscription<KrakenInfoEvent>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; } = new HashSet<string> { "info" };

        public KrakenFuturesInfoSubscription(ILogger logger) : base(logger, false)
        {
        }

        public override CallResult HandleMessage(SocketConnection connection, DataEvent<KrakenInfoEvent> message) => new CallResult(null);
    }
}
