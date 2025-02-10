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
        private readonly MessagePath _feedPath = MessagePath.Get().Property("feed");

        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenFuturesOrdersSubscription(ILogger logger, bool verbose, Action<DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesOpenOrdersUpdate>> updateHandler) : base(logger, true)
        {
            _snapshotHandler = snapshotHandler;
            _updateHandler = updateHandler;
            _verbose = verbose;

            if (verbose)
                ListenerIdentifiers = new HashSet<string> { "open_orders_verbose_snapshot", "open_orders_verbose" };
            else
                ListenerIdentifiers = new HashSet<string> { "open_orders_snapshot", "open_orders" };
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

        public override Type? GetMessageType(IMessageAccessor message)
        {
            var feed = message.GetValue<string>(_feedPath);
            if (string.Equals(feed, "open_orders_verbose_snapshot", StringComparison.Ordinal)
                || string.Equals(feed, "open_orders_snapshot", StringComparison.Ordinal))
            {
                return typeof(KrakenFuturesOpenOrdersSnapshotUpdate);
            }

            return typeof(KrakenFuturesOpenOrdersUpdate);
        }

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            if (message.Data is KrakenFuturesOpenOrdersSnapshotUpdate snapshot)
            {
                _snapshotHandler.Invoke(message.As(snapshot, snapshot.Feed, snapshot.Symbol, SocketUpdateType.Snapshot).WithDataTimestamp(snapshot.Orders.Any() ? snapshot.Orders.Max(x => x.LastUpdateTime) : null));
                return new CallResult(null);
            }
            else if (message.Data is KrakenFuturesOpenOrdersUpdate update)
            {
                _updateHandler.Invoke(message.As(update, update.Feed, update.Symbol, SocketUpdateType.Update).WithDataTimestamp(update.Order?.LastUpdateTime));
                return new CallResult(null);
            }

            return new CallResult(null);
        }
    }
}
