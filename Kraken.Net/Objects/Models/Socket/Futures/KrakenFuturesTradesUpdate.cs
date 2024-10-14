using Kraken.Net.Enums;
using Kraken.Net.Objects.Sockets;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Trades update
    /// </summary>
    public record KrakenFuturesTradesSnapshotUpdate : KrakenFuturesEvent
    {
        /// <summary>
        /// Trades
        /// </summary>
        [JsonPropertyName("trades")]
        public IEnumerable<KrakenFuturesTradeUpdate> Trades { get; set; } = Array.Empty<KrakenFuturesTradeUpdate>();
    }

    /// <summary>
    /// Trade info
    /// </summary>
    public record KrakenFuturesTradeUpdate : KrakenFuturesUpdateMessage
    {
        /// <summary>
        /// Uid
        /// </summary>
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = string.Empty;
        /// <summary>
        /// Order side
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Trade type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// Sequence number
        /// </summary>
        [JsonPropertyName("seq")]
        public long Sequence { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
