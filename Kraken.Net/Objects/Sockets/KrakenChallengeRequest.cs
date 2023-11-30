using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Sockets
{
    internal class KrakenChallengeRequest : KrakenEvent
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }
    }

    internal class KrakenChallengeResponse : KrakenEvent
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
