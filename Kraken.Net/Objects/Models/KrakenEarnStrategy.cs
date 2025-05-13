using CryptoExchange.Net.Converters.SystemTextJson;
using Kraken.Net.Enums;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Earn strategy info
    /// </summary>
    [SerializationModel]
    public record KrakenEarnStrategy
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Asset
        /// </summary>
        [JsonPropertyName("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Minimum amount (in USD) for an allocation or deallocation. Absence means no minimum.
        /// </summary>
        [JsonPropertyName("user_min_allocation")]
        public decimal MinAllocation { get; set; }
        /// <summary>
        /// Fee applied when allocating to this strategy
        /// </summary>
        [JsonPropertyName("allocation_fee")]
        public decimal AllocationFee { get; set; }
        /// <summary>
        /// Fee applied when deallocating from this strategy
        /// </summary>
        [JsonPropertyName("deallocation_fee")]
        public decimal DeallocationFee { get; set; }
        /// <summary>
        /// The maximum amount of funds that any given user may allocate to an account. Absence of value means there is no limit. Zero means that all new allocations will return an error (though auto-compound is unaffected).
        /// </summary>
        [JsonPropertyName("user_cap")]
        public decimal MaxAllocation { get; set; }
        /// <summary>
        /// Is allocation available for this strategy
        /// </summary>
        [JsonPropertyName("can_allocate")]
        public bool CanAllocate { get; set; }
        /// <summary>
        /// Is deallocation available for this strategy
        /// </summary>
        [JsonPropertyName("can_deallocate")]
        public bool CanDeallocate { get; set; }
        /// <summary>
        /// Reason for restrictions
        /// </summary>
        [JsonPropertyName("allocation_restriction_info")]
        public string[] AllocationRestrictionInfo { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Yield source
        /// </summary>
        [JsonPropertyName("yield_source")]
        public YieldSourceType YieldSource { get; set; } = null!;
        /// <summary>
        /// Auto compound
        /// </summary>
        [JsonPropertyName("auto_compound")]
        public AutoCompoundType AutoCompound { get; set; } = null!;
        /// <summary>
        /// Apr estimation
        /// </summary>
        [JsonPropertyName("apr_estimate")]
        public AprEstimate AprEstimate { get; set; } = null!;
        /// <summary>
        /// Lock type info
        /// </summary>
        [JsonPropertyName("lock_type")]
        public LockTypeInfo LockType { get; set; } = null!;
    }

    /// <summary>
    /// Lock type info
    /// </summary>
    [SerializationModel]
    public record LockTypeInfo
    {
        /// <summary>
        /// Duration of the bonding period, in seconds
        /// </summary>
        [JsonPropertyName("bonding_period")]
        public int? BondingPeriod { get; set; }
        /// <summary>
        /// Is the bonding period length variable (true) or static (false)
        /// </summary>
        [JsonPropertyName("bonding_period_variable")]
        public bool? IsBondingPeriodVariable { get; set; }
        /// <summary>
        /// Whether rewards are earned during the bonding period (payouts occur after bonding is complete)
        /// </summary>
        [JsonPropertyName("bonding_rewards")]
        public bool? RewardsDuringBonding { get; set; }
        /// <summary>
        /// In order to remove funds, if this value is greater than 0, funds will first have to enter an exit queue and will have to wait for the exit queue period to end. Once ended, her funds will then follow and respect the unbonding_period.         If the value of the exit queue period is 0, then no waiting will have to occur and the exit queue will be skipped. Rewards are always paid out for the exit queue
        /// </summary>
        [JsonPropertyName("exit_queue_period")]
        public int? ExitQueuePeriod { get; set; }
        /// <summary>
        /// At what intervals are rewards distributed and credited to the user’s ledger, in seconds
        /// </summary>
        [JsonPropertyName("payout_frequency")]
        public int? PayoutFrequency { get; set; }
        /// <summary>
        /// Duration of the unbonding period in seconds. In order to remove funds, you must wait for the unbonding period to pass after requesting removal before funds become available in her spot wallet
        /// </summary>
        [JsonPropertyName("unbonding_period")]
        public int? UnbondingPeriod { get; set; }
        /// <summary>
        /// Lock type
        /// </summary>
        [JsonPropertyName("type")]
        public LockType Type { get; set; }
        /// <summary>
        /// Is the unbonding period length variable (true) or static (false)
        /// </summary>
        [JsonPropertyName("unbonding_period_variable")]
        public bool? IsUnbondingPeriodVariable { get; set; }
        /// <summary>
        /// Whether rewards are earned during the unbonding period (payouts occur after bonding is complete)
        /// </summary>
        [JsonPropertyName("unbonding_rewards")]
        public bool? RewardsDuringUnbonding { get; set; }
    }

    /// <summary>
    /// Yield source type
    /// </summary>
    [SerializationModel]
    public record YieldSourceType
    {
        /// <summary>
        /// Yield source type
        /// </summary>
        [JsonPropertyName("type")]
        public YieldSource Type { get; set; }
    }

    /// <summary>
    /// Auto compound type
    /// </summary>
    [SerializationModel]
    public record AutoCompoundType
    {
        /// <summary>
        /// Whether it is the default (if Type is Optional)
        /// </summary>
        [JsonPropertyName("default")]
        public bool? Default { get; set; }
        /// <summary>
        /// Auto Compound type
        /// </summary>
        [JsonPropertyName("type")]
        public AutoCompound Type { get; set; }
    }

    /// <summary>
    /// Apr estimate
    /// </summary>
    [SerializationModel]
    public record AprEstimate
    {
        /// <summary>
        /// High estimate
        /// </summary>
        [JsonPropertyName("high")]
        public decimal High { get; set; }
        /// <summary>
        /// Low estimate
        /// </summary>
        [JsonPropertyName("low")]
        public decimal Low { get; set; }
    }
}
