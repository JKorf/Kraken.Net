namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFeeSchedulesResult : KrakenFuturesResult<IEnumerable<KrakenFeeSchedule>>
    {
        [JsonPropertyName("feeSchedules")]
        public override IEnumerable<KrakenFeeSchedule> Data { get; set; } = new List<KrakenFeeSchedule>();
    }

    /// <summary>
    /// Fee info
    /// </summary>
    public record KrakenFeeSchedule
    {
        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = string.Empty;
        /// <summary>
        /// Fee tiers
        /// </summary>
        [JsonPropertyName("tiers")]
        public IEnumerable<KrakenFee> Tiers { get; set; } = new List<KrakenFee>();
    }

    /// <summary>
    /// Fee info when trading volume is above specific volume
    /// </summary>
    public record KrakenFee
    {
        /// <summary>
        /// Fee for maker orders
        /// </summary>
        [JsonPropertyName("makerFee")]
        public decimal MakerFee { get; set; }
        /// <summary>
        /// Fee for taker orders
        /// </summary>
        [JsonPropertyName("takerFee")]
        public decimal TakerFee { get; set; }
        /// <summary>
        /// Usd trade volume threshold
        /// </summary>
        [JsonPropertyName("usdVolume")]
        public decimal UsdVolume { get; set; }
    }
}
