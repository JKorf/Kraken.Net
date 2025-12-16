using Kraken.Net.Enums;
using Kraken.Net.Interfaces;

namespace Kraken.Net.Objects.Models.Futures
{
    [SerializationModel]
    internal record KrakenFuturesOpenOrderResult : KrakenFuturesResult<KrakenFuturesOpenOrder[]>
    {
        [JsonPropertyName("openOrders")]
        public override KrakenFuturesOpenOrder[] Data { get; set; } = Array.Empty<KrakenFuturesOpenOrder>();
    }

    /// <summary>
    /// Order info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesOpenOrder : IKrakenFuturesOrder
    {
        /// <summary>
        /// Client order id
        /// </summary>
        [JsonPropertyName("cliOrdId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Quantity filled
        /// </summary>
        [JsonPropertyName("filledSize")]
        public decimal QuantityFilled { get; set; }
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("lastUpdateTime")]
        public DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("limitPrice")]
        public decimal? Price { get; set; }
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Quantity
        /// </summary>
        public decimal Quantity
        { 
            get { return QuantityRemaining + QuantityFilled; }
            set { }
        }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("unfilledSize")]
        public decimal QuantityRemaining { get; set; }
        /// <summary>
        /// Reduce only
        /// </summary>
        [JsonPropertyName("reduceOnly")]
        public bool ReduceOnly { get; set; }
        /// <summary>
        /// Order side
        /// </summary>

        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("receivedTime")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Order type
        /// </summary>
        [JsonPropertyName("orderType")]

        public FuturesOrderType Type { get; set; }
        /// <summary>
        /// Order status
        /// </summary>

        [JsonPropertyName("status")]
        public OpenOrderStatus Status { get; set; }
        /// <summary>
        /// Trigger signal
        /// </summary>

        [JsonPropertyName("triggerSignal")]
        public TriggerSignal? TriggerSignal { get; set; }
        /// <summary>
        /// Stop price
        /// </summary>
        [JsonPropertyName("stopPrice")]
        public decimal? StopPrice { get; set; }
    }
}
