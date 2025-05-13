using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("cl_ord_id")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Order quantity in quote asset
        /// </summary>
        [JsonPropertyName("cash_order_qty")]
        public decimal? QuoteOrderQuantity { get; set; }
        /// <summary>
        /// Order quantity
        /// </summary>
        [JsonPropertyName("order_qty")]
        public decimal? OrderQuantity { get; set; }
        /// <summary>
        /// Filled quantity value
        /// </summary>
        [JsonPropertyName("cum_cost")]
        public decimal? ValueFilled { get; set; }
        /// <summary>
        /// Filled quantity
        /// </summary>
        [JsonPropertyName("cum_qty")]
        public decimal? QuantityFilled { get; set; }
        /// <summary>
        /// Display quantity for iceberg orders
        /// </summary>
        [JsonPropertyName("display_qty")]
        public decimal? IcebergQuantity { get; set; }
        /// <summary>
        /// Time in force
        /// </summary>
        [JsonPropertyName("time_in_force")]
        public TimeInForce? TimeInForce { get; set; }
        /// <summary>
        /// Order event
        /// </summary>
        [JsonPropertyName("exec_type")]
        public OrderEventType OrderEventType { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide? OrderSide { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("order_type")]
        public OrderType? OrderType { get; set; }
        /// <summary>
        /// Order user reference
        /// </summary>
        [JsonPropertyName("order_userref")]
        public decimal OrderUserReference { get; set; }
        /// <summary>
        /// Limit price
        /// </summary>
        [JsonPropertyName("limit_price")]
        public decimal? LimitPrice { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        [JsonPropertyName("stop_price")]
        public decimal? StopPrice { get; set; }
        /// <summary>
        /// Order status
        /// </summary>
        [JsonPropertyName("order_status")]
        public OrderStatusUpdate OrderStatus { get; set; }
        /// <summary>
        /// Fee paid expressed in USD
        /// </summary>
        [JsonPropertyName("fee_usd_equiv")]
        public decimal? FeeUsdEquiv { get; set; }
        /// <summary>
        /// Fee asset preference
        /// </summary>
        [JsonPropertyName("fee_ccy_pref")]
        public OrderFlags? FeeAssetPreference { get; set; }
        /// <summary>
        /// Scheduled start time of the order
        /// </summary>
        [JsonPropertyName("effective_time")]
        public DateTime? EffectiveTime { get; set; }
        /// <summary>
        /// Scheduled expiration time of the order
        /// </summary>
        [JsonPropertyName("expire_time")]
        public DateTime? ExpireTime { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Average order trade price
        /// </summary>
        [JsonPropertyName("avg_price")]
        public decimal? AveragePrice { get; set; }
        /// <summary>
        /// Fees paid
        /// </summary>
        [JsonPropertyName("fees")]
        public KrakenOrderUpdateFee[]? Fees { get; set; }
        /// <summary>
        /// Whether the order has been amended
        /// </summary>
        [JsonPropertyName("amended")]
        public bool? Amended { get; set; }
        /// <summary>
        /// Indicates if the order has been liquidated by the engine
        /// </summary>
        [JsonPropertyName("liquidated")]
        public bool? Liquidated { get; set; }
        /// <summary>
        /// Indicates if the order can be funded on margin
        /// </summary>
        [JsonPropertyName("margin")]
        public bool? Margin { get; set; }
        /// <summary>
        /// Indicates if an execution is on margin, i.e. if the trade increased or reduced size of margin borrowing. On trade events only
        /// </summary>
        [JsonPropertyName("margin_borrow")]
        public bool? MarginBorrow { get; set; }
        /// <summary>
        /// Indicates if the order has market price protection
        /// </summary>
        [JsonPropertyName("no_mpp")]
        public bool? NoMarketPriceProtection { get; set; }
        /// <summary>
        /// Post only flag
        /// </summary>
        [JsonPropertyName("post_only")]
        public bool? PostOnly { get; set; }
        /// <summary>
        /// Reduce only flag
        /// </summary>
        [JsonPropertyName("reduce_only")]
        public bool? ReduceOnly { get; set; }
        /// <summary>
        /// Indicates status of the position on a margin order
        /// </summary>
        [JsonPropertyName("position_status")]
        public string? PositionStatus { get; set; }
        /// <summary>
        /// The reason associated with an event, if applicable
        /// </summary>
        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        /// <summary>
        /// Id of the execution this update is for
        /// </summary>
        [JsonPropertyName("exec_id")]
        public string? ExecutionId { get; set; }
        /// <summary>
        /// Id of the trade this update is for
        /// </summary>
        [JsonPropertyName("trade_id")]
        public long? LastTradeId { get; set; }
        /// <summary>
        /// Quantity of the trade this update is for
        /// </summary>
        [JsonPropertyName("last_qty")]
        public decimal? LastTradeQuantity { get; set; }
        /// <summary>
        /// Price of the trade this update is for
        /// </summary>
        [JsonPropertyName("last_price")]
        public decimal? LastTradePrice { get; set; }
        /// <summary>
        /// Value of the trade this update is for
        /// </summary>
        [JsonPropertyName("cost")]
        public decimal? LastTradeValue { get; set; }
        /// <summary>
        /// Trade role of the trade this update is for, maker or taker
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
        /// Fee asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
    }
}
