using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenBalanceSubscription : KrakenSubscription
    {
        private static readonly MessagePath _typePath = MessagePath.Get().Property("type");

        private readonly Action<DataEvent<IEnumerable<KrakenBalanceSnapshot>>>? _snapshotHandler;
        private readonly Action<DataEvent<IEnumerable<KrakenBalanceUpdate>>> _updateHandler;

        private bool? _snapshot;

        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenBalanceSubscription(ILogger logger, bool? snapshot, string token, Action<DataEvent<IEnumerable<KrakenBalanceSnapshot>>>? snapshotHandler, Action<DataEvent<IEnumerable<KrakenBalanceUpdate>>> updateHandler) : base(logger, true)
        {
            _snapshot = snapshot;
            Token = token;

            _snapshotHandler = snapshotHandler;
            _updateHandler = updateHandler;

            ListenerIdentifiers = new HashSet<string> { "balances" };
        }

        public override Type? GetMessageType(IMessageAccessor message)
        {
            if (message.GetValue<string>(_typePath) == "snapshot")
                return typeof(KrakenSocketUpdateV2<IEnumerable<KrakenBalanceSnapshot>>);

            return typeof(KrakenSocketUpdateV2<IEnumerable<KrakenBalanceUpdate>>);
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
                        Channel = "balances",
                        Snapshot = _snapshot,
                        Token = Token
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
                        Channel = "balances",
                        Snapshot = _snapshot,
                        Token = Token
                    }
                }, false);
        }

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            if (message.Data is KrakenSocketUpdateV2<IEnumerable<KrakenBalanceSnapshot>> snapshot)
                _snapshotHandler?.Invoke(message.As(snapshot.Data, "balances", null, SocketUpdateType.Snapshot).WithDataTimestamp(snapshot.Timestamp));
            else if (message.Data is KrakenSocketUpdateV2<IEnumerable<KrakenBalanceUpdate>> update)
                _updateHandler?.Invoke(message.As(update.Data, "balances", null, SocketUpdateType.Update).WithDataTimestamp(update.Timestamp));
            return new CallResult(null);
        }
    }
}
