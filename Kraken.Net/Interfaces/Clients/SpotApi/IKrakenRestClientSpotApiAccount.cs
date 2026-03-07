using Kraken.Net.Enums;
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
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-account-balance" /><br />
        /// Endpoint:<br />
        /// POST /0/private/Balance
        /// </para>
        /// </summary>
        /// <param name="newAssetNameResponse">["<c>assetVersion</c>"] When set to true the asset names will be in the new format, for example `BTC` instead of `XBT`. Default is false.</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with balances for assets</returns>
        Task<WebCallResult<Dictionary<string, decimal>>> GetBalancesAsync(bool? newAssetNameResponse = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get balances including quantity in holding
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-trade-balance" /><br />
        /// Endpoint:<br />
        /// POST /0/private/BalanceEx
        /// </para>
        /// </summary>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with balances for assets</returns>
        Task<WebCallResult<Dictionary<string, KrakenBalanceAvailable>>> GetAvailableBalancesAsync(string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get trade balance
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-extended-balance" /><br />
        /// Endpoint:<br />
        /// POST /0/private/TradeBalance
        /// </para>
        /// </summary>
        /// <param name="baseAsset">["<c>asset</c>"] Base asset to get trade balance for, for example `USDT`</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade balance data</returns>
        Task<WebCallResult<KrakenTradeBalance>> GetTradeBalanceAsync(string? baseAsset = null, string? twoFactorPassword = null, CancellationToken ct = default);


        /// <summary>
        /// Get a list of open positions
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-open-positions" /><br />
        /// Endpoint:<br />
        /// POST /0/private/OpenPositions
        /// </para>
        /// </summary>
        /// <param name="transactionIds">["<c>txid</c>"] Filter by transaction ids</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with position info</returns>
        Task<WebCallResult<Dictionary<string, KrakenPosition>>> GetOpenPositionsAsync(IEnumerable<string>? transactionIds = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get ledger entries info
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-ledgers" /><br />
        /// Endpoint:<br />
        /// POST /0/private/Ledgers
        /// </para>
        /// </summary>
        /// <param name="assets">["<c>asset</c>"] Filter list by asset names, for example `USDT`</param>
        /// <param name="entryTypes">["<c>type</c>"] Filter list by entry types</param>
        /// <param name="assetClass">["<c>aClass</c>"] Filter by asset class</param>
        /// <param name="startTime">["<c>start</c>"] Return data after this time</param>
        /// <param name="endTime">["<c>end</c>"] Return data before this time</param>
        /// <param name="resultOffset">["<c>ofs</c>"] Offset the results by</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Ledger entries page</returns>
        Task<WebCallResult<KrakenLedgerPage>> GetLedgerInfoAsync(
            IEnumerable<string>? assets = null,
            IEnumerable<LedgerEntryType>? entryTypes = null,
            AClass? assetClass = null,
            DateTime? startTime = null,
            DateTime? endTime = null, 
            int? resultOffset = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get info on specific ledger entries
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-ledgers-info" /><br />
        /// Endpoint:<br />
        /// POST /0/private/QueryLedgers
        /// </para>
        /// </summary>
        /// <param name="ledgerIds">["<c>id</c>"] The ids to get info for</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with ledger entry info</returns>
        Task<WebCallResult<Dictionary<string, KrakenLedgerEntry>>> GetLedgersEntryAsync(IEnumerable<string>? ledgerIds = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get trade volume
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-trade-volume" /><br />
        /// Endpoint:<br />
        /// POST /0/private/TradeVolume
        /// </para>
        /// </summary>
        /// <param name="symbols">["<c>pair</c>"] Symbols to get data for, for example `ETHUSDT`</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade fee info</returns>
        Task<WebCallResult<KrakenTradeVolume>> GetTradeVolumeAsync(IEnumerable<string>? symbols = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get deposit methods
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-deposit-methods" /><br />
        /// Endpoint:<br />
        /// POST /0/private/DepositMethods
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] Asset to get methods for, for example `ETH`</param>
        /// <param name="assetClass">["<c>aclass</c>"] Filter by asset class</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Array with methods for deposit</returns>
        Task<WebCallResult<KrakenDepositMethod[]>> GetDepositMethodsAsync(
            string asset,
            AClass? assetClass = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get deposit addresses for an asset
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-deposit-addresses" /><br />
        /// Endpoint:<br />
        /// POST /0/private/DepositAddresses
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] The asset to get the deposit address for, for example `ETH`</param>
        /// <param name="depositMethod">["<c>method</c>"] The method of deposit</param>
        /// <param name="generateNew">["<c>new</c>"] Whether to generate a new address</param>
        /// <param name="quantity">["<c>amount</c>"] Amount you wish to deposit (only required for depositMethod=Bitcoin Lightning)</param>
        /// <param name="assetClass">["<c>aclass</c>"] Filter by asset class</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenDepositAddress[]>> GetDepositAddressesAsync(
            string asset,
            string depositMethod,
            bool generateNew = false,
            decimal? quantity = null,
            AClass? assetClass = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get status of deposits
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-status-recent-deposits" /><br />
        /// Endpoint:<br />
        /// POST /0/private/DepositStatus
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] Asset to get deposit info for, for example `ETH`</param>
        /// <param name="depositMethod">["<c>method</c>"] The deposit method</param>
        /// <param name="assetClass">["<c>aclass</c>"] Filter by asset class</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Deposit status list</returns>
        Task<WebCallResult<KrakenMovementStatus[]>> GetDepositStatusAsync(
            string? asset = null,
            string? depositMethod = null,
            AClass? assetClass = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get deposit history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-status-recent-deposits" /><br />
        /// Endpoint:<br />
        /// POST /0/private/DepositStatus
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] Asset filter, for example `ETH`</param>
        /// <param name="depositMethod">["<c>method</c>"] Deposit method</param>
        /// <param name="assetClass">["<c>aclass</c>"] Filter by asset class</param>
        /// <param name="startTime">["<c>start</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="cursor">["<c>cursor</c>"] Pagination cursor</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenCursorPage<KrakenMovementStatus>>> GetDepositHistoryAsync(
            string? asset = null,
            string? depositMethod = null,
            AClass? assetClass = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            string? cursor = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default);

        /// <summary>
        /// Retrieve fee information about potential withdrawals for a particular asset, key and amount.
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-withdrawal-information" /><br />
        /// Endpoint:<br />
        /// POST /0/private/WithdrawInfo
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] The asset, for example `ETH`</param>
        /// <param name="key">["<c>key</c>"] The withdrawal key name</param>
        /// <param name="quantity">["<c>amount</c>"] The quantity to withdraw</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenWithdrawInfo>> GetWithdrawInfoAsync(string asset, string key, decimal quantity,
            string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Withdraw funds
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/withdraw-funds" /><br />
        /// Endpoint:<br />
        /// POST /0/private/Withdraw
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] The asset being withdrawn, for example `ETH`</param>
        /// <param name="key">["<c>key</c>"] The withdrawal key name, as set up on your account</param>
        /// <param name="quantity">["<c>amount</c>"] The quantity to withdraw, including fees</param>
        /// <param name="address">["<c>address</c>"] Crypto address that can be used to confirm address matches key(will return Invalid withdrawal address error if different)</param>
        /// <param name="assetClass">["<c>aclass</c>"] Asset class</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Withdraw reference id</returns>
        Task<WebCallResult<KrakenWithdraw>> WithdrawAsync(string asset, string key, decimal quantity, string? address = null, AClass? assetClass = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get withdraw addresses
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-withdrawal-addresses" /><br />
        /// Endpoint:<br />
        /// POST /0/private/WithdrawAddresses
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] The asset to get the deposit address for, for example `ETH`</param>
        /// <param name="assetClass">["<c>aclass</c>"] Filter addresses for specific asset class</param>
        /// <param name="method">["<c>method</c>"] Filter addresses for specific method</param>
        /// <param name="key">["<c>key</c>"] Find address for by withdrawal key name, as set up on your account</param>
        /// <param name="verified">["<c>verified</c>"] Filter by verification status of the withdrawal address. Withdrawal addresses successfully completing email confirmation will have a verification status of true</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of withdraw addresses</returns>
        Task<WebCallResult<KrakenWithdrawAddress[]>> GetWithdrawAddressesAsync(string? asset = null, AClass? assetClass = null, string? method = null, string? key = null, bool? verified = null, CancellationToken ct = default);

        /// <summary>
        /// Retrieve a list of withdrawal methods available for the user.
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-withdrawal-methods" /><br />
        /// Endpoint:<br />
        /// POST /0/private/WithdrawMethods
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] The asset to get the deposit address for, for example `ETH`</param>
        /// <param name="assetClass">["<c>aclass</c>"] Filter addresses for specific asset class</param>
        /// <param name="network">["<c>network</c>"] Filter methods for specific network</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of withdraw addresses</returns>
        Task<WebCallResult<KrakenWithdrawMethod[]>> GetWithdrawMethodsAsync(string? asset = null, AClass? assetClass = null, string? network = null, CancellationToken ct = default);

        /// <summary>
        /// Get the token to connect to the private websocket streams. Note that this endpoint is used internally and there is normally no need to call this.
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-websockets-token" /><br />
        /// Endpoint:<br />
        /// POST /0/private/GetWebSocketsToken
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenWebSocketToken>> GetWebsocketTokenAsync(CancellationToken ct = default);

        /// <summary>
        /// Get status of withdrawals
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-status-recent-withdrawals" /><br />
        /// Endpoint:<br />
        /// POST /0/private/WithdrawStatus
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] Filter by asset, for example `ETH`</param>
        /// <param name="assetClass">["<c>aclass</c>"] Filter addresses for specific asset class</param>
        /// <param name="withdrawalMethod">["<c>method</c>"] Filter by method</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenMovementStatus[]>> GetWithdrawalStatusAsync(string? asset = null, string? withdrawalMethod = null, AClass? assetClass = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get withdrawal history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/get-status-recent-withdrawals" /><br />
        /// Endpoint:<br />
        /// POST /0/private/WithdrawStatus
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] Asset filter, for example `ETH`</param>
        /// <param name="withdrawalMethod">["<c>method</c>"] Withdrawal method</param>
        /// <param name="startTime">["<c>start</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results</param>
        /// <param name="cursor">["<c>cursor</c>"] Pagination cursor</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
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
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/cancel-withdrawal" /><br />
        /// Endpoint:<br />
        /// POST /0/private/WithdrawCancel
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] Asset, for example `ETH`</param>
        /// <param name="referenceId">["<c>refid</c>"] Reference id</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<bool>> CancelWithdrawalAsync(string asset, string referenceId, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Transfer funds between wallets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.kraken.com/api/docs/rest-api/wallet-transfer" /><br />
        /// Endpoint:<br />
        /// POST /0/private/WalletTransfer
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] Asset, for example `ETH`</param>
        /// <param name="quantity">["<c>amount</c>"] Quantity</param>
        /// <param name="fromWallet">["<c>from</c>"] Source wallet</param>
        /// <param name="toWallet">["<c>to</c>"] Target wallet</param>
        /// <param name="twoFactorPassword">["<c>otp</c>"] Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenReferenceId>> TransferAsync(string asset, decimal quantity, string fromWallet, string toWallet, string? twoFactorPassword = null, CancellationToken ct = default);
    }
}
