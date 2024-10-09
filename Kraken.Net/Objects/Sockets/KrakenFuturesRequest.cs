namespace Kraken.Net.Objects.Sockets
{
    internal class KrakenFuturesRequest
    {
        /// <summary>
        /// The event type
        /// </summary>
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;
        /// <summary>
        /// The feed
        /// </summary>
        [JsonPropertyName("feed")]
        public string Feed { get; set; } = string.Empty;

        /// <summary>
        /// The symbols
        /// </summary>
        [JsonPropertyName("product_ids")]
        public List<string>? Symbols { get; set; }
    }

    internal class KrakenFuturesAuthRequest : KrakenFuturesRequest
    {
        [JsonPropertyName("api_key")]
        public string ApiKey { get; set; } = string.Empty;
        [JsonPropertyName("original_challenge")]
        public string OriginalChallenge { get; set; } = string.Empty;
        [JsonPropertyName("signed_challenge")]
        public string SignedChallenge { get; set; } = string.Empty;
    }
}
