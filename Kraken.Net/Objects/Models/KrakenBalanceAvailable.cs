namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Balance info
    /// </summary>
    [SerializationModel]
    public record KrakenBalanceAvailable
    {
        /// <summary>
        /// ["<c>asset</c>"] Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>balance</c>"] Balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Total { get; set; }

        /// <summary>
        /// ["<c>hold_trade</c>"] The quantity currently locked into a trade
        /// </summary>
        [JsonPropertyName("hold_trade")]
        public decimal Locked { get; set; }

        /// <summary>
        /// The quantity available
        /// </summary>
        [JsonIgnore]
        public decimal Available => Total - Locked;
    }
}
