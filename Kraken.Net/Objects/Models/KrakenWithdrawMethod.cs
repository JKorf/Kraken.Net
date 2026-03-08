namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Info about a withdraw method
    /// </summary>
    [SerializationModel]
    public record KrakenWithdrawMethod
    {
        /// <summary>
        /// ["<c>asset</c>"] Name of the asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>method</c>"] Name of the method
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>network</c>"] Name of the Network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>minimum</c>"] Minimum amount
        /// </summary>
        [JsonPropertyName("minimum")]
        public decimal Minimum { get; set; }
    }
}
