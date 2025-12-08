namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Order id info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesOrderId
    {
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("cliOrdId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
    }
}
