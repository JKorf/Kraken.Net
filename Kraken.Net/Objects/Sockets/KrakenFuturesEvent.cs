using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Sockets
{
    /// <summary>
    /// Socket event
    /// </summary>
    [SerializationModel]
    public record KrakenFuturesEvent
    {
        /// <summary>
        /// The event type
        /// </summary>
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;
        /// <summary>
        /// The feed
        /// </summary>
        [JsonPropertyName("feed")]
        public string Feed { get; set; } = string.Empty;

        /// <summary>
        /// The symbols
        /// </summary>
        [JsonPropertyName("product_id")]
        public string? Symbol { get; set; }
    }
}
