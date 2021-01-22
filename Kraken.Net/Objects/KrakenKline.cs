using System;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Kline data
    /// </summary>
    [JsonConverter(typeof(ArrayConverter))]
    public class KrakenKline: ICommonKline
    {
        /// <summary>
        /// Timestamp of the kline
        /// </summary>
        [ArrayProperty(0), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// The open price for this kline
        /// </summary>
        [ArrayProperty(1)]
        public decimal Open { get; set; }
        /// <summary>
        /// The highest price during this kline
        /// </summary>
        [ArrayProperty(2)]
        public decimal High { get; set; }
        /// <summary>
        /// The lowest price during this kline
        /// </summary>
        [ArrayProperty(3)]
        public decimal Low { get; set; }
        /// <summary>
        /// The close price of this kline (or price of last trade if kline isn't closed yet)
        /// </summary>
        [ArrayProperty(4)]
        public decimal Close { get; set; }
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

        decimal ICommonKline.CommonHigh => High;
        decimal ICommonKline.CommonLow => Low;
        decimal ICommonKline.CommonOpen => Open;
        decimal ICommonKline.CommonClose => Close;
        decimal ICommonKline.CommonVolume => Volume;
        DateTime ICommonKline.CommonOpenTime => Timestamp;
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
        [ArrayProperty(0), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// The end time for the kline
        /// </summary>
        [ArrayProperty(1), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime EndTimestamp { get; set; }
        /// <summary>
        /// The open price for this kline
        /// </summary>
        [ArrayProperty(2)]
        public decimal Open { get; set; }
        /// <summary>
        /// The highest price during this kline
        /// </summary>
        [ArrayProperty(3)]
        public decimal High { get; set; }
        /// <summary>
        /// The lowest price during this kline
        /// </summary>
        [ArrayProperty(4)]
        public decimal Low { get; set; }
        /// <summary>
        /// The close price of this kline (or price of last trade if kline isn't closed yet)
        /// </summary>
        [ArrayProperty(5)]
        public decimal Close { get; set; }
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
