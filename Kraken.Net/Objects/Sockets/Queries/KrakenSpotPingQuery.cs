using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Objects.Sockets.Queries
{
    internal class KrakenSpotPingQuery : Query<KrakenSocketResponseV2<object>>
    {
        public KrakenSpotPingQuery() : base(new KrakenSocketRequestV2 { Method = "ping", RequestId = ExchangeHelpers.NextId() }, false)
        {
            RequestTimeout = TimeSpan.FromSeconds(5);
            MessageMatcher = MessageMatcher.Create<object>(((KrakenSocketRequestV2)Request).RequestId.ToString());
            MessageRouter = MessageRouter.Create<object>(((KrakenSocketRequestV2)Request).RequestId.ToString());
        }
    }
}
