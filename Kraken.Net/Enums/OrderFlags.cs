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
        /// ["<c>post</c>"] Post only order (only availalbe for limit orders)
        /// </summary>
        [Map("post")]
        PostOnly,
        /// <summary>
        /// ["<c>fcib</c>"] Prefer fee in base asset (fcib)
        /// </summary>
        [Map("fcib")]
        FeeCalculationInBaseAsset,
        /// <summary>
        /// ["<c>fciq</c>"] Prefer fee in quote asset (fciq)
        /// </summary>
        [Map("fciq")]
        FeeCalculationInQuoteAsset,
        /// <summary>
        /// ["<c>nompp</c>"] Disable market price protection (nompp)
        /// </summary>
        [Map("nompp")]
        NoMarketPriceProtection,
        /// <summary>
        /// ["<c>viqc</c>"] Order volume expressed in quote asset. This is supported only for market orders (viqc)
        /// </summary>
        [Map("viqc")]
        OrderVolumeExpressedInQuoteAsset
    }
}
