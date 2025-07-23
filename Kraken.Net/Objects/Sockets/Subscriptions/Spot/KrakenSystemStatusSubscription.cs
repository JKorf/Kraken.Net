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

        public override Query? GetSubQuery(SocketConnection connection) => null;

        public override Query? GetUnsubQuery() => null;

        public CallResult DoHandleMessage(SocketConnection connection, DataEvent<KrakenSocketUpdateV2<KrakenStreamSystemStatus[]>> message)
        {
            _handler.Invoke(message.As(message.Data.Data.First(), message.Data.Channel, null, SocketUpdateType.Update).WithDataTimestamp(message.Data.Timestamp));
            return CallResult.SuccessResult;
        }
    }
}
