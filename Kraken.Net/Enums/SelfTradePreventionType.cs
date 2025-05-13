using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Self trade prevention type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SelfTradePreventionType>))]
    public enum SelfTradePreventionType
    {
        /// <summary>
        /// Cancel newest order
        /// </summary>
        [Map("cancel-newest")]
        CancelNewest,
        /// <summary>
        /// Cancel oldest order
        /// </summary>
        [Map("cancel-oldest")]
        CancelOldest,
        /// <summary>
        /// Cancel both orders
        /// </summary>
        [Map("cancel-both")]
        CancelBoth
    }
}
