namespace Kraken.Net.Objects.Models
{
    using Kraken.Net.Converters;
    using Kraken.Net.Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Describes the rewards earned while staking.
    /// </summary>
    public class KrakenStakingRewardInfo
    {
        /// <summary>
        /// Reward earned while staking.
        /// </summary>
        [JsonProperty("reward")]
        public string? Reward { get; set; }

        /// <summary>
        /// The type of the reward e.g. "percentage".
        /// </summary>

        [JsonProperty("type"), JsonConverter(typeof(StakingRewardTypeConverter))]
        public StakingRewardType Type { get; set; }
    }
}
