using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Order type, limited to market or limit
    /// </summary>
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
