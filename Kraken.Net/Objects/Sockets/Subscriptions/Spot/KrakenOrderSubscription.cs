using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenOrderSubscription : Subscription<KrakenSocketResponseV2<KrakenSocketSubResponse>, KrakenSocketResponseV2<KrakenSocketSubResponse>>
    {
        private static readonly MessagePath _typePath = MessagePath.Get().Property("type");

        private readonly Action<DataEvent<IEnumerable<KrakenOrderUpdate>>> _updateHandler;

        private bool? _snapshotOrder;
        private bool? _snapshotTrades;
        private string _token;

        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenOrderSubscription(ILogger logger, bool? snapshotOrder, bool? snapshotTrades, string token, Action<DataEvent<IEnumerable<KrakenOrderUpdate>>> updateHandler) : base(logger, false)
        {
            _snapshotOrder = snapshotOrder;
            _snapshotTrades = snapshotTrades;
            _token = token;

            _updateHandler = updateHandler;

            ListenerIdentifiers = new HashSet<string> { "executions" };
        }

        public override Type? GetMessageType(IMessageAccessor message)
        {
            return typeof(KrakenSocketUpdateV2<IEnumerable<KrakenOrderUpdate>>);
        }

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenSpotQueryV2<KrakenSocketSubResponse, KrakenSocketSubRequest>(
                new KrakenSocketRequestV2<KrakenSocketSubRequest>()
                {
                    Method = "subscribe",
                    RequestId = ExchangeHelpers.NextId(),
                    Parameters = new KrakenSocketSubRequest
                    {
                        Channel = "executions",
                        SnapshotOrders = _snapshotOrder,
                        SnapshotTrades = _snapshotTrades,
                        Token = _token
                    }
                }, false);
        }

        public override Query? GetUnsubQuery()
        {
            return new KrakenSpotQueryV2<KrakenSocketSubResponse, KrakenSocketSubRequest>(
                new KrakenSocketRequestV2<KrakenSocketSubRequest>()
                {
                    Method = "unsubscribe",
                    RequestId = ExchangeHelpers.NextId(),
                    Parameters = new KrakenSocketSubRequest
                    {
                        Channel = "executions",
                        Token = _token
                    }
                }, false);
        }

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            var data = (KrakenSocketUpdateV2<IEnumerable<KrakenOrderUpdate>>)message.Data;
            _updateHandler.Invoke(message.As(data.Data, "executions", null, data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return new CallResult(null);
        }
    }
}
