using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenFuturesSubscription<T> : Subscription<KrakenFuturesResponse, KrakenFuturesResponse> where T : KrakenFuturesEvent
    {
        private string _feed;
        private List<string>? _symbols;
        protected readonly Action<DataEvent<T>> _handler;

        public KrakenFuturesSubscription(ILogger logger, string feed, List<string>? symbols, Action<DataEvent<T>> handler) : base(logger, false)
        {
            _feed = feed;
            _symbols = symbols;
            _handler = handler;

            if (symbols?.Count > 0)
                MessageMatcher = MessageMatcher.Create(symbols.Select(x => new MessageHandlerLink<T>(_feed + "-" + x.ToLowerInvariant(), DoHandleMessage)).ToArray());
            else
                MessageMatcher = MessageMatcher.Create<T>(_feed, DoHandleMessage);
        }

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesRequest()
                {
                    Event = "subscribe",
                    Feed = _feed,
                    Symbols = _symbols
                },
                Authenticated)
            {
                RequiredResponses = _symbols?.Count() ?? 1
            };
        }

        public override Query? GetUnsubQuery()
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesRequest()
                {
                    Event = "unsubscribe",
                    Feed = _feed,
                    Symbols = _symbols
                },
                Authenticated)
            {
                RequiredResponses = _symbols?.Count() ?? 1
            };
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<T> message)
        {
            _handler.Invoke(message.As(message.Data, message.Data.Feed, message.Data!.Symbol, SocketUpdateType.Update));
            return CallResult.SuccessResult;
        }
    }
}
