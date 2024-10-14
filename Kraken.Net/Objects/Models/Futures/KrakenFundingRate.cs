namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFundingRatesResult : KrakenFuturesResult<IEnumerable<KrakenFundingRate>>
    {
        [JsonPropertyName("rates")]
        public override IEnumerable<KrakenFundingRate> Data { get; set; } = Array.Empty<KrakenFundingRate>();
    }

    /// <summary>
    /// Funding rate info
    /// </summary>
    public record KrakenFundingRate
    {
        /// <summary>
        /// Funding rate
        /// </summary>
        [JsonPropertyName("fundingRate")]
        public decimal FundingRate { get; set; }
        /// <summary>
        /// Relative funding rate
        /// </summary>
        [JsonPropertyName("relativeFundingRate")]
        public decimal RelativeFundingRate { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
