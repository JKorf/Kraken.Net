using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesBalanceSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesResponse>
    {
        private readonly SocketApiClient _client;
        protected readonly Action<DataEvent<KrakenFuturesBalancesUpdate>> _handler;

        public KrakenFuturesBalanceSubscription(ILogger logger, SocketApiClient client, Action<DataEvent<KrakenFuturesBalancesUpdate>> handler) : base(logger, true)
        {
            _client = client;
            _handler = handler;

            MessageMatcher = MessageMatcher.Create<KrakenFuturesBalancesUpdate>(["balances_snapshot", "balances"], DoHandleMessage);
        }

        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
                new KrakenFuturesAuthRequest()
                {
                    Event = "subscribe",
                    Feed = "balances",
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
                    Feed = "balances",
                },
                Authenticated);
        }

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenFuturesBalancesUpdate message)
        {
            _handler.Invoke(
                new DataEvent<KrakenFuturesBalancesUpdate>(message, receiveTime, originalData)
                    .WithStreamId(message.Feed)
                    .WithDataTimestamp(message.Timestamp)
                    .WithUpdateType(string.Equals(message.Feed, "balances_snapshot", StringComparison.Ordinal) ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                );

            return CallResult.SuccessResult;
        }
    }
}
