using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesUserTradeSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesUserTradesUpdate>
    {
        private readonly SocketApiClient _client;
        protected readonly Action<DataEvent<KrakenFuturesUserTradesUpdate>> _handler;

        public KrakenFuturesUserTradeSubscription(ILogger logger, SocketApiClient client, Action<DataEvent<KrakenFuturesUserTradesUpdate>> handler) : base(logger, true)
        {
            _client = client;
            _handler = handler;

            MessageMatcher = MessageMatcher.Create<KrakenFuturesUserTradesUpdate>(["fills_snapshot", "fills"], DoHandleMessage);
        }

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
                new KrakenFuturesAuthRequest()
                {
                    Event = "subscribe",
                    Feed = "fills",
                    OriginalChallenge = (string)connection.Properties["OriginalChallenge"],
                    SignedChallenge = (string)connection.Properties["SignedChallenge"],
                    ApiKey = ((KrakenFuturesAuthenticationProvider)connection.ApiClient.AuthenticationProvider!).GetApiKey(),
                },
                Authenticated);
        }

        public override Query? GetUnsubQuery()
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
                new KrakenFuturesAuthRequest()
                {
                    Event = "unsubscribe",
                    Feed = "fills",
                },
                Authenticated);
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenFuturesUserTradesUpdate> message)
        {
            _handler.Invoke(message.As(message.Data, message.Data.Feed, null, string.Equals(message.Data.Feed, "fills_snapshot", StringComparison.Ordinal) ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                .WithDataTimestamp(message.Data.Trades.Any() ? message.Data.Trades.Max(x => x.Timestamp) : null));
            return CallResult.SuccessResult;
        }
    }
}
