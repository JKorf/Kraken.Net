using CryptoExchange.Net.Objects;
using Kraken.Net.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Kraken account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IKrakenRestClientSpotApiAccount
    {
        /// <summary>
        /// Get balances
        /// <para><a href="https://docs.kraken.com/rest/#operation/getAccountBalance" /></para>
        /// </summary>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with balances for assets</returns>
        Task<WebCallResult<Dictionary<string, decimal>>> GetBalancesAsync(string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get balances including quantity in holding
        /// <para><a href="https://docs.kraken.com/rest/#operation/getTradeBalance" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <returns>Dictionary with balances for assets</returns>
        Task<WebCallResult<Dictionary<string, KrakenBalanceAvailable>>> GetAvailableBalancesAsync(string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get trade balance
        /// <para><a href="https://docs.kraken.com/rest/#tag/Account-Data/operation/getExtendedBalance" /></para>
        /// </summary>
        /// <param name="baseAsset">Base asset to get trade balance for, for example `USDT`</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade balance data</returns>
        Task<WebCallResult<KrakenTradeBalance>> GetTradeBalanceAsync(string? baseAsset = null, string? twoFactorPassword = null, CancellationToken ct = default);


        /// <summary>
        /// Get a list of open positions
        /// <para><a href="https://docs.kraken.com/rest/#operation/getOpenPositions" /></para>
        /// </summary>
        /// <param name="transactionIds">Filter by transaction ids</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with position info</returns>
        Task<WebCallResult<Dictionary<string, KrakenPosition>>> GetOpenPositionsAsync(IEnumerable<string>? transactionIds = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get ledger entries info
        /// <para><a href="https://docs.kraken.com/rest/#operation/getLedgers" /></para>
        /// </summary>
        /// <param name="assets">Filter list by asset names, for example `USDT`</param>
        /// <param name="entryTypes">Filter list by entry types</param>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Ledger entries page</returns>
        Task<WebCallResult<KrakenLedgerPage>> GetLedgerInfoAsync(IEnumerable<string>? assets = null, IEnumerable<LedgerEntryType>? entryTypes = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific ledger entries
        /// <para><a href="https://docs.kraken.com/rest/#operation/getLedgersInfo" /></para>
        /// </summary>
        /// <param name="ledgerIds">The ids to get info for</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with ledger entry info</returns>
        Task<WebCallResult<Dictionary<string, KrakenLedgerEntry>>> GetLedgersEntryAsync(IEnumerable<string>? ledgerIds = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get trade volume
        /// <para><a href="https://docs.kraken.com/rest/#operation/getTradeVolume" /></para>
        /// </summary>
        /// <param name="symbols">Symbols to get data for, for example `ETHUSDT`</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade fee info</returns>
        Task<WebCallResult<KrakenTradeVolume>> GetTradeVolumeAsync(IEnumerable<string>? symbols = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit methods
        /// <para><a href="https://docs.kraken.com/rest/#operation/getDepositMethods" /></para>
        /// </summary>
        /// <param name="asset">Asset to get methods for, for example `ETH`</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Array with methods for deposit</returns>
        Task<WebCallResult<IEnumerable<KrakenDepositMethod>>> GetDepositMethodsAsync(string asset, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit addresses for an asset
        /// <para><a href="https://docs.kraken.com/rest/#operation/getDepositAddresses" /></para>
        /// </summary>
        /// <param name="asset">The asset to get the deposit address for, for example `ETH`</param>
        /// <param name="depositMethod">The method of deposit</param>
        /// <param name="generateNew">Whether to generate a new address</param>
        /// <param name="quantity">Amount you wish to deposit (only required for depositMethod=Bitcoin Lightning)</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenDepositAddress>>> GetDepositAddressesAsync(string asset, string depositMethod, bool generateNew = false, decimal? quantity = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get status of deposits
        /// <para><a href="https://docs.kraken.com/rest/#operation/getStatusRecentDeposits" /></para>
        /// </summary>
        /// <param name="asset">Asset to get deposit info for, for example `ETH`</param>
        /// <param name="depositMethod">The deposit method</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit status list</returns>
        Task<WebCallResult<IEnumerable<KrakenMovementStatus>>> GetDepositStatusAsync(string? asset = null, string? depositMethod = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit history
        /// <para><a href="https://docs.kraken.com/rest/#operation/getStatusRecentDeposits" /></para>
        /// </summary>
        /// <param name="asset">Asset filter, for example `ETH`</param>
        /// <param name="depositMethod">Deposit method</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="cursor">Pagination cursor</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenCursorPage<KrakenMovementStatus>>> GetDepositHistoryAsync(
            string? asset = null,
            string? depositMethod = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            string? cursor = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default);

        /// <summary>
        /// Retrieve fee information about potential withdrawals for a particular asset, key and amount.
        /// <para><a href="https://docs.kraken.com/rest/#operation/getWithdrawalInformation" /></para>
        /// </summary>
        /// <param name="asset">The asset, for example `ETH`</param>
        /// <param name="key">The withdrawal key name</param>
        /// <param name="quantity">The quantity to withdraw</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenWithdrawInfo>> GetWithdrawInfoAsync(string asset, string key, decimal quantity,
            string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Withdraw funds
        /// <para><a href="https://docs.kraken.com/rest/#operation/withdrawFunds" /></para>
        /// </summary>
        /// <param name="asset">The asset being withdrawn, for example `ETH`</param>
        /// <param name="key">The withdrawal key name, as set up on your account</param>
        /// <param name="quantity">The quantity to withdraw, including fees</param>
        /// <param name="address">Crypto address that can be used to confirm address matches key(will return Invalid withdrawal address error if different)</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdraw reference id</returns>
        Task<WebCallResult<KrakenWithdraw>> WithdrawAsync(string asset, string key, decimal quantity, string? address = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get withdraw addresses
        /// <para><a href="https://docs.kraken.com/rest/#operation/getWithdrawalAddresses" /></para>
        /// </summary>
        /// <param name="asset">The asset to get the deposit address for, for example `ETH`</param>
        /// <param name="aclass">Filter addresses for specific asset class</param>
        /// <param name="method">Filter addresses for specific method</param>
        /// <param name="key">Find address for by withdrawal key name, as set up on your account</param>
        /// <param name="verified">ilter by verification status of the withdrawal address. Withdrawal addresses successfully completing email confirmation will have a verification status of true</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of withdraw addresses</returns>
        Task<WebCallResult<IEnumerable<KrakenWithdrawAddress>>> GetWithdrawAddressesAsync(string? asset = null, string? aclass = null, string? method = null, string? key = null, bool? verified = null, CancellationToken ct = default);

        /// <summary>
        /// Retrieve a list of withdrawal methods available for the user.
        /// <para><a href="https://docs.kraken.com/rest/#operation/getWithdrawalMethods" /></para>
        /// </summary>
        /// <param name="asset">The asset to get the deposit address for, for example `ETH`</param>
        /// <param name="aclass">Filter addresses for specific asset class</param>
        /// <param name="network">Filter methods for specific network</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of withdraw addresses</returns>
        Task<WebCallResult<IEnumerable<KrakenWithdrawMethod>>> GetWithdrawMethodsAsync(string? asset = null, string? aclass = null, string? network = null, CancellationToken ct = default);

        /// <summary>
        /// Get the token to connect to the private websocket streams
        /// <para><a href="https://docs.kraken.com/rest/#operation/getWebsocketsToken" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenWebSocketToken>> GetWebsocketTokenAsync(CancellationToken ct = default);

        /// <summary>
        /// Get status of withdrawals
        /// <para><a href="https://docs.kraken.com/rest/#tag/User-Funding/operation/getStatusRecentWithdrawals" /></para>
        /// </summary>
        /// <param name="asset">Filter by asset, for example `ETH`</param>
        /// <param name="withdrawalMethod">Filter by method</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenMovementStatus>>> GetWithdrawalStatusAsync(string? asset = null, string? withdrawalMethod = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get withdrawal history
        /// <para><a href="https://docs.kraken.com/rest/#tag/User-Funding/operation/getStatusRecentWithdrawals" /></para>
        /// </summary>
        /// <param name="asset">Asset filter, for example `ETH`</param>
        /// <param name="withdrawalMethod">Withdrawal method</param>
        /// <param name="startTime">Filter by start time</param>
        /// <param name="endTime">Filter by end time</param>
        /// <param name="limit">Max number of results</param>
        /// <param name="cursor">Pagination cursor</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenCursorPage<KrakenMovementStatus>>> GetWithdrawalHistoryAsync(
            string? asset = null,
            string? withdrawalMethod = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            string? cursor = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel an active withdrawal
        /// <para><a href="https://docs.kraken.com/rest/#tag/User-Funding/operation/cancelWithdrawal" /></para>
        /// </summary>
        /// <param name="asset">Asset, for example `ETH`</param>
        /// <param name="referenceId">Reference id</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<bool>> CancelWithdrawalAsync(string asset, string referenceId, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Transfer funds between wallets
        /// <para><a href="https://docs.kraken.com/rest/#tag/User-Funding/operation/walletTransfer" /></para>
        /// </summary>
        /// <param name="asset">Asset, for example `ETH`</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="fromWallet">Source wallet</param>
        /// <param name="toWallet">Target wallet</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenReferenceId>> TransferAsync(string asset, decimal quantity, string fromWallet, string toWallet, string? twoFactorPassword = null, CancellationToken ct = default);
    }
}
