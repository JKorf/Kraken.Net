namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using CryptoExchange.Net.Objects;

    using Kraken.Net.Objects.Models;

    /// <summary>
    /// Kraken staking endpoints.
    /// </summary>
    public interface IKrakenClientSpotStakingApi
    {
        /// <summary>
        /// Stake an asset from your spot wallet.
        /// <para><a href="https://docs.kraken.com/rest/#operation/stake"/></para>
        /// </summary>
        /// <param name="asset">Asset to stake (asset ID or altname) e.g. DOT</param>
        /// <param name="amount">Amount of the asset to stake</param>
        /// <param name="method">Name of the staking option to use as returned by <see cref="KrakenStakingAsset.Method"/></param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <seealso cref="GetStakableAssets"/>
        /// <returns>A reference to the staking request.</returns>
        Task<WebCallResult<KrakenStakeResponse>> StakeAsync(string asset, decimal amount, string method, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Unstake an asset from your staking wallet
        /// <para><a href="https://docs.kraken.com/rest/#operation/unstake"/></para>
        /// </summary>
        /// <param name="asset">Asset to unstake (asset ID or altname). Must be a valid staking asset (e.g. XBT.M, XTZ.S, ADA.S)</param>
        /// <param name="amount">Amount of the asset to stake</param>
        /// <param name="method">Amount of the asset to stake</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>A reference to the unstaking request.</returns>
        Task<WebCallResult<KrakenUnstakeResponse>> UnstakeAsync(string asset, decimal amount, string method, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Returns the list of pending staking transactions. Once completed, these transactions will be accessible
        /// through <see cref="GetRecentTransactionsAsync"/> and will not be accessible through this API.
        /// <para><a href="https://docs.kraken.com/rest/#operation/getStakingPendingDeposits"/></para>
        /// </summary>
        /// <param name="twoFactorPassword"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenStakingTransaction>>> GetPendingTransactionsAsync(string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Returns a list of 1000 recent staking transactions from past 90 days.
        /// <para><a href="https://docs.kraken.com/rest/#operation/getStakingTransactions"/></para>
        /// </summary>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The last 1000 staking transactions from the past 90 days</returns>
        Task<WebCallResult<IEnumerable<KrakenStakingTransaction>>> GetRecentTransactionsAsync(string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Returns the list of assets that you're able to stake.
        /// <para><a href="https://docs.kraken.com/rest/#operation/getStakingAssetInfo"/></para>
        /// </summary>
        /// <param name="twoFactorPassword"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenStakingAsset>>> GetStakableAssets(string? twoFactorPassword = null, CancellationToken ct = default);
    }
}
