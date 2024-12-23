using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Objects.Sockets.Queries
{
    internal class KrakenSpotPingQuery : Query<KrakenSocketResponseV2<object>>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenSpotPingQuery() : base(new KrakenSocketRequestV2 { Method = "ping", RequestId = ExchangeHelpers.NextId() }, false)
        {
            RequestTimeout = TimeSpan.FromSeconds(5);
            ListenerIdentifiers = new HashSet<string>() { ((KrakenSocketRequestV2)Request).RequestId.ToString() };
        }
    }
}
