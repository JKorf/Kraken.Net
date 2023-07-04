using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    public class KrakenFuturesSocketMessage
    {
        [JsonProperty("event")]
        public string Event { get; set; }
        [JsonProperty("feed")]
        public string Feed { get; set; }
        [JsonProperty("error")]
        public string? Error { get; set; }
    }

    public class KrakenFuturesSubscribeMessage : KrakenFuturesSocketMessage
    {
        [JsonProperty("product_ids")]
        public List<string>? Symbols { get; set; }
    }

    public class KrakenFuturesSubscribeAuthMessage : KrakenFuturesSubscribeMessage
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }
        [JsonProperty("original_challenge")]
        public string OriginalChallenge { get; set; }
        [JsonProperty("signed_challenge")]
        public string SignedChallenge { get; set; }
    }

    public class KrakenFuturesUpdateMessage : KrakenFuturesSocketMessage
    {
        [JsonProperty("product_id")]
        public string ProductId { get; set; }
    }
}
