namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Placed order info
    /// </summary>
    [SerializationModel]
    public record KrakenPlacedOrder
    {
        /// <summary>
        /// Order ids
        /// </summary>
        [JsonPropertyName("txid")]
        public string[] OrderIds { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Descriptions
        /// </summary>
        [JsonPropertyName("descr")]
        public KrakenPlacedOrderDescription Descriptions { get; set; } = default!;
    }

    /// <summary>
    /// Order descriptions
    /// </summary>
    [SerializationModel]
    public record KrakenPlacedOrderDescription
    {
        /// <summary>
        /// Order description
        /// </summary>
        [JsonPropertyName("order")]
        public string OrderDescription { get; set; } = string.Empty;
        /// <summary>
        /// Close order description
        /// </summary>
        [JsonPropertyName("close")]
        public string CloseOrderDescription { get; set; } = string.Empty;
    }
}
