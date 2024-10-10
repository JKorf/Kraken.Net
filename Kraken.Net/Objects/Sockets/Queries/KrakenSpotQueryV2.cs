using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Queries
{
    internal class KrakenSpotQueryV2<TResponse, TRequest> : Query<KrakenSocketResponseV2<TResponse>>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenSpotQueryV2(KrakenSocketRequestV2<TRequest> request, bool authenticated) : base(request, authenticated)
        {
            ListenerIdentifiers = new HashSet<string>() { request.RequestId.ToString() };
        }

        public override CallResult<KrakenSocketResponseV2<TResponse>> HandleMessage(SocketConnection connection, DataEvent<KrakenSocketResponseV2<TResponse>> message)
        {
            if (message.Data.Success)
                return message.ToCallResult();
            else
                return new CallResult<KrakenSocketResponseV2<TResponse>>(new ServerError(message.Data.Error!));
        }
    }
}
