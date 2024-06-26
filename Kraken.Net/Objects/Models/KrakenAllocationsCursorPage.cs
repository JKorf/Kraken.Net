using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Allocations page
    /// </summary>
    public class KrakenAllocationsCursorPage : KrakenCursorPage<KrakenAllocation>
    {
        /// <summary>
        /// Converted asset
        /// </summary>
        [JsonProperty("converted_asset")]
        public string ConvertedAsset { get; set; } = string.Empty;
        /// <summary>
        /// Total allocated
        /// </summary>
        [JsonProperty("total_allocated")]
        public decimal TotalAllocated { get; set; }
        /// <summary>
        /// Total rewarded
        /// </summary>
        [JsonProperty("total_rewarded")]
        public decimal TotalRewarded { get; set; }
    }

    /// <summary>
    /// Allocation info
    /// </summary>
    public class KrakenAllocation
    {
        /// <summary>
        /// Strategy id
        /// </summary>
        [JsonProperty("strategy_id")]
        public string StrategyId { get; set; } = string.Empty;
        /// <summary>
        /// Native asset
        /// </summary>
        [JsonProperty("native_asset")]
        public string Asset { get; set; } = string.Empty;

        /// <summary>
        /// Allocated amounts
        /// </summary>
        [JsonProperty("amount_allocated")]
        public KrakenAllocatedAmount AmountAllocated { get; set; } = null!;
        /// <summary>
        /// Total rewarded
        /// </summary>
        [JsonProperty("total_rewarded")]
        public KrakenAllocationRewarded TotalRewarded { get; set; } = null!;

        /// <summary>
        /// Information about the current payout period, absent if when there is no current payout period.
        /// </summary>
        [JsonProperty("payout")]
        public KrakenAllocationPayout Payout { get; set; } = null!;
    }

    /// <summary>
    /// Allocated amounts
    /// </summary>
    public class KrakenAllocatedAmount
    {
        /// <summary>
        /// Bonding allocations
        /// </summary>
        [JsonProperty("bonding")]
        public KrakenBondingAwarded Bonding { get; set; } = null!;
        /// <summary>
        /// Unbonding allocations
        /// </summary>
        [JsonProperty("unbonding")]
        public KrakenBondingAwarded Unbonding { get; set; } = null!;
        /// <summary>
        /// Total allocations
        /// </summary>
        [JsonProperty("total")]
        public KrakenAllocationRewarded Total { get; set; } = null!;
    }

    /// <summary>
    /// Bonding rewards
    /// </summary>
    public class KrakenBondingAwarded : KrakenAllocationRewarded
    {
        /// <summary>
        /// Allocation count
        /// </summary>
        [JsonProperty("allocation_count")]
        public int AllocationCount { get; set; }
        /// <summary>
        /// Allocations
        /// </summary>
        [JsonProperty("allocations")]
        public IEnumerable<KrakenBondingAllocation> Allocations { get; set; } = Array.Empty<KrakenBondingAllocation>();
    }

    /// <summary>
    /// Bonding allocation
    /// </summary>
    public class KrakenBondingAllocation : KrakenAllocationRewarded
    {
        /// <summary>
        /// Create time
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Expire time
        /// </summary>
        [JsonProperty("expires")]
        public DateTime ExpireTime { get; set; }
    }

    /// <summary>
    /// Rewards
    /// </summary>
    public class KrakenAllocationRewarded
    {
        /// <summary>
        /// Rewarded in native
        /// </summary>
        [JsonProperty("native")]
        public decimal Native { get; set; }
        /// <summary>
        /// Rewarded in converted
        /// </summary>
        [JsonProperty("converted")]
        public decimal Converted { get; set; }
    }

    /// <summary>
    /// Payout
    /// </summary>
    public class KrakenAllocationPayout
    {
        /// <summary>
        /// Reward accumulated in the payout period until now
        /// </summary>
        [JsonProperty("accumulated_reward")]
        public KrakenAllocationRewarded AccumulatedReward { get; set; } = default!;

        /// <summary>
        /// Estimated reward from now until the payout
        /// </summary>
        [JsonProperty("estimated_reward")]
        public KrakenAllocationRewarded EstimatedReward { get; set; } = default!;

        /// <summary>
        /// Tentative date of the next reward payout.
        /// </summary>
        [JsonProperty("period_end")]
        public DateTime PeriodEnd { get; set; }

        /// <summary>
        /// When the current payout period started. Either the date of the last payout or when it was enabled.
        /// </summary>
        [JsonProperty("period_start")]
        public DateTime PeriodStart { get; set; }
    }
}
