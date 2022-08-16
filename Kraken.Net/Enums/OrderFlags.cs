using System;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Flags for an order
    /// </summary>
    public enum OrderFlags
    {
        /// <summary>
        /// Post only order (only availalbe for limit orders)
        /// </summary>
        PostOnly,
        /// <summary>
        /// Prefer fee in base asset (fcib)
        /// </summary>
        FeeCalculationInBaseAsset,
        /// <summary>
        /// Prefer fee in quote asset (fciq)
        /// </summary>
        FeeCalculationInQuoteAsset,
        /// <summary>
        /// Disable market price protection (nompp)
        /// </summary>
        NoMarketPriceProtection,
        /// <summary>
        /// Order volume expressed in quote asset. This is supported only for market orders (viqc)
        /// </summary>
        OrderVolumeExpressedInQuoteAsset
    }
}