using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Cancel all result
    /// </summary>
    [SerializationModel]
    public record KrakenStreamCancelAllResult
    {
        /// <summary>
        /// Number of orders canceled
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
