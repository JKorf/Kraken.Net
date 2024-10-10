using Kraken.Net.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Book update
    /// </summary>
    public record KrakenIndividualBookUpdate
    {
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Asks in the book
        /// </summary>
        [JsonPropertyName("asks")]
        public IEnumerable<KrakenIndividualBookUpdateEntry> Asks { get; set; } = Array.Empty<KrakenIndividualBookUpdateEntry>();
        /// <summary>
        /// Bids in the book
        /// </summary>
        [JsonPropertyName("bids")]
        public IEnumerable<KrakenIndividualBookUpdateEntry> Bids { get; set; } = Array.Empty<KrakenIndividualBookUpdateEntry>();
    }

    /// <summary>
    /// Book order entry
    /// </summary>
    public record KrakenIndividualBookUpdateEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// The order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("limit_price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonPropertyName("order_qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Event
        /// </summary>
        [JsonPropertyName("event")]
        public OrderBookChange Event { get; set; }
    }
}
