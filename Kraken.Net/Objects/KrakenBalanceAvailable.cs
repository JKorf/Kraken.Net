using Newtonsoft.Json;

namespace Kraken.Net.Objects
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
        public decimal Balance { get; set; }

        /// <summary>
        /// The amount currently locked into a trade
        /// </summary>
        [JsonProperty("hold_trade")]
        public decimal Locked { get; set; }

        /// <summary>
        /// The amount available
        /// </summary>
        [JsonIgnore]
        public decimal Available => Balance - Locked;
    }
}
