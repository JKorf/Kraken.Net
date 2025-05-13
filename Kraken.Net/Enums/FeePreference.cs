using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Fee preference
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FeePreference>))]
    public enum FeePreference
    {
        /// <summary>
        /// In base asset, default for buy orders
        /// </summary>
        [Map("base")]
        Static,
        /// <summary>
        /// In quote asset, default for sell orders
        /// </summary>
        [Map("quote")]
        Quote
    }
}
