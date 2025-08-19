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
        public async Task<WebCallResult<KrakenFuturesBalances>> GetBalancesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/accounts", KrakenExchange.RateLimiter.FuturesApi, 2, true);
            return await _baseClient.SendAsync<KrakenBalancesResult, KrakenFuturesBalances>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Pnl Currency

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesPnlCurrency[]>> GetPnlCurrencyAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/pnlpreferences", KrakenExchange.RateLimiter.FuturesApi, 2, true);
            return await _baseClient.SendAsync<KrakenFuturesPnlCurrencyResult, KrakenFuturesPnlCurrency[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Set Pnl Currency

        /// <inheritdoc />
        public async Task<WebCallResult> SetPnlCurrencyAsync(string symbol, string pnlCurrency, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "pnlPreference", pnlCurrency },
                { "symbol", symbol }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Put, "derivatives/api/v3/pnlpreferences", KrakenExchange.RateLimiter.FuturesApi, 10, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Transfer

        /// <inheritdoc />
        public async Task<WebCallResult> TransferAsync(
            string asset, decimal quantity, string fromAccount, string toAccount, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection()
            {
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
                { "fromAccount", fromAccount },
                { "toAccount", toAccount },
                { "unit", asset }
            };

            var request = _definitions.GetOrCreate(HttpMethod.Post, "derivatives/api/v3/transfer", KrakenExchange.RateLimiter.FuturesApi, 10, true);
            return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Account Log

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenAccountLogResult>> GetAccountLogAsync(DateTime? startTime = null, DateTime? endTime = null, int? fromId = null, int? toId = null, string? sort = null, string? type = null, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalParameter("before", DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("count", limit);
            parameters.AddOptionalParameter("from", fromId);
            parameters.AddOptionalParameter("info", type);
            parameters.AddOptionalParameter("since", DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("sort", sort);
            parameters.AddOptionalParameter("to", toId);

            var weight = limit == null ? 3 : limit <= 25 ? 1 : limit <= 50 ? 2 : limit <= 1000 ? 3 : limit <= 5000 ? 6 : 10;
            var request = _definitions.GetOrCreate(HttpMethod.Get, "api/history/v3/account-log", KrakenExchange.RateLimiter.FuturesApi, weight, true);
            return await _baseClient.SendRawAsync<KrakenAccountLogResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Fee Schedule Volume

        /// <inheritdoc />
        public async Task<WebCallResult<Dictionary<string, decimal>>> GetFeeScheduleVolumeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/feeschedules/volumes", KrakenExchange.RateLimiter.FuturesApi, 1, true);
            return await _baseClient.SendAsync<KrakenFeeScheduleVolumeResult, Dictionary<string, decimal>>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Initial Margin Requirements

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesMarginRequirements>> GetInitialMarginRequirementsAsync(string symbol, FuturesOrderType orderType, OrderSide side, decimal quantity, decimal? price = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddEnum("orderType", orderType);
            parameters.AddEnum("side", side);
            parameters.AddString("size", quantity);
            parameters.AddOptionalString("limitPrice", price);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/initialmargin", KrakenExchange.RateLimiter.FuturesApi, 1, true);
            var result = await _baseClient.SendRawAsync<KrakenFuturesMarginRequirementsInternal> (request, null, ct).ConfigureAwait(false);
            if (!result)
                return result.As<KrakenFuturesMarginRequirements>(null);

            if (result.Data.Error != null)
                return result.AsError<KrakenFuturesMarginRequirements>(new ServerError(ErrorInfo.Unknown with { Message = result.Data.Error }));

            return result.As(new KrakenFuturesMarginRequirements
            {
                InitialMargin = result.Data.InitialMargin,
                Price = result.Data.Price
            });
        }

        #endregion

        #region Get Max Order Quantity

        /// <inheritdoc />
        public async Task<WebCallResult<KrakenFuturesMaxOrderSize>> GetMaxOrderQuantityAsync(string symbol, FuturesOrderType orderType, decimal? price = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("symbol", symbol);
            parameters.AddEnum("orderType", orderType);
            parameters.AddOptionalString("limitPrice", price);

            var request = _definitions.GetOrCreate(HttpMethod.Get, "derivatives/api/v3/initialmargin/maxordersize", KrakenExchange.RateLimiter.FuturesApi, 1, true);
            var result = await _baseClient.SendRawAsync<KrakenFuturesMaxOrderSizeInternal>(request, null, ct).ConfigureAwait(false);
            if (!result)
                return result.As<KrakenFuturesMaxOrderSize>(null);

            if (result.Data.Error != null)
                return result.AsError<KrakenFuturesMaxOrderSize>(new ServerError(ErrorInfo.Unknown with { Message = result.Data.Error }));

            return result.As(new KrakenFuturesMaxOrderSize
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
