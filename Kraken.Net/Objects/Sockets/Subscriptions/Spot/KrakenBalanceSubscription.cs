using CryptoExchange.Net.Clients;
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

            MessageMatcher = MessageMatcher.Create([
                new MessageHandlerLink<KrakenSocketUpdateV2<KrakenBalanceSnapshot[]>>("balancessnapshot", DoHandleMessage),
                new MessageHandlerLink<KrakenSocketUpdateV2<KrakenBalanceUpdate[]>>("balances", DoHandleMessage)

                ]);
        }

        public override Query? GetSubQuery(SocketConnection connection)
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

        public override Query? GetUnsubQuery()
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

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenSocketUpdateV2<KrakenBalanceSnapshot[]>> message)
        {
                _snapshotHandler?.Invoke(message.As(message.Data.Data, "balances", null, SocketUpdateType.Snapshot).WithDataTimestamp(message.Data.Timestamp));
            return CallResult.SuccessResult;
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenSocketUpdateV2<KrakenBalanceUpdate[]>> message)
        {
            _updateHandler?.Invoke(message.As(message.Data.Data, "balances", null, SocketUpdateType.Update).WithDataTimestamp(message.Data.Timestamp));
            return CallResult.SuccessResult;
        }
    }
}
