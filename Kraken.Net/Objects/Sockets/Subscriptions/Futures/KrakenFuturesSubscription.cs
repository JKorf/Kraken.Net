using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Sockets.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenFuturesSubscription<T> : Subscription<KrakenFuturesResponse, T> where T : KrakenFuturesEvent
    {
        private string _feed;
        private List<string>? _symbols;
        protected readonly Action<DataEvent<T>> _handler;

        public override List<string> Identifiers { get; }

        public KrakenFuturesSubscription(ILogger logger, string feed, List<string>? symbols, Action<DataEvent<T>> handler) : base(logger, false)
        {
            _feed = feed;
            _symbols = symbols;
            _handler = handler;

            Identifiers = symbols?.Any() == true ? symbols.Select(s => _feed + "-" + s.ToLowerInvariant()).ToList() : new List<string> { _feed };
        }

        public override BaseQuery? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesRequest()
                {
                    Event = "subscribe",
                    Feed = _feed,
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
                    Feed = _feed,
                    Symbols = _symbols
                },
                Authenticated);
        }

        public override Task<CallResult> HandleEventAsync(SocketConnection connection, DataEvent<ParsedMessage<T>> message)
        {
            _handler.Invoke(message.As(message.Data.TypedData!, message.Data.TypedData!.Symbol, SocketUpdateType.Update));
            return Task.FromResult(new CallResult(null));
        }
    }
}
