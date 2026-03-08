namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Batch order result
    /// </summary>
    [SerializationModel]
    public record KrakenBatchOrderResult
    {
        /// <summary>
        /// ["<c>orders</c>"] Orders
        /// </summary>
        [JsonPropertyName("orders")]
        public KrakenPlacedBatchOrder[] Orders { get; set; } = Array.Empty<KrakenPlacedBatchOrder>();
    }

    /// <summary>
    /// Placed batch order
    /// </summary>
    [SerializationModel]
    public record KrakenPlacedBatchOrder
    {
        /// <summary>
        /// ["<c>txid</c>"] Order id
        /// </summary>
        [JsonPropertyName("txid")]
        public string OrderId { get; set; } = null!;
        /// <summary>
        /// ["<c>error</c>"] Error message
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }
        /// <summary>
        /// ["<c>descr</c>"] Description
        /// </summary>
        [JsonPropertyName("descr")]
        public KrakenPlacedOrderDescription Description { get; set; } = null!;
        /// <summary>
        /// ["<c>close</c>"] Close order description
        /// </summary>
        [JsonPropertyName("close")]
        public string? CloseOrderInfo { get; set; }
    }
}
