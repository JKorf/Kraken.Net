using Kraken.Net.Enums;
using Kraken.Net.Objects.Sockets;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Trades update
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesTradesSnapshotUpdate : KrakenFuturesEvent
    {
        /// <summary>
        /// ["<c>trades</c>"] Trades
        /// </summary>
        [JsonPropertyName("trades")]
        public KrakenFuturesTradeUpdate[] Trades { get; set; } = Array.Empty<KrakenFuturesTradeUpdate>();
    }

    /// <summary>
    /// Trade info
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesTradeUpdate : KrakenFuturesUpdateMessage
    {
        /// <summary>
        /// ["<c>uid</c>"] Uid
        /// </summary>
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>side</c>"] Order side
        /// </summary>

        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Trade type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>seq</c>"] Sequence number
        /// </summary>
        [JsonPropertyName("seq")]
        public long Sequence { get; set; }
        /// <summary>
        /// ["<c>time</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>qty</c>"] Quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
