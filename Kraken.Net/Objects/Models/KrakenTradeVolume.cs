using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Trade volume info
    /// </summary>
    [SerializationModel]
    public record KrakenTradeVolume
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }

        /// <summary>
        /// Fees structure
        /// </summary>
        [JsonPropertyName("fees")]
        public Dictionary<string, KrakenFeeStruct> Fees { get; set; } = new Dictionary<string, KrakenFeeStruct>();
        /// <summary>
        /// Maker fees structure
        /// </summary>
        [JsonPropertyName("fees_maker")]
        public Dictionary<string, KrakenFeeStruct> MakerFees { get; set; } = new Dictionary<string, KrakenFeeStruct>();
    }

    /// <summary>
    /// Fee level info
    /// </summary>
    [SerializationModel]
    public record KrakenFeeStruct
    {
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Minimal fee
        /// </summary>
        [JsonPropertyName("minfee")]
        public decimal MinimalFee { get; set; }
        /// <summary>
        /// Maximal fee
        /// </summary>
        [JsonPropertyName("maxfee")]
        public decimal MaximumFee { get; set; }
        /// <summary>
        /// Next fee
        /// </summary>
        [JsonPropertyName("nextFee")]
        public decimal? NextFee { get; set; }
        /// <summary>
        /// Next volume
        /// </summary>
        [JsonPropertyName("nextVolume")]
        public decimal? NextVolume { get; set; }
        /// <summary>
        /// Tier volume
        /// </summary>
        [JsonPropertyName("tierVolume")]
        public decimal TierVolume { get; set; }
    }
}
