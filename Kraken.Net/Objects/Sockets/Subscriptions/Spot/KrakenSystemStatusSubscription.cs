using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenSystemStatusSubscription : Subscription<Query, Query>
    {
        private readonly Action<DataEvent<KrakenStreamSystemStatus>> _handler;

        public override HashSet<string> ListenerIdentifiers { get; set; } = new HashSet<string>() { "systemstatus" };

        public KrakenSystemStatusSubscription(ILogger logger, Action<DataEvent<KrakenStreamSystemStatus>> handler) : base(logger, false)
        {
            _handler = handler;
        }

        public override Query? GetSubQuery(SocketConnection connection) => null;

        public override Query? GetUnsubQuery() => null;

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<object> message)
        {
            _handler.Invoke(message.As((KrakenStreamSystemStatus)message.Data!, null, SocketUpdateType.Update));
            return Task.FromResult(new CallResult(null));
        }

        public override Type? GetMessageType(IMessageAccessor message) => typeof(KrakenStreamSystemStatus);
    }
}
