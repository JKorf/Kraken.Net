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
    }

    /// <summary>
    /// Allocated amounts
    /// </summary>
    public class KrakenAllocatedAmount
    {
        /// <summary>
        /// Bonding allocaitions
        /// </summary>
        [JsonProperty("bonding")]
        public KrakenBondingAwarded Bonding { get; set; } = null!;
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
}
