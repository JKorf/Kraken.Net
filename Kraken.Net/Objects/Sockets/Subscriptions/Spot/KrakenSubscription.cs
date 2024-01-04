﻿using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Sockets.Queries;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenSubscription<T> : Subscription<KrakenSubscriptionEvent>
    {
        private KrakenSubscribeRequest _request;
        private readonly Action<DataEvent<T>> _handler;

        public override List<string> StreamIdentifiers { get; set; }
        public override Dictionary<string, Type> TypeMapping { get; } = new Dictionary<string, Type>
        {
            { "", typeof(KrakenSocketUpdate<T>) }
        };
        
        public KrakenSubscription(ILogger logger, KrakenSubscribeRequest subRequest, Action<DataEvent<T>> handler) : base(logger, false)
        {
            _request = subRequest;
            _handler = handler;

            StreamIdentifiers = subRequest.Symbols?.Any() == true ? subRequest.Symbols.Select(s => subRequest.Details.ChannelName.ToLowerInvariant() + "-" + s.ToLowerInvariant()).ToList() : new List<string> { subRequest.Details.ChannelName };
        }

        public override BaseQuery? GetSubQuery(SocketConnection connection)
        {
            return new KrakenSpotQuery<KrakenSubscriptionEvent>(
                new KrakenSubscribeRequest(_request.Details.Topic, ExchangeHelpers.NextId(), _request.Symbols)
                {
                    Details = _request.Details,
                    Event = "subscribe"
                },
                Authenticated);
        }

        public override BaseQuery? GetUnsubQuery()
        {
            return new KrakenSpotQuery<KrakenSubscriptionEvent>(
                new KrakenSubscribeRequest(_request.Details.Topic, ExchangeHelpers.NextId(), _request.Symbols)
                {
                    Details = _request.Details,
                    Event = "unsubscribe"
                }, Authenticated);
        }

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<BaseParsedMessage> message)
        {
            var data = (KrakenSocketUpdate<T>)message.Data.Data!;
            _handler.Invoke(message.As(data.Data, data.Symbol, SocketUpdateType.Update));
            return Task.FromResult(new CallResult(null));
        }
    }
}
