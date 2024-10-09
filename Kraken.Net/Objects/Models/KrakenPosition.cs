using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Position info
    /// </summary>
    public record KrakenPosition
    {
        /// <summary>
        /// The position id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("ordertxid")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("posstatus")]
        public string Status { get; set; } = string.Empty;
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("pair")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Rollover time
        /// </summary>
        [JsonPropertyName("rollovertm"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime? RollOverTime { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("type"), JsonConverter(typeof(EnumConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        [JsonPropertyName("ordertype"), JsonConverter(typeof(EnumConverter))]
        public OrderType Type { get; set; }
        /// <summary>
        /// Cost
        /// </summary>
        [JsonPropertyName("cost")]
        public decimal Cost { get; set; }
        /// <summary>
        /// Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("vol")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Closed quantity 
        /// </summary>
        [JsonPropertyName("vol_closed")]
        public decimal QuantityClosed { get; set; }
        /// <summary>
        /// Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal Margin { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        [JsonPropertyName("value")]
        public decimal? Value { get; set; }
        /// <summary>
        /// Net profit/loss
        /// </summary>
        [JsonPropertyName("net")]
        public decimal? ProfitLoss { get; set; }
        /// <summary>
        /// Misc info
        /// </summary>
        [JsonPropertyName("misc")]
        public string Misc { get; set; } = string.Empty;
        /// <summary>
        /// Flags
        /// </summary>
        [JsonPropertyName("oflags")]
        public string OFlags { get; set; } = string.Empty;
        /// <summary>
        /// Terms
        /// </summary>
        [JsonPropertyName("terms")]
        public string Terms { get; set; } = string.Empty;
    }
}
