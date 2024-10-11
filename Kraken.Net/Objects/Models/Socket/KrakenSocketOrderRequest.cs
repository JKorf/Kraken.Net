using Kraken.Net.Enums;
using Kraken.Net.Objects.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Order request
    /// </summary>
    public record KrakenSocketOrderRequest
    {
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("order_type")]
        [JsonConverter(typeof(EnumConverter))]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonPropertyName("side")]
        [JsonConverter(typeof(EnumConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Quote quantity
        /// </summary>
        [JsonPropertyName("cash_order_qty"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? QuoteQuantity { get; set; }
        /// <summary>
        /// Conditional order
        /// </summary>
        [JsonPropertyName("conditional"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public KrakenSocketPlaceOrderRequestV2Condition? Conditional { get; set; }
        /// <summary>
        /// Iceberg quantity
        /// </summary>
        [JsonPropertyName("display_qty"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? IcebergQuantity { get; set; }
        /// <summary>
        /// Start time
        /// </summary>
        [JsonPropertyName("effective_time"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// Expire time
        /// </summary>
        [JsonPropertyName("expire_time"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime? ExpireTime { get; set; }
        /// <summary>
        /// Fee preference setting
        /// </summary>
        [JsonPropertyName("fee_preference"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public FeePreference? FeePreference { get; set; }
        /// <summary>
        /// Limit price
        /// </summary>
        [JsonPropertyName("limit_price"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Price { get; set; }
        /// <summary>
        /// Limit price type
        /// </summary>
        [JsonPropertyName("limit_price_type"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public PriceType? LimitPriceType { get; set; }
        /// <summary>
        /// Funds the order on margin using the maximum leverage for the pair (maximum is leverage of 5).
        /// </summary>
        [JsonPropertyName("margin"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? Margin { get; set; }
        /// <summary>
        /// Disable market price protection
        /// </summary>
        [JsonPropertyName("no_mpp"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? NoMarketPriceProtection { get; set; }
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("cl_ord_id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// User reference
        /// </summary>
        [JsonPropertyName("order_userref"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public uint? UserReference { get; set; }
        /// <summary>
        /// Order quantity
        /// </summary>
        [JsonPropertyName("order_qty"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// Post only flag
        /// </summary>
        [JsonPropertyName("post_only"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? PostOnly { get; set; }
        /// <summary>
        /// Reduce only flag
        /// </summary>
        [JsonPropertyName("reduce_only"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? ReduceOnly { get; set; }
        /// <summary>
        /// Self trade prevention type
        /// </summary>
        [JsonPropertyName("stp_type"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public SelfTradePreventionType? SelfTradePreventionType { get; set; }
        /// <summary>
        /// Time in force
        /// </summary>
        [JsonPropertyName("time_in_force"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(EnumConverter))]
        public TimeInForce? TimeInForce { get; set; }
        /// <summary>
        /// Trigger info
        /// </summary>
        [JsonPropertyName("triggers"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public KrakenSocketPlaceOrderRequestV2Trigger? Trigger { get; set; }
    }
}
