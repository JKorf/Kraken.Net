namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Allocations page
    /// </summary>
    [SerializationModel]
    public record KrakenAllocationsCursorPage : KrakenCursorPage<KrakenAllocation>
    {
        /// <summary>
        /// Converted asset
        /// </summary>
        [JsonPropertyName("converted_asset")]
        public string ConvertedAsset { get; set; } = string.Empty;
        /// <summary>
        /// Total allocated
        /// </summary>
        [JsonPropertyName("total_allocated")]
        public decimal TotalAllocated { get; set; }
        /// <summary>
        /// Total rewarded
        /// </summary>
        [JsonPropertyName("total_rewarded")]
        public decimal TotalRewarded { get; set; }
    }

    /// <summary>
    /// Allocation info
    /// </summary>
    [SerializationModel]
    public record KrakenAllocation
    {
        /// <summary>
        /// Strategy id
        /// </summary>
        [JsonPropertyName("strategy_id")]
        public string StrategyId { get; set; } = string.Empty;
        /// <summary>
        /// Native asset
        /// </summary>
        [JsonPropertyName("native_asset")]
        public string Asset { get; set; } = string.Empty;

        /// <summary>
        /// Allocated amounts
        /// </summary>
        [JsonPropertyName("amount_allocated")]
        public KrakenAllocatedAmount AmountAllocated { get; set; } = null!;
        /// <summary>
        /// Total rewarded
        /// </summary>
        [JsonPropertyName("total_rewarded")]
        public KrakenAllocationRewarded TotalRewarded { get; set; } = null!;

        /// <summary>
        /// Information about the current payout period, absent if when there is no current payout period.
        /// </summary>
        [JsonPropertyName("payout")]
        public KrakenAllocationPayout Payout { get; set; } = null!;
    }

    /// <summary>
    /// Allocated amounts
    /// </summary>
    [SerializationModel]
    public record KrakenAllocatedAmount
    {
        /// <summary>
        /// Bonding allocations
        /// </summary>
        [JsonPropertyName("bonding")]
        public KrakenBondingAwarded Bonding { get; set; } = null!;
        /// <summary>
        /// Unbonding allocations
        /// </summary>
        [JsonPropertyName("unbonding")]
        public KrakenBondingAwarded Unbonding { get; set; } = null!;
        /// <summary>
        /// Total allocations
        /// </summary>
        [JsonPropertyName("total")]
        public KrakenAllocationRewarded Total { get; set; } = null!;
    }

    /// <summary>
    /// Bonding rewards
    /// </summary>
    [SerializationModel]
    public record KrakenBondingAwarded : KrakenAllocationRewarded
    {
        /// <summary>
        /// Allocation count
        /// </summary>
        [JsonPropertyName("allocation_count")]
        public int AllocationCount { get; set; }
        /// <summary>
        /// Allocations
        /// </summary>
        [JsonPropertyName("allocations")]
        public KrakenBondingAllocation[] Allocations { get; set; } = Array.Empty<KrakenBondingAllocation>();
    }

    /// <summary>
    /// Bonding allocation
    /// </summary>
    [SerializationModel]
    public record KrakenBondingAllocation : KrakenAllocationRewarded
    {
        /// <summary>
        /// Create time
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Expire time
        /// </summary>
        [JsonPropertyName("expires")]
        public DateTime ExpireTime { get; set; }
    }

    /// <summary>
    /// Rewards
    /// </summary>
    [SerializationModel]
    public record KrakenAllocationRewarded
    {
        /// <summary>
        /// Rewarded in native
        /// </summary>
        [JsonPropertyName("native")]
        public decimal Native { get; set; }
        /// <summary>
        /// Rewarded in converted
        /// </summary>
        [JsonPropertyName("converted")]
        public decimal Converted { get; set; }
    }

    /// <summary>
    /// Payout
    /// </summary>
    [SerializationModel]
    public record KrakenAllocationPayout
    {
        /// <summary>
        /// Reward accumulated in the payout period until now
        /// </summary>
        [JsonPropertyName("accumulated_reward")]
        public KrakenAllocationRewarded AccumulatedReward { get; set; } = default!;

        /// <summary>
        /// Estimated reward from now until the payout
        /// </summary>
        [JsonPropertyName("estimated_reward")]
        public KrakenAllocationRewarded EstimatedReward { get; set; } = default!;

        /// <summary>
        /// Tentative date of the next reward payout.
        /// </summary>
        [JsonPropertyName("period_end")]
        public DateTime PeriodEnd { get; set; }

        /// <summary>
        /// When the current payout period started. Either the date of the last payout or when it was enabled.
        /// </summary>
        [JsonPropertyName("period_start")]
        public DateTime PeriodStart { get; set; }
    }
}
