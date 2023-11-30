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
    internal class KrakenFuturesUserTradeSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesUserTradesUpdate>
    {
        protected readonly Action<DataEvent<KrakenFuturesUserTradesUpdate>> _handler;
        public override List<string> Identifiers { get; }

        public KrakenFuturesUserTradeSubscription(ILogger logger, Action<DataEvent<KrakenFuturesUserTradesUpdate>> handler) : base(logger, true)
        {
            _handler = handler;
            Identifiers = new List<string> { "fills_snapshot", "fills" };
        }

        public override BaseQuery? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesAuthRequest()
                {
                    Event = "subscribe",
                    Feed = "fills",
                    OriginalChallenge = (string)connection.Properties["OriginalChallenge"],
                    SignedChallenge = (string)connection.Properties["SignedChallenge"],
                    ApiKey = ((KrakenFuturesAuthenticationProvider)connection.ApiClient.AuthenticationProvider).GetApiKey(),
                },
                Authenticated);
        }

        public override BaseQuery? GetUnsubQuery()
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesAuthRequest()
                {
                    Event = "unsubscribe",
                    Feed = "fills",
                },
                Authenticated);
        }

        public override Task<CallResult> HandleEventAsync(SocketConnection connection, DataEvent<ParsedMessage<KrakenFuturesUserTradesUpdate>> message)
        {
            _handler.Invoke(message.As(message.Data.TypedData, null, ConnectionInvocations == 1 ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return Task.FromResult(new CallResult(null));
        }
    }
}
