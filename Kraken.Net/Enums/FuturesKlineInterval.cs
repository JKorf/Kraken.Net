using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Futures kline interval
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FuturesKlineInterval>))]
    public enum FuturesKlineInterval
    {
        /// <summary>
        /// 1 minute
        /// </summary>
        [Map("1m")]
        OneMinute = 60,
        /// <summary>
        /// 5 minutes
        /// </summary>
        [Map("5m")]
        FiveMinutes = 60 * 5,
        /// <summary>
        /// 15 minutes
        /// </summary>
        [Map("15m")]
        FifteenMinutes = 60 * 15,
        /// <summary>
        /// 30 minutes
        /// </summary>
        [Map("30m")]
        ThirtyMinutes = 60 * 30,
        /// <summary>
        /// 1 hour
        /// </summary>
        [Map("1h")]
        OneHour = 60 * 60,
        /// <summary>
        /// 4 hours
        /// </summary>
        [Map("4h")]
        FourHours = 60 * 60 * 4,
        /// <summary>
        /// 12 hours
        /// </summary>
        [Map("12h")]
        TwelfHours = 60 * 60 * 12,
        /// <summary>
        /// 1 day
        /// </summary>
        [Map("1d")]
        OneDay = 60 * 60 * 24,
        /// <summary>
        /// 1 week
        /// </summary>
        [Map("1w")]
        OneWeek = 60 * 60 * 24 * 7
    }
}
