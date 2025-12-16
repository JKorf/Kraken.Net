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
        /// The id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Reference id
        /// </summary>
        [JsonPropertyName("refid")]
        public string ReferenceId { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("time"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// The type of entry
        /// </summary>
        [JsonPropertyName("type")]
        public LedgerEntryType Type { get; set; }
        /// <summary>
        /// Sub type
        /// </summary>
        [JsonPropertyName("subtype")]
        public string? SubType { get; set; }
        /// <summary>
        /// Class of the asset
        /// </summary>
        [JsonPropertyName("aclass")]
        public string AssetClass { get; set; } = string.Empty;

        /// <summary>
        /// Name of the asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// The quantity of the entry
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Fee paid
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// Resulting balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal BalanceAfter { get; set; }
    }
}
