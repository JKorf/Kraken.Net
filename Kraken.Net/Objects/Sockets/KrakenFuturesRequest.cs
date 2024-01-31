using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Sockets
{
    internal class KrakenFuturesRequest
    {
        /// <summary>
        /// The event type
        /// </summary>
        [JsonProperty("event")]
        public string Event { get; set; } = string.Empty;
        /// <summary>
        /// The feed
        /// </summary>
        [JsonProperty("feed")]
        public string Feed { get; set; } = string.Empty;

        /// <summary>
        /// The symbols
        /// </summary>
        [JsonProperty("product_ids")]
        public List<string>? Symbols { get; set; }
    }

    internal class KrakenFuturesAuthRequest : KrakenFuturesRequest
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; } = string.Empty;
        [JsonProperty("original_challenge")]
        public string OriginalChallenge { get; set; } = string.Empty;
        [JsonProperty("signed_challenge")]
        public string SignedChallenge { get; set; } = string.Empty;
    }
}
