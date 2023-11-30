using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    public class SystemStatusSubscription : SystemSubscription<KrakenStreamSystemStatus>
    {
        public override List<string> Identifiers => new List<string> { "systemStatus" };

        public SystemStatusSubscription(ILogger logger) : base(logger, false)
        {
        }

        public override Task<CallResult> HandleMessageAsync(SocketConnection connection, DataEvent<ParsedMessage<KrakenStreamSystemStatus>> message) => Task.FromResult(new CallResult(null)); // TODO
    }
}
