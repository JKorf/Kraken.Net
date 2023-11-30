using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Queries
{
    internal class KrakenFuturesQuery<T> : Query<T> where T: KrakenFuturesResponse
    {
        public override List<string> Identifiers { get; }

        public KrakenFuturesQuery(KrakenFuturesRequest request, bool authenticated) : base(request, authenticated)
        {
            string evnt = request.Event;
            if (request.Event == "subscribe" || request.Event == "unsubscribe")
                evnt += "d";

            Identifiers = request.Symbols?.Any() == true ? request.Symbols.Select(s => evnt + "-" + request.Feed.ToLowerInvariant() + "-" + s.ToLowerInvariant()).ToList() : new List<string> { evnt + "-" + request.Feed.ToLowerInvariant() };
        }

        //public override Task<CallResult<T>> HandleMessageAsync(DataEvent<ParsedMessage<T>> message)
        //{
        //    if (message.Data.Data!.Status != "error")
        //        return Task.FromResult(new CallResult<T>(message.Data.Data!));
        //    else
        //        return Task.FromResult(new CallResult<T>(new ServerError(message.Data.Data.ErrorMessage!)));
        //}
    }
}
