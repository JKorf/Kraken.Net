using Kraken.Net.Enums;
using Kraken.Net.Interfaces;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Order info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesCachedOrder : IKrakenFuturesOrder
    {
        /// <summary>
        /// ["<c>cliOrdId</c>"] Client order id
        /// </summary>
        [JsonPropertyName("cliOrdId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>filled</c>"] Quantity filled
        /// </summary>
        [JsonPropertyName("filled")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("lastUpdateTimestamp")]
        public DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// ["<c>limitPrice</c>"] Price
        /// </summary>
        [JsonPropertyName("limitPrice")]
        public decimal? Price { get; set; }
        /// <summary>
        /// ["<c>orderId</c>"] Order id
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>quantity</c>"] Quantity
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Quantity remaining
        /// </summary>
        public decimal QuantityRemaining
        {
            get { return Quantity - QuantityFilled; }
            set { }
        }
        /// <summary>
        /// ["<c>reduceOnly</c>"] Reduce only
        /// </summary>
        [JsonPropertyName("reduceOnly")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// ["<c>side</c>"] Order side
        /// </summary>

        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>symbol</c>"] Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// ["<c>type</c>"] Trigger order type
        /// </summary>

        [JsonPropertyName("type")]
        public TriggerOrderType Type { get; set; }
        /// <summary>
        /// ["<c>priceTriggerOptions</c>"] Trigger options
        /// </summary>
        [JsonPropertyName("priceTriggerOptions")]
        public KrakenTriggerOptions? PriceTriggerOptions { get; set; }
        /// <summary>
        /// Trigger timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("triggerTime")]
        public DateTime? TriggerTime { get; set; }
    }
}
