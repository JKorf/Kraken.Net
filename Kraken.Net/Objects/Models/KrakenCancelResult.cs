namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Result of a cancel request
    /// </summary>
    public record KrakenCancelResult
    {
        /// <summary>
        /// Amount of canceled orders
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }
        /// <summary>
        /// Pending cancelation orders
        /// </summary>
        [JsonPropertyName("pending")]
        public IEnumerable<long> Pending { get; set; } = Array.Empty<long>();
    }
}
