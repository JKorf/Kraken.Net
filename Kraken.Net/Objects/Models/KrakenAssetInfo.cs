using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Info on an asset
    /// </summary>
    [SerializationModel]
    public record KrakenAssetInfo
    {
        /// <summary>
        /// ["<c>altname</c>"] Alternative name
        /// </summary>
        [JsonPropertyName("altname")]
        public string AlternateName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>aclass</c>"] Class of the asset
        /// </summary>
        [JsonPropertyName("aclass")]
        public string AssetClass { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>decimals</c>"] Decimal precision of the asset
        /// </summary>
        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }
        /// <summary>
        /// ["<c>display_decimals</c>"] Decimals to display
        /// </summary>
        [JsonPropertyName("display_decimals")]
        public int DisplayDecimals { get; set; }
        /// <summary>
        /// ["<c>collateral_value</c>"] Collateral value
        /// </summary>
        [JsonPropertyName("collateral_value")]
        public decimal? CollateralValue { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>

        [JsonPropertyName("status")]
        public AssetStatus Status { get; set; }
    }
}
