namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Info about a withdraw address
    /// </summary>
    [SerializationModel]
    public record KrakenWithdrawAddress
    {
        /// <summary>
        /// ["<c>address</c>"] The actual address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
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
        /// ["<c>key</c>"] Key
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>verified</c>"] Verified indicator
        /// </summary>
        [JsonPropertyName("verified")]
        public bool Verified { get; set; }
    }
}
