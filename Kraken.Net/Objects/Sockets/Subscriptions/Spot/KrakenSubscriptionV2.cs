using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Sockets.Queries;
using System.Linq;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenSubscriptionV2<T> : KrakenSubscription
    {
        private readonly SocketApiClient _client;
        private string _topic;
        private string? _eventTrigger;
        private int? _interval;
        private bool? _snapshot;
        private int? _depth;
        private IEnumerable<string>? _symbols;
        private readonly Action<DataEvent<T>> _handler;

        public KrakenSubscriptionV2(
            ILogger logger,
            SocketApiClient client,
            string topic,
            IEnumerable<string>? symbols, 
            int? interval,
            bool? snapshot,
            int? depth,
            TriggerEvent? eventTrigger,
            string? token,
            Action<DataEvent<T>> handler) : base(logger, token != null)
        {
            _client = client;
            _topic = topic;
            _symbols = symbols;
            _handler = handler;
            _snapshot = snapshot;
            _depth = depth;
            _interval = interval;
            _eventTrigger = eventTrigger == TriggerEvent.BestOfferChange ? "bbo" : eventTrigger == TriggerEvent.Trade ? "trades" : null;
            Token = token;

            if (symbols?.Any() == true)
                MessageMatcher = MessageMatcher.Create(symbols.Select(x => new MessageHandlerLink<KrakenSocketUpdateV2<T>>(topic + "-" + x, DoHandleMessage)).ToArray());
            else
                MessageMatcher = MessageMatcher.Create<KrakenSocketUpdateV2<T>>(topic, DoHandleMessage);
        }

        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenSpotQueryV2<KrakenSocketSubResponse, KrakenSocketSubRequest>(
                _client,
                new KrakenSocketRequestV2<KrakenSocketSubRequest>()
                {
                    Method = "subscribe",
                    RequestId = ExchangeHelpers.NextId(),
                    Parameters = new KrakenSocketSubRequest
                    {
                        Channel = _topic,
                        Symbol = _symbols?.ToArray(),
                        Interval = _interval,
                        Depth = _depth,
                        Snapshot = _snapshot,
                        EventTrigger = _eventTrigger,
                        Token = Token
                    }
                }, Authenticated)
                {
                    RequiredResponses = _symbols?.Count() ?? 1
                };
        }

        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new KrakenSpotQueryV2<KrakenSocketSubResponse, KrakenSocketSubRequest>(
                _client,
                new KrakenSocketRequestV2<KrakenSocketSubRequest>()
                {
                    Method = "unsubscribe",
                    RequestId = ExchangeHelpers.NextId(),
                    Parameters = new KrakenSocketSubRequest
                    {
                        Channel = _topic,
                        Symbol = _symbols?.ToArray(),
                        Interval = _interval,
                        Depth = _depth,
                        Snapshot = _snapshot,
                        EventTrigger = _eventTrigger,
                        Token = Token
                    }
                }, Authenticated)
            {
                RequiredResponses = _symbols?.Count() ?? 1
            };
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenSocketUpdateV2<T>> message)
        {
            _handler.Invoke(message.As(message.Data.Data, message.Data.Channel, null, message.Data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update).WithDataTimestamp(message.Data.Timestamp));
            return CallResult.SuccessResult;
        }

    }
}
