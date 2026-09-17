namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Authenticated trading instruments response.
    /// </summary>
    [SerializationModel]
    internal record KrakenFuturesTradingSymbolResult : KrakenFuturesResult<KrakenFuturesTradingSymbol[]>
    {
        /// <inheritdoc />
        [JsonPropertyName("instruments")]
        public override KrakenFuturesTradingSymbol[] Data { get; set; } = [];
    }
}
