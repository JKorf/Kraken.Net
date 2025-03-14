using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Edited order info
    /// </summary>
    [SerializationModel]
    public record KrakenEditOrder
    {
        /// <summary>
        /// Order ids
        /// </summary>
        [JsonPropertyName("txid")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// Descriptions
        /// </summary>
        [JsonPropertyName("descr")]
        public KrakenPlacedOrderDescription Descriptions { get; set; } = default!;
    }
}
