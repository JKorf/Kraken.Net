using CryptoExchange.Net.Objects;
using Kraken.Net.Objects.Models.Futures;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kraken.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Kraken futures account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IKrakenRestClientFuturesApiAccount
    {
        /// <summary>
        /// Get account log entries
        /// <para><a href="https://docs.futures.kraken.com/#http-api-history-account-history-get-account-log" /></para>
        /// </summary>
        /// <param name="startTime">Return results after this time</param>
        /// <param name="endTime">Return results before this time</param>
        /// <param name="fromId">Return results after this id</param>
        /// <param name="toId">Return results before this id</param>
        /// <param name="sort">Sort asc or desc</param>
        /// <param name="type">Type of entry to filter by.</param>
        /// <param name="limit">Amount of entries to be returned.</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenAccountLogResult>> GetAccountLogAsync(DateTime? startTime = null, DateTime? endTime = null, int? fromId = null, int? toId = null, string? sort = null, string? type = null, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get asset balances and margin info
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-account-information-get-wallets" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<Dictionary<string, KrakenBalances>>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the PNL currency preference is used to determine which currency to pay out when realizing PNL gains.
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-multi-collateral-get-pnl-currency-preference-for-a-market" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenFuturesPnlCurrency>>> GetPnlCurrencyAsync(CancellationToken ct = default);

        /// <summary>
        /// Set the PNL currency preference is used to determine which currency to pay out when realizing PNL gains.
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-multi-collateral-set-pnl-currency-preference-for-a-market" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to update</param>
        /// <param name="pnlCurrency">Currency to use</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> SetPnlCurrencyAsync(string symbol, string pnlCurrency, CancellationToken ct = default);

        /// <summary>
        /// Transfer between 2 margin accounts or between margin and cash account
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-transfers-initiate-wallet-transfer" /></para>
        /// </summary>
        /// <param name="asset">The asset to transfer</param>
        /// <param name="quantity">The amount to transfer</param>
        /// <param name="fromAccount">The wallet (cash or margin account) to which funds should be credited</param>
        /// <param name="toAccount">The wallet (cash or margin account) from which funds should be debited</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> TransferAsync(string asset, decimal quantity, string fromAccount, string toAccount, CancellationToken ct = default);

        /// <summary>
        /// Get fee schedule volume
        /// <para><a href="https://docs.futures.kraken.com/#http-api-trading-v3-api-fee-schedules-get-fee-schedule-volumes" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<Dictionary<string, decimal>>> GetFeeScheduleVolumeAsync(CancellationToken ct = default);
    }
}