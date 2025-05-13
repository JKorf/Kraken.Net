using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
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
