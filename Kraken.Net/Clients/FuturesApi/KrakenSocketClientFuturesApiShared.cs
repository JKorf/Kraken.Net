using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Clients.SpotApi;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Objects.Models.Socket.Futures;

namespace Kraken.Net.Clients.FuturesApi
{
    internal partial class KrakenSocketClientFuturesSharedApi : 
        SharedApiBase,
        IKrakenSocketClientFuturesApiShared,
        IKrakenSocketClientFuturesSharedApi
    {
        private readonly KrakenSocketClientFuturesApi _api;

        private const string _topicId = "KrakenFutures";
        private const string _exchangeName = "Kraken";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(KrakenExchange.Metadata, this);

        public KrakenSocketClientFuturesSharedApi(KrakenSocketClientFuturesApi api)
            : base(
                  api.Exchange, 
                  new[] { TradingMode.PerpetualLinear, TradingMode.DeliveryLinear, TradingMode.PerpetualInverse, TradingMode.DeliveryInverse },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeTickerOptions,
                SubscribeTradeOptions,
                SubscribeBookTickerOptions,
                SubscribeBalanceOptions,
                SubscribeFuturesOrderOptions,
                SubscribeUserTradeOptions,
                SubscribePositionOptions
                );
        }

        #region Ticker client
        async Task<WebSocketResult<UpdateSubscription>> ISubscribeTickerOperation.SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedTicker>> handler, CancellationToken ct)
            => await SubscribeToTickerUpdatesAsync(request, x => handler(x.ToType<SharedTicker>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeTickerOptions SubscribeTickerOptions { get; } = new SubscribeTickerOptions(_exchangeName)
        {
            SupportsMultipleSymbols = true
        };
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedSpotTicker>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await _api.SubscribeToTickerUpdatesAsync(symbols, update => handler(update.ToType(
                new SharedSpotTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Data.Symbol),
                    update.Data.Symbol!,
                    update.Data.LastPrice,
                    update.Data.HighPrice,
                    update.Data.LowPrice, 
                    new SharedOrderQuantity(update.Data.Volume, update.Data.VolumeQuote),
                    update.Data.ChangePercentage24h)
            {
            })), ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Trade client

        public SubscribeTradeOptions SubscribeTradeOptions { get; } = new SubscribeTradeOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true
        };
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await _api.SubscribeToTradeUpdatesAsync(symbols, update => handler(update.ToType(update.Data.Select(x =>
            new SharedTrade(ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol), x.Symbol!, new SharedOrderQuantity(x.Quantity), x.Price, x.Timestamp)
            {
                Side = x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell
            }).ToArray())), ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Book Ticker client

        public SubscribeBookTickerOptions SubscribeBookTickerOptions { get; } = new SubscribeBookTickerOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true
        };
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(SubscribeBookTickerRequest request, Action<DataEvent<SharedBookTicker>> handler, CancellationToken ct)
        {
            var validationError = SubscribeBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await _api.SubscribeToTickerUpdatesAsync(symbols, update => handler(
                update.ToType(
                    new SharedBookTicker(ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Data.Symbol!),
                    update.Data.Symbol!,
                    update.Data.BestAskPrice,
                    new SharedOrderQuantity(contractQuantity: update.Data.BestAskQuantity),
                    update.Data.BestBidPrice, 
                    new SharedOrderQuantity(contractQuantity: update.Data.BestBidQuantity)))), ct).ConfigureAwait(false);

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
                update => {
                    if (update.UpdateType == SocketUpdateType.Snapshot || update.Data.FlexFutures == null)
                        return;

                    handler(update.ToType<SharedBalance[]>(update.Data.FlexFutures.Currencies!.Select(x =>
                        new SharedBalance(SupportedTradingModes, x.Key, x.Value.Available, x.Value.Quantity)).ToArray()));
                },
                ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Futures Order client
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

        #endregion

        #region User Trade client

        public SubscribeUserTradeOptions SubscribeUserTradeOptions { get; } = new SubscribeUserTradeOptions(_exchangeName, true);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(SubscribeUserTradeRequest request, Action<DataEvent<SharedUserTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeUserTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var result = await _api.SubscribeToUserTradeUpdatesAsync(
                update =>
                {
                    if (update.UpdateType == SocketUpdateType.Snapshot)
                        return;

                    handler(update.ToType<SharedUserTrade[]>(update.Data.Trades.Select(x =>
                        new SharedUserTrade(
                            ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                            x.Symbol,
                            x.OrderId.ToString(),
                            x.TradeId.ToString(),
                            x.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            new SharedOrderQuantity(contractQuantity: x.Quantity),
                            x.Price,
                            x.Timestamp)
                        {
                            ClientOrderId = x.ClientOrderId,
                            Fee = Math.Abs(x.FeePaid),
                            FeeAsset = x.FeeCurrency,
                            Role = x.TradeType == Enums.TradeType.Taker ? SharedRole.Taker : SharedRole.Maker
                        }
                    ).ToArray()));
                },
                ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

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
