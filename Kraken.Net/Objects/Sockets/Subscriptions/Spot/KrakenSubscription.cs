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

        public KrakenSubscription(ILogger logger, string topic, IEnumerable<string>? symbols, int? interval, bool? snapshot, Action<DataEvent<T>> handler) : base(logger, false)
        {
            _topic = topic;
            _symbols = symbols;
            _handler = handler;
            _snapshot = snapshot;
            _interval = interval;

            ListenerIdentifiers = symbols?.Any() == true ? new HashSet<string>(symbols.Select(s => topic.ToLowerInvariant() + "-" + s.ToLowerInvariant())) : new HashSet<string> { topic };
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
            ListenerIdentifiers = _symbols?.Any() == true ? new HashSet<string>(_symbols.Select(s => message.ChannelName + "-" + s.ToLowerInvariant())) : new HashSet<string> { message.ChannelName };
        }

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<object> message)
        {
            var data = (KrakenSocketUpdate<T>)message.Data!;
            _handler.Invoke(message.As(data.Data, data.Symbol, SocketUpdateType.Update));
            return Task.FromResult(new CallResult(null));
        }

    }
}
