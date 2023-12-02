using CryptoExchange.Net.Converters;
using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Order to place
    /// </summary>
    public class KrakenOrderRequest
    {
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonProperty("userref", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonProperty("ordertype"), JsonConverter(typeof(OrderTypeConverter))]
        public OrderType OrderType { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonProperty("type"), JsonConverter(typeof(EnumConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("volume"), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Iceberg quantity
        /// </summary>
        [JsonProperty("displayvol", DefaultValueHandling = DefaultValueHandling.Ignore), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal? IcebergQuanty { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonProperty("price", DefaultValueHandling = DefaultValueHandling.Ignore), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal? Price { get; set; }
        /// <summary>
        /// Secondary price
        /// </summary>
        [JsonProperty("price2", DefaultValueHandling = DefaultValueHandling.Ignore), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal? SecondaryPrice { get; set; }
        /// <summary>
        /// Trigger
        /// </summary>
        [JsonProperty("trigger", DefaultValueHandling = DefaultValueHandling.Ignore), JsonConverter(typeof(EnumConverter))]
        public Trigger? Trigger { get; set; }
        /// <summary>
        /// Leverage
        /// </summary>
        [JsonProperty("leverage", DefaultValueHandling = DefaultValueHandling.Ignore), JsonConverter(typeof(DecimalStringWriterConverter))]
        public decimal? Leverage { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        [JsonProperty("reduce_only", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? ReduceOnly { get; set; }
        /// <summary>
        /// Self trade prevention type
        /// </summary>
        [JsonProperty("stptype", DefaultValueHandling = DefaultValueHandling.Ignore), JsonConverter(typeof(EnumConverter))]
        public SelfTradePreventionType? SelfTradePreventionType { get; set; }
        /// <summary>
        /// Order flags
        /// </summary>
        [JsonProperty("oflags", DefaultValueHandling = DefaultValueHandling.Ignore, ItemConverterType = typeof(OrderFlagsConverter))]
        public IEnumerable<OrderFlags>? Flags { get; set; }
        /// <summary>
        /// Time in force
        /// </summary>
        [JsonProperty("timeinforce", DefaultValueHandling = DefaultValueHandling.Ignore), JsonConverter(typeof(EnumConverter))]
        public TimeInForce? TimeInForce { get; set; }
        /// <summary>
        /// Start time
        /// </summary>
        [JsonProperty("starttm", DefaultValueHandling = DefaultValueHandling.Ignore), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// Expire time
        /// </summary>
        [JsonProperty("expiretm", DefaultValueHandling = DefaultValueHandling.Ignore), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? ExpireTime { get; set; }
    }
}
