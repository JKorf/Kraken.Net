using System;
using CryptoExchange.Net.Converters;
using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Internal
{
    /// <summary>
    /// Place order request
    /// </summary>
    internal class KrakenSocketPlaceOrderRequest: KrakenSocketRequestBase
    {
        [JsonProperty("pair")]
        public string Symbol { get; set; } = string.Empty;
        [JsonProperty("ordertype")]
        [JsonConverter(typeof(OrderTypeConverter))]
        public OrderType OrderType { get; set; }
        [JsonProperty("type")]
        [JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Type { get; set; }
        [JsonProperty("leverage")]
        public string? Leverage { get; set; }
        [JsonProperty("volume")]
        public string? Volume { get; set; }
        [JsonProperty("userref")]
        public string? ClientOrderId { get; set; }
        [JsonProperty("price")]
        public string? Price { get; set; }
        [JsonProperty("price2")]
        public string? SecondaryPrice { get; set; }
        [JsonProperty("starttm")]
        public string? StartTime { get; set; }
        [JsonProperty("expiretm")]
        public string? ExpireTime { get; set; }
        [JsonProperty("validate")]
        public bool? ValidateOnly { get; set; }
        [JsonProperty("close[ordertype]")]
        [JsonConverter(typeof(OrderTypeConverter))]
        public OrderType? CloseOrderType { get; set; }
        [JsonProperty("close[price]")]
        public string? ClosePrice { get; set; }
        [JsonProperty("close[price2]")]
        public string? SecondaryClosePrice { get; set; }
        [JsonProperty("oflags")]
        public string? Flags { get; set; }
        [JsonProperty("reduce_only")]
        public bool? ReduceOnly { get; set; }
    }
}
