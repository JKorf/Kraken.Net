using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            if (feed == "open_orders_verbose_snapshot" || feed == "open_orders_snapshot")
                return typeof(KrakenFuturesOpenOrdersSnapshotUpdate);

            return typeof(KrakenFuturesOpenOrdersUpdate);
        }

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            if (message.Data is KrakenFuturesOpenOrdersSnapshotUpdate snapshot)
            {
                _snapshotHandler.Invoke(message.As(snapshot, snapshot.Symbol, SocketUpdateType.Snapshot));
                return new CallResult(null);
            }
            else if (message.Data is KrakenFuturesOpenOrdersUpdate update)
            {
                _updateHandler.Invoke(message.As(update, update.Symbol, SocketUpdateType.Update));
                return new CallResult(null);
            }

            return new CallResult(null);
        }
    }
}
