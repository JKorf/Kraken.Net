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
        /// ["<c>open</c>"] Open time
        /// </summary>
        [Map("open")]
        Open,
        /// <summary>
        /// ["<c>close</c>"] Close time
        /// </summary>
        [Map("close")]
        Close,
        /// <summary>
        /// ["<c>both</c>"] Both open and close time
        /// </summary>
        [Map("both")]
        Both
    }
}
