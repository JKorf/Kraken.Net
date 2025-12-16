using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Type of symbol
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolType>))]
    public enum SymbolType
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
        VanillaFutures,
        /// <summary>
        /// Spot index
        /// </summary>
        [Map("spot index")]
        SpotIndex
    }
}
