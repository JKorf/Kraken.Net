﻿using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Models.Socket.Futures;
using Kraken.Net.Objects.Sockets.Queries;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Futures
{
    internal class KrakenFuturesNotificationSubscription : Subscription<KrakenFuturesResponse, KrakenFuturesNotificationUpdate>
    {
        protected readonly Action<DataEvent<KrakenFuturesNotificationUpdate>> _handler;

        public KrakenFuturesNotificationSubscription(ILogger logger, Action<DataEvent<KrakenFuturesNotificationUpdate>> handler) : base(logger, true)
        {
            _handler = handler;

            MessageMatcher = MessageMatcher.Create<KrakenFuturesNotificationUpdate>("notifications_auth", DoHandleMessage);
        }

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return new KrakenFuturesQuery<KrakenFuturesResponse>(
                new KrakenFuturesAuthRequest()
                {
                    Event = "subscribe",
                    Feed = "notifications_auth",
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
                    Feed = "notifications_auth"
                },
                Authenticated);
        }

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenFuturesNotificationUpdate> message)
        {
            _handler.Invoke(message.As(message.Data, message.Data.Feed, null, SocketUpdateType.Update));
            return CallResult.SuccessResult;
        }
    }
}
