using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;
using Kraken.Net.Objects.Sockets.Subscriptions.Spot;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesOrdersSubscription : Subscription<KrakenFuturesResponse, object>
    {
        protected readonly Action<DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate>> _snapshotHandler;
        protected readonly Action<DataEvent<KrakenFuturesOpenOrdersUpdate>> _updateHandler;
        private bool _verbose;

        public override List<string> Identifiers { get; }

        public KrakenFuturesOrdersSubscription(ILogger logger, bool verbose, Action<DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesOpenOrdersUpdate>> updateHandler) : base(logger, true)
        {
            _snapshotHandler = snapshotHandler;
            _updateHandler = updateHandler;
            _verbose = verbose;

            if (verbose)
                Identifiers = new List<string> { "open_orders_verbose_snapshot", "open_orders_verbose" };
            else
                Identifiers = new List<string> { "open_orders_snapshot", "open_orders" };
        }

        public override BaseQuery? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesAuthRequest()
                {
                    Event = "subscribe",
                    Feed = _verbose ? "open_orders_verbose" : "open_orders",
                    OriginalChallenge = (string)connection.Properties["OriginalChallenge"],
                    SignedChallenge = (string)connection.Properties["SignedChallenge"],
                    ApiKey = ((KrakenFuturesAuthenticationProvider)connection.ApiClient.AuthenticationProvider).GetApiKey(),
                },
                Authenticated);
        }

        public override BaseQuery? GetUnsubQuery()
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesAuthRequest()
                {
                    Event = "unsubscribe",
                    Feed = _verbose ? "open_orders_verbose" : "open_orders",
                },
                Authenticated);
        }

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<BaseParsedMessage> message)
        {
            if (message.Data.Data is KrakenFuturesOpenOrdersSnapshotUpdate snapshot)
            {
                _snapshotHandler.Invoke(message.As(snapshot, snapshot.Symbol, SocketUpdateType.Snapshot));
                return Task.FromResult(new CallResult(null));
            }
            else if(message.Data.Data is KrakenFuturesOpenOrdersUpdate update)
            {
                _updateHandler.Invoke(message.As(update, update.Symbol, SocketUpdateType.Update));
                return Task.FromResult(new CallResult(null));
            }

            return Task.FromResult(new CallResult(null));
        }

        public override Task<CallResult> HandleEventAsync(SocketConnection connection, DataEvent<ParsedMessage<object>> message) => throw new NotImplementedException();
    }
}
