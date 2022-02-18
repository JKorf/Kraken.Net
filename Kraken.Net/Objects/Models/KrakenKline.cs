using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Kline data
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class KrakenKline
    {
        /// <summary>
        /// Timestamp of the kline
        /// </summary>
        [ArrayProperty(0), JsonConverter(typeof(DateTimeConverter))]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// The open price for this kline
        /// </summary>
        [ArrayProperty(1)]
        [JsonProperty("open")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// The highest price during this kline
        /// </summary>
        [ArrayProperty(2)]
        [JsonProperty("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// The lowest price during this kline
        /// </summary>
        [ArrayProperty(3)]
        [JsonProperty("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// The close price of this kline (or price of last trade if kline isn't closed yet)
        /// </summary>
        [ArrayProperty(4)]
        [JsonProperty("close")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// The volume weighted average price
        /// </summary>
        [ArrayProperty(5)]
        public decimal VolumeWeightedAveragePrice { get; set; }
        /// <summary>
        /// Volume during this kline
        /// </summary>
        [ArrayProperty(6)]
        public decimal Volume { get; set; }
        /// <summary>
        /// The number of trades during this kline
        /// </summary>
        [ArrayProperty(7)]
        public int TradeCount { get; set; }
    }

    /// <summary>
    /// Kline data from stream
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class KrakenStreamKline
    {
        /// <summary>
        /// Timestamp of the kline
        /// </summary>
        [ArrayProperty(0), JsonConverter(typeof(DateTimeConverter))]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// The end time for the kline
        /// </summary>
        [ArrayProperty(1), JsonConverter(typeof(DateTimeConverter))]
        public DateTime CloseTime { get; set; }
        /// <summary>
        /// The open price for this kline
        /// </summary>
        [ArrayProperty(2)]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// The highest price during this kline
        /// </summary>
        [ArrayProperty(3)]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// The lowest price during this kline
        /// </summary>
        [ArrayProperty(4)]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// The close price of this kline (or price of last trade if kline isn't closed yet)
        /// </summary>
        [ArrayProperty(5)]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// The volume weighted average price
        /// </summary>
        [ArrayProperty(6)]
        public decimal VolumeWeightedAveragePrice { get; set; }
        /// <summary>
        /// Volume during this kline
        /// </summary>
        [ArrayProperty(7)]
        public decimal Volume { get; set; }
        /// <summary>
        /// The number of trades during this kline
        /// </summary>
        [ArrayProperty(8)]
        public int TradeCount { get; set; }
    }
}
