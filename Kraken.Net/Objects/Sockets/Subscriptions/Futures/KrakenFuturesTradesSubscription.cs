using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesTradesSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesResponse>
    {
        private readonly MessagePath _feedPath = MessagePath.Get().Property("feed");

        private List<string>? _symbols;
        protected readonly Action<DataEvent<KrakenFuturesTradeUpdate[]>> _handler;

        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenFuturesTradesSubscription(ILogger logger, List<string> symbols, Action<DataEvent<KrakenFuturesTradeUpdate[]>> handler) : base(logger, false)
        {
            _symbols = symbols;
            _handler = handler;

            ListenerIdentifiers = new HashSet<string>(symbols.Select(s => "trade-" + s.ToLowerInvariant()));
            foreach (var symbol in _symbols)
                ListenerIdentifiers.Add("trade_snapshot-" + symbol.ToLowerInvariant());
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

        public override Type? GetMessageType(IMessageAccessor message)
        {
            if (string.Equals(message.GetValue<string>(_feedPath), "trade_snapshot", StringComparison.Ordinal))
                return typeof(KrakenFuturesTradesSnapshotUpdate);
            return typeof(KrakenFuturesTradeUpdate);
        }

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            if (message.Data is KrakenFuturesTradesSnapshotUpdate snapshot)
            {
                _handler.Invoke(message.As(snapshot.Trades, snapshot.Feed, snapshot.Symbol, SocketUpdateType.Snapshot));
                return new CallResult(null);
            }
            else if (message.Data is KrakenFuturesTradeUpdate update)
            {
                _handler.Invoke(message.As<KrakenFuturesTradeUpdate[]>(new[] { update }, update.Feed, update.Symbol, SocketUpdateType.Update));
                return new CallResult(null);
            }

            return new CallResult(null);
        }
    }
}
