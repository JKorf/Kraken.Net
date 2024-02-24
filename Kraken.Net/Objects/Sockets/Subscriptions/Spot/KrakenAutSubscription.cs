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
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal abstract class KrakenAuthSubscription : Subscription<KrakenSubscriptionEvent, KrakenSubscriptionEvent>
    {
        protected string _token;

        protected KrakenAuthSubscription(string token, ILogger logger, bool authenticated) : base(logger, authenticated)
        {
            _token = token;
        }

        internal void UpdateToken(string token)
        {
            _token = token;
        }
    }

    internal class KrakenAuthSubscription<T> : KrakenAuthSubscription
    {
        private string _topic;
        private readonly Action<DataEvent<KrakenAuthSocketUpdate<T>>> _handler;

        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenAuthSubscription(ILogger logger, string topic, string token, Action<DataEvent<KrakenAuthSocketUpdate<T>>> handler) : base(token, logger, false)
        {
            _topic = topic;
            _handler = handler;

            ListenerIdentifiers = new HashSet<string> { topic };
        }
        public override Type? GetMessageType(IMessageAccessor message) => typeof(KrakenAuthSocketUpdate<T>);

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenSpotQuery<KrakenSubscriptionEvent>(
                new KrakenSubscribeRequest(_topic, _token, null, null, null, ExchangeHelpers.NextId())
                {
                    Event = "subscribe",
                },
                Authenticated);
        }

        public override Query? GetUnsubQuery()
        {
            return new KrakenSpotQuery<KrakenSubscriptionEvent>(
                new KrakenSubscribeRequest(_topic, _token, null, null, null, ExchangeHelpers.NextId())
                {
                    Event = "unsubscribe"
                },
                Authenticated);
        }

        public override void HandleSubQueryResponse(KrakenSubscriptionEvent message)
        {
            ListenerIdentifiers = new HashSet<string> { message.ChannelName };
        }

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<object> message)
        {
            var data = (KrakenAuthSocketUpdate<T>)message.Data!;
            _handler.Invoke(message.As(data, data.ChannelName, data.Sequence.Sequence == 1 ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return Task.FromResult(new CallResult(null));
        }

    }
}
