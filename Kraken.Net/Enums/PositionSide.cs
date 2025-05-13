using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
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
        /// Long position
        /// </summary>
        [Map("long")]
        Long,
        /// <summary>
        /// Short position
        /// </summary>
        [Map("short")]
        Short
    }
}
