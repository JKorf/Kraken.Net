namespace Kraken.Net.Objects.Internal
{
    /// <summary>
    /// Socket request base
    /// </summary>
    internal class KrakenSocketRequest
    {
        /// <summary>
        /// Request id
        /// </summary>
        [JsonPropertyName("req_id")]
        public int RequestId { get; set; }
        /// <summary>
        /// Event
        /// </summary>
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;
    }

    internal class KrakenSocketAuthRequest : KrakenSocketRequest
    {
        /// <summary>
        /// Token
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}
