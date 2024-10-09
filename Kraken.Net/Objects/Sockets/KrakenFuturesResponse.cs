namespace Kraken.Net.Objects.Sockets
{
    internal class KrakenFuturesResponse
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
        /// Message
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// The symbols
        /// </summary>
        [JsonPropertyName("product_ids")]
        public List<string>? Symbols { get; set; }
    }
}
