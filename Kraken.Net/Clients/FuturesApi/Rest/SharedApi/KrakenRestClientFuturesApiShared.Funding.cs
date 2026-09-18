using CryptoExchange.Net.SharedApis;
using Kraken.Net.Objects.Models.Futures;

namespace Kraken.Net.Clients.FuturesApi
{
	internal partial class KrakenRestClientFuturesSharedApi
    {

        #region Get Funding Rate History

        async Task<IExchangeCallResult<SharedFundingRate[]>> IGetFundingRateHistory.GetFundingRateHistoryAsync(GetFundingRateHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetFundingRateHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        public GetFundingRateHistoryOptions GetFundingRateHistoryOptions { get; } = new GetFundingRateHistoryOptions(_exchangeName, true, true, true, 10000, false);

        public async Task<HttpResult<SharedFundingRate[]>> GetFundingRateHistoryAsync(GetFundingRateHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetFundingRateHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFundingRate[]>(Exchange, validationError);

            // Get data
            var result = await _api.ExchangeData.GetHistoricalFundingRatesAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFundingRate[]>(result);

            IEnumerable<KrakenFundingRate> data = result.Data;
            if (request.StartTime != null)
                data = data.Where(x => x.Timestamp >= request.StartTime.Value);
            if (request.EndTime != null)
                data = data.Where(x => x.Timestamp <= request.EndTime.Value);

            // Return
            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, request.Direction ?? DataDirection.Ascending)
                .Select(x =>
                    new SharedFundingRate(x.FundingRate, x.Timestamp))
                .ToArray());
        }

        #endregion

    }
}
