using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects.Models;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class KrakenRestClientSpotStakingApi : IKrakenClientSpotStakingApi
    {
        private readonly KrakenRestClientSpotApi _baseClient;

        internal KrakenRestClientSpotStakingApi(KrakenRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenStakeResponse>> StakeAsync(
            string asset,
            decimal amount,
            string method,
            string? twoFactorPassword = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("amount", amount.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("method", method);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            return await _baseClient.Execute<KrakenStakeResponse>(_baseClient.GetUri("0/private/Stake"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenUnstakeResponse>> UnstakeAsync(
            string asset,
            decimal amount,
            string method,
            string? twoFactorPassword = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("asset", asset);
            parameters.AddOptionalParameter("amount", amount.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("method", method);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            return await _baseClient.Execute<KrakenUnstakeResponse>(_baseClient.GetUri("0/private/Unstake"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenStakingTransaction>>> GetPendingTransactionsAsync(
            string? twoFactorPassword = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            return await _baseClient.Execute<IEnumerable<KrakenStakingTransaction>>(_baseClient.GetUri("0/private/Staking/Pending"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenStakingTransaction>>> GetRecentTransactionsAsync(string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            return await _baseClient.Execute<IEnumerable<KrakenStakingTransaction>>(_baseClient.GetUri("0/private/Staking/Transactions"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<KrakenStakingAsset>>> GetStakableAssets(string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            return await _baseClient.Execute<IEnumerable<KrakenStakingAsset>>(_baseClient.GetUri("0/private/Staking/Assets"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }
    }
}
