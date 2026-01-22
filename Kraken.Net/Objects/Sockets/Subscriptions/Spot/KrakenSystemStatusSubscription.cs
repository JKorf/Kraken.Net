using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Kraken.Net.Clients.SpotApi;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Objects.Sockets.Subscriptions.Spot
{
    internal class KrakenSystemStatusSubscription : Subscription
    {
        private readonly KrakenSocketClientSpotApi _client;
        private readonly Action<DataEvent<KrakenStreamSystemStatus>> _handler;

        public KrakenSystemStatusSubscription(ILogger logger, KrakenSocketClientSpotApi client, Action<DataEvent<KrakenStreamSystemStatus>> handler) : base(logger, false)
        {
            _client = client;
            _handler = handler;

            MessageRouter = MessageRouter.CreateWithoutTopicFilter<KrakenSocketUpdateV2<KrakenStreamSystemStatus[]>>("status", DoHandleMessage);
        }

        protected override Query? GetSubQuery(SocketConnection connection) => null;

        protected override Query? GetUnsubQuery(SocketConnection connection) => null;

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenSocketUpdateV2<KrakenStreamSystemStatus[]> message)
        {
            if (message.Timestamp != null)
                _client.UpdateTimeOffset(message.Timestamp.Value);

            _handler?.Invoke(
                new DataEvent<KrakenStreamSystemStatus>(KrakenExchange.ExchangeName, message.Data.First(), receiveTime, originalData)
                    .WithStreamId(message.Channel)
                    .WithUpdateType(SocketUpdateType.Update)
                    .WithDataTimestamp(message.Timestamp, _client.GetTimeOffset())
                );
            return CallResult.SuccessResult;
        }
    }
}
