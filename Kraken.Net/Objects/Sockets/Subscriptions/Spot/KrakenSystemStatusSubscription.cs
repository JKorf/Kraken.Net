using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenSystemStatusSubscription : Subscription<Query, Query>
    {
        private readonly Action<DataEvent<KrakenStreamSystemStatus>> _handler;

        public KrakenSystemStatusSubscription(ILogger logger, Action<DataEvent<KrakenStreamSystemStatus>> handler) : base(logger, false)
        {
            _handler = handler;

            MessageMatcher = MessageMatcher.Create<KrakenSocketUpdateV2<KrakenStreamSystemStatus[]>>("status", DoHandleMessage);
        }

        protected override Query? GetSubQuery(SocketConnection connection) => null;

        protected override Query? GetUnsubQuery(SocketConnection connection) => null;

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenSocketUpdateV2<KrakenStreamSystemStatus[]> message)
        {
            _handler?.Invoke(
                new DataEvent<KrakenStreamSystemStatus>(message.Data.First(), receiveTime, originalData)
                    .WithStreamId(message.Channel)
                    .WithUpdateType(SocketUpdateType.Update)
                    .WithDataTimestamp(message.Timestamp)
                );
            return CallResult.SuccessResult;
        }
    }
}
