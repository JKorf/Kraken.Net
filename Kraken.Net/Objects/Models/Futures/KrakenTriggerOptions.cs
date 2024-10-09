using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models.Futures
{
    /// <summary>
    /// Trigger options
    /// </summary>
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
        [JsonConverter(typeof(EnumConverter))]
        [JsonPropertyName("triggerSide")]
        public TriggerSide TriggerSide { get; set; }
        /// <summary>
        /// Trigger signal
        /// </summary>
        [JsonConverter(typeof(EnumConverter))]
        [JsonPropertyName("triggerSignal")]
        public TriggerSignal TriggerSignal { get; set; }
    }
}
