using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;
using Kraken.Net.Objects.Sockets.Subscriptions.Spot;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesTradesSubscription : Subscription<KrakenFuturesResponse, object>
    {
        private string _feed;
        private List<string>? _symbols;
        protected readonly Action<DataEvent<IEnumerable<KrakenFuturesTradeUpdate>>> _handler;

        public override List<string> Identifiers { get; }

        public KrakenFuturesTradesSubscription(ILogger logger, List<string> symbols, Action<DataEvent<IEnumerable<KrakenFuturesTradeUpdate>>> handler) : base(logger, false)
        {
            _symbols = symbols;
            _handler = handler;

            Identifiers = symbols.Select(s => "trade-" + s.ToLowerInvariant()).ToList();
            Identifiers.AddRange(symbols.Select(s => "trade_snapshot-" + s.ToLowerInvariant()).ToList());
        }

        public override BaseQuery? GetSubQuery(SocketConnection connection)
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

        public override BaseQuery? GetUnsubQuery()
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

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<BaseParsedMessage> message)
        {
            if (message.Data.Data is KrakenFuturesTradesSnapshotUpdate snapshot)
            {
                _handler.Invoke(message.As(snapshot.Trades, snapshot.Symbol, SocketUpdateType.Snapshot));
                return Task.FromResult(new CallResult(null));
            }
            else if(message.Data.Data is KrakenFuturesTradeUpdate update)
            {
                _handler.Invoke(message.As<IEnumerable<KrakenFuturesTradeUpdate>>(new[] { update }, update.Symbol, SocketUpdateType.Update));
                return Task.FromResult(new CallResult(null));
            }

            return Task.FromResult(new CallResult(null));
        }

        public override Task<CallResult> HandleEventAsync(SocketConnection connection, DataEvent<ParsedMessage<object>> message) => throw new NotImplementedException();
    }
}
