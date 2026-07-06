using Kraken.Net.Enums;

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
        /// ["<c>method_id</c>"] Method id
        /// </summary>
        [JsonPropertyName("method_id")]
        public string MethodId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>network</c>"] Name of the Network
        /// </summary>
        [JsonPropertyName("network")]
        public string Network { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>network_id</c>"] Network id
        /// </summary>
        [JsonPropertyName("network_id")]
        public string NetworkId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>minimum</c>"] Minimum amount
        /// </summary>
        [JsonPropertyName("minimum")]
        public decimal Minimum { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee info
        /// </summary>
        [JsonPropertyName("fee")]
        public KrakenWithdrawMethodFee Fee { get; set; } = default!;
    }

    /// <summary>
    /// Fee
    /// </summary>
    public record KrakenWithdrawMethodFee
    {
        /// <summary>
        /// ["<c>aclass</c>"] Asset class
        /// </summary>
        [JsonPropertyName("aclass")]
        public AClass AssetClass { get; set; }
        /// <summary>
        /// ["<c>asset</c>"] Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>fee</c>"] Flat fee amount
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee percentage
        /// </summary>
        [JsonPropertyName("fee_percentage")]
        public decimal? FeePercentage { get; set; }
    }
}
