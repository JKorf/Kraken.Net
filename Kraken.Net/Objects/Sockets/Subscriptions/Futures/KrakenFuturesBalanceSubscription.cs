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
    internal class KrakenFuturesBalanceSubscription : Subscription<KrakenFuturesResponse>
    {
        protected readonly Action<DataEvent<KrakenFuturesBalancesUpdate>> _handler;
        public override List<string> StreamIdentifiers { get; set; }
        public override Dictionary<string, Type> TypeMapping { get; } = new Dictionary<string, Type>
        {
            { "", typeof(KrakenFuturesBalancesUpdate) }
        };


        public KrakenFuturesBalanceSubscription(ILogger logger, Action<DataEvent<KrakenFuturesBalancesUpdate>> handler) : base(logger, true)
        {
            _handler = handler;
            StreamIdentifiers = new List<string> { "balances_snapshot", "balances" };
        }

        public override BaseQuery? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesAuthRequest()
                {
                    Event = "subscribe",
                    Feed = "balances",
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
                    Feed = "balances",
                },
                Authenticated);
        }

        public override Task<CallResult> DoHandleMessageAsync(SocketConnection connection, DataEvent<BaseParsedMessage> message)
        {
            _handler.Invoke(message.As((KrakenFuturesBalancesUpdate)message.Data.Data, null, ConnectionInvocations == 1 ? SocketUpdateType.Snapshot : SocketUpdateType.Update));
            return Task.FromResult(new CallResult(null));
        }
    }
}
