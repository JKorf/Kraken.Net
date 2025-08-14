using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesOpenPositionsSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesResponse>
    {
        private readonly SocketApiClient _client;
        protected readonly Action<DataEvent<KrakenFuturesOpenPositionUpdate>> _handler;

        public KrakenFuturesOpenPositionsSubscription(ILogger logger, SocketApiClient client, Action<DataEvent<KrakenFuturesOpenPositionUpdate>> handler) : base(logger, true)
        {
            _client = client;
            _handler = handler;

            MessageMatcher = MessageMatcher.Create<KrakenFuturesOpenPositionUpdate>("open_positions", DoHandleMessage);
        }

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
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
                _client,
                new KrakenFuturesAuthRequest()
                {
                    Event = "unsubscribe",
                    Feed = "open_positions",
                },
                Authenticated);
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenFuturesOpenPositionUpdate> message)
        {
            _handler.Invoke(message.As(message.Data, message.Data.Feed, null, ConnectionInvocations == 1 ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return CallResult.SuccessResult;
        }
    }
}
