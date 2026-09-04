using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Clients.SpotApi
{
    internal partial class KrakenRestClientSpotSharedApi
    {

        #region Get Spot Ticker

        async Task<ICallResult<SharedSpotTicker>> IGetSpotTicker.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
            => await GetSpotTickerAsync(request, ct).ConfigureAwait(false);

        public GetSpotTickerOptions GetSpotTickerOptions { get; } = new GetSpotTickerOptions(_exchangeName, SharedTickerType.Other);
        public async Task<HttpResult<SharedSpotTicker>> GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = GetSpotTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker>(Exchange, validationError);

            var result = await _api.ExchangeData.GetTickerAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker>(result);

            var ticker = result.Data.Single();
            return HttpResult.Ok(result,
                new SharedSpotTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, ticker.Value.Symbol),
                    ticker.Value.Symbol,
                    ticker.Value.LastTrade.Price,
                    ticker.Value.High.Value24H,
                    ticker.Value.Low.Value24H,
                    new SharedOrderQuantity(ticker.Value.Volume.Value24H, ticker.Value.VolumeWeightedAveragePrice.Value24H * ticker.Value.Volume.Value24H),
                    ticker.Value.OpenPrice == 0 ? null : Math.Round(ticker.Value.LastTrade.Price / ticker.Value.OpenPrice * 100 - 100, 2))
                {
                });
        }

        #endregion

        #region Get All Spot Tickers

        async Task<ICallResult<SharedSpotTicker[]>> IGetAllSpotTickers.GetAllSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
            => await GetAllSpotTickersAsync(request, ct).ConfigureAwait(false);

        Task<HttpResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
            => GetAllSpotTickersAsync(request, ct);
        GetAllSpotTickersOptions ISpotTickerRestClient.GetSpotTickersOptions => GetAllSpotTickersOptions;

        public GetAllSpotTickersOptions GetAllSpotTickersOptions { get; } = new GetAllSpotTickersOptions(_exchangeName, SharedTickerType.Other);
        public async Task<HttpResult<SharedSpotTicker[]>> GetAllSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllSpotTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker[]>(Exchange, validationError);

            var result = await _api.ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x =>
            new SharedSpotTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Value.Symbol),
                x.Value.Symbol,
                x.Value.LastTrade.Price,
                x.Value.High.Value24H,
                x.Value.Low.Value24H,
                new SharedOrderQuantity(x.Value.Volume.Value24H, x.Value.VolumeWeightedAveragePrice.Value24H * x.Value.Volume.Value24H),
                x.Value.OpenPrice == 0 ? null : Math.Round(x.Value.LastTrade.Price / x.Value.OpenPrice * 100 - 100, 2))
            {
            }).ToArray());
        }

        #endregion

    }
}
