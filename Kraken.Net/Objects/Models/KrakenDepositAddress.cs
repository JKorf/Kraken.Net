using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Deposit address
    /// </summary>
    [SerializationModel]
    public record KrakenDepositAddress
    {
        /// <summary>
        /// The actual address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// The expire time of the address
        /// </summary>
        [JsonPropertyName("expiretm"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// If the address has been used before
        /// </summary>
        [JsonPropertyName("new")]
        public bool IsNew { get; set; }
        /// <summary>
        /// Tag
        /// </summary>
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }
        /// <summary>
        /// Memo
        /// </summary>
        [JsonPropertyName("memo")]
        public string? Memo { get; set; }
    }
}
