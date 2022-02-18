using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Withdraw info
    /// </summary>
    public class KrakenWithdrawInfo
    {
        /// <summary>
        /// Method that will be used
        /// </summary>
        public string Method { get; set; } = string.Empty;
        /// <summary>
        /// Limit to what can be withdrawn right now
        /// </summary>
        public decimal Limit { get; set; }
        /// <summary>
        /// Quantity that will be send, after fees
        /// </summary>
        [JsonProperty("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Fee that will be paid
        /// </summary>
        public decimal Fee { get; set; }
    }
}
