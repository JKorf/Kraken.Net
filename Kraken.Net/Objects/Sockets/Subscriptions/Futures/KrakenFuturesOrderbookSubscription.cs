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
    internal class KrakenFuturesOrderbookSubscription : Subscription<KrakenFuturesResponse, object>
    {
        private List<string>? _symbols;
        protected readonly Action<DataEvent<KrakenFuturesBookSnapshotUpdate>> _snapshotHandler;
        protected readonly Action<DataEvent<KrakenFuturesBookUpdate>> _updateHandler;

        public override List<string> Identifiers { get; }

        public KrakenFuturesOrderbookSubscription(ILogger logger, List<string> symbols, Action<DataEvent<KrakenFuturesBookSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesBookUpdate>> updateHandler) : base(logger, false)
        {
            _symbols = symbols;
            _snapshotHandler = snapshotHandler;
            _updateHandler = updateHandler;

            Identifiers = symbols.Select(s => "book-" + s.ToLowerInvariant()).ToList();
            Identifiers.AddRange(symbols.Select(s => "book_snapshot-" + s.ToLowerInvariant()).ToList());
        }

        public override BaseQuery? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesRequest()
                {
                    Event = "subscribe",
                    Feed = "book",
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
                    Feed = "book",
                    Symbols = _symbols
                },
                Authenticated);
        }

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<BaseParsedMessage> message)
        {
            if (message.Data.Data is KrakenFuturesBookSnapshotUpdate snapshot)
            {
                _snapshotHandler.Invoke(message.As(snapshot, snapshot.Symbol, SocketUpdateType.Snapshot));
                return Task.FromResult(new CallResult(null));
            }
            else if(message.Data.Data is KrakenFuturesBookUpdate update)
            {
                _updateHandler.Invoke(message.As(update, update.Symbol, SocketUpdateType.Update));
                return Task.FromResult(new CallResult(null));
            }

            return Task.FromResult(new CallResult(null));
        }

        public override Task<CallResult> HandleEventAsync(SocketConnection connection, DataEvent<ParsedMessage<object>> message) => throw new NotImplementedException();
    }
}
