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
        public async Task<HttpResult<KrakenCursorPage<KrakenEarnStrategy>>> GetStrategiesAsync(string? asset = null, LockType? lockType = null, string? cursor = null, int? limit = null, bool? asc = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("asset", asset);
            parameters.Add("cursor", cursor);
            parameters.Add("lock_type", lockType);
            parameters.Add("limit", limit);
            parameters.Add("asc", asc.HasValue ? asc == true : null);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/Earn/Strategies", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenCursorPage<KrakenEarnStrategy>>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Allocations

        /// <inheritdoc />
        public async Task<HttpResult<KrakenAllocationsCursorPage>> GetAllocationsAsync(string? convertAsset = null, bool? hideZeroAllocations = null, bool? asc = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("converted_asset", convertAsset);
            parameters.Add("hide_zero_allocations", hideZeroAllocations);
            parameters.Add("asc", asc.HasValue ? asc == true : null);
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/Earn/Allocations", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenAllocationsCursorPage>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Allocation Status

        /// <inheritdoc />
        public async Task<HttpResult<KrakenEarnStatus>> GetAllocationStatusAsync(string strategyId, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                { "strategy_id", strategyId }
            };
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/Earn/AllocateStatus", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenEarnStatus>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deallocation Status

        /// <inheritdoc />
        public async Task<HttpResult<KrakenEarnStatus>> GetDeallocationStatusAsync(string strategyId, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                { "strategy_id", strategyId }
            };
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/Earn/DeallocateStatus", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync<KrakenEarnStatus>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Allocate Earn Funds

        /// <inheritdoc />
        public async Task<HttpResult> AllocateEarnFundsAsync(string strategyId, decimal quantity, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                { "strategy_id", strategyId },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/Earn/Allocate", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Deallocate Earn Funds

        /// <inheritdoc />
        public async Task<HttpResult> DeallocateEarnFundsAsync(string strategyId, decimal quantity, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                { "strategy_id", strategyId },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.Add("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "0/private/Earn/Deallocate", KrakenExchange.RateLimiter.SpotRest, 1, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
