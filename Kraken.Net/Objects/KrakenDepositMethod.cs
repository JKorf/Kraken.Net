using CryptoExchange.Net.Attributes;
using Newtonsoft.Json;

namespace Kraken.Net.Objects
{
    /// <summary>
    /// Info about a deposit method
    /// </summary>
    public class KrakenDepositMethod
    {
        /// <summary>
        /// Name of the method
        /// </summary>
        public string Method { get; set; } = "";
        /// <summary>
        /// Deposit limit (max) of the method
        /// </summary>
        public string Limit { get; set; } = "";
        /// <summary>
        /// The deposit fee for the method
        /// </summary>
        public decimal Fee { get; set; }
        /// <summary>
        /// The fee for setting up an address
        /// </summary>
        [JsonProperty("address-setup-fee"), JsonOptionalProperty]
        public decimal? AddressSetupFee { get; set; }
        /// <summary>
        /// Generate address
        /// </summary>
        [JsonProperty("gen-address")]
        public bool GenerateAddress { get; set; }
    }
}
