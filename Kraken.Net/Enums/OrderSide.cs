using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Side of an order
    /// </summary>
    public enum OrderSide
    {
        /// <summary>
        /// Buy
        /// </summary>
        [Map("buy", "0", "b")]
        Buy,
        /// <summary>
        /// Sell
        /// </summary>
        [Map("sell", "1", "s")]
        Sell
    }
}
