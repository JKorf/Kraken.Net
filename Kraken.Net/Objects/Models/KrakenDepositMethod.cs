using CryptoExchange.Net.Converters.SystemTextJson;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Info about a deposit method
    /// </summary>
    [SerializationModel]
    public record KrakenDepositMethod
    {
        /// <summary>
        /// Name of the method
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;
        /// <summary>
        /// Deposit limit (max) of the method
        /// </summary>
        [JsonPropertyName("limit"), JsonConverter(typeof(NumberStringConverter))]
        public string Limit { get; set; } = string.Empty;
        /// <summary>
        /// The deposit fee for the method
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// The fee for setting up an address
        /// </summary>
        [JsonPropertyName("address-setup-fee")]
        public decimal? AddressSetupFee { get; set; }
        /// <summary>
        /// Generate address
        /// </summary>
        [JsonPropertyName("gen-address")]
        public bool? GenerateAddress { get; set; }
        /// <summary>
        /// Minimum deposit amount
        /// </summary>
        [JsonPropertyName("minimum")]
        public decimal? MinimumDepositAmount { get; set; }
    }
}
