using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Converters;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Sockets.Queries;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenBookSubscription : Subscription<KrakenSubscriptionEvent, KrakenSubscriptionEvent>
    {
        private string _topic;
        private int? _depth;
        private IEnumerable<string>? _symbols;
        private readonly Action<DataEvent<KrakenStreamOrderBook>> _handler;

        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenBookSubscription(ILogger logger, IEnumerable<string>? symbols, int? depth, Action<DataEvent<KrakenStreamOrderBook>> handler) : base(logger, false)
        {
            _topic = "book";
            _symbols = symbols;
            _handler = handler;
            _depth = depth;

            ListenerIdentifiers = symbols?.Any() == true ? new HashSet<string>(symbols.Select(s => _topic.ToLowerInvariant() + "-" + s.ToLowerInvariant())) : new HashSet<string> { _topic };
        }
        public override Type? GetMessageType(IMessageAccessor message) => typeof(KrakenSocketUpdate<KrakenStreamOrderBook>);

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenSpotQuery<KrakenSubscriptionEvent>(
                new KrakenSubscribeRequest(_topic, null, null, null, _depth, ExchangeHelpers.NextId(), _symbols?.ToArray())
                {
                    Event = "subscribe",
                },
                Authenticated);
        }

        public override Query? GetUnsubQuery()
        {
            return new KrakenSpotQuery<KrakenSubscriptionEvent>(
                new KrakenSubscribeRequest(_topic, null, null, null, _depth, ExchangeHelpers.NextId(), _symbols?.ToArray())
                {
                    Event = "unsubscribe"
                },
                Authenticated);
        }

        public override CallResult<object> Deserialize(IMessageAccessor message, Type type)
        {
            return new CallResult<object>(StreamOrderBookConverter.Convert((JArray)message.Underlying!)!);
        }

        public override void HandleSubQueryResponse(KrakenSubscriptionEvent message)
        {
            ListenerIdentifiers = _symbols?.Any() == true ? new HashSet<string>(_symbols.Select(s => message.ChannelName + "-" + s.ToLowerInvariant())) : new HashSet<string> { message.ChannelName };
        }

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<object> message)
        {
            var data = (KrakenSocketUpdate<KrakenStreamOrderBook>)message.Data!;
            _handler.Invoke(message.As(data.Data, data.Symbol, data.Data.Snapshot ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return Task.FromResult(new CallResult(null));
        }

    }
}
