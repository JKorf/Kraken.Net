using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesTradesSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesResponse>
    {
        private readonly MessagePath _feedPath = MessagePath.Get().Property("feed");

        private List<string>? _symbols;
        protected readonly Action<DataEvent<IEnumerable<KrakenFuturesTradeUpdate>>> _handler;

        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenFuturesTradesSubscription(ILogger logger, List<string> symbols, Action<DataEvent<IEnumerable<KrakenFuturesTradeUpdate>>> handler) : base(logger, false)
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
                Authenticated);
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
                Authenticated);
        }

        public override Type? GetMessageType(IMessageAccessor message)
        {
            // TODO if we give a path a hash we can cache the results for the paths
            if (message.GetValue<string>(_feedPath) == "trade_snapshot")
                return typeof(KrakenFuturesTradesSnapshotUpdate);
            return typeof(KrakenFuturesTradeUpdate);
        }

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<object> message)
        {
            if (message.Data is KrakenFuturesTradesSnapshotUpdate snapshot)
            {
                _handler.Invoke(message.As(snapshot.Trades, snapshot.Symbol, SocketUpdateType.Snapshot));
                return Task.FromResult(new CallResult(null));
            }
            else if (message.Data is KrakenFuturesTradeUpdate update)
            {
                _handler.Invoke(message.As<IEnumerable<KrakenFuturesTradeUpdate>>(new[] { update }, update.Symbol, SocketUpdateType.Update));
                return Task.FromResult(new CallResult(null));
            }

            return Task.FromResult(new CallResult(null));
        }
    }
}
