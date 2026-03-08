namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Amend order result
    /// </summary>
    [SerializationModel]
    public record KrakenSocketAmendOrderResult
    {
        /// <summary>
        /// ["<c>amend_id</c>"] Amend id
        /// </summary>
        [JsonPropertyName("amend_id")]
        public string AmendId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>order_id</c>"] Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>cl_ord_id</c>"] Client order id
        /// </summary>
        [JsonPropertyName("cl_ord_id")]
        public string? ClientOrderId { get; set; }
    }
}
