using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;

namespace Kraken.Net.Clients.FuturesApi
{
	internal partial class KrakenRestClientFuturesSharedApi
    {
        #region Recent Trade client

        public GetRecentTradesOptions GetRecentTradesOptions { get; } = new GetRecentTradesOptions(_exchangeName, 100, false);
        public async Task<HttpResult<SharedTrade[]>> GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = GetRecentTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.ExchangeData.GetTradesAsync(
                symbol,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            var data = result.Data;
            if (request.Limit != null)
                data = data.Take(request.Limit.Value).ToArray();

            return HttpResult.Ok(result, data.Select(x =>
            new SharedTrade(request.Symbol, symbol, new SharedOrderQuantity(x.Quantity!.Value, x.NotionalAmount), x.Price, x.Timestamp!.Value)
            {
                Side = x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell
            }).ToArray());
        }

        #endregion
    }
}
