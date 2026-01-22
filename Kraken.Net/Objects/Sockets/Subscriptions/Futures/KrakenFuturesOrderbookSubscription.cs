using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesOrderbookSubscription : Subscription
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

            IndividualSubscriptionCount = symbols.Count;

            var routes = new List<MessageRoute>();
            foreach (var symbol in symbols)
            {
                routes.Add(MessageRoute<KrakenFuturesBookUpdate>.CreateWithoutTopicFilter("book-" + symbol.ToLowerInvariant(), DoHandleMessage));
                routes.Add(MessageRoute<KrakenFuturesBookSnapshotUpdate>.CreateWithoutTopicFilter("book_snapshot-" + symbol.ToLowerInvariant(), DoHandleMessage));
            }    

            MessageRouter = MessageRouter.Create(routes.ToArray());
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

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenFuturesBookSnapshotUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _snapshotHandler.Invoke(
                new DataEvent<KrakenFuturesBookSnapshotUpdate>(KrakenExchange.ExchangeName, message, receiveTime, originalData)
                    .WithStreamId(message.Feed)
                    .WithUpdateType(SocketUpdateType.Snapshot)
                    .WithSymbol(message.Symbol)
                    .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset())
                    .WithSequenceNumber(message.Sequence)
                );
            return CallResult.SuccessResult;
        }

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenFuturesBookUpdate message)
        {
            _client.UpdateTimeOffset(message.Timestamp);

            _updateHandler.Invoke(
                new DataEvent<KrakenFuturesBookUpdate>(KrakenExchange.ExchangeName, message, receiveTime, originalData)
                    .WithStreamId(message.Feed)
                    .WithUpdateType(SocketUpdateType.Update)
                    .WithSymbol(message.Symbol)
                    .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset())
                    .WithSequenceNumber(message.Sequence)
                );
            return CallResult.SuccessResult;
        }
    }
}
