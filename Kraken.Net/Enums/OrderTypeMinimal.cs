using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Order type, limited to market or limit
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderTypeMinimal>))]
    public enum OrderTypeMinimal
    {
        /// <summary>
        /// Limit order
        /// </summary>
        [Map("l")]
        Limit,
        /// <summary>
        /// Symbol order
        /// </summary>
        [Map("m")]
        Market
    }
}
