using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Queries
{
    internal class KrakenSpotQuery<T> : Query<T> where T : KrakenQueryEvent
    {
        public override List<string> Identifiers { get; }

        public KrakenSpotQuery(KrakenSocketRequest request, bool authenticated) : base(request, authenticated)
        {
            Identifiers = new List<string>() { request.RequestId.ToString() };
        }

        public override Task<CallResult<T>> HandleMessageAsync(SocketConnection connection, DataEvent<ParsedMessage<T>> message)
        {
            if (message.Data.TypedData!.Status != "error")
                return Task.FromResult(new CallResult<T>(message.Data.TypedData!));
            else
                return Task.FromResult(new CallResult<T>(new ServerError(message.Data.TypedData.ErrorMessage!)));
        }
    }
}
