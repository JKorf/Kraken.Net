using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenOrderSubscription : KrakenSubscription
    {
        private readonly Action<DataEvent<KrakenOrderUpdate[]>> _updateHandler;

        private bool? _snapshotOrder;
        private bool? _snapshotTrades;

        public KrakenOrderSubscription(ILogger logger, bool? snapshotOrder, bool? snapshotTrades, string token, Action<DataEvent<KrakenOrderUpdate[]>> updateHandler) : base(logger, true)
        {
            _snapshotOrder = snapshotOrder;
            _snapshotTrades = snapshotTrades;
            Token = token;

            _updateHandler = updateHandler;

            MessageMatcher = MessageMatcher.Create<KrakenSocketUpdateV2<KrakenOrderUpdate[]>>("executions", DoHandleMessage);
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
                        Channel = "executions",
                        Token = Token
                    }
                }, false);
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenSocketUpdateV2<KrakenOrderUpdate[]>> message)
        {
            _updateHandler.Invoke(message.As(message.Data.Data, "executions", null, message.Data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update).WithDataTimestamp(message.Data.Timestamp));
            return CallResult.SuccessResult;
        }
    }
}
