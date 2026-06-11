using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.FuturesApi;

namespace Kraken.Net.Clients.FuturesApi
{
    internal partial class KrakenSocketClientFuturesApi : IKrakenSocketClientFuturesApiShared
    {
        private const string _topicId = "KrakenFutures";
        private const string _exchangeName = "Kraken";

        public TradingMode[] SupportedTradingModes { get; } = new[] { TradingMode.PerpetualLinear, TradingMode.DeliveryLinear, TradingMode.PerpetualInverse, TradingMode.DeliveryInverse };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
        public SharedClientInfo Discover() => SharedUtils.GetClientInfo(this);

        #region Ticker client
        SubscribeTickerOptions ITickerSocketClient.SubscribeTickerOptions { get; } = new SubscribeTickerOptions(_exchangeName)
        {
            SupportsMultipleSymbols = true
        };
        async Task<WebSocketResult<UpdateSubscription>> ITickerSocketClient.SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedSpotTicker>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await SubscribeToTickerUpdatesAsync(symbols, update => handler(update.ToType(new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, update.Data.Symbol), update.Data.Symbol!, update.Data.LastPrice, null, null, update.Data.Volume, update.Data.ChangePercentage24h)
            {
                QuoteVolume = update.Data.VolumeQuote
            })), ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Trade client

        SubscribeTradeOptions ITradeSocketClient.SubscribeTradeOptions { get; } = new SubscribeTradeOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true
        };
        async Task<WebSocketResult<UpdateSubscription>> ITradeSocketClient.SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await SubscribeToTradeUpdatesAsync(symbols, update => handler(update.ToType(update.Data.Select(x =>
            new SharedTrade(ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol!, x.Quantity, x.Price, x.Timestamp)
            {
                Side = x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell
            }).ToArray())), ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Book Ticker client

        SubscribeBookTickerOptions IBookTickerSocketClient.SubscribeBookTickerOptions { get; } = new SubscribeBookTickerOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true
        };
        async Task<WebSocketResult<UpdateSubscription>> IBookTickerSocketClient.SubscribeToBookTickerUpdatesAsync(SubscribeBookTickerRequest request, Action<DataEvent<SharedBookTicker>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await SubscribeToTickerUpdatesAsync(symbols, update => handler(update.ToType(new SharedBookTicker(ExchangeSymbolCache.ParseSymbol(_topicId, update.Data.Symbol!), update.Data.Symbol!, update.Data.BestAskPrice, update.Data.BestAskQuantity, update.Data.BestBidPrice, update.Data.BestBidQuantity))), ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Balance client
        SubscribeBalanceOptions IBalanceSocketClient.SubscribeBalanceOptions { get; } = new SubscribeBalanceOptions(_exchangeName, false);
        async Task<WebSocketResult<UpdateSubscription>> IBalanceSocketClient.SubscribeToBalanceUpdatesAsync(SubscribeBalancesRequest request, Action<DataEvent<SharedBalance[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeBalanceOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var result = await SubscribeToBalanceUpdatesAsync(
                update => {
                    if (update.UpdateType == SocketUpdateType.Snapshot || update.Data.FlexFutures == null)
                        return;

                    handler(update.ToType<SharedBalance[]>(update.Data.FlexFutures.Currencies!.Select(x =>
                    new SharedBalance(x.Key, x.Value.Available, x.Value.Quantity)).ToArray()));
                },
                ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Futures Order client
        SubscribeFuturesOrderOptions IFuturesOrderSocketClient.SubscribeFuturesOrderOptions { get; } = new SubscribeFuturesOrderOptions(_exchangeName, true);
        async Task<WebSocketResult<UpdateSubscription>> IFuturesOrderSocketClient.SubscribeToFuturesOrderUpdatesAsync(SubscribeFuturesOrderRequest request, Action<DataEvent<SharedFuturesOrder[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var result = await SubscribeToOpenOrdersUpdatesAsync(
                true,
                update => { },
                update =>
                {
                    handler(update.ToType<SharedFuturesOrder[]>(new[] {
                        new SharedFuturesOrder(
                            ExchangeSymbolCache.ParseSymbol(_topicId, update.Data.Order?.Symbol),
                            update.Data.Order?.Symbol ?? string.Empty,
                            update.Data.OrderId.ToString(),
                            update.Data.Order == null ? default : update.Data.Order.Type == FuturesOrderType.Limit ? SharedOrderType.Limit : update.Data.Order.Type == FuturesOrderType.Market ? SharedOrderType.Market : SharedOrderType.Other,
                            update.Data.Order?.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            update.Data.IsCancel ? SharedOrderStatus.Canceled : update.Data.Order!.QuantityFilled == update.Data.Order.Quantity ? SharedOrderStatus.Filled : SharedOrderStatus.Open,
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

        #region User Trade client

        SubscribeUserTradeOptions IUserTradeSocketClient.SubscribeUserTradeOptions { get; } = new SubscribeUserTradeOptions(_exchangeName, true);
        async Task<WebSocketResult<UpdateSubscription>> IUserTradeSocketClient.SubscribeToUserTradeUpdatesAsync(SubscribeUserTradeRequest request, Action<DataEvent<SharedUserTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeUserTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var result = await SubscribeToUserTradeUpdatesAsync(
                update =>
                {
                    if (update.UpdateType == SocketUpdateType.Snapshot)
                        return;

                    handler(update.ToType<SharedUserTrade[]>(update.Data.Trades.Select(x =>
                        new SharedUserTrade(
                            ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol),
                            x.Symbol,
                            x.OrderId.ToString(),
                            x.TradeId.ToString(),
                            x.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            x.Quantity,
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
        SubscribePositionOptions IPositionSocketClient.SubscribePositionOptions { get; } = new SubscribePositionOptions(_exchangeName, true);
        async Task<WebSocketResult<UpdateSubscription>> IPositionSocketClient.SubscribeToPositionUpdatesAsync(SubscribePositionRequest request, Action<DataEvent<SharedPosition[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribePositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var result = await SubscribeToOpenPositionUpdatesAsync(
                update => {
                    if (update.UpdateType == SocketUpdateType.Snapshot)
                        return;

                    handler(update.ToType<SharedPosition[]>(update.Data.Positions.Select(
                        x => new SharedPosition(ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol), x.Symbol, Math.Abs(x.Balance), update.DataTime ?? update.ReceiveTime)
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
