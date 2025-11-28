using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesOrdersSubscription : Subscription<KrakenFuturesResponse, object>
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
                    new MessageRoute<KrakenFuturesOpenOrdersSnapshotUpdate>("open_orders_verbose_snapshot", (string?)null, DoHandleMessage),
                    new MessageRoute<KrakenFuturesOpenOrdersUpdate>("open_orders_verbose",  (string?)null, DoHandleMessage)
                    ]);
            }
            else
            {
                MessageMatcher = MessageMatcher.Create([
                    new MessageHandlerLink<KrakenFuturesOpenOrdersSnapshotUpdate>("open_orders_snapshot", DoHandleMessage),
                    new MessageHandlerLink<KrakenFuturesOpenOrdersUpdate>("open_orders", DoHandleMessage)
                    ]);
                MessageRouter = MessageRouter.Create([
                    new MessageRoute<KrakenFuturesOpenOrdersSnapshotUpdate>("open_orders_snapshot", (string?)null,DoHandleMessage),
                    new MessageRoute<KrakenFuturesOpenOrdersUpdate>("open_orders",(string?)null, DoHandleMessage)
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
            _snapshotHandler.Invoke(
                new DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate>(message, receiveTime, originalData)
                    .WithStreamId(message.Feed)
                    .WithUpdateType(SocketUpdateType.Snapshot)
                    .WithSymbol(message.Symbol)
                    .WithDataTimestamp(message.Orders.Any() ? message.Orders.Max(x => x.LastUpdateTime) : null)
                );
            
            return CallResult.SuccessResult;
        }

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenFuturesOpenOrdersUpdate message)
        {
            _updateHandler.Invoke(
                new DataEvent<KrakenFuturesOpenOrdersUpdate>(message, receiveTime, originalData)
                    .WithStreamId(message.Feed)
                    .WithUpdateType(SocketUpdateType.Update)
                    .WithSymbol(message.Symbol)
                    .WithDataTimestamp(message.Order?.LastUpdateTime)
                );
            return CallResult.SuccessResult;
        }
    }
}
