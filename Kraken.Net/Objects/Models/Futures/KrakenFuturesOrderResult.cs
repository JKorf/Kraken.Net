using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesOrderCancelResult : KrakenFuturesResult<KrakenFuturesOrderResult>
    {
        [JsonPropertyName("cancelStatus")]
        public override KrakenFuturesOrderResult Data { get; set; } = null!;
    }

    internal record KrakenFuturesOrderPlaceResult : KrakenFuturesResult<KrakenFuturesOrderResult>
    {
        [JsonPropertyName("sendStatus")]
        public override KrakenFuturesOrderResult Data { get; set; } = null!;
    }

    internal record KrakenFuturesOrderEditResult : KrakenFuturesResult<KrakenFuturesOrderResult>
    {
        [JsonPropertyName("editStatus")]
        public override KrakenFuturesOrderResult Data { get; set; } = null!;
    }

    /// <summary>
    /// Order status info
    /// </summary>
    public record KrakenFuturesOrderResult
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;

        [JsonPropertyName("orderId")]
        internal string OrderIdInternal
        {
            get => OrderId;
            set => OrderId = value;
        }

        /// <summary>
        /// Receive timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("receivedTime")]
        public DateTime ReceivedTime { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonPropertyName("status")]
        public KrakenFuturesOrderActionStatus Status { get; set; }
        /// <summary>
        /// Order events
        /// </summary>
        [JsonPropertyName("orderEvents")]
        public IEnumerable<KrakenFuturesOrderEvent> OrderEvents { get; set; } = Array.Empty<KrakenFuturesOrderEvent>();
    }

    /// <summary>
    /// Order event
    /// </summary>
    public record KrakenFuturesOrderEvent
    {
        /// <summary>
        /// Event type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// Reduced quantity
        /// </summary>
        [JsonPropertyName("reducedQuantity")]
        public decimal? ReducedQuantity { get; set; }
        /// <summary>
        /// Uid
        /// </summary>
        [JsonPropertyName("uid")]
        public string? Uid { get; set; }
        /// <summary>
        /// Execution id
        /// </summary>
        [JsonPropertyName("executionId")]
        public string? ExecutionId { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Order info
        /// </summary>
        [JsonPropertyName("order")]
        public KrakenFuturesOrder? Order { get; set; }
        /// <summary>
        /// New order info for edit event
        /// </summary>
        [JsonPropertyName("new")]
        public KrakenFuturesOrder? New { get; set; }
        /// <summary>
        /// Old order info for edit event
        /// </summary>
        [JsonPropertyName("old")]
        public KrakenFuturesOrder? Old { get; set; }
        /// <summary>
        /// Order before execution
        /// </summary>
        [JsonPropertyName("orderPriorExecution")]
        public KrakenFuturesOrder? OrderBeforeExecution { get; set; }
        /// <summary>
        /// Order before edit
        /// </summary>
        [JsonPropertyName("orderPriorEdit")]
        public KrakenFuturesOrder? OrderBeforeEdit { get; set; }
    }

}
