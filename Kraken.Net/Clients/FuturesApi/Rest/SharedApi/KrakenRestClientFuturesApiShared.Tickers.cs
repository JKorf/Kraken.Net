using CryptoExchange.Net.SharedApis;

namespace Kraken.Net.Clients.FuturesApi
{
    internal partial class KrakenRestClientFuturesSharedApi
    {

        #region Get Ticker

        async Task<ICallResult<SharedTicker>> IGetTicker.GetTickerAsync(GetTickerRequest request, CancellationToken ct)
            => await ((IGetTickerRest)this).GetTickerAsync(request, ct).ConfigureAwait(false);

        async Task<HttpResult<SharedTicker>> IGetTickerRest.GetTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var result = await GetFuturesTickerAsync(request, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTicker>(result);

            return HttpResult.Ok<SharedTicker>(result, result.Data);
        }

        GetTickerOptions IFuturesTickerRestClient.GetFuturesTickerOptions => GetTickerOptions;

        public GetTickerOptions GetTickerOptions { get; } = new GetTickerOptions(_exchangeName);
        public async Task<HttpResult<SharedFuturesTicker>> GetFuturesTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = GetTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTicker>(Exchange, validationError);

            var resultTicker = await _api.ExchangeData.GetTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedFuturesTicker>(resultTicker);

            var time = DateTime.UtcNow;

            return HttpResult.Ok(resultTicker,
                new SharedFuturesTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, resultTicker.Data.Symbol),
                    resultTicker.Data.Symbol,
                    resultTicker.Data.LastPrice,
                    null,
                    null,
                    new SharedOrderQuantity(resultTicker.Data.Volume24h, resultTicker.Data.Volume24hQuote),
                    (resultTicker.Data.OpenPrice24h > 0 && resultTicker.Data.LastPrice > 0) ? Math.Round(resultTicker.Data.LastPrice.Value / resultTicker.Data.OpenPrice24h.Value * 100 - 100, 2) : null)
                {
                    MarkPrice = resultTicker.Data.MarkPrice,
                    IndexPrice = resultTicker.Data.IndexPrice,
                    FundingRate = resultTicker.Data.FundingRate,
                    NextFundingTime = resultTicker.Data.FundingRate == null ? null : new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0, DateTimeKind.Utc).AddHours(1)
                });
        }

        #endregion

        #region Get All Tickers

        async Task<ICallResult<SharedTicker[]>> IGetAllTickers.GetAllTickersAsync(GetTickersRequest request, CancellationToken ct)
            => await ((IGetAllTickersRest)this).GetAllTickersAsync(request, ct).ConfigureAwait(false);

        async Task<HttpResult<SharedTicker[]>> IGetAllTickersRest.GetAllTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var result = await GetAllFuturesTickersAsync(request, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTicker[]>(result);

            return HttpResult.Ok<SharedTicker[]>(result, result.Data);
        }

        Task<HttpResult<SharedFuturesTicker[]>> IFuturesTickerRestClient.GetFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
            => GetAllFuturesTickersAsync(request, ct);
        GetAllTickersOptions IFuturesTickerRestClient.GetFuturesTickersOptions => GetAllTickersOptions;

        public GetAllTickersOptions GetAllTickersOptions { get; } = new GetAllTickersOptions(_exchangeName);
        public async Task<HttpResult<SharedFuturesTicker[]>> GetAllFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTicker[]>(Exchange, validationError);

            var resultTicker = await _api.ExchangeData.GetTickersAsync(ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedFuturesTicker[]>(resultTicker);

            var data = resultTicker.Data.Where(x => !x.Symbol.StartsWith("rr_") && !x.Symbol.StartsWith("in_"));
            if (request.TradingMode != null)
            {
                data = data.Where(x => request.TradingMode == TradingMode.PerpetualLinear ? x.Symbol.StartsWith("PF_") :
                    request.TradingMode == TradingMode.DeliveryLinear ? x.Symbol.StartsWith("FI_") :
                    request.TradingMode == TradingMode.PerpetualInverse ? x.Symbol.StartsWith("PI_") :
                    x.Symbol.StartsWith("FF"));
            }

            var time = DateTime.UtcNow;
            return HttpResult.Ok(resultTicker, data.Select(x =>
            new SharedFuturesTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                x.Symbol,
                x.LastPrice,
                null,
                null,
                new SharedOrderQuantity(x.Volume24h, x.Volume24hQuote),
                (x.OpenPrice24h > 0 && x.LastPrice > 0) ? Math.Round(x.LastPrice.Value / x.OpenPrice24h.Value * 100 - 100, 2) : null)
            {
                MarkPrice = x.MarkPrice,
                IndexPrice = x.IndexPrice,
                FundingRate = x.FundingRate,
                NextFundingTime = x.FundingRate == null ? null : new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0, DateTimeKind.Utc).AddHours(1)
            }).ToArray());
        }

        #endregion

    }
}
