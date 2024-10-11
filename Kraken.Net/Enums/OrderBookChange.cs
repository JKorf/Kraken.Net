using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Order book event
    /// </summary>
    public enum OrderBookChange
    {
        /// <summary>
        /// Add
        /// </summary>
        [Map("add")]
        Add,
        /// <summary>
        /// Modify
        /// </summary>
        [Map("modify")]
        Modify,
        /// <summary>
        /// Delete
        /// </summary>
        [Map("delete")]
        Delete
    }
}
