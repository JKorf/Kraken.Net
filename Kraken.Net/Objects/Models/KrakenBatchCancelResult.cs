namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Cancel result
    /// </summary>
    public record KrakenBatchCancelResult
    {
        /// <summary>
        /// Canceled count
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
