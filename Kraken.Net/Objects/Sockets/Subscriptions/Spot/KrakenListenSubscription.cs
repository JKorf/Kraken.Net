using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenListenSubscription<T> : Subscription<BaseQuery, T>
    {
        private readonly Action<DataEvent<T>> _handler;

        public override List<string> StreamIdentifiers { get; set; } = new List<string>() { "systemstatus" };
        public override Dictionary<string, Type> TypeMapping { get; } = new Dictionary<string, Type>
        {
            { "", typeof(T) }
        };

        public KrakenListenSubscription(ILogger logger, Action<DataEvent<T>> handler) : base(logger, false)
        {
            _handler = handler;
        }

        public override BaseQuery? GetSubQuery(SocketConnection connection) => null;

        public override BaseQuery? GetUnsubQuery() => null;

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<BaseParsedMessage> message)
        {
            _handler.Invoke(message.As((T)message.Data.Data!, null, SocketUpdateType.Update));
            return Task.FromResult(new CallResult(null));
        }
    }
}
