using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Clients.FuturesApi
{
    internal partial class KrakenSocketClientFuturesSharedApi
    {
        #region Position client
        public SubscribePositionOptions SubscribePositionOptions { get; } = new SubscribePositionOptions(_exchangeName, true);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToPositionUpdatesAsync(SubscribePositionRequest request, Action<DataEvent<SharedPosition[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribePositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var result = await _api.SubscribeToOpenPositionUpdatesAsync(
                update => {
                    if (update.UpdateType == SocketUpdateType.Snapshot)
                        return;

                    handler(update.ToType<SharedPosition[]>(update.Data.Positions.Select(
                        x => new SharedPosition(
                            ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                            x.Symbol,
                            new SharedOrderQuantity(contractQuantity: Math.Abs(x.Balance)),
                            update.DataTime ?? update.ReceiveTime)
                        {
                            AverageOpenPrice = x.EntryPrice,
                            PositionMode = SharedPositionMode.OneWay,
                            PositionSide = x.Balance > 0 ? SharedPositionSide.Long : SharedPositionSide.Short,
                            UnrealizedPnl = x.ProfitAndLoss,
                            Leverage = x.EffectiveLeverage,
                            LiquidationPrice = x.LiquidationThreshold
                        }).ToArray()));
                },
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion
    }
}
