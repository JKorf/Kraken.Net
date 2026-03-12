using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Price type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PriceType>))]
    public enum PriceType
    {
        /// <summary>
        /// ["<c>static</c>"] Static market price for the asset, i.e. 30000 for BTC/USD.
        /// </summary>
        [Map("static")]
        Static,
        /// <summary>
        /// ["<c>pct</c>"] Percentage offset from the reference price, i.e. -10% from index price.
        /// </summary>
        [Map("pct")]
        Percentage,
        /// <summary>
        /// ["<c>quote</c>"] Notional offset from the reference price in the quote currency, i.e, 150 BTC/USD from last price
        /// </summary>
        [Map("quote")]
        Quote
    }
}
