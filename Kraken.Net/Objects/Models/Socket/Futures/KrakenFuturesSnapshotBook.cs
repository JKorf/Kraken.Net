using CryptoExchange.Net.Converters.SystemTextJson;
using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Snapshot book update
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesBookSnapshotUpdate : KrakenFuturesUpdateMessage
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Sequence number
        /// </summary>
        [JsonPropertyName("seq")]
        public long Sequence { get; set; }

        /// <summary>
        /// List of asks
        /// </summary>
        [JsonPropertyName("asks")]
        public KrakenFuturesStreamOrderBookEntry[] Asks { get; set; } = Array.Empty<KrakenFuturesStreamOrderBookEntry>();
        /// <summary>
        /// List of bids
        /// </summary>
        [JsonPropertyName("bids")]
        public KrakenFuturesStreamOrderBookEntry[] Bids { get; set; } = Array.Empty<KrakenFuturesStreamOrderBookEntry>();
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesStreamOrderBookEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Book update
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesBookUpdate : KrakenFuturesUpdateMessage, ISymbolOrderBookEntry
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Sequence number
        /// </summary>
        [JsonPropertyName("seq")]
        public long Sequence { get; set; }
        /// <summary>
        /// Side
        /// </summary>

        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
