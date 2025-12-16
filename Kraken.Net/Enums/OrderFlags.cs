using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Flags for an order
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderFlags>))]
    public enum OrderFlags
    {
        /// <summary>
        /// Post only order (only availalbe for limit orders)
        /// </summary>
        [Map("post")]
        PostOnly,
        /// <summary>
        /// Prefer fee in base asset (fcib)
        /// </summary>
        [Map("fcib")]
        FeeCalculationInBaseAsset,
        /// <summary>
        /// Prefer fee in quote asset (fciq)
        /// </summary>
        [Map("fciq")]
        FeeCalculationInQuoteAsset,
        /// <summary>
        /// Disable market price protection (nompp)
        /// </summary>
        [Map("nompp")]
        NoMarketPriceProtection,
        /// <summary>
        /// Order volume expressed in quote asset. This is supported only for market orders (viqc)
        /// </summary>
        [Map("viqc")]
        OrderVolumeExpressedInQuoteAsset
    }
}
