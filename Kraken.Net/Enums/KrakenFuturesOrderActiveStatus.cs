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
        /// ["<c>ENTERED_BOOK</c>"] Entered the order book
        /// </summary>
        [Map("ENTERED_BOOK")]
        EnteredBook,
        /// <summary>
        /// ["<c>FULLY_EXECUTED</c>"] Fully executed
        /// </summary>
        [Map("FULLY_EXECUTED")]
        FullyExecuted,
        /// <summary>
        /// ["<c>REJECTED</c>"] Rejected
        /// </summary>
        [Map("REJECTED")]
        Rejected,
        /// <summary>
        /// ["<c>CANCELLED</c>"] Cancelled
        /// </summary>
        [Map("CANCELLED")]
        Cancelled,
        /// <summary>
        /// ["<c>TRIGGER_PLACED</c>"] Trigger placed
        /// </summary>
        [Map("TRIGGER_PLACED")]
        TriggerPlaced,
        /// <summary>
        /// ["<c>TRIGGER_ACTIVATION_FAILURE</c>"] Failed to activate trigger
        /// </summary>
        [Map("TRIGGER_ACTIVATION_FAILURE")]
        TriggerActivationFailure
    }
}
