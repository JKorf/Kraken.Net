using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenFuturesSubscription<T> : Subscription where T : KrakenFuturesEvent
    {
        private readonly SocketApiClient _client;
        private string _feed;
        private List<string>? _symbols;
        protected readonly Action<DateTime, string?, T> _handler;

        public KrakenFuturesSubscription(ILogger logger, SocketApiClient client, string feed, List<string>? symbols, Action<DateTime, string?, T> handler) : base(logger, false)
        {
            _client = client;
            _feed = feed;
            _symbols = symbols;
            _handler = handler;

            if (symbols?.Count > 0)
            {
                MessageMatcher = MessageMatcher.Create(symbols.Select(x => new MessageHandlerLink<T>(_feed + "-" + x.ToLowerInvariant(), DoHandleMessage)).ToArray());
                MessageRouter = MessageRouter.Create(symbols.Select(x => MessageRoute<T>.CreateWithoutTopicFilter(_feed + "-" + x.ToLowerInvariant(), DoHandleMessage)).ToArray());
            }
            else
            {
                MessageMatcher = MessageMatcher.Create<T>(_feed, DoHandleMessage);
                MessageRouter = MessageRouter.CreateWithoutTopicFilter<T>(_feed, DoHandleMessage);
            }
        }

        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
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

        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
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

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, T message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            //_handler.Invoke(message.As(message.Data, message.Data.Feed, message.Data!.Symbol, SocketUpdateType.Update));
            return CallResult.SuccessResult;
        }
    }
}
