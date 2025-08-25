﻿using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesOrderbookSubscription : Subscription<KrakenFuturesResponse, object>
    {
        private List<string> _symbols;
        private readonly SocketApiClient _client;
        protected readonly Action<DataEvent<KrakenFuturesBookSnapshotUpdate>> _snapshotHandler;
        protected readonly Action<DataEvent<KrakenFuturesBookUpdate>> _updateHandler;

        public KrakenFuturesOrderbookSubscription(ILogger logger, SocketApiClient client, List<string> symbols, Action<DataEvent<KrakenFuturesBookSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesBookUpdate>> updateHandler) : base(logger, false)
        {
            _client = client;
            _symbols = symbols;
            _snapshotHandler = snapshotHandler;
            _updateHandler = updateHandler;

            var checkers = new List<MessageHandlerLink>();
            foreach(var symbol in symbols)
            {
                checkers.Add(new MessageHandlerLink<KrakenFuturesBookUpdate>("book-" + symbol.ToLowerInvariant(), DoHandleMessage));
                checkers.Add(new MessageHandlerLink<KrakenFuturesBookSnapshotUpdate>("book_snapshot-" + symbol.ToLowerInvariant(), DoHandleMessage));
            }    

            MessageMatcher = MessageMatcher.Create(checkers.ToArray());
        }

        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                _client,
                new KrakenFuturesRequest()
                {
                    Event = "subscribe",
                    Feed = "book",
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
                    Feed = "book",
                    Symbols = _symbols
                },
                Authenticated)
            {
                RequiredResponses = _symbols.Count()
            };
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenFuturesBookSnapshotUpdate> message)
        {
            _snapshotHandler.Invoke(message.As(message.Data, message.Data.Feed, message.Data.Symbol, SocketUpdateType.Snapshot).WithDataTimestamp(message.Data.Timestamp));
            return CallResult.SuccessResult;
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenFuturesBookUpdate> message)
        {
            _updateHandler.Invoke(message.As(message.Data, message.Data.Feed, message.Data.Symbol, SocketUpdateType.Update).WithDataTimestamp(message.Data.Timestamp));
            return CallResult.SuccessResult;
        }
    }
}
