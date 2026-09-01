using CryptoExchange.Net.SharedApis;

namespace Kraken.Net.Clients.FuturesApi
{
	internal partial class KrakenRestClientFuturesSharedApi
	{

        #region Order Book client
        public GetOrderBookOptions GetOrderBookOptions { get; } = new GetOrderBookOptions(_exchangeName, 1, 1000, false);
        public async Task<HttpResult<SharedOrderBook>> GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = GetOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOrderBook>(Exchange, validationError);

            var result = await _api.ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOrderBook>(result);

            var asks = result.Data.Asks;
            var bids = result.Data.Bids;
            if (request.Limit != null)
            {
                asks = asks.Take(request.Limit.Value).ToArray();
                bids = bids.Take(request.Limit.Value).ToArray();
            }

            return HttpResult.Ok(result, new SharedOrderBook(SharedQuantityType.BaseAsset, null, asks, bids));
        }

        #endregion
    }
}
