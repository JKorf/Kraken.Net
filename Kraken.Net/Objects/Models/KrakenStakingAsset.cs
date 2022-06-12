namespace Kraken.Net.Objects.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an asset that can be staked by the user.
    /// </summary>
    public class KrakenStakingAsset
    {
        /// <summary>
        /// Unique ID of the staking option (used in Stake/Unstake operations).
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; } = null!;

        /// <summary>
        /// Asset code/name e.g. DOT.
        /// </summary>
        [JsonProperty("asset")]
        public string Asset { get; set; } = null!;

        /// <summary>
        /// Staking asset code/name e.g. DOT.S
        /// </summary>
        [JsonProperty("staking_asset")]
        public string StakingAsset { get; set; } = null!;

        /// <summary>
        /// Describes the rewards earned while staking.
        /// </summary>
        [JsonProperty("rewards")]
        public KrakenStakingRewardInfo? Rewards { get; set; }

        /// <summary>
        /// Whether the staking operation is on-chain or not.
        /// </summary>
        [JsonProperty("on_chain")]
        public bool OnChain { get; set; }

        /// <summary>
        /// Whether the user will be able to stake this asset.
        /// </summary>
        [JsonProperty("can_stake")]
        public bool CanStake { get; set; }

        /// <summary>
        /// Whether the user will be able to unstake this asset.
        /// </summary>
        [JsonProperty("can_unstake")]
        public bool CanUnstake { get; set; }

        /// <summary>
        /// Minimium amounts for staking/unstaking.
        /// </summary>
        [JsonProperty("minimum_amount")]
        public KrakenStakingMinimumInfo? Minimums { get; set; }
    }
}