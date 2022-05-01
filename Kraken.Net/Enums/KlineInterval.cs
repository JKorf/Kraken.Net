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
        OneMinute = 60,
        /// <summary>
        /// 5m
        /// </summary>
        FiveMinutes = 60 * 5,
        /// <summary>
        /// 15m
        /// </summary>
        FifteenMinutes = 60 * 15,
        /// <summary>
        /// 30m
        /// </summary>
        ThirtyMinutes = 60 * 30,
        /// <summary>
        /// 1h
        /// </summary>
        OneHour = 60 * 60,
        /// <summary>
        /// 4h
        /// </summary>
        FourHour = 60 * 60 * 4,
        /// <summary>
        /// 1d
        /// </summary>
        OneDay = 60 * 60 * 24,
        /// <summary>
        /// 1w
        /// </summary>
        OneWeek = 60 * 60 * 7,
        /// <summary>
        /// 15d
        /// </summary>
        FifteenDays = 60 * 60 * 15
    }
}
