using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Id
    /// </summary>
    [SerializationModel]
    public record KrakenReferenceId
    {
        /// <summary>
        /// The id
        /// </summary>
        [JsonPropertyName("refid")]
        public string ReferenceId { get; set; } = string.Empty;
    }
}
