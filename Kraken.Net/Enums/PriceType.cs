using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Price type
    /// </summary>
    public enum PriceType
    {
        /// <summary>
        /// Static market price for the asset, i.e. 30000 for BTC/USD.
        /// </summary>
        [Map("static")]
        Static,
        /// <summary>
        /// Percentage offset from the reference price, i.e. -10% from index price.
        /// </summary>
        [Map("pct")]
        Percentage,
        /// <summary>
        /// Notional offset from the reference price in the quote currency, i.e, 150 BTC/USD from last price
        /// </summary>
        [Map("quote")]
        Quote
    }
}
