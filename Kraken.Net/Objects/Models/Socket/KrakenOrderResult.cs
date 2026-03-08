namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Order result
    /// </summary>
    [SerializationModel]
    public record KrakenOrderResult
    {
        /// <summary>
        /// ["<c>order_id</c>"] Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>cl_ord_id</c>"] Order id
        /// </summary>
        [JsonPropertyName("cl_ord_id")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>order_userref</c>"] User reference id
        /// </summary>
        [JsonPropertyName("order_userref")]
        public long? UserReference { get; set; }
        /// <summary>
        /// ["<c>error</c>"] Error
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
