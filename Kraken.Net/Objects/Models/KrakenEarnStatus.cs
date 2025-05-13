using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Earn status
    /// </summary>
    [SerializationModel]
    public record KrakenEarnStatus
    {
        /// <summary>
        /// Is pending
        /// </summary>
        [JsonPropertyName("pending")]
        public bool Pending { get; set; }
    }
}
