using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenBalanceSubscription : KrakenSubscription
    {
        private readonly SocketApiClient _client;
        private readonly Action<DataEvent<KrakenBalanceSnapshot[]>>? _snapshotHandler;
        private readonly Action<DataEvent<KrakenBalanceUpdate[]>> _updateHandler;

        private bool? _snapshot;

        public KrakenBalanceSubscription(ILogger logger, SocketApiClient client, bool? snapshot, string token, Action<DataEvent<KrakenBalanceSnapshot[]>>? snapshotHandler, Action<DataEvent<KrakenBalanceUpdate[]>> updateHandler) : base(logger, true)
        {
            _client = client;
            _snapshot = snapshot;
            Token = token;

            _snapshotHandler = snapshotHandler;
            _updateHandler = updateHandler;

            MessageRouter = MessageRouter.Create([
                MessageRoute<KrakenSocketUpdateV2<KrakenBalanceSnapshot[]>>.CreateWithoutTopicFilter("balancessnapshot",DoHandleMessage),
                MessageRoute<KrakenSocketUpdateV2<KrakenBalanceUpdate[]>>.CreateWithoutTopicFilter("balances", DoHandleMessage)
                ]);
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
                        Channel = "balances",
                        Snapshot = _snapshot,
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
                        Channel = "balances",
                        Snapshot = _snapshot,
                        Token = Token
                    }
                }, false);
        }

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenSocketUpdateV2<KrakenBalanceSnapshot[]> message)
        {
            if (message.Timestamp != null)
                _client.UpdateTimeOffset(message.Timestamp.Value);

            _snapshotHandler?.Invoke(
                new DataEvent<KrakenBalanceSnapshot[]>(KrakenExchange.ExchangeName, message.Data, receiveTime, originalData)
                    .WithStreamId("balances")
                    .WithUpdateType(SocketUpdateType.Snapshot)
                    .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset())
                );
            return CallResult.SuccessResult;
        }

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenSocketUpdateV2<KrakenBalanceUpdate[]> message)
        {
            if (message.Timestamp != null)
                _client.UpdateTimeOffset(message.Timestamp.Value);

            _updateHandler?.Invoke(
                new DataEvent<KrakenBalanceUpdate[]>(KrakenExchange.ExchangeName, message.Data, receiveTime, originalData)
                    .WithStreamId("balances")
                    .WithUpdateType(SocketUpdateType.Update)
                    .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset())
                );
            return CallResult.SuccessResult;
        }
    }
}
