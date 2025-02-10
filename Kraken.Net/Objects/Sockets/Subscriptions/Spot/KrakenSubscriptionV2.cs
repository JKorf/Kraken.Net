using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal abstract class KrakenSubscription : Subscription<KrakenSocketResponseV2<KrakenSocketSubResponse>, KrakenSocketResponseV2<KrakenSocketSubResponse>>
    {
        public string? Token { get; set; }
        public bool TokenRequired { get; set; }

        protected KrakenSubscription(ILogger logger, bool auth) : base(logger, false)
        {
            TokenRequired = auth;
        }
    }

    internal class KrakenSubscriptionV2<T> : KrakenSubscription
    {
        private string _topic;
        private int? _interval;
        private bool? _snapshot;
        private int? _depth;
        private IEnumerable<string>? _symbols;
        private readonly Action<DataEvent<T>> _handler;

        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenSubscriptionV2(ILogger logger, string topic, IEnumerable<string>? symbols, int? interval, bool? snapshot, int? depth, string? token, Action<DataEvent<T>> handler) : base(logger, token != null)
        {
            _topic = topic;
            _symbols = symbols;
            _handler = handler;
            _snapshot = snapshot;
            _depth = depth;
            _interval = interval;
            Token = token;

            ListenerIdentifiers = symbols?.Any() == true ? new HashSet<string>(symbols.Select(s => topic + "-" + s)) : new HashSet<string> { topic };
        }

        public override Type? GetMessageType(IMessageAccessor message) => typeof(KrakenSocketUpdateV2<T>);

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenSpotQueryV2<KrakenSocketSubResponse, KrakenSocketSubRequest>(
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
                        Token = Token
                    }
                }, Authenticated)
                {
                    RequiredResponses = _symbols?.Count() ?? 1
                };
        }

        public override Query? GetUnsubQuery()
        {
            return new KrakenSpotQueryV2<KrakenSocketSubResponse, KrakenSocketSubRequest>(
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
                        Token = Token
                    }
                }, Authenticated)
            {
                RequiredResponses = _symbols?.Count() ?? 1
            };
        }

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            var data = (KrakenSocketUpdateV2<T>)message.Data!;
            _handler.Invoke(message.As(data.Data, data.Channel, null, data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update).WithDataTimestamp(data.Timestamp));
            return new CallResult(null);
        }

    }
}
