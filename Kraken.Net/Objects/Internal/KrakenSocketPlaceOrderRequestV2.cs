using Kraken.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Internal
{
    internal class KrakenSocketPlaceOrderRequestV2 : KrakenSocketAuthRequestV2
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        [JsonPropertyName("order_type")]
        [JsonConverter(typeof(EnumConverter))]
        public OrderType OrderType { get; set; }
        [JsonPropertyName("side")]
        [JsonConverter(typeof(EnumConverter))]
        public OrderSide Side { get; set; }
        [JsonPropertyName("order_qty"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Quantity { get; set; }
        [JsonPropertyName("limit_price"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Price { get; set; }
        [JsonPropertyName("limit_price_type"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public PriceType? LimitPriceType { get; set; }
        [JsonPropertyName("time_in_force"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(EnumConverter))]
        public TimeInForce? TimeInForce { get; set; }
        [JsonPropertyName("margin"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? Margin { get; set; }
        [JsonPropertyName("post_only"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? PostOnly { get; set; }
        [JsonPropertyName("reduce_only"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? ReduceOnly { get; set; }
        [JsonPropertyName("effective_time"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? EffectiveTime { get; set; }
        [JsonPropertyName("expire_time"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ExpireTime { get; set; }
        [JsonPropertyName("deadline"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Deadline { get; set; }
        [JsonPropertyName("cl_ord_id"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ClientOrderId { get; set; }
        [JsonPropertyName("order_userref"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public uint? UserReference { get; set; }
        [JsonPropertyName("display_qty"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? IcebergQuantity { get; set; }
        [JsonPropertyName("fee_preference"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public FeePreference? FeePreference { get; set; }
        [JsonPropertyName("no_mpp"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? NoMarketPriceProtection { get; set; }
        [JsonPropertyName("stp_type"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public SelfTradePreventionType? SelfTradePreventionType { get; set; }
        [JsonPropertyName("cash_order_qty"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? QuoteQuantity { get; set; }
        [JsonPropertyName("validate"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? ValidateOnly { get; set; }

        [JsonPropertyName("triggers"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public KrakenSocketPlaceOrderRequestV2Trigger? Trigger { get; set; }
        [JsonPropertyName("conditional"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public KrakenSocketPlaceOrderRequestV2Condition? Conditional { get; set; }
    }

    /// <summary>
    /// Order trigger
    /// </summary>
    public record KrakenSocketPlaceOrderRequestV2Trigger
    {
        /// <summary>
        /// Trigger price reference
        /// </summary>
        [JsonPropertyName("reference"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Trigger? Reference { get; set; }
        /// <summary>
        /// Trigger price
        /// </summary>
        [JsonPropertyName("price"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Price { get; set; }
        /// <summary>
        /// Trigger price type
        /// </summary>
        [JsonPropertyName("price_type"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public PriceType? LimitPriceType { get; set; }
    }

    /// <summary>
    /// Order condition
    /// </summary>
    public record KrakenSocketPlaceOrderRequestV2Condition
    {
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("order_type"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public OrderType OrderType { get; set; }
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
        /// Trigger rpice
        /// </summary>
        [JsonPropertyName("trigger_price"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// Trigger price type
        /// </summary>
        [JsonPropertyName("trigger_price_type"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public PriceType? TriggerPriceType { get; set; }
    }
}
