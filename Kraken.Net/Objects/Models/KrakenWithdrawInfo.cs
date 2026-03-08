namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Withdraw info
    /// </summary>
    [SerializationModel]
    public record KrakenWithdrawInfo
    {
        /// <summary>
        /// ["<c>method</c>"] Method that will be used
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>limit</c>"] Limit to what can be withdrawn right now
        /// </summary>
        [JsonPropertyName("limit")]
        public decimal Limit { get; set; }
        /// <summary>
        /// ["<c>amount</c>"] Quantity that will be send, after fees
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee that will be paid
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
    }
}
