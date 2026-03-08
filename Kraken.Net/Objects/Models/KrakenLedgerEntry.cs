using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Ledger entry info
    /// </summary>
    [SerializationModel]
    public record KrakenLedgerEntry
    {
        /// <summary>
        /// ["<c>id</c>"] The id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>refid</c>"] Reference id
        /// </summary>
        [JsonPropertyName("refid")]
        public string ReferenceId { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>type</c>"] The type of entry
        /// </summary>
        [JsonPropertyName("type")]
        public LedgerEntryType Type { get; set; }
        /// <summary>
        /// ["<c>subtype</c>"] Sub type
        /// </summary>
        [JsonPropertyName("subtype")]
        public string? SubType { get; set; }
        /// <summary>
        /// ["<c>aclass</c>"] Class of the asset
        /// </summary>
        [JsonPropertyName("aclass")]
        public string AssetClass { get; set; } = string.Empty;

        /// <summary>
        /// ["<c>asset</c>"] Name of the asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>amount</c>"] The quantity of the entry
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee paid
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>balance</c>"] Resulting balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal BalanceAfter { get; set; }
    }
}
