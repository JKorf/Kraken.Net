namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Result of a cancel request
    /// </summary>
    [SerializationModel]
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
        public long[] Pending { get; set; } = Array.Empty<long>();
    }
}
