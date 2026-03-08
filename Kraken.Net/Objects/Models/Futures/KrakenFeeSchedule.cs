namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFeeScheduleVolumeResult : KrakenFuturesResult<Dictionary<string, decimal>>
    {
        [JsonPropertyName("volumesByFeeSchedule")]
        public override Dictionary<string, decimal> Data { get; set; } = new Dictionary<string, decimal>();
    }

    [SerializationModel]
    internal record KrakenFeeSchedulesResult : KrakenFuturesResult<KrakenFeeSchedule[]>
    {
        [JsonPropertyName("feeSchedules")]
        public override KrakenFeeSchedule[] Data { get; set; } = [];
    }

    /// <summary>
    /// Fee info
    /// </summary>
    [SerializationModel]
    public record KrakenFeeSchedule
    {
        /// <summary>
        /// ["<c>name</c>"] Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>uid</c>"] Id
        /// </summary>
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tiers</c>"] Fee tiers
        /// </summary>
        [JsonPropertyName("tiers")]
        public KrakenFee[] Tiers { get; set; } = [];
    }

    /// <summary>
    /// Fee info when trading volume is above specific volume
    /// </summary>
    [SerializationModel]
    public record KrakenFee
    {
        /// <summary>
        /// ["<c>makerFee</c>"] Fee for maker orders
        /// </summary>
        [JsonPropertyName("makerFee")]
        public decimal MakerFee { get; set; }
        /// <summary>
        /// ["<c>takerFee</c>"] Fee for taker orders
        /// </summary>
        [JsonPropertyName("takerFee")]
        public decimal TakerFee { get; set; }
        /// <summary>
        /// ["<c>usdVolume</c>"] Usd trade volume threshold
        /// </summary>
        [JsonPropertyName("usdVolume")]
        public decimal UsdVolume { get; set; }
    }
}
