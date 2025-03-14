using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Info about a withdraw address
    /// </summary>
    [SerializationModel]
    public record KrakenWithdrawAddress
    {
        /// <summary>
        /// The actual address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
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
        /// Key
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;
        /// <summary>
        /// Verified indicator
        /// </summary>
        [JsonPropertyName("verified")]
        public bool Verified { get; set; }
    }
}
