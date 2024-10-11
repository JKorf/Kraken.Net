namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Amend order result
    /// </summary>
    public record KrakenSocketAmendOrderResult
    {
        /// <summary>
        /// Amend id
        /// </summary>
        [JsonPropertyName("amend_id")]
        public string AmendId { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("cl_ord_id")]
        public string? ClientOrderId { get; set; }
    }
}
