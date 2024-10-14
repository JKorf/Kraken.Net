using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Kraken Earn endpoints.
    /// </summary>
    public interface IKrakenRestClientSpotApiEarn
    {
        /// <summary>
        /// List earn strategies along with their parameters.
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/list-strategies"/></para>
        /// </summary>
        /// <param name="asset">Filter by asset, for example `USDT`</param>
        /// <param name="lockType">Filter by lock type</param>
        /// <param name="cursor">Next page cursor</param>
        /// <param name="limit">Result limit</param>
        /// <param name="asc">Ascending order</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenCursorPage<KrakenEarnStrategy>>> GetStrategiesAsync(string? asset = null, LockType? lockType = null, string? cursor = null, int? limit = null, bool? asc = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get earn allocations
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/list-allocations"/></para>
        /// </summary>
        /// <param name="convertAsset">Convert asset, defaults to USD</param>
        /// <param name="hideZeroAllocations">Hide zero allocations</param>
        /// <param name="asc">Ascending order</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenAllocationsCursorPage>> GetAllocationsAsync(string? convertAsset = null, bool? hideZeroAllocations = null, bool? asc = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get status of the last allocation request
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/get-allocate-strategy-status"/></para>
        /// </summary>
        /// <param name="strategyId">Strategy id</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenEarnStatus>> GetAllocationStatusAsync(string strategyId, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get status of the last deallocation request
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/get-deallocate-strategy-status"/></para>
        /// </summary>
        /// <param name="strategyId">Strategy id</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenEarnStatus>> GetDeallocationStatusAsync(string strategyId, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Allocate earn funds to a strategy
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/allocate-strategy"/></para>
        /// </summary>
        /// <param name="strategyId">Strategy id</param>
        /// <param name="quantity">Amount to allocate</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> AllocateEarnFundsAsync(string strategyId, decimal quantity, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Deallocate previously allocated funds
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/deallocate-strategy"/></para>
        /// </summary>
        /// <param name="strategyId">Strategy id</param>
        /// <param name="quantity">Amount to deallocate</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> DeallocateEarnFundsAsync(string strategyId, decimal quantity, string? twoFactorPassword = null, CancellationToken ct = default);
    }
}
