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
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenSubscription<T> : Subscription<KrakenSocketResponseV2<KrakenSocketSubResponse>, KrakenSocketResponseV2<KrakenSocketSubResponse>>
    {
        private string _topic;
        private int? _interval;
        private bool? _snapshot;
        private string? _token;
        private IEnumerable<string>? _symbols;
        private readonly Action<DataEvent<T>> _handler;

        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenSubscription(ILogger logger, string topic, IEnumerable<string>? symbols, int? interval, bool? snapshot, string? token, Action<DataEvent<T>> handler) : base(logger, false)
        {
            _topic = topic;
            _symbols = symbols;
            _handler = handler;
            _snapshot = snapshot;
            _interval = interval;
            _token = token;

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
                        Snapshot = _snapshot,
                        Token = _token
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
                        Interval = _interval
                    }
                }, Authenticated)
            {
                RequiredResponses = _symbols?.Count() ?? 1
            };
        }

        public override void HandleSubQueryResponse(KrakenSocketResponseV2<KrakenSocketSubResponse> message)
        {
            ListenerIdentifiers = _symbols?.Any() == true ? new HashSet<string>(_symbols.Select(s => message.Result.Channel + "-" + s)) : new HashSet<string> { message.Result.Channel };
        }

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            var data = (KrakenSocketUpdateV2<T>)message.Data!;
            _handler.Invoke(message.As(data.Data, data.Channel, null, data.Type == "snapshot" ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return new CallResult(null);
        }

    }
}
