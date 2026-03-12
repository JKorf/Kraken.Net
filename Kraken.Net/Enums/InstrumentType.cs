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
        /// ["<c>flexible_futures</c>"] Flexible futures
        /// </summary>
        [Map("flexible_futures")]
        FlexibleFutures,
        /// <summary>
        /// ["<c>futures_inverse</c>"] Inverse futures
        /// </summary>
        [Map("futures_inverse")]
        InverseFutures,
        /// <summary>
        /// ["<c>futures_vanilla</c>"] Vanilla futures
        /// </summary>
        [Map("futures_vanilla")]
        VanillaFutures,
        /// <summary>
        /// ["<c>spot index</c>"] Spot index
        /// </summary>
        [Map("spot index")]
        SpotIndex
    }
}
