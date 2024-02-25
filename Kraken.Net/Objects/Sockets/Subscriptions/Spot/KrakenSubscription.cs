using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.MessageParsing.Interfaces;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Sockets.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenSubscription<T> : Subscription<KrakenSubscriptionEvent, KrakenSubscriptionEvent>
    {
        private string _topic;
        private int? _interval;
        private bool? _snapshot;
        private IEnumerable<string>? _symbols;
        private readonly Action<DataEvent<T>> _handler;

        public override HashSet<string> ListenerIdentifiers { get; set; }

        private static readonly Dictionary<string, string> _replaceMap = new Dictionary<string, string>
        {
            { "btc", "xbt" },
            { "doge", "xdg" },
        };

        public KrakenSubscription(ILogger logger, string topic, IEnumerable<string>? symbols, int? interval, bool? snapshot, Action<DataEvent<T>> handler) : base(logger, false)
        {
            _topic = topic;
            _symbols = symbols;
            _handler = handler;
            _snapshot = snapshot;
            _interval = interval;

            ListenerIdentifiers = symbols?.Any() == true ? new HashSet<string>(symbols.Select(s => topic.ToLowerInvariant() + "-" + GetSymbolTopic(s))) : new HashSet<string> { topic };
        }

        private string GetSymbolTopic(string symbol)
        {
            var split = symbol.Split('/');
            if (split.Length < 2)
                return symbol;

            var baseAsset = split[0].ToLowerInvariant();
            var quoteAsset = split[1].ToLowerInvariant();

            if (_replaceMap.TryGetValue(baseAsset, out var replacementBase))
                baseAsset = replacementBase;

            if (_replaceMap.TryGetValue(quoteAsset, out var replacementQuote))
                quoteAsset = replacementQuote;

            return $"{baseAsset}/{quoteAsset}";
        }

        public override Type? GetMessageType(IMessageAccessor message) => typeof(KrakenSocketUpdate<T>);

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenSpotQuery<KrakenSubscriptionEvent>(
                new KrakenSubscribeRequest(_topic, null, _interval, _snapshot, null, ExchangeHelpers.NextId(), _symbols?.ToArray())
                {
                    Event = "subscribe",
                },
                Authenticated);
        }

        public override Query? GetUnsubQuery()
        {
            return new KrakenSpotQuery<KrakenSubscriptionEvent>(
                new KrakenSubscribeRequest(_topic, null, _interval, _snapshot, null, ExchangeHelpers.NextId(), _symbols?.ToArray())
                {
                    Event = "unsubscribe"
                },
                Authenticated);
        }

        public override void HandleSubQueryResponse(KrakenSubscriptionEvent message)
        {
            ListenerIdentifiers = _symbols?.Any() == true ? new HashSet<string>(_symbols.Select(s => message.ChannelName + "-" + GetSymbolTopic(s))) : new HashSet<string> { message.ChannelName };
        }

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<object> message)
        {
            var data = (KrakenSocketUpdate<T>)message.Data!;
            _handler.Invoke(message.As(data.Data, data.Symbol, SocketUpdateType.Update));
            return Task.FromResult(new CallResult(null));
        }

    }
}
