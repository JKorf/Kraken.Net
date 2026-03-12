using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Order type, limited to market or limit
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderTypeMinimal>))]
    public enum OrderTypeMinimal
    {
        /// <summary>
        /// ["<c>l</c>"] Limit order
        /// </summary>
        [Map("l")]
        Limit,
        /// <summary>
        /// ["<c>m</c>"] Symbol order
        /// </summary>
        [Map("m")]
        Market
    }
}
