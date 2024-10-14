using Kraken.Net.Enums;
using Kraken.Net.Objects.Models.Futures;

namespace Kraken.Net.Interfaces.Clients.FuturesApi
{
    /// <summary>
    /// Kraken futures account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IKrakenRestClientFuturesApiAccount
    {
        /// <summary>
        /// Get account log entries
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/history/account-log" /></para>
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
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/get-accounts" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesBalances>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the PNL currency preference is used to determine which currency to pay out when realizing PNL gains.
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/get-pnl-currency-preference" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<KrakenFuturesPnlCurrency>>> GetPnlCurrencyAsync(CancellationToken ct = default);

        /// <summary>
        /// Set the PNL currency preference is used to determine which currency to pay out when realizing PNL gains.
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/set-pnl-currency-preference" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to update, for example `PF_ETHUSD`</param>
        /// <param name="pnlCurrency">Currency to use</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> SetPnlCurrencyAsync(string symbol, string pnlCurrency, CancellationToken ct = default);

        /// <summary>
        /// Transfer between 2 margin accounts or between margin and cash account
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/transfer" /></para>
        /// </summary>
        /// <param name="asset">The asset to transfer, for example `USDT`</param>
        /// <param name="quantity">The amount to transfer</param>
        /// <param name="fromAccount">The wallet (cash or margin account) to which funds should be credited</param>
        /// <param name="toAccount">The wallet (cash or margin account) from which funds should be debited</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult> TransferAsync(string asset, decimal quantity, string fromAccount, string toAccount, CancellationToken ct = default);

        /// <summary>
        /// Get fee schedule volume
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/get-user-fee-schedule-volumes-v-3" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<Dictionary<string, decimal>>> GetFeeScheduleVolumeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the initial margin requirements for the provided parameters
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/get-initial-margin" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `PF_ETHUSD`</param>
        /// <param name="orderType">Order type</param>
        /// <param name="side">Side</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="price">Limit price</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesMarginRequirements>> GetInitialMarginRequirementsAsync(string symbol, FuturesOrderType orderType, OrderSide side, decimal quantity, decimal? price = null, CancellationToken ct = default);

        /// <summary>
        /// Get the max order quantity
        /// <para><a href="https://docs.kraken.com/api/docs/futures-api/trading/get-max-order-size" /></para>
        /// </summary>
        /// <param name="symbol">The symbol, for example `PF_ETHUSD`</param>
        /// <param name="orderType">Order type</param>
        /// <param name="price">Limit price</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenFuturesMaxOrderSize>> GetMaxOrderQuantityAsync(string symbol, FuturesOrderType orderType, decimal? price = null, CancellationToken ct = default);
    }
}