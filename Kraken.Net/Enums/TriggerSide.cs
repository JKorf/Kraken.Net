using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Trigger side
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TriggerSide>))]
    public enum TriggerSide
    {
        /// <summary>
        /// Trigger when the price is above the trigger rpcei
        /// </summary>
        [Map("TRIGGER_ABOVE")]
        TriggerAbove,
        /// <summary>
        /// Trigger when price is below the trigger price
        /// </summary>
        [Map("TRIGGER_BELOW")]
        TriggerBelow
    }
}
