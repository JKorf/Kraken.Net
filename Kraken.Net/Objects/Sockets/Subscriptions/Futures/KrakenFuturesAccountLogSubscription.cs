using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesAccountLogSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesResponse>
    {
        private readonly SocketApiClient _client;
        protected readonly Action<DataEvent<KrakenFuturesAccountLogsSnapshotUpdate>> _snapshotHandler;
        protected readonly Action<DataEvent<KrakenFuturesAccountLogsUpdate>> _updateHandler;

        public KrakenFuturesAccountLogSubscription(ILogger logger, SocketApiClient client, Action<DataEvent<KrakenFuturesAccountLogsSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesAccountLogsUpdate>> updateHandler) : base(logger, true)
        {
            _client = client;
            _snapshotHandler = snapshotHandler;
            _updateHandler = updateHandler;

            MessageMatcher = MessageMatcher.Create([
                new MessageHandlerLink<KrakenFuturesAccountLogsSnapshotUpdate>("account_log_snapshot", DoHandleMessage),
                new MessageHandlerLink<KrakenFuturesAccountLogsUpdate>("account_log", DoHandleMessage)
                ]);
        }

        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
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

        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
                new KrakenFuturesAuthRequest()
                {
                    Event = "unsubscribe",
                    Feed = "account_log",
                },
                Authenticated);
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenFuturesAccountLogsUpdate> message)
        {
            _updateHandler.Invoke(message.As(message.Data, message.Data.Feed, null, SocketUpdateType.Update).WithDataTimestamp(message.Data.NewEntry.Timestamp));
            return CallResult.SuccessResult;
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenFuturesAccountLogsSnapshotUpdate> message)
        {
            _snapshotHandler.Invoke(message.As(message.Data, message.Data.Feed, null, SocketUpdateType.Snapshot).WithDataTimestamp(message.Data.Logs.Any() ? message.Data.Logs.Max(x => x.Timestamp) : null));
            return CallResult.SuccessResult;
        }
    }
}
