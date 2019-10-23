using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Order book
    /// </summary>
    public class KrakenOrderBook
    {
        /// <summary>
        /// Asks in the book
        /// </summary>
        public IEnumerable<KrakenOrderBookEntry> Asks { get; set; } = new List<KrakenOrderBookEntry>();
        /// <summary>
        /// Bids in the book
        /// </summary>
        public IEnumerable<KrakenOrderBookEntry> Bids { get; set; } = new List<KrakenOrderBookEntry>();
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class KrakenOrderBookEntry: ISymbolOrderBookEntry
    {
        /// <summary>
        /// Price of the entry
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity of the entry
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Timestamp of change
        /// </summary>
        [ArrayProperty(2), JsonConverter(typeof(TimestampSecondsConverter))]
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
        public IEnumerable<KrakenStreamOrderBookEntry> Asks { get; set; }
        /// <summary>
        /// Bids
        /// </summary>
        [JsonProperty("bs")]
        public IEnumerable<KrakenStreamOrderBookEntry> Bids { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public KrakenStreamOrderBook()
        {
            Asks = new List<KrakenStreamOrderBookEntry>();
            Bids = new List<KrakenStreamOrderBookEntry>();
        }
    }

    /// <summary>
    /// Stream order book entry
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class KrakenStreamOrderBookEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// Price of the entry
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantity of the entry
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Timestamp of the entry
        /// </summary>
        [ArrayProperty(2), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Type of update
        /// </summary>
        [ArrayProperty(3)]
        public string UpdateType { get; set; } = "";
    }
}
