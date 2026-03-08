namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Trade volume info
    /// </summary>
    [SerializationModel]
    public record KrakenTradeVolume
    {
        /// <summary>
        /// ["<c>currency</c>"] Asset
        /// </summary>
        [JsonPropertyName("currency")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>volume</c>"] Volume
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }

        /// <summary>
        /// ["<c>fees</c>"] Fees structure
        /// </summary>
        [JsonPropertyName("fees")]
        public Dictionary<string, KrakenFeeStruct> Fees { get; set; } = new Dictionary<string, KrakenFeeStruct>();
        /// <summary>
        /// ["<c>fees_maker</c>"] Maker fees structure
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
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>minfee</c>"] Minimal fee
        /// </summary>
        [JsonPropertyName("minfee")]
        public decimal MinimalFee { get; set; }
        /// <summary>
        /// ["<c>maxfee</c>"] Maximal fee
        /// </summary>
        [JsonPropertyName("maxfee")]
        public decimal MaximumFee { get; set; }
        /// <summary>
        /// ["<c>nextfee</c>"] Next fee
        /// </summary>
        [JsonPropertyName("nextfee")]
        public decimal? NextFee { get; set; }
        /// <summary>
        /// ["<c>nextvolume</c>"] Next volume
        /// </summary>
        [JsonPropertyName("nextvolume")]
        public decimal? NextVolume { get; set; }
        /// <summary>
        /// ["<c>tiervolume</c>"] Tier volume
        /// </summary>
        [JsonPropertyName("tiervolume")]
        public decimal TierVolume { get; set; }
    }
}
