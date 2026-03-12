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
        /// ["<c>enabled</c>"] Enabled
        /// </summary>
        [Map("enabled")]
        Enabled,
        /// <summary>
        /// ["<c>deposit_only</c>"] Only deposits available
        /// </summary>
        [Map("deposit_only", "depositonly")]
        DepositOnly,
        /// <summary>
        /// ["<c>withdrawal_only</c>"] Only withdrawals available
        /// </summary>
        [Map("withdrawal_only", "withdrawalonly")]
        WithdrawalOnly,
        /// <summary>
        /// ["<c>funding_temporarily_disabled</c>"] Funding temp disabled
        /// </summary>
        [Map("funding_temporarily_disabled", "fundingtemporarilydisabled")]
        FundingTemporarilyDisabled,
        /// <summary>
        /// ["<c>disabled</c>"] Disabled
        /// </summary>
        [Map("disabled")]
        Disabled,
        /// <summary>
        /// ["<c>work_in_progress</c>"] Work in process
        /// </summary>
        [Map("work_in_progress")]
        WorkInProcess
    }
}
