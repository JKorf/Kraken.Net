using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// User execution events
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesUserExecutionEvents
    {
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("accountUid")]
        public string AccountUid { get; set; } = string.Empty;
        /// <summary>
        /// Continuation token for pagination
        /// </summary>
        [JsonPropertyName("continuationToken")]
        public string ContinuationToken { get; set; } = string.Empty;
        /// <summary>
        /// Total number of results
        /// </summary>
        [JsonPropertyName("len")]
        public long Total { get; set; }
        /// <summary>
        /// Server time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("serverTime")]
        public DateTime ServerTime { get; set; }
        /// <summary>
        /// Elements
        /// </summary>
        [JsonPropertyName("elements")]
        public KrakenFuturesExecutionElement[] Elements { get; set; } = Array.Empty<KrakenFuturesExecutionElement>();
    }

    /// <summary>
    /// Execution event info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesExecutionElement
    {
        /// <summary>
        /// Uid
        /// </summary>
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = string.Empty;
        /// <summary>
        /// Event timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Event info
        /// </summary>
        [JsonPropertyName("event")]
        public KrakenFuturesExecutionEventWrapper Event { get; set; } = null!;
    }

    /// <summary>
    /// Event info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesExecutionEventWrapper
    {
        /// <summary>
        /// Execution info
        /// </summary>
        [JsonPropertyName("execution")]
        public KrakenFuturesExecutionEvent Execution { get; set; } = null!;
    }

    /// <summary>
    /// Execution info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesExecutionEvent
    {
        /// <summary>
        /// Execution info
        /// </summary>
        [JsonPropertyName("execution")]
        public KrakenFuturesExecution Execution { get; set; } = null!;

        /// <summary>
        /// Taker reduced quantity
        /// </summary>
        [JsonPropertyName("takerReducedQuantity")]
        public decimal? TakerReducedQuantity { get; set; }
    }

    /// <summary>
    /// Execution info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesExecution
    {
        /// <summary>
        /// Limit filled
        /// </summary>
        [JsonPropertyName("limitFilled")]
        public bool? LimitFilled { get; set; }
        /// <summary>
        /// Maker order info
        /// </summary>
        [JsonPropertyName("makerOrder")]
        public KrakenFuturesExecutionOrder? MakerOrder { get; set; }
        /// <summary>
        /// Maker order data
        /// </summary>
        [JsonPropertyName("makerOrderData")]
        public KrakenFuturesOrderData? MakerOrderData { get; set; }
        /// <summary>
        /// Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal? MarkPrice { get; set; }
        /// <summary>
        /// Old taker order
        /// </summary>
        [JsonPropertyName("oldTakerOrder")]
        public KrakenFuturesExecutionOrder? OldTakerOrder { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// Taker order
        /// </summary>
        [JsonPropertyName("takerOrder")]
        public KrakenFuturesExecutionOrder? TakerOrder { get; set; }
        /// <summary>
        /// Taker order data
        /// </summary>
        [JsonPropertyName("takerOrderData")]
        public KrakenFuturesOrderData? TakerOrderData { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }
        /// <summary>
        /// Uid
        /// </summary>
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = string.Empty;
        /// <summary>
        /// Usd value
        /// </summary>
        [JsonPropertyName("usdValue")]
        public decimal? UsdValue { get; set; }
    }

    /// <summary>
    /// Additional order data
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesOrderData
    {
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }

        /// <summary>
        /// Position size
        /// </summary>
        [JsonPropertyName("positionSize")]
        public decimal PositionSize { get; set; }
    }

    /// <summary>
    /// Excecution order info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesExecutionOrder
    {
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("clientId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("accountUid")]
        public string AccountUid { get; set; } = string.Empty;
        /// <summary>
        /// Tradeable
        /// </summary>
        [JsonPropertyName("tradeable")]
        public string Tradeable { get; set; } = string.Empty;
        /// <summary>
        /// Quantity filled
        /// </summary>
        [JsonPropertyName("filled")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("lastUpdateTimestamp")]
        public DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("limitPrice")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("uid")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        [JsonPropertyName("reduceOnly")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// Order side
        /// </summary>

        [JsonPropertyName("direction")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Order type
        /// </summary>

        [JsonPropertyName("orderType")]
        public FuturesOrderType Type { get; set; }
    }
}
