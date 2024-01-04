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
    internal class KrakenFuturesAuthQuery : Query<KrakenChallengeResponse>
    {
        public override List<string> StreamIdentifiers { get; set; }

        public KrakenFuturesAuthQuery(string apiKey) : base(new KrakenChallengeRequest { ApiKey = apiKey, Event = "challenge" }, false)
        {
            StreamIdentifiers = new List<string>() { "challenge" };
        }
    }
}
