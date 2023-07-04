using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Type of instrument
    /// </summary>
    public enum InstrumentType
    {
        /// <summary>
        /// Flexible futures
        /// </summary>
        [Map("flexible_futures")]
        FlexibleFutures,
        /// <summary>
        /// Inverse futures
        /// </summary>
        [Map("futures_inverse")]
        InverseFutures,
        /// <summary>
        /// Vanilla futures
        /// </summary>
        [Map("futures_vanilla")]
        ValillaFutures,
        /// <summary>
        /// Spot index
        /// </summary>
        [Map("spot index")]
        SpotIndex
    }
}
