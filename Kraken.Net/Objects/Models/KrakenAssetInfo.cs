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
        /// Alternative name
        /// </summary>
        [JsonPropertyName("altname")]
        public string AlternateName { get; set; } = string.Empty;
        /// <summary>
        /// Class of the asset
        /// </summary>
        [JsonPropertyName("aclass")]
        public string AssetClass { get; set; } = string.Empty;
        /// <summary>
        /// Decimal precision of the asset
        /// </summary>
        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }
        /// <summary>
        /// Decimals to display
        /// </summary>
        [JsonPropertyName("display_decimals")]
        public int DisplayDecimals { get; set; }
        /// <summary>
        /// Collateral value
        /// </summary>
        [JsonPropertyName("collateral_value")]
        public decimal? CollateralValue { get; set; }
        /// <summary>
        /// Status
        /// </summary>

        [JsonPropertyName("status")]
        public AssetStatus Status { get; set; }
    }
}
