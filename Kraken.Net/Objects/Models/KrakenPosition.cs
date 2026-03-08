using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Position info
    /// </summary>
    [SerializationModel]
    public record KrakenPosition
    {
        /// <summary>
        /// ["<c>id</c>"] The position id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>ordertxid</c>"] Order id
        /// </summary>
        [JsonPropertyName("ordertxid")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>posstatus</c>"] Status
        /// </summary>
        [JsonPropertyName("posstatus")]
        public string Status { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>pair</c>"] Symbol
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
        /// ["<c>type</c>"] Side
        /// </summary>
        [JsonPropertyName("type")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>ordertype</c>"] Type
        /// </summary>
        [JsonPropertyName("ordertype")]
        public OrderType Type { get; set; }
        /// <summary>
        /// ["<c>cost</c>"] Cost
        /// </summary>
        [JsonPropertyName("cost")]
        public decimal Cost { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>vol</c>"] Quantity
        /// </summary>
        [JsonPropertyName("vol")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>vol_closed</c>"] Closed quantity 
        /// </summary>
        [JsonPropertyName("vol_closed")]
        public decimal QuantityClosed { get; set; }
        /// <summary>
        /// ["<c>margin</c>"] Margin
        /// </summary>
        [JsonPropertyName("margin")]
        public decimal Margin { get; set; }
        /// <summary>
        /// ["<c>value</c>"] Value
        /// </summary>
        [JsonPropertyName("value")]
        public decimal? Value { get; set; }
        /// <summary>
        /// ["<c>net</c>"] Net profit/loss
        /// </summary>
        [JsonPropertyName("net")]
        public decimal? ProfitLoss { get; set; }
        /// <summary>
        /// ["<c>misc</c>"] Misc info
        /// </summary>
        [JsonPropertyName("misc")]
        public string Misc { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>oflags</c>"] Flags
        /// </summary>
        [JsonPropertyName("oflags")]
        public string OFlags { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>terms</c>"] Terms
        /// </summary>
        [JsonPropertyName("terms")]
        public string Terms { get; set; } = string.Empty;
    }
}
