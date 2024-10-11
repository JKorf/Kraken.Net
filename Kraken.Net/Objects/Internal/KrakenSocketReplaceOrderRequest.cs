using Kraken.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Internal
{
    internal class KrakenSocketReplaceOrderRequest : KrakenSocketAuthRequestV2
    {
        [JsonPropertyName("deadline"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Deadline { get; set; }
        [JsonPropertyName("display_qty"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? IcebergQuantity { get; set; }
        [JsonPropertyName("fee_preference"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public FeePreference? FeePreference { get; set; }
        [JsonPropertyName("limit_price"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Price { get; set; }
        [JsonPropertyName("no_mpp"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? NoMarketPriceProtection { get; set; }
        [JsonPropertyName("order_id")]
        public string? OrderId { get; set; }
        [JsonPropertyName("order_qty"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Quantity { get; set; }
        [JsonPropertyName("order_userref"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public uint? UserReference { get; set; }
        [JsonPropertyName("post_only"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? PostOnly { get; set; }
        [JsonPropertyName("reduce_only"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? ReduceOnly { get; set; }
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        [JsonPropertyName("validate"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? ValidateOnly { get; set; }
        [JsonPropertyName("triggers"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public KrakenSocketPlaceOrderRequestV2Trigger? Trigger { get; set; }
    }
}
