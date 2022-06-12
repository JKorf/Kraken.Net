namespace Kraken.Net.Enums
{
    /// <summary>
    /// The current status of the staking transaction.
    /// </summary>
    public enum StakingTransactionStatus
    {
#pragma warning disable CS1591
        Initial,
        Pending,
        Settled,
        Success,
        Failure
    }
}