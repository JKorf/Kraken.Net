namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Replace order result
    /// </summary>
    [SerializationModel]
    public record KrakenSocketReplaceOrderResult
    {
        /// <summary>
        /// ["<c>order_id</c>"] Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>original_order_id</c>"] The original order id
        /// </summary>
        [JsonPropertyName("original_order_id")]
        public string OriginalOrderId { get; set; } = string.Empty;
    }
}
