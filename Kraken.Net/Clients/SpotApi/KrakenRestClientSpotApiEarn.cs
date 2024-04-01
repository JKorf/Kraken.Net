using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc />
    public class KrakenRestClientSpotApiEarn : IKrakenRestClientSpotApiEarn
    {
        private readonly KrakenRestClientSpotApi _baseClient;

        internal KrakenRestClientSpotApiEarn(KrakenRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

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
            return await _baseClient.Execute<KrakenCursorPage<KrakenEarnStrategy>>(_baseClient.GetUri("0/private/Earn/Strategies"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenAllocationsCursorPage>> GetAllocationsAsync(string? convertAsset = null, bool? hideZeroAllocations = null, bool? asc = null, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("converted_asset", convertAsset);
            parameters.AddOptional("hide_zero_allocations", hideZeroAllocations);
            parameters.AddOptional("asc", asc.HasValue ? asc == true : null);
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            return await _baseClient.Execute<KrakenAllocationsCursorPage>(_baseClient.GetUri("0/private/Earn/Allocations"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenEarnStatus>> GetAllocationStatusAsync(string strategyId, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "strategy_id", strategyId }
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            return await _baseClient.Execute<KrakenEarnStatus>(_baseClient.GetUri("0/private/Earn/AllocateStatus"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenEarnStatus>> GetDeallocationStatusAsync(string strategyId, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "strategy_id", strategyId }
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            return await _baseClient.Execute<KrakenEarnStatus>(_baseClient.GetUri("0/private/Earn/DeallocateStatus"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult> AllocateEarnFundsAsync(string strategyId, decimal quantity, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "strategy_id", strategyId },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            return await _baseClient.Execute(_baseClient.GetUri("0/private/Earn/Allocate"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebCallResult> DeallocateEarnFundsAsync(string strategyId, decimal quantity, string? twoFactorPassword = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "strategy_id", strategyId },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddOptionalParameter("otp", twoFactorPassword ?? _baseClient.ClientOptions.StaticTwoFactorAuthenticationPassword);
            return await _baseClient.Execute(_baseClient.GetUri("0/private/Earn/Deallocate"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }
    }
}
