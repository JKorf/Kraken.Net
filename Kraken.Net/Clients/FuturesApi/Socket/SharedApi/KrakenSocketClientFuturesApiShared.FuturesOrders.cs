using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models.Socket.Futures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Clients.FuturesApi
{
    internal partial class KrakenSocketClientFuturesSharedApi
    {

        #region Subscribe Futures Orders

        async Task<WebSocketResult<UpdateSubscription>> IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrder[]>> handler, CancellationToken ct)
            => await SubscribeToFuturesOrderUpdatesAsync(request, x => handler(x.ToType<SharedFuturesOrder[]>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeFuturesOrderOptions SubscribeFuturesOrderOptions { get; } = new SubscribeFuturesOrderOptions(_exchangeName, true);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrderUpdate[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var result = await _api.SubscribeToOpenOrdersUpdatesAsync(
                true,
                update => { },
                update =>
                {
                    handler(update.ToType(new[] {
                        new SharedFuturesOrderUpdate(
                            ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Data.Order?.Symbol),
                            update.Data.Order?.Symbol ?? string.Empty,
                            update.Data.Order?.OrderId ?? update.Data.OrderId!,
                            update.Data.Order == null ? default : update.Data.Order.Type == FuturesOrderType.Limit ? SharedOrderType.Limit : update.Data.Order.Type == FuturesOrderType.Market ? SharedOrderType.Market : SharedOrderType.Other,
                            update.Data.Order?.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            GetOrderStatus(update.Data),
                            update.Data.Order?.Timestamp)
                        {
                            ClientOrderId = update.Data.Order?.ClientOrderId,
                            OrderQuantity = new SharedOrderQuantity(contractQuantity: update.Data.Order?.Quantity == 0 ? null : update.Data.Order?.Quantity),
                            QuantityFilled = new SharedOrderQuantity(contractQuantity: update.Data.Order?.QuantityFilled),
                            UpdateTime = update.Data.Order?.LastUpdateTime,
                            OrderPrice = update.Data.Order?.Price,
                            ReduceOnly = update.Data.Order?.ReduceOnly,
                        }
                    }));
                },
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        private SharedOrderStatus GetOrderStatus(KrakenFuturesOpenOrdersUpdate data)
            => data.Reason switch
            {
                KrakenFuturesOrderUpdateReason.NewPlacedOrderByUser => SharedOrderStatus.Open,
                KrakenFuturesOrderUpdateReason.Liquidation => SharedOrderStatus.Canceled,
                KrakenFuturesOrderUpdateReason.StopOrderTriggered => SharedOrderStatus.Canceled,
                KrakenFuturesOrderUpdateReason.LimitOrderFromStop => SharedOrderStatus.Open,
                KrakenFuturesOrderUpdateReason.PartialFill => SharedOrderStatus.Open,
                KrakenFuturesOrderUpdateReason.FullFill => SharedOrderStatus.Filled,
                KrakenFuturesOrderUpdateReason.CancelledByUser => SharedOrderStatus.Canceled,
                KrakenFuturesOrderUpdateReason.ContractExpired => SharedOrderStatus.Canceled,
                KrakenFuturesOrderUpdateReason.NotEnoughMargin => SharedOrderStatus.Canceled,
                KrakenFuturesOrderUpdateReason.MarketInactive => SharedOrderStatus.Canceled,
                KrakenFuturesOrderUpdateReason.CancelledByAdmin => SharedOrderStatus.Canceled,
                KrakenFuturesOrderUpdateReason.DeadManSwitch => SharedOrderStatus.Canceled,
                KrakenFuturesOrderUpdateReason.IocOrderFailedBecauseItWouldNotBeExecuted => SharedOrderStatus.Canceled,
                KrakenFuturesOrderUpdateReason.PostOrderFailedBecauseItWouldFilled => SharedOrderStatus.Canceled,
                KrakenFuturesOrderUpdateReason.WouldExecuteSelf => SharedOrderStatus.Canceled,
                KrakenFuturesOrderUpdateReason.WouldNotReducePosition => SharedOrderStatus.Canceled,
                KrakenFuturesOrderUpdateReason.OrderForEditNotFound => SharedOrderStatus.Canceled,
                _ => SharedOrderStatus.Unknown
            };

    }
}
