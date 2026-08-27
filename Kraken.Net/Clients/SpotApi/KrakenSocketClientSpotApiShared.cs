using Kraken.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Sockets;
using Kraken.Net.Enums;

namespace Kraken.Net.Clients.SpotApi
{
    internal class KrakenSocketClientSpotSharedApi :
        SharedApiBase,
        IKrakenSocketClientSpotApiShared,
        IKrakenSocketClientSpotSharedApi
    {
        private readonly KrakenSocketClientSpotApi _api;

        private const string _topicId = "KrakenSpot";
        private const string _exchangeName = "Kraken";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(KrakenExchange.Metadata, this);

        public KrakenSocketClientSpotSharedApi(KrakenSocketClientSpotApi api)
            : base(
                  api.Exchange, 
                  new[] { TradingMode.Spot },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeTickerOptions,
                SubscribeTradeOptions,
                SubscribeBookTickerOptions,
                SubscribeBalanceOptions,
                SubscribeKlineOptions,
                SubscribeSpotOrderOptions,
                PlaceSpotOrderOptions
            );
        }

        #region Ticker client
        async Task<WebSocketResult<UpdateSubscription>> ISubscribeTickerOperation.SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedTicker>> handler, CancellationToken ct)
            => await SubscribeToTickerUpdatesAsync(request, x => handler(x.ToType<SharedTicker>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeTickerOptions SubscribeTickerOptions { get; } = new SubscribeTickerOptions(_exchangeName)
        {
            SupportsMultipleSymbols = true,
            MaxSymbolCount = 500
        };
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedSpotTicker>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            ClearSymbolNameIfIncorrect(request);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await _api.SubscribeToTickerUpdatesAsync(symbols, update => handler(update.ToType(
                new SharedSpotTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Data.Symbol),
                    update.Data.Symbol, 
                    update.Data.LastPrice,
                    update.Data.HighPrice,
                    update.Data.LowPrice,
                    new SharedOrderQuantity(update.Data.Volume, update.Data.Volume * update.Data.Vwap), 
                    update.Data.PriceChangePercentage)
            {
            })), ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Trade client

        public SubscribeTradeOptions SubscribeTradeOptions { get; } = new SubscribeTradeOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true,
            MaxSymbolCount = 500
        };
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            ClearSymbolNameIfIncorrect(request);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await _api.SubscribeToTradeUpdatesAsync(symbols, update => handler(update.ToType(update.Data.Select(x =>
            new SharedTrade(ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol), x.Symbol!, new SharedOrderQuantity(x.Quantity), x.Price, x.Timestamp)
            {
                Side = x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell
            }).ToArray())), false, ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Book Ticker client

        public SubscribeBookTickerOptions SubscribeBookTickerOptions { get; } = new SubscribeBookTickerOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true,
            MaxSymbolCount = 500
        };
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(SubscribeBookTickerRequest request, Action<DataEvent<SharedBookTicker>> handler, CancellationToken ct)
        {
            var validationError = SubscribeBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            ClearSymbolNameIfIncorrect(request);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await _api.SubscribeToTickerUpdatesAsync(symbols, update => handler(
                update.ToType(
                    new SharedBookTicker(
                        ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Data.Symbol),
                        update.Data.Symbol,
                        update.Data.BestAskPrice,
                        new SharedOrderQuantity(update.Data.BestAskQuantity),
                        update.Data.BestBidPrice,
                        new SharedOrderQuantity(update.Data.BestBidQuantity)))), TriggerEvent.BestOfferChange, ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Kline client
        public SubscribeKlineOptions SubscribeKlineOptions { get; } = new SubscribeKlineOptions(_exchangeName, false,
            SharedKlineInterval.OneMinute,
            SharedKlineInterval.ThreeMinutes,
            SharedKlineInterval.FiveMinutes,
            SharedKlineInterval.FifteenMinutes,
            SharedKlineInterval.ThirtyMinutes,
            SharedKlineInterval.OneHour,
            SharedKlineInterval.FourHours,
            SharedKlineInterval.OneDay,
            SharedKlineInterval.OneWeek)
        {
            SupportsMultipleSymbols = true,
            MaxSymbolCount = 500
        };
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(SubscribeKlineRequest request, Action<DataEvent<SharedKline>> handler, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;

            var validationError = SubscribeKlineOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            ClearSymbolNameIfIncorrect(request);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await _api.SubscribeToKlineUpdatesAsync(symbols, interval, update =>
            {
                if (update.UpdateType == SocketUpdateType.Snapshot)
                    return;

                foreach (var item in update.Data)
                {
                    handler(update.ToType(
                        new SharedKline(
                            ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, item.Symbol),
                            item.Symbol!, 
                            item.OpenTime, 
                            item.ClosePrice, 
                            item.HighPrice, 
                            item.LowPrice, 
                            item.OpenPrice,
                            new SharedOrderQuantity(item.Volume, item.Volume * item.Vwap))));
                }
            }, false, ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Balance client
        public SubscribeBalanceOptions SubscribeBalanceOptions { get; } = new SubscribeBalanceOptions(_exchangeName, false);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(SubscribeBalancesRequest request, Action<DataEvent<SharedBalance[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeBalanceOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);
            var result = await _api.SubscribeToBalanceUpdatesAsync(
                null,
                update => handler(update.ToType<SharedBalance[]>(update.Data.Select(x =>
                    new SharedBalance(
                        SupportedTradingModes, 
                        KrakenExchange.AssetAliases.ExchangeToCommonName(x.Asset), 
                        x.Balance, 
                        x.Balance)).ToArray())),
                false,
                ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Spot Order client

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
                    foreach(var item in updateData)
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

        private SharedOrderStatus ParseStatus(OrderStatusUpdate orderStatus)
        {
            if (orderStatus == OrderStatusUpdate.New || orderStatus == OrderStatusUpdate.Pending || orderStatus == OrderStatusUpdate.PartiallyFilled) return SharedOrderStatus.Open;
            if (orderStatus == OrderStatusUpdate.Canceled || orderStatus == OrderStatusUpdate.Expired) return SharedOrderStatus.Canceled;
            if (orderStatus == OrderStatusUpdate.Filled) return SharedOrderStatus.Filled;

            return SharedOrderStatus.Unknown;
        }
        #endregion

        #region Spot Order client

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
        #endregion

        private void ClearSymbolNameIfIncorrect(SharedSymbolRequest request)
        {
            if (request.Symbol?.SymbolName != null && !request.Symbol.SymbolName.Contains('/') && request.Symbol.BaseAsset != null)
                request.Symbol.SymbolName = null;

            if (request.Symbols?.Length > 0)
            {
                foreach (var symbol in request.Symbols)
                {
                    if (symbol?.SymbolName != null && !symbol.SymbolName.Contains('/') && symbol.BaseAsset != null)
                        symbol.SymbolName = null;
                }
            }
        }
    }
}
