namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesSymbolStatusResult : KrakenFuturesResult<IEnumerable<KrakenFuturesSymbolStatus>>
    {
        [JsonPropertyName("instrumentStatus")]
        public override IEnumerable<KrakenFuturesSymbolStatus> Data { get; set; } = new List<KrakenFuturesSymbolStatus>();
    }

    /// <summary>
    /// Symbol status
    /// </summary>
    public record KrakenFuturesSymbolStatus
    {
        /// <summary>
        /// Extreme volatility initial margin multiplier
        /// </summary>
        [JsonPropertyName("extremeVolatilityInitialMarginMultiplier")]
        public int ExtremeVolatilityInitialMarginMultiplier { get; set; }
        /// <summary>
        /// Is experiencing dislocation
        /// </summary>
        [JsonPropertyName("isExperiencingDislocation")]
        public bool IsExperiencingDislocation { get; set; }
        /// <summary>
        /// Is experiencing exterme volatility
        /// </summary>
        [JsonPropertyName("isExperiencingExtremeVolatility")]
        public bool IsExperiencingExtremeVolatility { get; set; }
        /// <summary>
        /// Price dislocation direction
        /// </summary>
        [JsonPropertyName("priceDislocationDirection")]
        public string? PriceDislocationDirection { get; set; }
        /// <summary>
        /// Tradeable
        /// </summary>
        [JsonPropertyName("tradeable")]
        public string Tradeable { get; set; } = string.Empty;
    }
}
