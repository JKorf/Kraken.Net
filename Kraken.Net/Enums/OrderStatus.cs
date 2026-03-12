using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Status of an order
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderStatus>))]
    public enum OrderStatus
    {
        /// <summary>
        /// ["<c>pending</c>"] Pending
        /// </summary>
        [Map("pending")]
        Pending,
        /// <summary>
        /// ["<c>open</c>"] Active, not (fully) filled
        /// </summary>
        [Map("open")]
        Open,
        /// <summary>
        /// ["<c>closed</c>"] Fully filled
        /// </summary>
        [Map("closed")]
        Closed,
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
