using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Internal
{
    internal class KrakenSocketEditOrderRequest : KrakenSocketAuthRequestV2
    {
        [JsonPropertyName("order_id")]
        public string? OrderId { get; set; }
        [JsonPropertyName("cl_ord_id")]
        public string? ClientOrderId { get; set; }
        [JsonPropertyName("order_qty"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Quantity { get; set; }
        [JsonPropertyName("limit_price"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Price { get; set; }
        [JsonPropertyName("limit_price_type"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public PriceType? LimitPriceType { get; set; }
        [JsonPropertyName("display_qty"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? IcebergQuantity { get; set; }
        [JsonPropertyName("post_only"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? PostOnly { get; set; }
        [JsonPropertyName("trigger_price"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? TriggerPrice { get; set; }
        [JsonPropertyName("trigger_price_type"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public PriceType? TriggerPriceType { get; set; }
        [JsonPropertyName("deadline"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Deadline { get; set; }
    }
}
