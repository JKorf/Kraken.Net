using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesNotificationSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesNotificationUpdate>
    {
        private readonly SocketApiClient _client;
        protected readonly Action<DataEvent<KrakenFuturesNotificationUpdate>> _handler;

        public KrakenFuturesNotificationSubscription(ILogger logger, SocketApiClient client, Action<DataEvent<KrakenFuturesNotificationUpdate>> handler) : base(logger, true)
        {
            _client = client;
            _handler = handler;

            MessageMatcher = MessageMatcher.Create<KrakenFuturesNotificationUpdate>("notifications_auth", DoHandleMessage);
        }

        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
                new KrakenFuturesAuthRequest()
                {
                    Event = "subscribe",
                    Feed = "notifications_auth",
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
                    Feed = "notifications_auth"
                },
                Authenticated);
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenFuturesNotificationUpdate> message)
        {
            _handler.Invoke(message.As(message.Data, message.Data.Feed, null, SocketUpdateType.Update));
            return CallResult.SuccessResult;
        }
    }
}
