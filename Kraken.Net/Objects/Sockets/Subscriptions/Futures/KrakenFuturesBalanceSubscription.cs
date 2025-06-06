﻿using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesBalanceSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesResponse>
    {
        protected readonly Action<DataEvent<KrakenFuturesBalancesUpdate>> _handler;
        public override HashSet<string> ListenerIdentifiers { get; set; }


        public KrakenFuturesBalanceSubscription(ILogger logger, Action<DataEvent<KrakenFuturesBalancesUpdate>> handler) : base(logger, true)
        {
            _handler = handler;
            ListenerIdentifiers = new HashSet<string> { "balances_snapshot", "balances" };
        }

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesAuthRequest()
                {
                    Event = "subscribe",
                    Feed = "balances",
                    OriginalChallenge = (string)connection.Properties["OriginalChallenge"],
                    SignedChallenge = (string)connection.Properties["SignedChallenge"],
                    ApiKey = ((KrakenFuturesAuthenticationProvider)connection.ApiClient.AuthenticationProvider!).GetApiKey(),
                },
                Authenticated);
        }

        public override Query? GetUnsubQuery()
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesAuthRequest()
                {
                    Event = "unsubscribe",
                    Feed = "balances",
                },
                Authenticated);
        }

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            var data = (KrakenFuturesBalancesUpdate)message.Data;
            _handler.Invoke(message.As(data, data.Feed, null, string.Equals(data.Feed, "balances_snapshot", StringComparison.Ordinal) ? SocketUpdateType.Snapshot : SocketUpdateType.Update)
                .WithDataTimestamp(data.Timestamp));
            return CallResult.SuccessResult;
        }

        public override Type? GetMessageType(IMessageAccessor message) => typeof(KrakenFuturesBalancesUpdate);
    }
}
