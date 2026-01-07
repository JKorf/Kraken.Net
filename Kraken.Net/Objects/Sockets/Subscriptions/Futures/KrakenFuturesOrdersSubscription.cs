using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesOrdersSubscription : Subscription
    {
        private readonly SocketApiClient _client;
        protected readonly Action<DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate>> _snapshotHandler;
        protected readonly Action<DataEvent<KrakenFuturesOpenOrdersUpdate>> _updateHandler;
        private bool _verbose;

        public KrakenFuturesOrdersSubscription(ILogger logger, SocketApiClient client, bool verbose, Action<DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesOpenOrdersUpdate>> updateHandler) : base(logger, true)
        {
            _client = client;
            _snapshotHandler = snapshotHandler;
            _updateHandler = updateHandler;
            _verbose = verbose;

            if (verbose)
            {
                MessageMatcher = MessageMatcher.Create([
                    new MessageHandlerLink<KrakenFuturesOpenOrdersSnapshotUpdate>("open_orders_verbose_snapshot", DoHandleMessage),
                    new MessageHandlerLink<KrakenFuturesOpenOrdersUpdate>("open_orders_verbose", DoHandleMessage)
                    ]);
                MessageRouter = MessageRouter.Create([
                    MessageRoute<KrakenFuturesOpenOrdersSnapshotUpdate>.CreateWithoutTopicFilter("open_orders_verbose_snapshot", DoHandleMessage),
                    MessageRoute<KrakenFuturesOpenOrdersUpdate>.CreateWithoutTopicFilter("open_orders_verbose", DoHandleMessage)
                    ]);
            }
            else
            {
                MessageMatcher = MessageMatcher.Create([
                    new MessageHandlerLink<KrakenFuturesOpenOrdersSnapshotUpdate>("open_orders_snapshot", DoHandleMessage),
                    new MessageHandlerLink<KrakenFuturesOpenOrdersUpdate>("open_orders", DoHandleMessage)
                    ]);
                MessageRouter = MessageRouter.Create([
                    MessageRoute<KrakenFuturesOpenOrdersSnapshotUpdate>.CreateWithoutTopicFilter("open_orders_snapshot",DoHandleMessage),
                    MessageRoute<KrakenFuturesOpenOrdersUpdate>.CreateWithoutTopicFilter("open_orders", DoHandleMessage)
                    ]);
            }
        }

        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
                new KrakenFuturesAuthRequest()
                {
                    Event = "subscribe",
                    Feed = _verbose ? "open_orders_verbose" : "open_orders",
                    OriginalChallenge = (string)connection.Properties["OriginalChallenge"],
                    SignedChallenge = (string)connection.Properties["SignedChallenge"],
                    ApiKey = ((KrakenFuturesAuthenticationProvider)connection.ApiClient.AuthenticationProvider!).GetApiKey(),
                },
                Authenticated);
        }

        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
                new KrakenFuturesAuthRequest()
                {
                    Event = "unsubscribe",
                    Feed = _verbose ? "open_orders_verbose" : "open_orders",
                },
                Authenticated);
        }

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenFuturesOpenOrdersSnapshotUpdate message)
        {
            DateTime? timestamp = message.Orders.Any() ? message.Orders.Max(x => x.LastUpdateTime) : null;
            if (timestamp != null)
                _client.UpdateTimeOffset(timestamp.Value);

            _snapshotHandler.Invoke(
                new DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate>(KrakenExchange.ExchangeName, message, receiveTime, originalData)
                    .WithStreamId(message.Feed)
                    .WithUpdateType(SocketUpdateType.Snapshot)
                    .WithSymbol(message.Symbol)
                    .WithDataTimestamp(timestamp, _client.GetTimeOffset())
                );
            
            return CallResult.SuccessResult;
        }

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenFuturesOpenOrdersUpdate message)
        {
            if (message.Order?.LastUpdateTime != null)
                _client.UpdateTimeOffset(message.Order.LastUpdateTime);

            _updateHandler.Invoke(
                new DataEvent<KrakenFuturesOpenOrdersUpdate>(KrakenExchange.ExchangeName, message, receiveTime, originalData)
                    .WithStreamId(message.Feed)
                    .WithUpdateType(SocketUpdateType.Update)
                    .WithSymbol(message.Symbol)
                    .WithDataTimestamp(message.Order?.LastUpdateTime, _client.GetTimeOffset())
                );
            return CallResult.SuccessResult;
        }
    }
}
