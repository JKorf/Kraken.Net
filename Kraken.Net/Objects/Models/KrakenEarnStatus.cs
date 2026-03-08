namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Earn status
    /// </summary>
    [SerializationModel]
    public record KrakenEarnStatus
    {
        /// <summary>
        /// ["<c>pending</c>"] Is pending
        /// </summary>
        [JsonPropertyName("pending")]
        public bool Pending { get; set; }
    }
}
