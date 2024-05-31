using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Internal
{
    /// <summary>
    /// Place order request
    /// </summary>
    internal class KrakenSocketPlaceOrderRequest: KrakenSocketAuthRequest
    {
        [JsonProperty("pair")]
        public string Symbol { get; set; } = string.Empty;
        [JsonProperty("ordertype")]
        [JsonConverter(typeof(OrderTypeConverter))]
        public OrderType OrderType { get; set; }
        [JsonProperty("type")]
        [JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Type { get; set; }
        [JsonProperty("leverage", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Leverage { get; set; }
        [JsonProperty("volume", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Volume { get; set; }
        [JsonProperty("userref", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ClientOrderId { get; set; }
        [JsonProperty("price", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Price { get; set; }
        [JsonProperty("price2", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? SecondaryPrice { get; set; }
        [JsonProperty("starttm", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? StartTime { get; set; }
        [JsonProperty("expiretm", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ExpireTime { get; set; }
        [JsonProperty("validate", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? ValidateOnly { get; set; }
        [JsonProperty("close[ordertype]", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(OrderTypeConverter))]
        public OrderType? CloseOrderType { get; set; }
        [JsonProperty("close[price]", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ClosePrice { get; set; }
        [JsonProperty("close[price2]", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? SecondaryClosePrice { get; set; }
        [JsonProperty("oflags", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Flags { get; set; }
        [JsonProperty("reduce_only", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? ReduceOnly { get; set; }
        [JsonProperty("margin", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? Margin { get; set; }
    }
}
