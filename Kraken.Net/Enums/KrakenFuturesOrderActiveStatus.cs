using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Order active status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<KrakenFuturesOrderActiveStatus>))]
    public enum KrakenFuturesOrderActiveStatus
    {
        /// <summary>
        /// Entered the order book
        /// </summary>
        [Map("ENTERED_BOOK")]
        EnteredBook,
        /// <summary>
        /// Fully executed
        /// </summary>
        [Map("FULLY_EXECUTED")]
        FullyExecuted,
        /// <summary>
        /// Rejected
        /// </summary>
        [Map("REJECTED")]
        Rejected,
        /// <summary>
        /// Cancelled
        /// </summary>
        [Map("CANCELLED")]
        Cancelled,
        /// <summary>
        /// Trigger placed
        /// </summary>
        [Map("TRIGGER_PLACED")]
        TriggerPlaced,
        /// <summary>
        /// Failed to activate trigger
        /// </summary>
        [Map("TRIGGER_ACTIVATION_FAILURE")]
        TriggerActivationFailure
    }
}
