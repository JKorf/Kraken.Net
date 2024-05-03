using Kraken.Net.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Earn strategy info
    /// </summary>
    public class KrakenEarnStrategy
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Asset
        /// </summary>
        [JsonProperty("asset")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// Minimum amount (in USD) for an allocation or deallocation. Absence means no minimum.
        /// </summary>
        [JsonProperty("user_min_allocation")]
        public decimal MinAllocation { get; set; }
        /// <summary>
        /// Fee applied when allocating to this strategy
        /// </summary>
        [JsonProperty("allocation_fee")]
        public decimal AllocationFee { get; set; }
        /// <summary>
        /// Fee applied when deallocating from this strategy
        /// </summary>
        [JsonProperty("deallocation_fee")]
        public decimal DeallocationFee { get; set; }
        /// <summary>
        /// The maximum amount of funds that any given user may allocate to an account. Absence of value means there is no limit. Zero means that all new allocations will return an error (though auto-compound is unaffected).
        /// </summary>
        [JsonProperty("user_cap")]
        public decimal MaxAllocation { get; set; }
        /// <summary>
        /// Is allocation available for this strategy
        /// </summary>
        [JsonProperty("can_allocate")]
        public bool CanAllocate { get; set; }
        /// <summary>
        /// Is deallocation available for this strategy
        /// </summary>
        [JsonProperty("can_deallocate")]
        public bool CanDeallocate { get; set; }
        /// <summary>
        /// Reason for restrictions
        /// </summary>
        [JsonProperty("allocation_restriction_info")]
        public IEnumerable<string> AllocationRestrictionInfo { get; set; } = Array.Empty<string>();
        /// <summary>
        /// Yield source
        /// </summary>
        [JsonProperty("yield_source")]
        public YieldSourceType YieldSource { get; set; } = null!;
        /// <summary>
        /// Auto compound
        /// </summary>
        [JsonProperty("auto_compound")]
        public AutoCompoundType AutoCompound { get; set; } = null!;
        /// <summary>
        /// Apr estimation
        /// </summary>
        [JsonProperty("apr_estimate")]
        public AprEstimate AprEstimate { get; set; } = null!;
        /// <summary>
        /// Lock type info
        /// </summary>
        [JsonProperty("lock_type")]
        public LockTypeInfo LockType { get; set; } = null!;
    }

    /// <summary>
    /// Lock type info
    /// </summary>
    public class LockTypeInfo
    {
        /// <summary>
        /// Duration of the bonding period, in seconds
        /// </summary>
        [JsonProperty("bonding_period")]
        public int? BondingPeriod { get; set; }
        /// <summary>
        /// Is the bonding period length variable (true) or static (false)
        /// </summary>
        [JsonProperty("bonding_period_variable")]
        public bool? IsBondingPeriodVariable { get; set; }
        /// <summary>
        /// Whether rewards are earned during the bonding period (payouts occur after bonding is complete)
        /// </summary>
        [JsonProperty("bonding_rewards")]
        public bool? RewardsDuringBonding { get; set; }
        /// <summary>
        /// In order to remove funds, if this value is greater than 0, funds will first have to enter an exit queue and will have to wait for the exit queue period to end. Once ended, her funds will then follow and respect the unbonding_period.         If the value of the exit queue period is 0, then no waiting will have to occur and the exit queue will be skipped. Rewards are always paid out for the exit queue
        /// </summary>
        [JsonProperty("exit_queue_period")]
        public int? ExitQueuePeriod { get; set; }
        /// <summary>
        /// At what intervals are rewards distributed and credited to the user’s ledger, in seconds
        /// </summary>
        [JsonProperty("payout_frequency")]
        public int? PayoutFrequency { get; set; }
        /// <summary>
        /// Duration of the unbonding period in seconds. In order to remove funds, you must wait for the unbonding period to pass after requesting removal before funds become available in her spot wallet
        /// </summary>
        [JsonProperty("unbonding_period")]
        public int? UnbondingPeriod { get; set; }
        /// <summary>
        /// Lock type
        /// </summary>
        [JsonProperty("type")]
        public LockType Type { get; set; }
        /// <summary>
        /// Is the unbonding period length variable (true) or static (false)
        /// </summary>
        [JsonProperty("unbonding_period_variable")]
        public bool? IsUnbondingPeriodVariable { get; set; }
        /// <summary>
        /// Whether rewards are earned during the unbonding period (payouts occur after bonding is complete)
        /// </summary>
        [JsonProperty("unbonding_rewards")]
        public bool? RewardsDuringUnbonding { get; set; }
    }

    /// <summary>
    /// Yield source type
    /// </summary>
    public class YieldSourceType
    {
        /// <summary>
        /// Yield source type
        /// </summary>
        [JsonProperty("type")]
        public YieldSource Type { get; set; }
    }

    /// <summary>
    /// Auto compound type
    /// </summary>
    public class AutoCompoundType
    {
        /// <summary>
        /// Whether it is the default (if Type is Optional)
        /// </summary>
        [JsonProperty("default")]
        public bool? Default { get; set; }
        /// <summary>
        /// Auto Compound type
        /// </summary>
        [JsonProperty("type")]
        public AutoCompound Type { get; set; }
    }

    /// <summary>
    /// Apr estimate
    /// </summary>
    public class AprEstimate
    {
        /// <summary>
        /// High estimate
        /// </summary>
        [JsonProperty("high")]
        public decimal High { get; set; }
        /// <summary>
        /// Low estimate
        /// </summary>
        [JsonProperty("low")]
        public decimal Low { get; set; }
    }
}
