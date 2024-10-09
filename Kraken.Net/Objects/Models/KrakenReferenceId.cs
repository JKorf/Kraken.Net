namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Id
    /// </summary>
    public record KrakenReferenceId
    {
        /// <summary>
        /// The id
        /// </summary>
        [JsonPropertyName("refid")]
        public string ReferenceId { get; set; } = string.Empty;
    }
}
