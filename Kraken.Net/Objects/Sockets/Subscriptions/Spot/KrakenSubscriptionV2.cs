using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Sockets.Queries;
using System.Linq;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenSubscriptionV2<T> : KrakenSubscription
    {
        private string _topic;
        private int? _interval;
        private bool? _snapshot;
        private int? _depth;
        private IEnumerable<string>? _symbols;
        private readonly Action<DataEvent<T>> _handler;

        public KrakenSubscriptionV2(ILogger logger, string topic, IEnumerable<string>? symbols, int? interval, bool? snapshot, int? depth, string? token, Action<DataEvent<T>> handler) : base(logger, token != null)
        {
            _topic = topic;
            _symbols = symbols;
            _handler = handler;
            _snapshot = snapshot;
            _depth = depth;
            _interval = interval;
            Token = token;

            if (symbols?.Any() == true)
                MessageMatcher = MessageMatcher.Create(symbols.Select(x => new MessageHandlerLink<KrakenSocketUpdateV2<T>>(topic + "-" + x, DoHandleMessage)).ToArray());
            else
                MessageMatcher = MessageMatcher.Create<KrakenSocketUpdateV2<T>>(topic, DoHandleMessage);
        }

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

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenSocketUpdateV2<T>> message)
        {
            _handler.Invoke(message.As(message.Data.Data, message.Data.Channel, null, message.Data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update).WithDataTimestamp(message.Data.Timestamp));
            return CallResult.SuccessResult;
        }

    }
}
