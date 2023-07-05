using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Socket update
    /// </summary>
    public class KrakenFuturesSocketMessage
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
        /// Error if any
        /// </summary>
        [JsonProperty("error")]
        public string? Error { get; set; }
    }

    internal class KrakenFuturesSubscribeMessage : KrakenFuturesSocketMessage
    {
        [JsonProperty("product_ids")]
        public List<string>? Symbols { get; set; }
    }

    internal class KrakenFuturesSubscribeAuthMessage : KrakenFuturesSubscribeMessage
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; } = string.Empty;
        [JsonProperty("original_challenge")]
        public string OriginalChallenge { get; set; } = string.Empty;
        [JsonProperty("signed_challenge")]
        public string SignedChallenge { get; set; } = string.Empty;
    }

    /// <summary>
    /// Update message
    /// </summary>
    public class KrakenFuturesUpdateMessage : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonProperty("product_id")]
        public string Symbol { get; set; } = string.Empty;
    }
}
