using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects.Models;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class KrakenRestClientSpotApiEarn : IKrakenRestClientSpotApiEarn
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly KrakenRestClientSpotApi _baseClient;

        internal KrakenRestClientSpotApiEarn(KrakenRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Strategies

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenCursorPage<KrakenEarnStrategy>>> GetStrategiesAsync(string? asset = null, LockType? lockType = null, string? cursor = null, int? limit = null, bool? asc = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("asset", asset);
            parameters.AddOptional("cursor", cursor);
            parameters.AddOptionalEnum("lock_type", lockType);
            parameters.AddOptional("limit", limit);
            parameters.AddOptional("asc", asc.HasValue ? asc == true : null);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/Earn/Strategies", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenCursorPage<KrakenEarnStrategy>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Allocations

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenAllocationsCursorPage>> GetAllocationsAsync(string? convertAsset = null, bool? hideZeroAllocations = null, bool? asc = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("converted_asset", convertAsset);
            parameters.AddOptional("hide_zero_allocations", hideZeroAllocations);
            parameters.AddOptional("asc", asc.HasValue ? asc == true : null);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/Earn/Allocations", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenAllocationsCursorPage>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Allocation Status

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenEarnStatus>> GetAllocationStatusAsync(string strategyId, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "strategy_id", strategyId }
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/Earn/AllocateStatus", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenEarnStatus>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deallocation Status

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenEarnStatus>> GetDeallocationStatusAsync(string strategyId, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "strategy_id", strategyId }
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/Earn/DeallocateStatus", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenEarnStatus>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Allocate Earn Funds

        /// <inheritdoc />
        public async Task<WebCallResult> AllocateEarnFundsAsync(string strategyId, decimal quantity, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "strategy_id", strategyId },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/Earn/Allocate", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Deallocate Earn Funds

        /// <inheritdoc />
        public async Task<WebCallResult> DeallocateEarnFundsAsync(string strategyId, decimal quantity, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "strategy_id", strategyId },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, "0/private/Earn/Deallocate", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
