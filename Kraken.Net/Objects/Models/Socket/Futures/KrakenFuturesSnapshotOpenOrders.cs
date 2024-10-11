using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Snapshot book update
    /// </summary>
    public record KrakenFuturesOpenOrdersSnapshotUpdate : KrakenFuturesUpdateMessage
    {
        /// <summary>
        /// Account id
        /// </summary>
        [JsonPropertyName("account")]
        public string Account { get; set; } = string.Empty;
        /// <summary>
        /// Current open orders
        /// </summary>
        [JsonPropertyName("orders")]
        public IEnumerable<KrakenFuturesSocketOpenOrder> Orders { get; set; } = Array.Empty<KrakenFuturesSocketOpenOrder>();
    }

    /// <summary>
    /// Open order update
    /// </summary>
    public record KrakenFuturesOpenOrdersUpdate : KrakenFuturesUpdateMessage
    {
        /// <summary>
        /// Is cancel
        /// </summary>
        [JsonPropertyName("is_cancel")]
        public bool IsCancel { get; set; }
        /// <summary>
        /// Reason
        /// </summary>
        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;
        /// <summary>
        /// Reason
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Order info
        /// </summary>
        [JsonPropertyName("order")]
        public KrakenFuturesSocketOpenOrder? Order { get; set; } = null!;
    }

    /// <summary>
    /// Open order
    /// </summary>
    public record KrakenFuturesSocketOpenOrder
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("instrument")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
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
        /// Quantitiy
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Filled quantity
        /// </summary>
        [JsonPropertyName("filled")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Limit price
        /// </summary>
        [JsonPropertyName("limit_price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        [JsonPropertyName("stop_price")]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("type")]
        public FuturesOrderType Type { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("cli_ord_id")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("direction")]
        [JsonConverter(typeof(EnumConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        [JsonPropertyName("reduce_only")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// Trigger signal
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonPropertyName("triggerSignal")]
        public TriggerSignal? TriggerSignal { get; set; }
        /// <summary>
        /// Trailing stop options
        /// </summary>
        [JsonPropertyName("trailing_stop_options")]
        public KrakenFuturesTrailingStopOptions? TrailingStopOptions { get; set; }
    }

    /// <summary>
    /// Trailing stop options
    /// </summary>
    public record KrakenFuturesTrailingStopOptions
    {
        /// <summary>
        /// Max deviation
        /// </summary>
        [JsonPropertyName("max_deviation")]
        public decimal? MaxDeviation { get; set; }
        /// <summary>
        /// Unit
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonPropertyName("unit")]
        public TrailingStopDeviationUnit Unit { get; set; }
    }
}
