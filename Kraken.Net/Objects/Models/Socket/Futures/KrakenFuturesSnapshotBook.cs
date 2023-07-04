using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Interfaces;
using Kraken.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Socket.Futures
{
    /// <summary>
    /// Snapshot book update
    /// </summary>
    public class KrakenFuturesSnapshotBook : KrakenFuturesUpdateMessage
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Sequence number
        /// </summary>
        [JsonProperty("seq")]
        public long Sequence { get; set; }

        /// <summary>
        /// List of asks
        /// </summary>
        public IEnumerable<KrakenFuturesOrderBookEntry> Asks { get; set; } = Array.Empty<KrakenFuturesOrderBookEntry>();
        /// <summary>
        /// List of bids
        /// </summary>
        public IEnumerable<KrakenFuturesOrderBookEntry> Bids { get; set; } = Array.Empty<KrakenFuturesOrderBookEntry>();
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    public class KrakenFuturesOrderBookEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Book update
    /// </summary>
    public class KrakenFuturesBookUpdate : ISymbolOrderBookEntry
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Sequence number
        /// </summary>
        [JsonProperty("seq")]
        public long Sequence { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        public OrderSide Side { get; set; }
        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty("qty")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }
    }
}
