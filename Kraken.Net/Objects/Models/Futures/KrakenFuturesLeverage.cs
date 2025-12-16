namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesLeverageResult : KrakenFuturesResult<KrakenFuturesLeverage[]>
    {
        [JsonPropertyName("leveragePreferences")]
        public override KrakenFuturesLeverage[] Data { get; set; } = Array.Empty<KrakenFuturesLeverage>();
    }

    /// <summary>
    /// Leverage setting
    /// </summary>
    [SerializationModel]
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
