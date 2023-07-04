using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Kraken.Net.Objects.Models.Futures
{
    internal class KrakenFuturesOrderBookResult : KrakenFuturesResult<KrakenFuturesOrderBook>
    {
        [JsonProperty("orderBook")]
        public override KrakenFuturesOrderBook Data { get; set; } = null!;
    }

    /// <summary>
    /// Order book
    /// </summary>
    public class KrakenFuturesOrderBook
    {
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
    [JsonConverter(typeof(ArrayConverter))]
    public class KrakenFuturesOrderBookEntry
    {
        /// <summary>
        /// Price
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        /// <summary>
        /// Quantiy
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
    }
}
