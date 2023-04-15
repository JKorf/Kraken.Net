using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Trigger price type
    /// </summary>
    public enum Trigger
    {
        /// <summary>
        /// Last price
        /// </summary>
        [Map("last")]
        Last,
        /// <summary>
        /// Index price
        /// </summary>
        [Map("index")]
        Index
    }
}
