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
        public async Task<HttpResult<Dictionary<string, decimal>>> GetBalancesAsync(bool? newAssetNameResponse = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("assetVersion", newAssetNameResponse);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/Balance", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<Dictionary<string, decimal>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Available Balances

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, KrakenBalanceAvailable>>> GetAvailableBalancesAsync(string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/BalanceEx", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenBalanceAvailable>>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success)
            {
                foreach (var item in result.Data)
                    item.Value.Asset = item.Key;
            }

            return result;
        }

        #endregion

        #region Get Trade Balances

        /// <inheritdoc />
        public async Task<HttpResult<KrakenTradeBalance>> GetTradeBalanceAsync(string? baseAsset = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("aclass", "currency");
            parameters.Add("asset", baseAsset);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/TradeBalance", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenTradeBalance>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Open Positions

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, KrakenPosition>>> GetOpenPositionsAsync(IEnumerable<string>? transactionIds = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("docalcs", true);
            parameters.Add("txid", transactionIds?.Any() == true ? string.Join(",", transactionIds) : null);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/OpenPositions", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenPosition>>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success)
            {
                foreach (var item in result.Data)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Get Ledger Info

        /// <inheritdoc />
        public async Task<HttpResult<KrakenLedgerPage>> GetLedgerInfoAsync(
            IEnumerable<string>? assets = null, 
            IEnumerable<LedgerEntryType>? entryTypes = null, 
            AClass? assetClass = null,
            DateTime? startTime = null,
            DateTime? endTime = null, 
            int? resultOffset = null, 
            string? twoFactorPassword = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("asset", assets != null ? string.Join(",", assets) : null);
            parameters.Add("type", entryTypes != null ? string.Join(",", entryTypes.Select(EnumConverter.GetString)) : null);
            parameters.Add("start", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.Add("end", DateTimeConverter.ConvertToSeconds(endTime));
            parameters.Add("ofs", resultOffset);
            parameters.Add("aClass", assetClass);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/Ledgers", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<KrakenLedgerPage>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success)
            {
                foreach (var item in result.Data.Ledger)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Get Ledgers Entry

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, KrakenLedgerEntry>>> GetLedgersEntryAsync(IEnumerable<string>? ledgerIds = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("id", ledgerIds?.Any() == true ? string.Join(",", ledgerIds) : null);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/QueryLedgers", KrakenExchange.RateLimiter.SpotRest, 2, true);
            var result = await _baseClient.SendAsync<Dictionary<string, KrakenLedgerEntry>>(request, parameters, ct).ConfigureAwait(false);
            if (result.Success)
            {
                foreach (var item in result.Data)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        #endregion

        #region Get Trade Volume

        /// <inheritdoc />
        public async Task<HttpResult<KrakenTradeVolume>> GetTradeVolumeAsync(IEnumerable<string>? symbols = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("fee-info", true);
            parameters.Add("pair", symbols?.Any() == true ? string.Join(",", symbols) : null);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/TradeVolume", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenTradeVolume>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deposit Methods

        /// <inheritdoc />
        public async Task<HttpResult<KrakenDepositMethod[]>> GetDepositMethodsAsync(string asset, AClass? assetClass = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                {"asset", asset}
            };
            parameters.Add("aclass", assetClass);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/DepositMethods", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenDepositMethod[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deposit Addresses

        /// <inheritdoc />
        public async Task<HttpResult<KrakenDepositAddress[]>> GetDepositAddressesAsync(string asset, string depositMethod, bool generateNew = false, decimal? quantity = null, AClass? assetClass = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            depositMethod.ValidateNotNull(nameof(depositMethod));
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                {"asset", asset},
                {"method", depositMethod }
            };

            parameters.Add("aclass", assetClass);
            if (generateNew)
                // If False is send it will still generate new, so only add it when it's true
                parameters.Add("new", true);

            parameters.Add("amount", quantity);
            parameters.Add("otp", twoFactorPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/DepositAddresses", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenDepositAddress[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deposit Status

        /// <inheritdoc />
        public async Task<HttpResult<KrakenMovementStatus[]>> GetDepositStatusAsync(string? asset = null, string? depositMethod = null, AClass? assetClass = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("asset", asset);
            parameters.Add("method", depositMethod);
            parameters.Add("aclass", assetClass);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/DepositStatus", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenMovementStatus[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deposit History

        /// <inheritdoc />
        public async Task<HttpResult<KrakenCursorPage<KrakenMovementStatus>>> GetDepositHistoryAsync(
            string? asset = null,
            string? depositMethod = null,
            AClass? assetClass = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            string? cursor = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("asset", asset);
            parameters.Add("method", depositMethod);
            parameters.Add("start", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.Add("end", DateTimeConverter.ConvertToSeconds(endTime));
            parameters.Add("limit", limit);
            parameters.Add("aclass", assetClass);
            parameters.Add("cursor", cursor ?? "true");
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/DepositStatus", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenCursorPage<KrakenMovementStatus>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Withdraw Info

        /// <inheritdoc />
        public async Task<HttpResult<KrakenWithdrawInfo>> GetWithdrawInfoAsync(string asset, string key, decimal quantity, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            key.ValidateNotNull(nameof(key));

            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                { "asset", asset },
                { "key", key },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
            };

            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/WithdrawInfo", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenWithdrawInfo>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Withdraw

        /// <inheritdoc />
        public async Task<HttpResult<KrakenWithdraw>> WithdrawAsync(string asset, string key, decimal quantity, string? address = null, AClass? assetClass = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            key.ValidateNotNull(nameof(key));

            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                {"asset", asset},
                {"key", key},
                {"amount", quantity.ToString(CultureInfo.InvariantCulture)}
            };
            parameters.Add("address", address);
            parameters.Add("aclass", assetClass);
            parameters.Add("otp", twoFactorPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/Withdraw", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenWithdraw>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Withdraw Addresses

        /// <inheritdoc />
        public async Task<HttpResult<KrakenWithdrawAddress[]>> GetWithdrawAddressesAsync(string? asset = null, AClass? assetClass = null, string? method = null, string? key = null, bool? verified = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("asset", asset);
            parameters.Add("aclass", assetClass);
            parameters.Add("method", method);
            parameters.Add("key", key);
            parameters.Add("verified", verified);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/WithdrawAddresses", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenWithdrawAddress[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Withdraw Methods

        /// <inheritdoc />
        public async Task<HttpResult<KrakenWithdrawMethod[]>> GetWithdrawMethodsAsync(string? asset = null, AClass? assetClass = null, string? network = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("asset", asset);
            parameters.Add("aclass", assetClass);
            parameters.Add("network", network);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/WithdrawMethods", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenWithdrawMethod[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Withdrawal Status

        /// <inheritdoc />
        public async Task<HttpResult<KrakenMovementStatus[]>> GetWithdrawalStatusAsync(string? asset = null, string? withdrawalMethod = null, AClass? assetClass = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("asset", asset);
            parameters.Add("method", withdrawalMethod);
            parameters.Add("aclass", assetClass);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/WithdrawStatus", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenMovementStatus[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Withdrawal History

        /// <inheritdoc />
        public async Task<HttpResult<KrakenCursorPage<KrakenMovementStatus>>> GetWithdrawalHistoryAsync(
            string? asset = null,
            string? withdrawalMethod = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            string? cursor = null,
            string? twoFactorPassword = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("asset", asset);
            parameters.Add("method", withdrawalMethod);
            parameters.Add("start", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.Add("end", DateTimeConverter.ConvertToSeconds(endTime));
            parameters.Add("limit", limit);
            parameters.Add("cursor", cursor ?? "true");
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/WithdrawStatus", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenCursorPage<KrakenMovementStatus>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Withdrawal

        /// <inheritdoc />
        public async Task<HttpResult<bool>> CancelWithdrawalAsync(string asset, string referenceId, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            referenceId.ValidateNotNull(nameof(referenceId));

            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                {"asset", asset},
                {"refid", referenceId}
            };
            parameters.Add("otp", twoFactorPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/WithdrawCancel", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<bool>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Transfer

        /// <inheritdoc />
        public async Task<HttpResult<KrakenReferenceId>> TransferAsync(string asset, decimal quantity, string fromWallet, string toWallet, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            fromWallet.ValidateNotNull(nameof(fromWallet));
            toWallet.ValidateNotNull(nameof(toWallet));

            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                {"asset", asset},
                {"from", fromWallet},
                {"to", toWallet},
                {"amount", quantity.ToString(CultureInfo.InvariantCulture)}
            };
            parameters.Add("otp", twoFactorPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/WalletTransfer", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenReferenceId>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Api Key Info

        /// <inheritdoc />
        public async Task<HttpResult<KrakenApiKey>> GetApiKeyInfoAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/GetApiKeyInfo", KrakenExchange.RateLimiter.SpotRest, 1, true);
            var result = await _baseClient.SendAsync<KrakenApiKey>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Websocket Token

        /// <inheritdoc />
        public async Task<HttpResult<KrakenWebSocketToken>> GetWebsocketTokenAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/GetWebSocketsToken", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenWebSocketToken>(request, null, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
