using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Trigger options
    /// </summary>
    [SerializationModel]
    public record KrakenTriggerOptions
    {
        /// <summary>
        /// Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public decimal TriggerPrice { get; set; }
        /// <summary>
        /// Trigger side
        /// </summary>

        [JsonPropertyName("triggerSide")]
        public TriggerSide TriggerSide { get; set; }
        /// <summary>
        /// Trigger signal
        /// </summary>

        [JsonPropertyName("triggerSignal")]
        public TriggerSignal TriggerSignal { get; set; }
    }
}
