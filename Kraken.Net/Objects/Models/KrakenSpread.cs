using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Spread info
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class KrakenSpread
    {
        /// <summary>
        /// Timestamp of the data
        /// </summary>
        [ArrayProperty(0), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Best bid price
        /// </summary>
        [ArrayProperty(1)]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// Best ask price
        /// </summary>
        [ArrayProperty(2)]
        public decimal BestAskPrice { get; set; }
    }

    /// <summary>
    /// Stream spread data
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class KrakenStreamSpread
    {
        /// <summary>
        /// Best bid price
        /// </summary>
        [ArrayProperty(0)]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// Best ask price
        /// </summary>
        [ArrayProperty(1)]
        public decimal BestAskPrice { get; set; }
        /// <summary>
        /// Timestamp of the data
        /// </summary>
        [ArrayProperty(2), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Best bid quantity
        /// </summary>
        [ArrayProperty(3)]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// Best ask quantity
        /// </summary>
        [ArrayProperty(4)]
        public decimal BestAskQuantity { get; set; }

    }
}
