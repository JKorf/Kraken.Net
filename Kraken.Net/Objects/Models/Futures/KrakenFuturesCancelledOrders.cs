namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesCancelledOrdersResult : KrakenFuturesResult<KrakenFuturesCancelledOrders>
    {
        [JsonPropertyName("cancelStatus")]
        public override KrakenFuturesCancelledOrders Data { get; set; } = null!;
    }

    /// <summary>
    /// Cancelled order info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesCancelledOrders
    {
        /// <summary>
        /// ["<c>cancelOnly</c>"] Cancelled all or a specific symbol
        /// </summary>
        [JsonPropertyName("cancelOnly")]
        public string CancelOnly { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>cancelledOrders</c>"] Cancelled order ids
        /// </summary>
        [JsonPropertyName("cancelledOrders")]
        public KrakenFuturesOrderId[] CancelledOrders { get; set; } = Array.Empty<KrakenFuturesOrderId>();
        /// <summary>
        /// ["<c>orderEvents</c>"] Order events
        /// </summary>
        [JsonPropertyName("orderEvents")]
        public KrakenFuturesOrderEvent[] OrderEvents { get; set; } = Array.Empty<KrakenFuturesOrderEvent>();
        /// <summary>
        /// Received timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("receivedTime")]
        public DateTime ReceivedTime { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Status
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}
