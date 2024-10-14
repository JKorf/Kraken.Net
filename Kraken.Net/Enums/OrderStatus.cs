using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Status of an order
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Pending
        /// </summary>
        [Map("pending")]
        Pending,
        /// <summary>
        /// Active, not (fully) filled
        /// </summary>
        [Map("open")]
        Open,
        /// <summary>
        /// Fully filled
        /// </summary>
        [Map("closed")]
        Closed,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("canceled")]
        Canceled,
        /// <summary>
        /// Expired
        /// </summary>
        [Map("expired")]
        Expired
    }
}
