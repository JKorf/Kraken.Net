using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;
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
            else if (message.Data is KrakenSocketResponseV2<IEnumerable<KrakenOrderResult>> response // We'll want to return the actual response data, so return as no error and handle it in the method itself
                || message.Data.Error == "Already subscribed") // Duplicate subscription shouldn't be treated as an error
            {
                
                return message.ToCallResult();
            }
            else
                return new CallResult<KrakenSocketResponseV2<TResponse>>(new ServerError(message.Data.Error!));
        }
    }
}
