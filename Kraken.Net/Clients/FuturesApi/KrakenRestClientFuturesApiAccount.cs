using Kraken.Net.Objects.Models.Futures;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Enums;
using CryptoExchange.Net.Objects.Errors;

namespace Kraken.Net.Clients.FuturesApi
{
    /// <inheritdoc />
    internal class KrakenRestClientFuturesApiAccount : IKrakenRestClientFuturesApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly KrakenRestClientFuturesApi _baseClient;

        internal KrakenRestClientFuturesApiAccount(KrakenRestClientFuturesApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<HttpResult<KrakenFuturesBalances>> GetBalancesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "derivatives/api/v3/accounts", KrakenExchange.RateLimiter.FuturesApi, 2, true);
            return await _baseClient.SendAsync<KrakenBalancesResult, KrakenFuturesBalances>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Pnl Currency

        /// <inheritdoc />
        public async Task<HttpResult<KrakenFuturesPnlCurrency[]>> GetPnlCurrencyAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "derivatives/api/v3/pnlpreferences", KrakenExchange.RateLimiter.FuturesApi, 2, true);
            return await _baseClient.SendAsync<KrakenFuturesPnlCurrencyResult, KrakenFuturesPnlCurrency[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Pnl Currency

        /// <inheritdoc />
        public async Task<HttpResult> SetPnlCurrencyAsync(string symbol, string pnlCurrency, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                { "pnlPreference", pnlCurrency },
                { "symbol", symbol }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Put, _baseClient.BaseAddress, "derivatives/api/v3/pnlpreferences", KrakenExchange.RateLimiter.FuturesApi, 10, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Transfer

        /// <inheritdoc />
        public async Task<HttpResult> TransferAsync(
            string asset, decimal quantity, string fromAccount, string toAccount, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings)
            {
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
                { "fromAccount", fromAccount },
                { "toAccount", toAccount },
                { "unit", asset }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "derivatives/api/v3/transfer", KrakenExchange.RateLimiter.FuturesApi, 10, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Account Log

        /// <inheritdoc />
        public async Task<HttpResult<KrakenAccountLogResult>> GetAccountLogAsync(DateTime? startTime = null, DateTime? endTime = null, int? fromId = null, int? toId = null, string? sort = null, string? type = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("before", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.Add("count", limit);
            parameters.Add("from", fromId);
            parameters.Add("info", type);
            parameters.Add("since", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.Add("sort", sort);
            parameters.Add("to", toId);

            var weight = limit == null ? 3 : limit <= 25 ? 1 : limit <= 50 ? 2 : limit <= 1000 ? 3 : limit <= 5000 ? 6 : 10;
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/history/v3/account-log", KrakenExchange.RateLimiter.FuturesApi, weight, true);
            return await _baseClient.SendRawAsync<KrakenAccountLogResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Fee Schedule Volume

        /// <inheritdoc />
        public async Task<HttpResult<Dictionary<string, decimal>>> GetFeeScheduleVolumeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "derivatives/api/v3/feeschedules/volumes", KrakenExchange.RateLimiter.FuturesApi, 1, true);
            return await _baseClient.SendAsync<KrakenFeeScheduleVolumeResult, Dictionary<string, decimal>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Initial Margin Requirements

        /// <inheritdoc />
        public async Task<HttpResult<KrakenFuturesMarginRequirements>> GetInitialMarginRequirementsAsync(string symbol, FuturesOrderType orderType, OrderSide side, decimal quantity, decimal? price = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("orderType", orderType);
            parameters.Add("side", side);
            parameters.Add("size", quantity);
            parameters.Add("limitPrice", price);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "derivatives/api/v3/initialmargin", KrakenExchange.RateLimiter.FuturesApi, 1, true);
            var result = await _baseClient.SendRawAsync<KrakenFuturesMarginRequirementsInternal> (request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<KrakenFuturesMarginRequirements>(result);

            if (result.Data.Error != null)
                return HttpResult.Fail<KrakenFuturesMarginRequirements>(result, new ServerError(ErrorInfo.Unknown with { Message = result.Data.Error }));

            return HttpResult.Ok(result, new KrakenFuturesMarginRequirements
            {
                InitialMargin = result.Data.InitialMargin,
                Price = result.Data.Price
            });
        }

        #endregion

        #region Get Max Order Quantity

        /// <inheritdoc />
        public async Task<HttpResult<KrakenFuturesMaxOrderSize>> GetMaxOrderQuantityAsync(string symbol, FuturesOrderType orderType, decimal? price = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("orderType", orderType);
            parameters.Add("limitPrice", price);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "derivatives/api/v3/initialmargin/maxordersize", KrakenExchange.RateLimiter.FuturesApi, 1, true);
            var result = await _baseClient.SendRawAsync<KrakenFuturesMaxOrderSizeInternal>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<KrakenFuturesMaxOrderSize>(result);

            if (result.Data.Error != null)
                return HttpResult.Fail<KrakenFuturesMaxOrderSize>(result, new ServerError(ErrorInfo.Unknown with { Message = result.Data.Error }));

            return HttpResult.Ok(result, new KrakenFuturesMaxOrderSize
            {
                BuyPrice = result.Data.BuyPrice,
                SellPrice = result.Data.SellPrice,
                MaxBuyQuantity = result.Data.MaxBuyQuantity,
                MaxSellQuantity = result.Data.MaxSellQuantity
            });
        }

        #endregion
    }
}
