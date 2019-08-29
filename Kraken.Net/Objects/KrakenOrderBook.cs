using System;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.OrderBook;
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
        public KrakenOrderBookEntry[] Asks { get; set; }
        /// <summary>
        /// Bids in the book
        /// </summary>
        public KrakenOrderBookEntry[] Bids { get; set; }
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
        public KrakenStreamOrderBookEntry[] Asks { get; set; }
        /// <summary>
        /// Bids
        /// </summary>
        [JsonProperty("bs")]
        public KrakenStreamOrderBookEntry[] Bids { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public KrakenStreamOrderBook()
        {
            Asks = new KrakenStreamOrderBookEntry[0];
            Bids = new KrakenStreamOrderBookEntry[0];
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
        public string UpdateType { get; set; }
    }
}
