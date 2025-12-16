using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Asset status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AssetStatus>))]
    public enum AssetStatus
    {
        /// <summary>
        /// Enabled
        /// </summary>
        [Map("enabled")]
        Enabled,
        /// <summary>
        /// Only deposits available
        /// </summary>
        [Map("deposit_only", "depositonly")]
        DepositOnly,
        /// <summary>
        /// Only withdrawals available
        /// </summary>
        [Map("withdrawal_only", "withdrawalonly")]
        WithdrawalOnly,
        /// <summary>
        /// Funding temp disabled
        /// </summary>
        [Map("funding_temporarily_disabled", "fundingtemporarilydisabled")]
        FundingTemporarilyDisabled,
        /// <summary>
        /// Disabled
        /// </summary>
        [Map("disabled")]
        Disabled,
        /// <summary>
        /// Work in process
        /// </summary>
        [Map("work_in_progress")]
        WorkInProcess
    }
}
