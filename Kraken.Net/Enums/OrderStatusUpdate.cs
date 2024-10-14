using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Status of an order
    /// </summary>
    public enum OrderStatusUpdate
    {
        /// <summary>
        /// Pending
        /// </summary>
        [Map("pending_new")]
        Pending,
        /// <summary>
        /// Active, not filled
        /// </summary>
        [Map("new")]
        New,
        /// <summary>
        /// Active, partially filled
        /// </summary>
        [Map("partially_filled")]
        PartiallyFilled,
        /// <summary>
        /// Fully filled
        /// </summary>
        [Map("filled")]
        Filled,
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
