using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models.Socket
{
    /// <summary>
    /// Replace order result
    /// </summary>
    [SerializationModel]
    public record KrakenSocketReplaceOrderResult
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// The original order id
        /// </summary>
        [JsonPropertyName("original_order_id")]
        public string OriginalOrderId { get; set; } = string.Empty;
    }
}
