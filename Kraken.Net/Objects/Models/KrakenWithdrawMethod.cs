namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Info about a withdraw method
    /// </summary>
    public record KrakenWithdrawMethod
    {
        /// <summary>
        /// Name of the asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Name of the method
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;
        /// <summary>
        /// Name of the Network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// Minimum amount
        /// </summary>
        [JsonPropertyName("minimum")]
        public decimal Minimum { get; set; }
    }
}
