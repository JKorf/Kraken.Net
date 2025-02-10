using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesUserTradeSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesUserTradesUpdate>
    {
        protected readonly Action<DataEvent<KrakenFuturesUserTradesUpdate>> _handler;
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenFuturesUserTradeSubscription(ILogger logger, Action<DataEvent<KrakenFuturesUserTradesUpdate>> handler) : base(logger, true)
        {
            _handler = handler;
            ListenerIdentifiers = new HashSet<string> { "fills_snapshot", "fills" };
        }

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
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
                new KrakenFuturesAuthRequest()
                {
                    Event = "unsubscribe",
                    Feed = "fills",
                },
                Authenticated);
        }

        public override Type? GetMessageType(IMessageAccessor message) => typeof(KrakenFuturesUserTradesUpdate);

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            var data = (KrakenFuturesUserTradesUpdate)message.Data;
            _handler.Invoke(message.As(data, data.Feed, null, string.Equals(data.Feed, "fills_snapshot", StringComparison.Ordinal) ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                .WithDataTimestamp(data.Trades.Any() ? data.Trades.Max(x => x.Timestamp) : null));
            return new CallResult(null);
        }
    }
}
