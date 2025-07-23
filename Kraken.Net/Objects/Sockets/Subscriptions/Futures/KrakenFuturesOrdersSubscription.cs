using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesOrdersSubscription : Subscription<KrakenFuturesResponse, object>
    {
        protected readonly Action<DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate>> _snapshotHandler;
        protected readonly Action<DataEvent<KrakenFuturesOpenOrdersUpdate>> _updateHandler;
        private bool _verbose;

        public KrakenFuturesOrdersSubscription(ILogger logger, bool verbose, Action<DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesOpenOrdersUpdate>> updateHandler) : base(logger, true)
        {
            _snapshotHandler = snapshotHandler;
            _updateHandler = updateHandler;
            _verbose = verbose;

            if (verbose)
            {
                MessageMatcher = MessageMatcher.Create([
                    new MessageHandlerLink<KrakenFuturesOpenOrdersSnapshotUpdate>("open_orders_verbose_snapshot", DoHandleMessage),
                    new MessageHandlerLink<KrakenFuturesOpenOrdersUpdate>("open_orders_verbose", DoHandleMessage)
                    ]);
            }
            else
            {
                MessageMatcher = MessageMatcher.Create([
                    new MessageHandlerLink<KrakenFuturesOpenOrdersSnapshotUpdate>("open_orders_snapshot", DoHandleMessage),
                    new MessageHandlerLink<KrakenFuturesOpenOrdersUpdate>("open_orders", DoHandleMessage)
                    ]);
            }
        }

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
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

        public override Query? GetUnsubQuery()
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesAuthRequest()
                {
                    Event = "unsubscribe",
                    Feed = _verbose ? "open_orders_verbose" : "open_orders",
                },
                Authenticated);
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate> message)
        {
            _snapshotHandler.Invoke(message.As(message.Data, message.Data.Feed, message.Data.Symbol, SocketUpdateType.Snapshot).WithDataTimestamp(message.Data.Orders.Any() ? message.Data.Orders.Max(x => x.LastUpdateTime) : null));
            return CallResult.SuccessResult;
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenFuturesOpenOrdersUpdate> message)
        {
            _updateHandler.Invoke(message.As(message.Data, message.Data.Feed, message.Data.Symbol, SocketUpdateType.Update).WithDataTimestamp(message.Data.Order?.LastUpdateTime));
            return CallResult.SuccessResult;
        }
    }
}
