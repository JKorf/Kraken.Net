using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Whether it is above or below
    /// </summary>
    public enum TrailingStopSign
    {
        /// <summary>
        /// Above
        /// </summary>
        Plus,
        /// <summary>
        /// Below
        /// </summary>
        Minus
    }
}
