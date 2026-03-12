using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Trigger price type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<Trigger>))]
    public enum Trigger
    {
        /// <summary>
        /// ["<c>last</c>"] Last price
        /// </summary>
        [Map("last")]
        Last,
        /// <summary>
        /// ["<c>index</c>"] Index price
        /// </summary>
        [Map("index")]
        Index
    }
}
