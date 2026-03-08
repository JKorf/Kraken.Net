using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Book update
    /// </summary>
    [SerializationModel]
    public record KrakenIndividualBookUpdate
    {
        /// <summary>
        /// ["<c>symbol</c>"] The symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>asks</c>"] Asks in the book
        /// </summary>
        [JsonPropertyName("asks")]
        public KrakenIndividualBookUpdateEntry[] Asks { get; set; } = Array.Empty<KrakenIndividualBookUpdateEntry>();
        /// <summary>
        /// ["<c>bids</c>"] Bids in the book
        /// </summary>
        [JsonPropertyName("bids")]
        public KrakenIndividualBookUpdateEntry[] Bids { get; set; } = Array.Empty<KrakenIndividualBookUpdateEntry>();
    }

    /// <summary>
    /// Book order entry
    /// </summary>
    [SerializationModel]
    public record KrakenIndividualBookUpdateEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// ["<c>order_id</c>"] The order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>limit_price</c>"] Price
        /// </summary>
        [JsonPropertyName("limit_price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>order_qty</c>"] Quantity
        /// </summary>
        [JsonPropertyName("order_qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>event</c>"] Event
        /// </summary>
        [JsonPropertyName("event")]
        public OrderBookChange Event { get; set; }
    }
}
