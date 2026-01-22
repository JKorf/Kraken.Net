using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenOrderSubscription : KrakenSubscription
    {
        private readonly SocketApiClient _client;
        private readonly Action<DataEvent<KrakenOrderUpdate[]>> _updateHandler;

        private bool? _snapshotOrder;
        private bool? _snapshotTrades;

        public KrakenOrderSubscription(ILogger logger, SocketApiClient client, bool? snapshotOrder, bool? snapshotTrades, string token, Action<DataEvent<KrakenOrderUpdate[]>> updateHandler) : base(logger, true)
        {
            _client = client;
            _snapshotOrder = snapshotOrder;
            _snapshotTrades = snapshotTrades;
            Token = token;

            _updateHandler = updateHandler;

            MessageRouter = MessageRouter.CreateWithoutTopicFilter<KrakenSocketUpdateV2<KrakenOrderUpdate[]>>("executions", DoHandleMessage);
        }

        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenSpotQueryV2<KrakenSocketSubResponse, KrakenSocketSubRequest>(
                _client,
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

        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new KrakenSpotQueryV2<KrakenSocketSubResponse, KrakenSocketSubRequest>(
                _client,
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

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenSocketUpdateV2<KrakenOrderUpdate[]> message)
        {
            if (message.Timestamp != null)
                _client.UpdateTimeOffset(message.Timestamp.Value);

            _updateHandler?.Invoke(
                new DataEvent<KrakenOrderUpdate[]>(KrakenExchange.ExchangeName, message.Data, receiveTime, originalData)
                    .WithStreamId("executions")
                    .WithUpdateType(message.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                    .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset())
                );
            return CallResult.SuccessResult;
        }
    }
}
