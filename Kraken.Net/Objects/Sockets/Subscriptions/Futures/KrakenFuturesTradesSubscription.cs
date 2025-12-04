using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesTradesSubscription : Subscription
    {
        private readonly SocketApiClient _client;
        private List<string> _symbols;
        protected readonly Action<DataEvent<KrakenFuturesTradeUpdate[]>> _handler;

        public KrakenFuturesTradesSubscription(ILogger logger, SocketApiClient client, List<string> symbols, Action<DataEvent<KrakenFuturesTradeUpdate[]>> handler) : base(logger, false)
        {
            _client = client;
            _symbols = symbols;
            _handler = handler;

            var routes = new List<MessageRoute>();
            var checkers = new List<MessageHandlerLink>();
            foreach (var symbol in symbols)
            {
                checkers.Add(new MessageHandlerLink<KrakenFuturesTradesSnapshotUpdate>("trade_snapshot-" + symbol.ToLowerInvariant(), DoHandleMessage));
                checkers.Add(new MessageHandlerLink<KrakenFuturesTradeUpdate>("trade-" + symbol.ToLowerInvariant(), DoHandleMessage));

                routes.Add(MessageRoute<KrakenFuturesTradesSnapshotUpdate>.CreateWithoutTopicFilter("trade_snapshot-" + symbol.ToLowerInvariant(), DoHandleMessage));
                routes.Add(MessageRoute<KrakenFuturesTradeUpdate>.CreateWithoutTopicFilter("trade-" + symbol.ToLowerInvariant(), DoHandleMessage));
            }

            MessageMatcher = MessageMatcher.Create(checkers.ToArray());
            MessageRouter = MessageRouter.Create(routes.ToArray());
        }

        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
                new KrakenFuturesRequest()
                {
                    Event = "subscribe",
                    Feed = "trade",
                    Symbols = _symbols
                },
                Authenticated)
            {
                RequiredResponses = _symbols.Count()
            };
        }

        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
                new KrakenFuturesRequest()
                {
                    Event = "unsubscribe",
                    Feed = "trade",
                    Symbols = _symbols
                },
                Authenticated)
            {
                RequiredResponses = _symbols.Count()
            };
        }

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenFuturesTradesSnapshotUpdate message)
        {
            _handler.Invoke(
                new DataEvent<KrakenFuturesTradeUpdate[]>(message.Trades, receiveTime, originalData)
                    .WithStreamId(message.Feed)
                    .WithUpdateType(SocketUpdateType.Snapshot)
                    .WithSymbol(message.Symbol)
                );

            return CallResult.SuccessResult;
        }

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenFuturesTradeUpdate message)
        {
            _handler.Invoke(
                new DataEvent<KrakenFuturesTradeUpdate[]>([message], receiveTime, originalData)
                    .WithStreamId(message.Feed)
                    .WithUpdateType(SocketUpdateType.Update)
                    .WithSymbol(message.Symbol)
                );

            return CallResult.SuccessResult;
        }
    }
}
