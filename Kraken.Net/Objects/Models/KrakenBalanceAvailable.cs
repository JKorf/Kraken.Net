using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Balance info
    /// </summary>
    public class KrakenBalanceAvailable
    {
        /// <summary>
        /// Asset
        /// </summary>
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Balance
        /// </summary>
        [JsonProperty("balance")]
        public decimal Total { get; set; }

        /// <summary>
        /// The quantity currently locked into a trade
        /// </summary>
        [JsonProperty("hold_trade")]
        public decimal Locked { get; set; }

        /// <summary>
        /// The quantity available
        /// </summary>
        [JsonIgnore]
        public decimal Available => Total - Locked;
    }
}
