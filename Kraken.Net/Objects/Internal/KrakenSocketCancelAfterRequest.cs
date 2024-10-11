namespace Kraken.Net.Objects.Internal
{
    /// <summary>
    /// Cancel orders after request
    /// </summary>
    internal class KrakenSocketCancelAfterRequest : KrakenSocketAuthRequestV2
    {
        /// <summary>
        /// Timeout
        /// </summary>
        [JsonPropertyName("timeout")]
        public int Timeout { get; set; }
    }
}
