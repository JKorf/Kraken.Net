namespace Kraken.Net.Objects.Models.Futures
{
    internal record KrakenFuturesCancelAfterResult : KrakenFuturesResult<KrakenFuturesCancelAfter>
    {
        [JsonPropertyName("status")]
        public override KrakenFuturesCancelAfter Data { get; set; } = null!;
    }

    /// <summary>
    /// Cancel after info
    /// </summary>
    public record KrakenFuturesCancelAfter
    {
        /// <summary>
        /// Current timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("currentTime")]
        public DateTime CurrentTime { get; set; }
        /// <summary>
        /// Trigger time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("triggerTime")]
        public DateTime? TriggerTime { get; set; }
    }
}
