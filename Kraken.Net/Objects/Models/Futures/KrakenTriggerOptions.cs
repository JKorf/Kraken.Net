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
        /// ["<c>triggerPrice</c>"] Trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public decimal TriggerPrice { get; set; }
        /// <summary>
        /// ["<c>triggerSide</c>"] Trigger side
        /// </summary>

        [JsonPropertyName("triggerSide")]
        public TriggerSide TriggerSide { get; set; }
        /// <summary>
        /// ["<c>triggerSignal</c>"] Trigger signal
        /// </summary>

        [JsonPropertyName("triggerSignal")]
        public TriggerSignal TriggerSignal { get; set; }
    }
}
