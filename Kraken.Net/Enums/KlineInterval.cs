using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// The time interval of kline data, the int value respresents the time in seconds
    /// </summary>
    [JsonConverter(typeof(EnumConverter<KlineInterval>))]
    public enum KlineInterval
    {
        /// <summary>
        /// ["<c>1</c>"] 1m
        /// </summary>
        [Map("1")]
        OneMinute = 60,
        /// <summary>
        /// ["<c>5</c>"] 5m
        /// </summary>
        [Map("5")]
        FiveMinutes = 60 * 5,
        /// <summary>
        /// ["<c>15</c>"] 15m
        /// </summary>
        [Map("15")]
        FifteenMinutes = 60 * 15,
        /// <summary>
        /// ["<c>30</c>"] 30m
        /// </summary>
        [Map("30")]
        ThirtyMinutes = 60 * 30,
        /// <summary>
        /// ["<c>60</c>"] 1h
        /// </summary>
        [Map("60")]
        OneHour = 60 * 60,
        /// <summary>
        /// ["<c>240</c>"] 4h
        /// </summary>
        [Map("240")]
        FourHour = 60 * 60 * 4,
        /// <summary>
        /// ["<c>1440</c>"] 1d
        /// </summary>
        [Map("1440")]
        OneDay = 60 * 60 * 24,
        /// <summary>
        /// ["<c>10080</c>"] 1w
        /// </summary>
        [Map("10080")]
        OneWeek = 60 * 60 * 24 * 7,
        /// <summary>
        /// ["<c>21600</c>"] 15d
        /// </summary>
        [Map("21600")]
        FifteenDays = 60 * 60 * 24 * 15
    }
}
