using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
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
        [ArrayProperty(0), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Best bid price
        /// </summary>
        [ArrayProperty(1)]
        public decimal Bid { get; set; }
        /// <summary>
        /// Best ask price
        /// </summary>
        [ArrayProperty(2)]
        public decimal Ask { get; set; }
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
        public decimal Bid { get; set; }
        /// <summary>
        /// Best ask price
        /// </summary>
        [ArrayProperty(1)]
        public decimal Ask { get; set; }
        /// <summary>
        /// Timestamp of the data
        /// </summary>
        [ArrayProperty(2), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Best bid volume
        /// </summary>
        [ArrayProperty(3)]
        public decimal BidVolume { get; set; }
        /// <summary>
        /// Best ask volume
        /// </summary>
        [ArrayProperty(4)]
        public decimal AskVolume { get; set; }

    }
}
