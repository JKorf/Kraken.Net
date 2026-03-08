namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesPnlCurrencyResult : KrakenFuturesResult<KrakenFuturesPnlCurrency[]>
    {
        [JsonPropertyName("preferences")]
        public override KrakenFuturesPnlCurrency[] Data { get; set; } = Array.Empty<KrakenFuturesPnlCurrency>();
    }

    /// <summary>
    /// Profit and loss currency preference
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesPnlCurrency
    {
        /// <summary>
        /// ["<c>pnlCurrency</c>"] Profit and loss currency
        /// </summary>
        [JsonPropertyName("pnlCurrency")]
        public string PnlCurrency { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
    }
}
