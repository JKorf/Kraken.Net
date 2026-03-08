using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Snapshot book update
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesOpenOrdersSnapshotUpdate : KrakenFuturesUpdateMessage
    {
        /// <summary>
        /// ["<c>account</c>"] Account id
        /// </summary>
        [JsonPropertyName("account")]
        public string Account { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>orders</c>"] Current open orders
        /// </summary>
        [JsonPropertyName("orders")]
        public KrakenFuturesSocketOpenOrder[] Orders { get; set; } = Array.Empty<KrakenFuturesSocketOpenOrder>();
    }

    /// <summary>
    /// Open order update
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesOpenOrdersUpdate : KrakenFuturesUpdateMessage
    {
        /// <summary>
        /// ["<c>is_cancel</c>"] Is cancel
        /// </summary>
        [JsonPropertyName("is_cancel")]
        public bool IsCancel { get; set; }
        /// <summary>
        /// ["<c>reason</c>"] Reason
        /// </summary>
        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>order_id</c>"] Reason
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>order</c>"] Order info
        /// </summary>
        [JsonPropertyName("order")]
        public KrakenFuturesSocketOpenOrder? Order { get; set; } = null!;
    }

    /// <summary>
    /// Open order
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesSocketOpenOrder
    {
        /// <summary>
        /// ["<c>instrument</c>"] Symbol
        /// </summary>
        [JsonPropertyName("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>time</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("last_update_time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// ["<c>qty</c>"] Quantitiy
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>filled</c>"] Filled quantity
        /// </summary>
        [JsonPropertyName("filled")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// ["<c>limit_price</c>"] Limit price
        /// </summary>
        [JsonPropertyName("limit_price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>stop_price</c>"] Stop price
        /// </summary>
        [JsonPropertyName("stop_price")]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Order type
        /// </summary>
        [JsonPropertyName("type")]
        public FuturesOrderType Type { get; set; }
        /// <summary>
        /// ["<c>order_id</c>"] Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>cli_ord_id</c>"] Client order id
        /// </summary>
        [JsonPropertyName("cli_ord_id")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>direction</c>"] Side
        /// </summary>
        [JsonPropertyName("direction")]

        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>reduce_only</c>"] Reduce only
        /// </summary>
        [JsonPropertyName("reduce_only")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// ["<c>triggerSignal</c>"] Trigger signal
        /// </summary>

        [JsonPropertyName("triggerSignal")]
        public TriggerSignal? TriggerSignal { get; set; }
        /// <summary>
        /// ["<c>trailing_stop_options</c>"] Trailing stop options
        /// </summary>
        [JsonPropertyName("trailing_stop_options")]
        public KrakenFuturesTrailingStopOptions? TrailingStopOptions { get; set; }
    }

    /// <summary>
    /// Trailing stop options
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesTrailingStopOptions
    {
        /// <summary>
        /// ["<c>max_deviation</c>"] Max deviation
        /// </summary>
        [JsonPropertyName("max_deviation")]
        public decimal? MaxDeviation { get; set; }
        /// <summary>
        /// ["<c>unit</c>"] Unit
        /// </summary>

        [JsonPropertyName("unit")]
        public TrailingStopDeviationUnit Unit { get; set; }
    }
}
