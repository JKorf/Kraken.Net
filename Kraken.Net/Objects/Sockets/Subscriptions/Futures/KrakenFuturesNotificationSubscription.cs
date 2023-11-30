using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;
using Kraken.Net.Objects.Sockets.Subscriptions.Spot;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesNotificationSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesNotificationUpdate>
    {
        protected readonly Action<DataEvent<KrakenFuturesNotificationUpdate>> _handler;

        public override List<string> Identifiers { get; }

        public KrakenFuturesNotificationSubscription(ILogger logger, Action<DataEvent<KrakenFuturesNotificationUpdate>> handler) : base(logger, false)
        {
            _handler = handler; 
            
            Identifiers = new List<string> { "notifications_auth" };
        }

        public override BaseQuery? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesRequest()
                {
                    Event = "subscribe",
                    Feed = "notifications_auth"
                },
                Authenticated);
        }

        public override BaseQuery? GetUnsubQuery()
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesRequest()
                {
                    Event = "unsubscribe",
                    Feed = "notifications_auth",
                },
                Authenticated);
        }

        public override Task<CallResult> HandleEventAsync(SocketConnection connection, DataEvent<ParsedMessage<KrakenFuturesNotificationUpdate>> message)
        {
            _handler.Invoke(message.As(message.Data.TypedData, null, ConnectionInvocations == 1 ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return Task.FromResult(new CallResult(null));
        }
    }
}
