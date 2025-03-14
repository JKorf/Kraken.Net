using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Deposit status info
    /// </summary>
    [SerializationModel]
    public record KrakenMovementStatus
    {
        /// <summary>
        /// The name of the deposit method
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;
        /// <summary>
        /// The record of the asset
        /// </summary>
        [JsonPropertyName("aclass")]
        public string AssetClass { get; set; } = string.Empty;
        /// <summary>
        /// The asset name
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Reference id
        /// </summary>
        [JsonPropertyName("refid")]
        public string ReferenceId { get; set; } = string.Empty;
        /// <summary>
        /// Transaction id
        /// </summary>
        [JsonPropertyName("txid")]
        public string TransactionId { get; set; } = string.Empty;
        /// <summary>
        /// Info about the transaction
        /// </summary>
        [JsonPropertyName("info")]
        public string TransactionInfo { get; set; } = string.Empty;
        /// <summary>
        /// The quantity involved in the deposit
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// The fee paid for the deposit
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// The timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonPropertyName("time")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Status of the transaction
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        /// <summary>
        /// Additional status info
        /// </summary>
        [JsonPropertyName("status-prop")]
        public string? AdditionalStatus { get; set; } = string.Empty;
        /// <summary>
        /// Withdrawal key name, as set up on your account
        /// </summary>
        [JsonPropertyName("key")]
        public string? Key { get; set; }
        /// <summary>
        /// Originators
        /// </summary>
        [JsonPropertyName("originators")]
        public string[]? Originators { get; set; } = Array.Empty<string>();
    }
}
