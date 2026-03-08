namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Edited order info
    /// </summary>
    [SerializationModel]
    public record KrakenEditOrder
    {
        /// <summary>
        /// ["<c>txid</c>"] Order ids
        /// </summary>
        [JsonPropertyName("txid")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>descr</c>"] Descriptions
        /// </summary>
        [JsonPropertyName("descr")]
        public KrakenPlacedOrderDescription Descriptions { get; set; } = default!;
    }
}
