namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Cancel after result
    /// </summary>
    public record KrakenCancelAfterResult
    {
        /// <summary>
        /// Current time
        /// </summary>
        [JsonPropertyName("currentTime")]
        public DateTime CurrentTime { get; set; }
        /// <summary>
        /// Trigger time
        /// </summary>
        [JsonPropertyName("triggerTime")]
        public DateTime? TriggerTime { get; set; }
    }
}
