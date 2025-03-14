using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
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
