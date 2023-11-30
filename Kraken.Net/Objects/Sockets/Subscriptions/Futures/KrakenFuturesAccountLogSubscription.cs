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
    internal class KrakenFuturesAccountLogSubscription : Subscription<KrakenFuturesResponse, object>
    {
        protected readonly Action<DataEvent<KrakenFuturesAccountLogsSnapshotUpdate>> _snapshotHandler;
        protected readonly Action<DataEvent<KrakenFuturesAccountLogsUpdate>> _updateHandler;

        public override List<string> Identifiers { get; }

        public KrakenFuturesAccountLogSubscription(ILogger logger, Action<DataEvent<KrakenFuturesAccountLogsSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesAccountLogsUpdate>> updateHandler) : base(logger, true)
        {
            _snapshotHandler = snapshotHandler;
            _updateHandler = updateHandler;

            Identifiers = new List<string> { "account_log_snapshot", "account_log" };
        }

        public override BaseQuery? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesAuthRequest()
                {
                    Event = "subscribe",
                    Feed = "account_log",
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
                    Feed = "account_log",
                },
                Authenticated);
        }

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<BaseParsedMessage> message)
        {
            if (message.Data.Data is KrakenFuturesAccountLogsSnapshotUpdate snapshot)
            {
                _snapshotHandler.Invoke(message.As(snapshot, null, SocketUpdateType.Snapshot));
                return Task.FromResult(new CallResult(null));
            }
            else if(message.Data.Data is KrakenFuturesAccountLogsUpdate update)
            {
                _updateHandler.Invoke(message.As(update, null, SocketUpdateType.Update));
                return Task.FromResult(new CallResult(null));
            }

            return Task.FromResult(new CallResult(null));
        }

        public override Task<CallResult> HandleEventAsync(SocketConnection connection, DataEvent<ParsedMessage<object>> message) => throw new NotImplementedException();
    }
}
