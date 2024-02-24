using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.MessageParsing.Interfaces;
using Kraken.Net.Objects.Sockets.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenFuturesSubscription<T> : Subscription<KrakenFuturesResponse, KrakenFuturesResponse> where T : KrakenFuturesEvent
    {
        private string _feed;
        private List<string>? _symbols;
        protected readonly Action<DataEvent<T>> _handler;

        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenFuturesSubscription(ILogger logger, string feed, List<string>? symbols, Action<DataEvent<T>> handler) : base(logger, false)
        {
            _feed = feed;
            _symbols = symbols;
            _handler = handler;

            ListenerIdentifiers = symbols?.Any() == true ? new HashSet<string>(symbols.Select(s => _feed + "-" + s.ToLowerInvariant())) : new HashSet<string> { _feed };
        }

        public override Type? GetMessageType(IMessageAccessor message) => typeof(T);

        public override Query? GetSubQuery(SocketConnection connection)
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

        public override Query? GetUnsubQuery()
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

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<object> message)
        {
            var data = (T)message.Data!;
            _handler.Invoke(message.As(data, data!.Symbol, SocketUpdateType.Update));
            return Task.FromResult(new CallResult(null));
        }
    }
}
