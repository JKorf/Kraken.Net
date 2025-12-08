using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Timestamp to use for searching
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SearchTime>))]
    public enum SearchTime
    {
        /// <summary>
        /// Open time
        /// </summary>
        [Map("open")]
        Open,
        /// <summary>
        /// Close time
        /// </summary>
        [Map("close")]
        Close,
        /// <summary>
        /// Both open and close time
        /// </summary>
        [Map("both")]
        Both
    }
}
