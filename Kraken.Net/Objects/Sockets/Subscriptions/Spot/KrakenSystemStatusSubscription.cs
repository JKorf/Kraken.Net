using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenSystemStatusSubscription : Subscription<Query, Query>
    {
        private readonly Action<DataEvent<KrakenStreamSystemStatus>> _handler;

        public override HashSet<string> ListenerIdentifiers { get; set; } = new HashSet<string>() { "status" };

        public KrakenSystemStatusSubscription(ILogger logger, Action<DataEvent<KrakenStreamSystemStatus>> handler) : base(logger, false)
        {
            _handler = handler;
        }

        public override Query? GetSubQuery(SocketConnection connection) => null;

        public override Query? GetUnsubQuery() => null;

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            var data = (KrakenSocketUpdateV2<KrakenStreamSystemStatus[]>)message.Data!;
            _handler.Invoke(message.As(data.Data.First(), data.Channel, null, SocketUpdateType.Update).WithDataTimestamp(data.Timestamp));
            return new CallResult(null);
        }

        public override Type? GetMessageType(IMessageAccessor message) => typeof(KrakenSocketUpdateV2<KrakenStreamSystemStatus[]>);
    }
}
