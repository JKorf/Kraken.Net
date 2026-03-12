using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Position side
    /// </summary>
    [JsonConverter(typeof(EnumConverter<PositionSide>))]
    public enum PositionSide
    {
        /// <summary>
        /// ["<c>long</c>"] Long position
        /// </summary>
        [Map("long")]
        Long,
        /// <summary>
        /// ["<c>short</c>"] Short position
        /// </summary>
        [Map("short")]
        Short
    }
}
