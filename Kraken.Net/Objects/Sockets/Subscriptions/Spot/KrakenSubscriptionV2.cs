using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenSubscriptionV2<T> : KrakenSubscription
    {
        private readonly SocketApiClient _client;
        private string _topic;
        private string? _eventTrigger;
        private KlineInterval? _interval;
        private bool? _snapshot;
        private int? _depth;
        private string[]? _symbols;
        private readonly Action<DateTime, string?, KrakenSocketUpdateV2<T>> _handler;

        public KrakenSubscriptionV2(
            ILogger logger,
            SocketApiClient client,
            string topic,
            string[]? symbols,
            KlineInterval? interval,
            bool? snapshot,
            int? depth,
            TriggerEvent? eventTrigger,
            string? token,
            Action<DateTime, string?, KrakenSocketUpdateV2<T>> handler) : base(logger, token != null)
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

            IndividualSubscriptionCount = symbols?.Length ?? 1;

            MessageRouter = MessageRouter.CreateWithOptionalTopicFilters<KrakenSocketUpdateV2<T>>(topic, symbols?.Select(x => x + interval).ToArray(), DoHandleMessage);
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
                        Interval = _interval == null ? null : int.Parse(EnumConverter.GetString(_interval)),
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
                        Interval = _interval == null ? null : int.Parse(EnumConverter.GetString(_interval)),
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

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenSocketUpdateV2<T> message)
        {
            _handler?.Invoke(receiveTime, originalData, message);
            return CallResult.SuccessResult;
        }

    }
}
