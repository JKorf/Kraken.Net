namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Info about a deposit method
    /// </summary>
    [SerializationModel]
    public record KrakenDepositMethod
    {
        /// <summary>
        /// ["<c>method</c>"] Name of the method
        /// </summary>
        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;
        /// <summary>
        /// Deposit limit (max) of the method
        /// </summary>
        [JsonPropertyName("limit"), JsonConverter(typeof(NumberStringConverter))]
        public string Limit { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>fee</c>"] The deposit fee for the method
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>address-setup-fee</c>"] The fee for setting up an address
        /// </summary>
        [JsonPropertyName("address-setup-fee")]
        public decimal? AddressSetupFee { get; set; }
        /// <summary>
        /// ["<c>gen-address</c>"] Generate address
        /// </summary>
        [JsonPropertyName("gen-address")]
        public bool? GenerateAddress { get; set; }
        /// <summary>
        /// ["<c>minimum</c>"] Minimum deposit amount
        /// </summary>
        [JsonPropertyName("minimum")]
        public decimal? MinimumDepositAmount { get; set; }
    }
}
