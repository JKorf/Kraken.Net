namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Placed order info
    /// </summary>
    [SerializationModel]
    public record KrakenPlacedOrder
    {
        /// <summary>
        /// ["<c>txid</c>"] Order ids
        /// </summary>
        [JsonPropertyName("txid")]
        public string[] OrderIds { get; set; } = Array.Empty<string>();
        /// <summary>
        /// ["<c>descr</c>"] Descriptions
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
        /// ["<c>order</c>"] Order description
        /// </summary>
        [JsonPropertyName("order")]
        public string OrderDescription { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>close</c>"] Close order description
        /// </summary>
        [JsonPropertyName("close")]
        public string CloseOrderDescription { get; set; } = string.Empty;
    }
}
