namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Socket update
    /// </summary>
    public record KrakenFuturesSocketMessage
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
        /// Error if any
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }

    internal record KrakenFuturesSubscribeMessage : KrakenFuturesSocketMessage
    {
        [JsonPropertyName("product_ids")]
        public List<string>? Symbols { get; set; }
    }

    internal record KrakenFuturesSubscribeAuthMessage : KrakenFuturesSubscribeMessage
    {
        [JsonPropertyName("api_key")]
        public string ApiKey { get; set; } = string.Empty;
        [JsonPropertyName("original_challenge")]
        public string OriginalChallenge { get; set; } = string.Empty;
        [JsonPropertyName("signed_challenge")]
        public string SignedChallenge { get; set; } = string.Empty;
    }

    /// <summary>
    /// Update message
    /// </summary>
    public record KrakenFuturesUpdateMessage : KrakenFuturesSocketMessage
    {
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonPropertyName("product_id")]
        public string Symbol { get; set; } = string.Empty;
    }
}
