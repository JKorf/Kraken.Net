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
        /// ["<c>accountUid</c>"] Account id
        /// </summary>
        [JsonPropertyName("accountUid")]
        public string AccountUid { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>continuationToken</c>"] Continuation token for pagination
        /// </summary>
        [JsonPropertyName("continuationToken")]
        public string ContinuationToken { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>len</c>"] Total number of results
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
        /// ["<c>elements</c>"] Elements
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
        /// ["<c>uid</c>"] Uid
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
        /// ["<c>event</c>"] Event info
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
        /// ["<c>execution</c>"] Execution info
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
        /// ["<c>execution</c>"] Execution info
        /// </summary>
        [JsonPropertyName("execution")]
        public KrakenFuturesExecution Execution { get; set; } = null!;

        /// <summary>
        /// ["<c>takerReducedQuantity</c>"] Taker reduced quantity
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
        /// ["<c>limitFilled</c>"] Limit filled
        /// </summary>
        [JsonPropertyName("limitFilled")]
        public bool? LimitFilled { get; set; }
        /// <summary>
        /// ["<c>makerOrder</c>"] Maker order info
        /// </summary>
        [JsonPropertyName("makerOrder")]
        public KrakenFuturesExecutionOrder? MakerOrder { get; set; }
        /// <summary>
        /// ["<c>makerOrderData</c>"] Maker order data
        /// </summary>
        [JsonPropertyName("makerOrderData")]
        public KrakenFuturesOrderData? MakerOrderData { get; set; }
        /// <summary>
        /// ["<c>markPrice</c>"] Mark price
        /// </summary>
        [JsonPropertyName("markPrice")]
        public decimal? MarkPrice { get; set; }
        /// <summary>
        /// ["<c>oldTakerOrder</c>"] Old taker order
        /// </summary>
        [JsonPropertyName("oldTakerOrder")]
        public KrakenFuturesExecutionOrder? OldTakerOrder { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// ["<c>quantity</c>"] Quantity
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// ["<c>takerOrder</c>"] Taker order
        /// </summary>
        [JsonPropertyName("takerOrder")]
        public KrakenFuturesExecutionOrder? TakerOrder { get; set; }
        /// <summary>
        /// ["<c>takerOrderData</c>"] Taker order data
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
        /// ["<c>uid</c>"] Uid
        /// </summary>
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>usdValue</c>"] Usd value
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
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }

        /// <summary>
        /// ["<c>positionSize</c>"] Position size
        /// </summary>
        [JsonPropertyName("positionSize")]
        public decimal PositionSize { get; set; }
    }

    /// <summary>
    /// Execution order info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesExecutionOrder
    {
        /// <summary>
        /// ["<c>clientId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("clientId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>accountUid</c>"] Account id
        /// </summary>
        [JsonPropertyName("accountUid")]
        public string AccountUid { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tradeable</c>"] Tradeable
        /// </summary>
        [JsonPropertyName("tradeable")]
        public string Tradeable { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>filled</c>"] Quantity filled
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
        /// ["<c>limitPrice</c>"] Price
        /// </summary>
        [JsonPropertyName("limitPrice")]
        public decimal? Price { get; set; }
        /// <summary>
        /// ["<c>uid</c>"] Order id
        /// </summary>
        [JsonPropertyName("uid")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>quantity</c>"] Quantity
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>reduceOnly</c>"] Reduce only
        /// </summary>
        [JsonPropertyName("reduceOnly")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// ["<c>direction</c>"] Order side
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
        /// ["<c>orderType</c>"] Order type
        /// </summary>

        [JsonPropertyName("orderType")]
        public FuturesOrderType Type { get; set; }
    }
}
