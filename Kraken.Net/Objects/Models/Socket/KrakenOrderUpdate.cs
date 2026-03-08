using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Kraken order update
    /// </summary>
    [SerializationModel]
    public record KrakenOrderUpdate
    {
        /// <summary>
        /// ["<c>order_id</c>"] Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }
        /// <summary>
        /// ["<c>cl_ord_id</c>"] Client order id
        /// </summary>
        [JsonPropertyName("cl_ord_id")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>cash_order_qty</c>"] Order quantity in quote asset
        /// </summary>
        [JsonPropertyName("cash_order_qty")]
        public decimal? QuoteOrderQuantity { get; set; }
        /// <summary>
        /// ["<c>order_qty</c>"] Order quantity
        /// </summary>
        [JsonPropertyName("order_qty")]
        public decimal? OrderQuantity { get; set; }
        /// <summary>
        /// ["<c>cum_cost</c>"] Filled quantity value
        /// </summary>
        [JsonPropertyName("cum_cost")]
        public decimal? ValueFilled { get; set; }
        /// <summary>
        /// ["<c>cum_qty</c>"] Filled quantity
        /// </summary>
        [JsonPropertyName("cum_qty")]
        public decimal? QuantityFilled { get; set; }
        /// <summary>
        /// ["<c>display_qty</c>"] Display quantity for iceberg orders
        /// </summary>
        [JsonPropertyName("display_qty")]
        public decimal? IcebergQuantity { get; set; }
        /// <summary>
        /// ["<c>time_in_force</c>"] Time in force
        /// </summary>
        [JsonPropertyName("time_in_force")]
        public TimeInForce? TimeInForce { get; set; }
        /// <summary>
        /// ["<c>exec_type</c>"] Order event
        /// </summary>
        [JsonPropertyName("exec_type")]
        public OrderEventType OrderEventType { get; set; }
        /// <summary>
        /// ["<c>side</c>"] Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide? OrderSide { get; set; }
        /// <summary>
        /// ["<c>order_type</c>"] Order type
        /// </summary>
        [JsonPropertyName("order_type")]
        public OrderType? OrderType { get; set; }
        /// <summary>
        /// ["<c>order_userref</c>"] Order user reference
        /// </summary>
        [JsonPropertyName("order_userref")]
        public decimal OrderUserReference { get; set; }
        /// <summary>
        /// ["<c>limit_price</c>"] Limit price
        /// </summary>
        [JsonPropertyName("limit_price")]
        public decimal? LimitPrice { get; set; }
        /// <summary>
        /// ["<c>stop_price</c>"] Stop price
        /// </summary>
        [JsonPropertyName("stop_price")]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// ["<c>order_status</c>"] Order status
        /// </summary>
        [JsonPropertyName("order_status")]
        public OrderStatusUpdate OrderStatus { get; set; }
        /// <summary>
        /// ["<c>fee_usd_equiv</c>"] Fee paid expressed in USD
        /// </summary>
        [JsonPropertyName("fee_usd_equiv")]
        public decimal? FeeUsdEquiv { get; set; }
        /// <summary>
        /// ["<c>fee_ccy_pref</c>"] Fee asset preference
        /// </summary>
        [JsonPropertyName("fee_ccy_pref")]
        public OrderFlags? FeeAssetPreference { get; set; }
        /// <summary>
        /// ["<c>effective_time</c>"] Scheduled start time of the order
        /// </summary>
        [JsonPropertyName("effective_time")]
        public DateTime? EffectiveTime { get; set; }
        /// <summary>
        /// ["<c>expire_time</c>"] Scheduled expiration time of the order
        /// </summary>
        [JsonPropertyName("expire_time")]
        public DateTime? ExpireTime { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>avg_price</c>"] Average order trade price
        /// </summary>
        [JsonPropertyName("avg_price")]
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// ["<c>fees</c>"] Fees paid
        /// </summary>
        [JsonPropertyName("fees")]
        public KrakenOrderUpdateFee[]? Fees { get; set; }
        /// <summary>
        /// ["<c>amended</c>"] Whether the order has been amended
        /// </summary>
        [JsonPropertyName("amended")]
        public bool? Amended { get; set; }
        /// <summary>
        /// ["<c>liquidated</c>"] Indicates if the order has been liquidated by the engine
        /// </summary>
        [JsonPropertyName("liquidated")]
        public bool? Liquidated { get; set; }
        /// <summary>
        /// ["<c>margin</c>"] Indicates if the order can be funded on margin
        /// </summary>
        [JsonPropertyName("margin")]
        public bool? Margin { get; set; }
        /// <summary>
        /// ["<c>margin_borrow</c>"] Indicates if an execution is on margin, i.e. if the trade increased or reduced size of margin borrowing. On trade events only
        /// </summary>
        [JsonPropertyName("margin_borrow")]
        public bool? MarginBorrow { get; set; }
        /// <summary>
        /// ["<c>no_mpp</c>"] Indicates if the order has market price protection
        /// </summary>
        [JsonPropertyName("no_mpp")]
        public bool? NoMarketPriceProtection { get; set; }
        /// <summary>
        /// ["<c>post_only</c>"] Post only flag
        /// </summary>
        [JsonPropertyName("post_only")]
        public bool? PostOnly { get; set; }
        /// <summary>
        /// ["<c>reduce_only</c>"] Reduce only flag
        /// </summary>
        [JsonPropertyName("reduce_only")]
        public bool? ReduceOnly { get; set; }
        /// <summary>
        /// ["<c>position_status</c>"] Indicates status of the position on a margin order
        /// </summary>
        [JsonPropertyName("position_status")]
        public string? PositionStatus { get; set; }
        /// <summary>
        /// ["<c>reason</c>"] The reason associated with an event, if applicable
        /// </summary>
        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        /// <summary>
        /// ["<c>exec_id</c>"] Id of the execution this update is for
        /// </summary>
        [JsonPropertyName("exec_id")]
        public string? ExecutionId { get; set; }
        /// <summary>
        /// ["<c>trade_id</c>"] Id of the trade this update is for
        /// </summary>
        [JsonPropertyName("trade_id")]
        public long? LastTradeId { get; set; }
        /// <summary>
        /// ["<c>last_qty</c>"] Quantity of the trade this update is for
        /// </summary>
        [JsonPropertyName("last_qty")]
        public decimal? LastTradeQuantity { get; set; }
        /// <summary>
        /// ["<c>last_price</c>"] Price of the trade this update is for
        /// </summary>
        [JsonPropertyName("last_price")]
        public decimal? LastTradePrice { get; set; }
        /// <summary>
        /// ["<c>cost</c>"] Value of the trade this update is for
        /// </summary>
        [JsonPropertyName("cost")]
        public decimal? LastTradeValue { get; set; }
        /// <summary>
        /// ["<c>liquidity_ind</c>"] Trade role of the trade this update is for, maker or taker
        /// </summary>
        [JsonPropertyName("liquidity_ind")]
        public TradeType? LastTradeRole { get; set; }
    }

    /// <summary>
    /// Fee info
    /// </summary>
    [SerializationModel]
    public record KrakenOrderUpdateFee
    {
        /// <summary>
        /// ["<c>asset</c>"] Fee asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>qty</c>"] Quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
    }
}
