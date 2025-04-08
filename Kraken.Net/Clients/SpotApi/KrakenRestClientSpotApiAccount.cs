using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Interfaces.Clients.SpotApi;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class KrakenRestClientSpotApiAccount : IKrakenRestClientSpotApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly KrakenRestClientSpotApi _baseClient;

        internal KrakenRestClientSpotApiAccount(KrakenRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, decimal>>> GetBalancesAsync(bool? newAssetNameResponse = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("assetVersion", newAssetNameResponse);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/Balance", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<Dictionary<string, decimal>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Available Balances

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenBalanceAvailable>>> GetAvailableBalancesAsync(string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/BalanceEx", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenBalanceAvailable>>(request, parameters, ct).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data)
                    item.Value.Asset = item.Key;
            }

            return result;
        }

        #endregion

        #region Get Trade Balances

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenTradeBalance>> GetTradeBalanceAsync(string? baseAsset = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("aclass", "currency");
            parameters.AddOptionalParameter("asset", baseAsset);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/TradeBalance", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenTradeBalance>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Open Positions

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenPosition>>> GetOpenPositionsAsync(IEnumerable<string>? transactionIds = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("docalcs", true);
            parameters.AddOptionalParameter("txid", transactionIds?.Any() == true ? string.Join(",", transactionIds) : null);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/OpenPositions", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenPosition>>(request, parameters, ct).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Get Ledger Info

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenLedgerPage>> GetLedgerInfoAsync(IEnumerable<string>? assets = null, IEnumerable<LedgerEntryType>? entryTypes = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("asset", assets != null ? string.Join(",", assets) : null);
            parameters.AddOptionalParameter("type", entryTypes != null ? string.Join(",", entryTypes.Select(EnumConverter.GetString)) : null);
            parameters.AddOptionalParameter("start", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.AddOptionalParameter("end", DateTimeConverter.ConvertToSeconds(endTime));
            parameters.AddOptionalParameter("ofs", resultOffset);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/Ledgers", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<KrakenLedgerPage>(request, parameters, ct).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data.Ledger)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Get Ledgers Entry

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenLedgerEntry>>> GetLedgersEntryAsync(IEnumerable<string>? ledgerIds = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("id", ledgerIds?.Any() == true ? string.Join(",", ledgerIds) : null);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/QueryLedgers", KrakenExchange.RateLimiter.SpotRest, 2, true);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenLedgerEntry>>(request, parameters, ct).ConfigureAwait(false);

            if (result)
            {
                foreach (var item in result.Data)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Get Trade Volume

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenTradeVolume>> GetTradeVolumeAsync(IEnumerable<string>? symbols = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("fee-info", true);
            parameters.AddOptionalParameter("pair", symbols?.Any() == true ? string.Join(",", symbols) : null);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/TradeVolume", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenTradeVolume>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deposit Methods

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenDepositMethod>>> GetDepositMethodsAsync(string asset, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            var parameters = new ParameterCollection()
            {
                {"asset", asset}
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/DepositMethods", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<IEnumerable<KrakenDepositMethod>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deposit Addresses

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenDepositAddress>>> GetDepositAddressesAsync(string asset, string depositMethod, bool generateNew = false, decimal? quantity = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            depositMethod.ValidateNotNull(nameof(depositMethod));
            var parameters = new ParameterCollection()
            {
                {"asset", asset},
                {"method", depositMethod }
            };

            if (generateNew)
                // If False is send it will still generate new, so only add it when it's true
                parameters.Add("new", true);

            parameters.AddOptionalParameter("amount", quantity);
            parameters.AddOptionalParameter("otp", twoFactorPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/DepositAddresses", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<IEnumerable<KrakenDepositAddress>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deposit Status

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenMovementStatus>>> GetDepositStatusAsync(string? asset = null, string? depositMethod = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("method", depositMethod);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/DepositStatus", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<IEnumerable<KrakenMovementStatus>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deposit History

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenCursorPage<KrakenMovementStatus>>> GetDepositHistoryAsync(
            string? asset = null,
            string? depositMethod = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            string? cursor = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("method", depositMethod);
            parameters.AddOptionalParameter("start", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.AddOptionalParameter("end", DateTimeConverter.ConvertToSeconds(endTime));
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("cursor", cursor ?? "true");
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/DepositStatus", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenCursorPage<KrakenMovementStatus>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Withdraw Info

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenWithdrawInfo>> GetWithdrawInfoAsync(string asset, string key, decimal quantity, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            key.ValidateNotNull(nameof(key));

            var parameters = new ParameterCollection
            {
                { "asset", asset },
                { "key", key },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
            };

            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/WithdrawInfo", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenWithdrawInfo>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Withdraw

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenWithdraw>> WithdrawAsync(string asset, string key, decimal quantity, string? address = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            key.ValidateNotNull(nameof(key));

            var parameters = new ParameterCollection
            {
                {"asset", asset},
                {"key", key},
                {"amount", quantity.ToString(CultureInfo.InvariantCulture)}
            };
            parameters.AddOptionalParameter("address", address);
            parameters.AddOptionalParameter("otp", twoFactorPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/Withdraw", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenWithdraw>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Withdraw Addresses

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenWithdrawAddress>>> GetWithdrawAddressesAsync(string? asset = null, string? aclass = null, string? method = null, string? key = null, bool? verified = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("aclass", aclass);
            parameters.AddOptionalParameter("method", method);
            parameters.AddOptionalParameter("key", key);
            parameters.AddOptionalParameter("verified", verified);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/WithdrawAddresses", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<IEnumerable<KrakenWithdrawAddress>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Withdraw Methods

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenWithdrawMethod>>> GetWithdrawMethodsAsync(string? asset = null, string? aclass = null, string? network = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("aclass", aclass);
            parameters.AddOptionalParameter("network", network);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/WithdrawMethods", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<IEnumerable<KrakenWithdrawMethod>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Withdrawal Status

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenMovementStatus>>> GetWithdrawalStatusAsync(string? asset = null, string? withdrawalMethod = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("method", withdrawalMethod);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/WithdrawStatus", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<IEnumerable<KrakenMovementStatus>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Withdrawal History

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenCursorPage<KrakenMovementStatus>>> GetWithdrawalHistoryAsync(
            string? asset = null,
            string? withdrawalMethod = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            string? cursor = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("method", withdrawalMethod);
            parameters.AddOptionalParameter("start", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.AddOptionalParameter("end", DateTimeConverter.ConvertToSeconds(endTime));
            parameters.AddOptionalParameter("limit", limit);
            parameters.AddOptionalParameter("cursor", cursor ?? "true");
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/WithdrawStatus", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenCursorPage<KrakenMovementStatus>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Withdrawal

        /// <inheritdoc />
        public async Task<WebCallResult<bool>> CancelWithdrawalAsync(string asset, string referenceId, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            referenceId.ValidateNotNull(nameof(referenceId));

            var parameters = new ParameterCollection
            {
                {"asset", asset},
                {"refid", referenceId}
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/WithdrawCancel", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<bool>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Transfer

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenReferenceId>> TransferAsync(string asset, decimal quantity, string fromWallet, string toWallet, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            fromWallet.ValidateNotNull(nameof(fromWallet));
            toWallet.ValidateNotNull(nameof(toWallet));

            var parameters = new ParameterCollection
            {
                {"asset", asset},
                {"from", fromWallet},
                {"to", toWallet},
                {"amount", quantity.ToString(CultureInfo.InvariantCulture)}
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/WalletTransfer", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenReferenceId>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Websocket Token

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenWebSocketToken>> GetWebsocketTokenAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/GetWebSocketsToken", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenWebSocketToken>(request, null, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
