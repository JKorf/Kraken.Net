using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Wallet type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<WalletType>))]
    public enum WalletType
    {
        /// <summary>
        /// Spot
        /// </summary>
        [Map("spot")]
        Spot,
        /// <summary>
        /// Earn
        /// </summary>
        [Map("earn")]
        Earn
    }
}
