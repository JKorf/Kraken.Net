using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesNotificationSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesNotificationUpdate>
    {
        protected readonly Action<DataEvent<KrakenFuturesNotificationUpdate>> _handler;

        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenFuturesNotificationSubscription(ILogger logger, Action<DataEvent<KrakenFuturesNotificationUpdate>> handler) : base(logger, true)
        {
            _handler = handler;

            ListenerIdentifiers = new HashSet<string> { "notifications_auth" };
        }

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
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

        public override Query? GetUnsubQuery()
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesAuthRequest()
                {
                    Event = "unsubscribe",
                    Feed = "notifications_auth"
                },
                Authenticated);
        }

        public override Type? GetMessageType(IMessageAccessor message) => typeof(KrakenFuturesNotificationUpdate);

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            var data = (KrakenFuturesNotificationUpdate)message.Data;
            _handler.Invoke(message.As(data, data.Feed, null, SocketUpdateType.Update));
            return new CallResult(null);
        }
    }
}
