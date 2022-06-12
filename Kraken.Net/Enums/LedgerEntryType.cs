namespace Kraken.Net.Enums
{
    /// <summary>
    /// The type of a ledger entry
    /// </summary>
    public enum LedgerEntryType
    {
        /// <summary>
        /// Deposit
        /// </summary>
        Deposit,
        /// <summary>
        /// Withdrawal
        /// </summary>
        Withdrawal,
        /// <summary>
        /// Trade change
        /// </summary>
        Trade,
        /// <summary>
        /// Margin
        /// </summary>
        Margin,
        /// <summary>
        /// Adjustment
        /// </summary>
        Adjustment,
        /// <summary>
        /// Transfer
        /// </summary>
        Transfer,
        /// <summary>
        /// Rollover
        /// </summary>
        Rollover,
        /// <summary>
        /// Spend
        /// </summary>
        Spend,
        /// <summary>
        /// Receive
        /// </summary>
        Receive,
        /// <summary>
        /// Settled
        /// </summary>
        Settled,
        /// <summary>
        /// Staking
        /// </summary>
        Staking
    }
}
