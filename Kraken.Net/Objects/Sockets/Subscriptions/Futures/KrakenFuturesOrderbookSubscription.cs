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
    internal class KrakenFuturesOrderbookSubscription : Subscription<KrakenFuturesResponse, object>
    {
        private readonly MessagePath _feedPath = MessagePath.Get().Property("feed");

        private List<string>? _symbols;
        protected readonly Action<DataEvent<KrakenFuturesBookSnapshotUpdate>> _snapshotHandler;
        protected readonly Action<DataEvent<KrakenFuturesBookUpdate>> _updateHandler;

        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenFuturesOrderbookSubscription(ILogger logger, List<string> symbols, Action<DataEvent<KrakenFuturesBookSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesBookUpdate>> updateHandler) : base(logger, false)
        {
            _symbols = symbols;
            _snapshotHandler = snapshotHandler;
            _updateHandler = updateHandler;

            ListenerIdentifiers = new HashSet<string>(symbols.Select(s => "book-" + s.ToLowerInvariant()).ToList());
            foreach (var symbol in _symbols)
                ListenerIdentifiers.Add("book_snapshot-" + symbol.ToLowerInvariant());
        }

        public override Query? GetSubQuery(SocketConnection connection)
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

        public override Query? GetUnsubQuery()
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

        public override Type? GetMessageType(IMessageAccessor message)
        {
            if (message.GetValue<string>(_feedPath) == "book_snapshot")
                return typeof(KrakenFuturesBookSnapshotUpdate);
            return typeof(KrakenFuturesBookUpdate);
        }

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<object> message)
        {
            if (message.Data is KrakenFuturesBookSnapshotUpdate snapshot)
            {
                _snapshotHandler.Invoke(message.As(snapshot, snapshot.Symbol, SocketUpdateType.Snapshot));
                return Task.FromResult(new CallResult(null));
            }
            else if (message.Data is KrakenFuturesBookUpdate update)
            {
                _updateHandler.Invoke(message.As(update, update.Symbol, SocketUpdateType.Update));
                return Task.FromResult(new CallResult(null));
            }

            return Task.FromResult(new CallResult(null));
        }
    }
}
