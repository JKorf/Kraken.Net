using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Wallet type
    /// </summary>
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
