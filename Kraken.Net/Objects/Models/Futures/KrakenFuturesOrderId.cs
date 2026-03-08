namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Order id info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesOrderId
    {
        /// <summary>
        /// ["<c>cliOrdId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("cliOrdId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>order_id</c>"] Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
    }
}
