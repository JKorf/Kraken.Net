using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;

namespace Kraken.Net.Clients.FuturesApi
{
	internal partial class KrakenRestClientFuturesSharedApi
    {

        #region Set Futures TP/SL

        async Task<ICallResult<SharedId>> ISetFuturesTpSl.SetFuturesTpSlAsync(SetTpSlRequest request, CancellationToken ct)
            => await SetFuturesTpSlAsync(request, ct).ConfigureAwait(false);

        public SetFuturesTpSlOptions SetFuturesTpSlOptions { get; } = new SetFuturesTpSlOptions(_exchangeName, true)
        {
            RequiredRequestParameters = [
                RequestParameterRule<SetTpSlRequest>.Required(x => x.Quantity, "The quantity to close", 0.123m)
                ]
        };

        public async Task<HttpResult<SharedId>> SetFuturesTpSlAsync(SetTpSlRequest request, CancellationToken ct)
        {
            var validationError = SetFuturesTpSlOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.PositionSide == SharedPositionSide.Long ? OrderSide.Buy : OrderSide.Sell,
                request.TpSlSide == SharedTpSlSide.TakeProfit ? FuturesOrderType.TakeProfit : FuturesOrderType.Stop,
                request.Quantity!.Value,
                stopPrice: request.TriggerPrice,
                reduceOnly: true,
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            // Return
            return HttpResult.Ok(result, new SharedId(result.Data.OrderId));
        }

        #endregion

        #region Cancel Futures TP/SL

        async Task<ICallResult<bool>> ICancelFuturesTpSl.CancelFuturesTpSlAsync(CancelTpSlRequest request, CancellationToken ct)
            => await CancelFuturesTpSlAsync(request, ct).ConfigureAwait(false);

        public CancelFuturesTpSlOptions CancelFuturesTpSlOptions { get; } = new CancelFuturesTpSlOptions(_exchangeName, true)
        {
            RequiredRequestParameters = [
                RequestParameterRule<CancelTpSlRequest>.Required(x => x.OrderId, "Id of the tp/sl order", "123123")
                ]
        };

        public async Task<HttpResult<bool>> CancelFuturesTpSlAsync(CancelTpSlRequest request, CancellationToken ct)
        {
            var validationError = CancelFuturesTpSlOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<bool>(Exchange, validationError);

            var result = await _api.Trading.CancelOrderAsync(
                request.OrderId,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<bool>(result);

            // Return
            return HttpResult.Ok(result, true);
        }

        #endregion

    }
}
