using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Kline info
    /// </summary>
    public class KrakenFuturesKlines
    {
        /// <summary>
        /// True if there are more candles in the time range
        /// </summary>
        [JsonProperty("more_candles")]
        public bool MoreKlines { get; set; }
        /// <summary>
        /// Candles
        /// </summary>
        [JsonProperty("candles")]
        public IEnumerable<KrakenFuturesKline> Klines { get; set; } = Array.Empty<KrakenFuturesKline>();
    }

    /// <summary>
    /// Kline info
    /// </summary>
    public class KrakenFuturesKline
    {
        /// <summary>
        /// High price
        /// </summary>
        [JsonProperty("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Low price
        /// </summary>
        [JsonProperty("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Close price
        /// </summary>
        [JsonProperty("close")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// Open price
        /// </summary>
        [JsonProperty("open")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// Volume
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
    }
}
