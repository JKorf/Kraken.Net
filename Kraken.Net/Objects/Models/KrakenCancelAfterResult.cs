namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Cancel after result
    /// </summary>
    [SerializationModel]
    public record KrakenCancelAfterResult
    {
        /// <summary>
        /// ["<c>currentTime</c>"] Current time
        /// </summary>
        [JsonPropertyName("currentTime")]
        public DateTime CurrentTime { get; set; }
        /// <summary>
        /// ["<c>triggerTime</c>"] Trigger time
        /// </summary>
        [JsonPropertyName("triggerTime")]
        public DateTime? TriggerTime { get; set; }
    }
}
