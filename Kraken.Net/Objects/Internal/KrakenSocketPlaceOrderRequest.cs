using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Internal
{
    /// <summary>
    /// Place order request
    /// </summary>
    internal class KrakenSocketPlaceOrderRequest: KrakenSocketAuthRequest
    {
        [JsonPropertyName("pair")]
        public string Symbol { get; set; } = string.Empty;
        [JsonPropertyName("ordertype")]
        [JsonConverter(typeof(EnumConverter))]
        public OrderType OrderType { get; set; }
        [JsonPropertyName("type")]
        [JsonConverter(typeof(EnumConverter))]
        public OrderSide Type { get; set; }
        [JsonPropertyName("leverage"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Leverage { get; set; }
        [JsonPropertyName("volume"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Volume { get; set; }
        [JsonPropertyName("userref"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public uint? ClientOrderId { get; set; }
        [JsonPropertyName("price"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Price { get; set; }
        [JsonPropertyName("price2"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? SecondaryPrice { get; set; }
        [JsonPropertyName("starttm"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? StartTime { get; set; }
        [JsonPropertyName("expiretm"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ExpireTime { get; set; }
        [JsonPropertyName("validate"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? ValidateOnly { get; set; }
        [JsonPropertyName("close[ordertype]"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(EnumConverter))]
        public OrderType? CloseOrderType { get; set; }
        [JsonPropertyName("close[price]"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ClosePrice { get; set; }
        [JsonPropertyName("close[price2]"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? SecondaryClosePrice { get; set; }
        [JsonPropertyName("oflags"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Flags { get; set; }
        [JsonPropertyName("reduce_only"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? ReduceOnly { get; set; }
        [JsonPropertyName("margin"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? Margin { get; set; }
    }
}
