using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Order book
    /// </summary>
    public class KrakenOrderBook
    {
        /// <summary>
        /// Asks in the book
        /// </summary>
        public IEnumerable<ISymbolOrderBookEntry> Asks { get; set; } = Array.Empty<KrakenOrderBookEntry>();
        /// <summary>
        /// Bids in the book
        /// </summary>
        public IEnumerable<ISymbolOrderBookEntry> Bids { get; set; } = Array.Empty<KrakenOrderBookEntry>();
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class KrakenOrderBookEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// Price of the entry
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }

        /// <summary>
        /// Price of the entry as a string literal
        /// </summary>
        [ArrayProperty(0)]
        public string RawPrice { get; set; } = string.Empty;
        /// <summary>
        /// Quantity of the entry
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Quantity of the entry as a string literal
        /// </summary>
        [ArrayProperty(1)]
        public string RawQuantity { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp of change
        /// </summary>
        [ArrayProperty(2), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// Stream order book
    /// </summary>
    public class KrakenStreamOrderBook
    {
        /// <summary>
        /// Asks
        /// </summary>
        [JsonProperty("as")]
        public IEnumerable<KrakenStreamOrderBookEntry> Asks { get; set; } = Array.Empty<KrakenStreamOrderBookEntry>();
        /// <summary>
        /// Bids
        /// </summary>
        [JsonProperty("bs")]
        public IEnumerable<KrakenStreamOrderBookEntry> Bids { get; set; } = Array.Empty<KrakenStreamOrderBookEntry>();

        /// <summary>
        /// Checksum
        /// </summary>
        public uint? Checksum { get; set; }
    }

    /// <summary>
    /// Stream order book entry
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class KrakenStreamOrderBookEntry : ISymbolOrderSequencedBookEntry
    {
        /// <summary>
        /// Price of the entry
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }

        /// <summary>
        /// Price of the entry as a string literal
        /// </summary>
        [ArrayProperty(0)]
        public string RawPrice { get; set; } = string.Empty;

        /// <summary>
        /// Quantity of the entry
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Quantity of the entry as a string literal
        /// </summary>
        [ArrayProperty(1)]
        public string RawQuantity { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp of the entry
        /// </summary>
        [ArrayProperty(2), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Type of update
        /// </summary>
        [ArrayProperty(3)]
        public string UpdateType { get; set; } = string.Empty;

        /// <summary>
        /// Sequence
        /// </summary>
        [JsonIgnore]
        public long Sequence
        {
            get => Timestamp.Ticks;
            set => Timestamp = new DateTime(value);
        }
    }
}
