using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Clients.SpotApi
{
    internal partial class KrakenSocketClientSpotSharedApi
    {

        #region Subscribe Spot Orders

        async Task<WebSocketResult<UpdateSubscription>> ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrder[]>> handler, CancellationToken ct)
            => await SubscribeToSpotOrderUpdatesAsync(request, x => handler(x.ToType<SharedSpotOrder[]>(x.Data)), ct).ConfigureAwait(false);

        private readonly Dictionary<string, string> _idSymbolMap = new Dictionary<string, string>();
        public SubscribeSpotOrderOptions SubscribeSpotOrderOptions { get; } = new SubscribeSpotOrderOptions(_exchangeName, false);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrderUpdate[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var result = await _api.SubscribeToOrderUpdatesAsync(
                update =>
                {
                    // Not all updates contain the symbol, but symbol is required for us to give proper updates
                    // The first update of an order always contains the symbol, so cache that so we can retrieve the symbol for later updates with the same order id
                    // Only subsequent updates for orders placed before subscribing will be lost that way

                    var updateData = update.Data.Where(x => !string.IsNullOrEmpty(x.Symbol));
                    foreach (var item in updateData)
                    {
                        if (!_idSymbolMap.ContainsKey(item.OrderId))
                            _idSymbolMap[item.OrderId] = item.Symbol!;
                    }

                    foreach (var item in update.Data.Where(x => x.Symbol == null))
                    {
                        if (_idSymbolMap.TryGetValue(item.OrderId, out var symbol))
                            item.Symbol = symbol;
                    }

                    if (!updateData.Any())
                        return;

                    handler(update.ToType<SharedSpotOrderUpdate[]>(updateData.Select(
                        x => new SharedSpotOrderUpdate(
                            ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                            x.Symbol ?? string.Empty,
                            x.OrderId,
                            x.OrderType == Enums.OrderType.Limit ? SharedOrderType.Limit : x.OrderType == Enums.OrderType.Market ? SharedOrderType.Market : SharedOrderType.Other,
                            x.OrderSide == Enums.OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            ParseStatus(x.OrderStatus),
                            x.Timestamp)
                        {
                            AveragePrice = x.AveragePrice,
                            ClientOrderId = x.ClientOrderId,
                            OrderId = x.OrderId,
                            OrderPrice = x.LimitPrice == 0 ? null : x.LimitPrice,
                            OrderQuantity = new SharedOrderQuantity(x.OrderQuantity, x.QuoteOrderQuantity),
                            QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.ValueFilled),
                            TimeInForce = x.TimeInForce == TimeInForce.IOC ? SharedTimeInForce.ImmediateOrCancel : x.TimeInForce == TimeInForce.GTC ? SharedTimeInForce.GoodTillCanceled : null,
                            LastTrade = x.LastTradeId == null ? null :
                                new SharedUserTrade(
                                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                                    x.Symbol ?? string.Empty,
                                    x.OrderId,
                                    x.LastTradeId.ToString()!,
                                    x.OrderSide == OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
                                    new SharedOrderQuantity(x.LastTradeQuantity),
                                    x.LastTradePrice ?? 0,
                                    x.Timestamp)
                                {
                                    ClientOrderId = x.ClientOrderId,
                                    Role = x.LastTradeRole == TradeType.Maker ? SharedRole.Maker : SharedRole.Taker
                                }
                        }
                        ).ToArray()));
                },
                false,
                false,
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        private SharedOrderStatus ParseStatus(OrderStatusUpdate orderStatus)
        {
            if (orderStatus == OrderStatusUpdate.New || orderStatus == OrderStatusUpdate.Pending || orderStatus == OrderStatusUpdate.PartiallyFilled) return SharedOrderStatus.Open;
            if (orderStatus == OrderStatusUpdate.Canceled || orderStatus == OrderStatusUpdate.Expired) return SharedOrderStatus.Canceled;
            if (orderStatus == OrderStatusUpdate.Filled) return SharedOrderStatus.Filled;

            return SharedOrderStatus.Unknown;
        }

        #region Place Spot Order

        public SharedFeeDeductionType SpotFeeDeductionType => SharedFeeDeductionType.DeductFromOutput;
        public SharedFeeAssetType SpotFeeAssetType => SharedFeeAssetType.QuoteAsset;
        public SharedOrderType[] SpotSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker };
        public SharedTimeInForce[] SpotSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };

        public SharedQuantitySupport SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAndQuoteAsset,
                SharedQuantityType.BaseAsset);

        public string GenerateClientOrderId() => ExchangeHelpers.RandomString(18);

        async Task<IExchangeCallResult<SharedId>> IPlaceSpotOrder.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
            => await PlaceSpotOrderAsync(request, ct).ConfigureAwait(false);

        PlaceSpotOrderOptions IPlaceSpotOrder.PlaceSpotOrderOptions 
            => PlaceSpotOrderOptions;

        public PlaceSpotOrderSocketOptions PlaceSpotOrderOptions { get; } = new PlaceSpotOrderSocketOptions(_exchangeName);
        public async Task<QueryResult<SharedId>> PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = PlaceSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                GetPlaceOrderType(request.OrderType),
                request.Quantity?.QuantityInBaseAsset ?? 0,
                limitPrice: request.Price,
                quoteQuantity: request.Quantity?.QuantityInQuoteAsset,
                clientOrderId: request.ClientOrderId,
                postOnly: request.OrderType == SharedOrderType.LimitMaker,
                timeInForce: GetTimeInForce(request.TimeInForce),
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return QueryResult.Fail<SharedId>(result);

            return QueryResult.Ok(result, new SharedId(result.Data.OrderId));
        }

        #endregion

        #region Cancel Spot Order

        async Task<IExchangeCallResult<SharedId>> ICancelSpotOrder.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
            => await CancelSpotOrderAsync(request, ct).ConfigureAwait(false);

        CancelSpotOrderOptions ICancelSpotOrder.CancelSpotOrderOptions
            => CancelSpotOrderOptions;

        public CancelSpotOrderSocketOptions CancelSpotOrderOptions { get; } = new CancelSpotOrderSocketOptions(_exchangeName, true);
        public async Task<QueryResult<SharedId>> CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = CancelSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return QueryResult.Fail<SharedId>(Exchange, validationError);

            var order = await _api.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return QueryResult.Fail<SharedId>(order);

            return QueryResult.Ok(order, new SharedId(order.Data.ToString()));
        }

        #endregion

        private OrderType GetPlaceOrderType(SharedOrderType type)
        {
            if (type == SharedOrderType.Market) return OrderType.Market;

            return OrderType.Limit;
        }

        private TimeInForce? GetTimeInForce(SharedTimeInForce? tif)
        {
            if (tif == SharedTimeInForce.ImmediateOrCancel) return TimeInForce.IOC;
            if (tif == SharedTimeInForce.GoodTillCanceled) return TimeInForce.GTC;
            if (tif == SharedTimeInForce.FillOrKill) return TimeInForce.FOK;

            return null;
        }
    }
}
