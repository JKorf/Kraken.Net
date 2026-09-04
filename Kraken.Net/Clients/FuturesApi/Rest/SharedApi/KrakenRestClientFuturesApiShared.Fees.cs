using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;

namespace Kraken.Net.Clients.FuturesApi
{
	internal partial class KrakenRestClientFuturesSharedApi
    {

        #region Get Fees

        async Task<ICallResult<SharedFee>> IGetFees.GetFeesAsync(GetFeeRequest request, CancellationToken ct)
            => await GetFeesAsync(request, ct).ConfigureAwait(false);

        public GetFeeOptions GetFeeOptions { get; } = new GetFeeOptions(_exchangeName, true);

        public async Task<HttpResult<SharedFee>> GetFeesAsync(GetFeeRequest request, CancellationToken ct)
        {
            var validationError = GetFeeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFee>(Exchange, validationError);

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api._baseClient.SpotApi.Account.GetTradeVolumeAsync([new TradeVolumeRequest {
                Symbol = symbol,
                AssetClass = request.Symbol.TradingMode.IsPerpetual() ?  AssetClassExtended.Derivatives : AssetClassExtended.FuturesContract
            }], ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFee>(result);

            if (result.Data.Fees.Count != 1 || result.Data.MakerFees.Count != 1)
                return HttpResult.Fail<SharedFee>(result, new ServerError(new ErrorInfo(ErrorType.Unknown, "Unexpected fee data returned")));

            var takerFee = result.Data.Fees.First().Value.Fee;
            var makerFee = result.Data.MakerFees.First().Value.Fee;

            // Return
            return HttpResult.Ok(result, new SharedFee(makerFee, takerFee));
        }

        #endregion

    }
}
