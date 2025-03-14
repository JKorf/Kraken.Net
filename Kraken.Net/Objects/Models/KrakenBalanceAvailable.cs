using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Balance info
    /// </summary>
    [SerializationModel]
    public record KrakenBalanceAvailable
    {
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Balance
        /// </summary>
        [JsonPropertyName("balance")]
        public decimal Total { get; set; }

        /// <summary>
        /// The quantity currently locked into a trade
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
