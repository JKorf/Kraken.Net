using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Status of an order
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderStatusUpdate>))]
    public enum OrderStatusUpdate
    {
        /// <summary>
        /// ["<c>pending_new</c>"] Pending
        /// </summary>
        [Map("pending_new")]
        Pending,
        /// <summary>
        /// ["<c>new</c>"] Active, not filled
        /// </summary>
        [Map("new")]
        New,
        /// <summary>
        /// ["<c>partially_filled</c>"] Active, partially filled
        /// </summary>
        [Map("partially_filled")]
        PartiallyFilled,
        /// <summary>
        /// ["<c>filled</c>"] Fully filled
        /// </summary>
        [Map("filled")]
        Filled,
        /// <summary>
        /// ["<c>canceled</c>"] Canceled
        /// </summary>
        [Map("canceled")]
        Canceled,
        /// <summary>
        /// ["<c>expired</c>"] Expired
        /// </summary>
        [Map("expired")]
        Expired
    }
}
