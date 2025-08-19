using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Batch order result
    /// </summary>
    [SerializationModel]
    public record KrakenBatchOrderResult
    {
        /// <summary>
        /// Orders
        /// </summary>
        [JsonPropertyName("orders")]
        public KrakenPlacedBatchOrder[] Orders { get; set; } = Array.Empty<KrakenPlacedBatchOrder>();
    }

    /// <summary>
    /// Placed batch order
    /// </summary>
    [SerializationModel]
    public record KrakenPlacedBatchOrder
    {
        /// <summary>
        /// Order id
        /// </summary>
        [JsonPropertyName("txid")]
        public string OrderId { get; set; } = null!;
        /// <summary>
        /// Error message
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [JsonPropertyName("descr")]
        public KrakenPlacedOrderDescription Description { get; set; } = null!;
        /// <summary>
        /// Close order description
        /// </summary>
        [JsonPropertyName("close")]
        public string? CloseOrderInfo { get; set; }
    }
}
