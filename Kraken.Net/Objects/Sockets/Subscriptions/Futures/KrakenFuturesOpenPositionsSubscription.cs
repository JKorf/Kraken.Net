using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesOpenPositionsSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesResponse>
    {
        protected readonly Action<DataEvent<KrakenFuturesOpenPositionUpdate>> _handler;
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenFuturesOpenPositionsSubscription(ILogger logger, Action<DataEvent<KrakenFuturesOpenPositionUpdate>> handler) : base(logger, true)
        {
            _handler = handler;
            ListenerIdentifiers = new HashSet<string> { "open_positions" };
        }

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesAuthRequest()
                {
                    Event = "subscribe",
                    Feed = "open_positions",
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
                    Feed = "open_positions",
                },
                Authenticated);
        }

        public override Type? GetMessageType(IMessageAccessor message) => typeof(KrakenFuturesOpenPositionUpdate);
        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            var data = (KrakenFuturesOpenPositionUpdate)message.Data;
            _handler.Invoke(message.As(data, data.Feed, null, ConnectionInvocations == 1 ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return new CallResult(null);
        }
    }
}
