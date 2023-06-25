using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using Kraken.Net.Converters;
using Kraken.Net.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Interfaces.Clients.SpotApi;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class KrakenRestClientSpotApiAccount : IKrakenClientSpotApiAccount
    {
        private readonly KrakenRestClientSpotApi _baseClient;

        internal KrakenRestClientSpotApiAccount(KrakenRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }


        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, decimal>>> GetBalancesAsync(string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            return await _baseClient.Execute<Dictionary<string, decimal>>(_baseClient.GetUri("0/private/Balance"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenBalanceAvailable>>> GetAvailableBalancesAsync(string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            return await _baseClient.Execute<Dictionary<string, KrakenBalanceAvailable>>(_baseClient.GetUri("0/private/BalanceEx"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenTradeBalance>> GetTradeBalanceAsync(string? baseAsset = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("aclass", "currency");
            parameters.AddOptionalParameter("asset", baseAsset);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            return await _baseClient.Execute<KrakenTradeBalance>(_baseClient.GetUri("0/private/TradeBalance"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }


        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenPosition>>> GetOpenPositionsAsync(IEnumerable<string>? transactionIds = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("docalcs", true);
            parameters.AddOptionalParameter("txid", transactionIds?.Any() == true ? string.Join(",", transactionIds) : null);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var result = await _baseClient.Execute<Dictionary<string, KrakenPosition>>(_baseClient.GetUri("0/private/OpenPositions"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenLedgerPage>> GetLedgerInfoAsync(IEnumerable<string>? assets = null, IEnumerable<LedgerEntryType>? entryTypes = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("asset", assets != null ? string.Join(",", assets) : null);
            parameters.AddOptionalParameter("type", entryTypes != null ? string.Join(",", entryTypes.Select(e => JsonConvert.SerializeObject(e, new LedgerEntryTypeConverter(false)))) : null);
            parameters.AddOptionalParameter("start", DateTimeConverter.ConvertToSeconds(startTime));
            parameters.AddOptionalParameter("end", DateTimeConverter.ConvertToSeconds(endTime));
            parameters.AddOptionalParameter("ofs", resultOffset);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var result = await _baseClient.Execute<KrakenLedgerPage>(_baseClient.GetUri("0/private/Ledgers"), HttpMethod.Post, ct, parameters, true, weight: 2).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data.Ledger)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, KrakenLedgerEntry>>> GetLedgersEntryAsync(IEnumerable<string>? ledgerIds = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("id", ledgerIds?.Any() == true ? string.Join(",", ledgerIds) : null);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            var result = await _baseClient.Execute<Dictionary<string, KrakenLedgerEntry>>(_baseClient.GetUri("0/private/QueryLedgers"), HttpMethod.Post, ct, parameters, true, weight: 2).ConfigureAwait(false);
            if (result)
            {
                foreach (var item in result.Data)
                    item.Value.Id = item.Key;
            }
            return result;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenTradeVolume>> GetTradeVolumeAsync(IEnumerable<string>? symbols = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("fee-info", true);
            parameters.AddOptionalParameter("pair", symbols?.Any() == true ? string.Join(",", symbols) : null);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            return await _baseClient.Execute<KrakenTradeVolume>(_baseClient.GetUri("0/private/TradeVolume"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenDepositMethod>>> GetDepositMethodsAsync(string asset, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            var parameters = new Dictionary<string, object>()
            {
                {"asset", asset}
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            return await _baseClient.Execute<IEnumerable<KrakenDepositMethod>>(_baseClient.GetUri("0/private/DepositMethods"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenDepositAddress>>> GetDepositAddressesAsync(string asset, string depositMethod, bool generateNew = false, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            depositMethod.ValidateNotNull(nameof(depositMethod));
            var parameters = new Dictionary<string, object>()
            {
                {"asset", asset},
                {"method", depositMethod},
            };

            if (generateNew)
                // If False is send it will still generate new, so only add it when it's true
                parameters.Add("new", true);

            parameters.AddOptionalParameter("otp", twoFactorPassword);

            return await _baseClient.Execute<IEnumerable<KrakenDepositAddress>>(_baseClient.GetUri("0/private/DepositAddresses"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenMovementStatus>>> GetDepositStatusAsync(string? asset = null, string? depositMethod = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("method", depositMethod);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            return await _baseClient.Execute<IEnumerable<KrakenMovementStatus>>(_baseClient.GetUri("0/private/DepositStatus"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenWithdrawInfo>> GetWithdrawInfoAsync(string asset, string key, decimal quantity, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            key.ValidateNotNull(nameof(key));

            var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "key", key },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
            };

            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            return await _baseClient.Execute<KrakenWithdrawInfo>(_baseClient.GetUri("0/private/WithdrawInfo"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenWithdraw>> WithdrawAsync(string asset, string key, decimal quantity, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            key.ValidateNotNull(nameof(key));

            var parameters = new Dictionary<string, object>
            {
                {"asset", asset},
                {"key", key},
                {"amount", quantity.ToString(CultureInfo.InvariantCulture)}
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword);

            return await _baseClient.Execute<KrakenWithdraw>(_baseClient.GetUri("0/private/Withdraw"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenMovementStatus>>> GetWithdrawalStatusAsync(string? asset = null, string? withdrawalMethod = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("method", withdrawalMethod);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            return await _baseClient.Execute<IEnumerable<KrakenMovementStatus>>(_baseClient.GetUri("0/private/WithdrawStatus"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<bool>> CancelWithdrawalAsync(string asset, string referenceId, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            referenceId.ValidateNotNull(nameof(referenceId));

            var parameters = new Dictionary<string, object>
            {
                {"asset", asset},
                {"refid", referenceId}
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword);

            return await _baseClient.Execute<bool>(_baseClient.GetUri("0/private/WithdrawCancel"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenReferenceId>> TransferAsync(string asset, decimal quantity, string fromWallet, string toWallet, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            asset.ValidateNotNull(nameof(asset));
            fromWallet.ValidateNotNull(nameof(fromWallet));
            toWallet.ValidateNotNull(nameof(toWallet));

            var parameters = new Dictionary<string, object>
            {
                {"asset", asset},
                {"from", fromWallet},
                {"to", toWallet},
                {"amount", quantity.ToString(CultureInfo.InvariantCulture)}
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword);

            return await _baseClient.Execute<KrakenReferenceId>(_baseClient.GetUri("0/private/WalletTransfer"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenWebSocketToken>> GetWebsocketTokenAsync(CancellationToken ct = default)
        {
            return await _baseClient.Execute<KrakenWebSocketToken>(_baseClient.GetUri("0/private/GetWebSocketsToken"), HttpMethod.Post, ct, null, true).ConfigureAwait(false);
        }

    }
}
