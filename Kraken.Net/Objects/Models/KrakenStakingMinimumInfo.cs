namespace Kraken.Net.Objects.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Minimum amounts for staking/unstaking.
    /// </summary>
    public class KrakenStakingMinimumInfo
    {
        /// <summary>
        /// The minimum amount of value that can be staked.
        /// </summary>
        [JsonProperty("staking")]
        public decimal Staking { get; set; }

        /// <summary>
        /// The minimum amount of value that can be unstaked.
        /// </summary>
        [JsonProperty("unstaking")]
        public decimal Unstaking { get; set; }
    }
}
