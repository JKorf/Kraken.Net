namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesLeverageResult : KrakenFuturesResult<IEnumerable<KrakenFuturesLeverage>>
    {
        [JsonPropertyName("leveragePreferences")]
        public override IEnumerable<KrakenFuturesLeverage> Data { get; set; } = Array.Empty<KrakenFuturesLeverage>();
    }

    /// <summary>
    /// Leverage setting
    /// </summary>
    public record KrakenFuturesLeverage
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Max leverage
        /// </summary>
        [JsonPropertyName("maxLeverage")]
        public decimal MaxLeverage { get; set; }
    }
}
