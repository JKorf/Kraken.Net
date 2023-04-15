using CryptoExchange.Net.Attributes;

namespace Kraken.Net.Enums
{
    /// <summary>
    /// Asset status
    /// </summary>
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
        [Map("deposit_only")]
        DepositOnly,
        /// <summary>
        /// Only withdrawals available
        /// </summary>
        [Map("withdrawal_only")]
        WithdrawalOnly,
        /// <summary>
        /// Funding temp disabled
        /// </summary>
        [Map("funding_temporarily_disabled")]
        FundingTemporarilyDisabled
    }
}
