namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesSymbolStatusResult : KrakenFuturesResult<KrakenFuturesSymbolStatus[]>
    {
        [JsonPropertyName("instrumentStatus")]
        public override KrakenFuturesSymbolStatus[] Data { get; set; } = [];
    }

    /// <summary>
    /// Symbol status
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesSymbolStatus
    {
        /// <summary>
        /// ["<c>extremeVolatilityInitialMarginMultiplier</c>"] Extreme volatility initial margin multiplier
        /// </summary>
        [JsonPropertyName("extremeVolatilityInitialMarginMultiplier")]
        public int ExtremeVolatilityInitialMarginMultiplier { get; set; }
        /// <summary>
        /// ["<c>isExperiencingDislocation</c>"] Is experiencing dislocation
        /// </summary>
        [JsonPropertyName("isExperiencingDislocation")]
        public bool IsExperiencingDislocation { get; set; }
        /// <summary>
        /// ["<c>isExperiencingExtremeVolatility</c>"] Is experiencing exterme volatility
        /// </summary>
        [JsonPropertyName("isExperiencingExtremeVolatility")]
        public bool IsExperiencingExtremeVolatility { get; set; }
        /// <summary>
        /// ["<c>priceDislocationDirection</c>"] Price dislocation direction
        /// </summary>
        [JsonPropertyName("priceDislocationDirection")]
        public string? PriceDislocationDirection { get; set; }
        /// <summary>
        /// ["<c>tradeable</c>"] Tradeable
        /// </summary>
        [JsonPropertyName("tradeable")]
        public string Tradeable { get; set; } = string.Empty;
    }
}
