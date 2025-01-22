using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// The time interval of kline data, the int value respresents the time in seconds
    /// </summary>
    public enum KlineInterval
    {
        /// <summary>
        /// 1m
        /// </summary>
        [Map("1")]
        OneMinute = 60,
        /// <summary>
        /// 5m
        /// </summary>
        [Map("5")]
        FiveMinutes = 60 * 5,
        /// <summary>
        /// 15m
        /// </summary>
        [Map("15")]
        FifteenMinutes = 60 * 15,
        /// <summary>
        /// 30m
        /// </summary>
        [Map("30")]
        ThirtyMinutes = 60 * 30,
        /// <summary>
        /// 1h
        /// </summary>
        [Map("60")]
        OneHour = 60 * 60,
        /// <summary>
        /// 4h
        /// </summary>
        [Map("240")]
        FourHour = 60 * 60 * 4,
        /// <summary>
        /// 1d
        /// </summary>
        [Map("1440")]
        OneDay = 60 * 60 * 24,
        /// <summary>
        /// 1w
        /// </summary>
        [Map("10080")]
        OneWeek = 60 * 60 * 24 * 7,
        /// <summary>
        /// 15d
        /// </summary>
        [Map("21600")]
        FifteenDays = 60 * 60 * 24 * 15
    }
}
