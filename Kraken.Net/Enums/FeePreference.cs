using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Fee preference
    /// </summary>
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
