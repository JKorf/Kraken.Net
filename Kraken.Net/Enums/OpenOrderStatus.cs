using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Status of an open order
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OpenOrderStatus>))]
    public enum OpenOrderStatus
    {
        /// <summary>
        /// The entire size of the order is unfilled
        /// </summary>
        [Map("untouched")]
        Untouched,
        /// <summary>
        /// The size of the order is partially but not entirely filled
        /// </summary>
        [Map("partiallyFilled")]
        PartiallyFilled
    }
}
