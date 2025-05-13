using CryptoExchange.Net.Converters.SystemTextJson;
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
