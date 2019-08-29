using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Position info
    /// </summary>
    public class KrakenPosition
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty("ordertxid")]
        public string OrderId { get; set; }
        /// <summary>
        /// Market
        /// </summary>
        [JsonProperty("pair")]
        public string Market { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("time"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonProperty("type")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        [JsonProperty("ordertype")]
        public OrderType Type { get; set; }
        /// <summary>
        /// Cost
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("vol")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Closed quantity 
        /// </summary>
        [JsonProperty("vol_closed")]
        public decimal QuantityClosed { get; set; }
        /// <summary>
        /// Margin
        /// </summary>
        public decimal Margin { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        public decimal? Value { get; set; }
        /// <summary>
        /// Net profit/loss
        /// </summary>
        [JsonProperty("net")]
        public decimal? ProfitLoss { get; set; }
        /// <summary>
        /// Misc info
        /// </summary>
        public string Misc { get; set; }
        /// <summary>
        /// Flags
        /// </summary>
        public string OFlags { get; set; }
    }
}
