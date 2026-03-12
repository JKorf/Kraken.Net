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
        /// ["<c>base</c>"] In base asset, default for buy orders
        /// </summary>
        [Map("base")]
        Static,
        /// <summary>
        /// ["<c>quote</c>"] In quote asset, default for sell orders
        /// </summary>
        [Map("quote")]
        Quote
    }
}
