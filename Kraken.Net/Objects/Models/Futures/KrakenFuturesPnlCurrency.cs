namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesPnlCurrencyResult : KrakenFuturesResult<IEnumerable<KrakenFuturesPnlCurrency>>
    {
        [JsonPropertyName("preferences")]
        public override IEnumerable<KrakenFuturesPnlCurrency> Data { get; set; } = Array.Empty<KrakenFuturesPnlCurrency>();
    }

    /// <summary>
    /// Profit and loss currency preference
    /// </summary>
    public record KrakenFuturesPnlCurrency
    {
        /// <summary>
        /// Profit and loss currency
        /// </summary>
        [JsonPropertyName("pnlCurrency")]
        public string PnlCurrency { get; set; } = string.Empty;
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
    }
}
