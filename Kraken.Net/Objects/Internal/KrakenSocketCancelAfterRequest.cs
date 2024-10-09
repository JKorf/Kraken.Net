namespace Kraken.Net.Objects.Internal
{
    /// <summary>
    /// Place order request
    /// </summary>
    internal class KrakenSocketCancelAfterRequest : KrakenSocketAuthRequest
    {
        /// <summary>
        /// Timeout
        /// </summary>
        [JsonPropertyName("timeout")]
        public int Timeout { get; set; }
    }
}
