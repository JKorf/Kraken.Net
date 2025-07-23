using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesTradesSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesResponse>
    {
        private List<string> _symbols;
        protected readonly Action<DataEvent<KrakenFuturesTradeUpdate[]>> _handler;

        public KrakenFuturesTradesSubscription(ILogger logger, List<string> symbols, Action<DataEvent<KrakenFuturesTradeUpdate[]>> handler) : base(logger, false)
        {
            _symbols = symbols;
            _handler = handler;

            var checkers = new List<MessageHandlerLink>();
            foreach(var symbol in symbols)
            {
                checkers.Add(new MessageHandlerLink<KrakenFuturesTradesSnapshotUpdate>("trade_snapshot-" + symbol.ToLowerInvariant(), DoHandleMessage));
                checkers.Add(new MessageHandlerLink<KrakenFuturesTradeUpdate>("trade-" + symbol.ToLowerInvariant(), DoHandleMessage));
            }

            MessageMatcher = MessageMatcher.Create(checkers.ToArray());
        }

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
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

        public override Query? GetUnsubQuery()
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
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

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenFuturesTradesSnapshotUpdate> message)
        {
            _handler.Invoke(message.As(message.Data.Trades, message.Data.Feed, message.Data.Symbol, SocketUpdateType.Snapshot));
            return CallResult.SuccessResult;
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenFuturesTradeUpdate> message)
        {
            _handler.Invoke(message.As<KrakenFuturesTradeUpdate[]>(new[] { message.Data }, message.Data.Feed, message.Data.Symbol, SocketUpdateType.Update));
            return CallResult.SuccessResult;
        }
    }
}
