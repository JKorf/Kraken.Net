﻿using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesAccountLogSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesResponse>
    {
        protected readonly Action<DataEvent<KrakenFuturesAccountLogsSnapshotUpdate>> _snapshotHandler;
        protected readonly Action<DataEvent<KrakenFuturesAccountLogsUpdate>> _updateHandler;
        private readonly MessagePath _feedPath = MessagePath.Get().Property("feed");

        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenFuturesAccountLogSubscription(ILogger logger, Action<DataEvent<KrakenFuturesAccountLogsSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesAccountLogsUpdate>> updateHandler) : base(logger, true)
        {
            _snapshotHandler = snapshotHandler;
            _updateHandler = updateHandler;

            ListenerIdentifiers = new HashSet<string> { "account_log_snapshot", "account_log" };
        }

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesAuthRequest()
                {
                    Event = "subscribe",
                    Feed = "account_log",
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
                    Feed = "account_log",
                },
                Authenticated);
        }

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            if (message.Data is KrakenFuturesAccountLogsSnapshotUpdate snapshot)
            {
                _snapshotHandler.Invoke(message.As(snapshot, snapshot.Feed, null, SocketUpdateType.Snapshot).WithDataTimestamp(snapshot.Logs.Any() ? snapshot.Logs.Max(x => x.Timestamp) : null));
                return CallResult.SuccessResult;
            }
            else if (message.Data is KrakenFuturesAccountLogsUpdate update)
            {
                _updateHandler.Invoke(message.As(update, update.Feed, null, SocketUpdateType.Update).WithDataTimestamp(update.NewEntry.Timestamp));
                return CallResult.SuccessResult;
            }

            return CallResult.SuccessResult;
        }

        public override Type? GetMessageType(IMessageAccessor message)
        {
            var feed = message.GetValue<string>(_feedPath);
            if (string.Equals(feed, "account_log_snapshot", StringComparison.Ordinal))
                return typeof(KrakenFuturesAccountLogsSnapshotUpdate);

            return typeof(KrakenFuturesAccountLogsUpdate);
        }
    }
}
