using Kraken.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Sockets;
using Kraken.Net.Enums;

namespace Kraken.Net.Clients.SpotApi
{
    internal partial class KrakenSocketClientSpotApi : IKrakenSocketClientSpotApiShared
    {
        private const string _topicId = "KrakenSpot";
        private const string _exchangeName = "Kraken";

        public TradingMode[] SupportedTradingModes { get; } = new [] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
        public SharedClientInfo Discover() => SharedUtils.GetClientInfo(KrakenExchange.Metadata, this);

        #region Ticker client
        SubscribeTickerOptions ITickerSocketClient.SubscribeTickerOptions { get; } = new SubscribeTickerOptions(_exchangeName)
        {
            SupportsMultipleSymbols = true,
            MaxSymbolCount = 500
        };
        async Task<WebSocketResult<UpdateSubscription>> ITickerSocketClient.SubscribeToTickerUpdatesAsync(SubscribeTickerRequest request, Action<DataEvent<SharedSpotTicker>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            ClearSymbolNameIfIncorrect(request);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await SubscribeToTickerUpdatesAsync(symbols, update => handler(update.ToType(new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, update.Data.Symbol), update.Data.Symbol, update.Data.LastPrice, update.Data.HighPrice, update.Data.LowPrice, update.Data.Volume, update.Data.PriceChangePercentage)
            {
                QuoteVolume = update.Data.Volume * update.Data.Vwap
            })), ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Trade client

        SubscribeTradeOptions ITradeSocketClient.SubscribeTradeOptions { get; } = new SubscribeTradeOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true,
            MaxSymbolCount = 500
        };
        async Task<WebSocketResult<UpdateSubscription>> ITradeSocketClient.SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            ClearSymbolNameIfIncorrect(request);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await SubscribeToTradeUpdatesAsync(symbols, update => handler(update.ToType(update.Data.Select(x =>
            new SharedTrade(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol), x.Symbol!, x.Quantity, x.Price, x.Timestamp)
            {
                Side = x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell
            }).ToArray())), false, ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Book Ticker client

        SubscribeBookTickerOptions IBookTickerSocketClient.SubscribeBookTickerOptions { get; } = new SubscribeBookTickerOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true,
            MaxSymbolCount = 500
        };
        async Task<WebSocketResult<UpdateSubscription>> IBookTickerSocketClient.SubscribeToBookTickerUpdatesAsync(SubscribeBookTickerRequest request, Action<DataEvent<SharedBookTicker>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            ClearSymbolNameIfIncorrect(request);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await SubscribeToTickerUpdatesAsync(symbols, update => handler(update.ToType(new SharedBookTicker(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, update.Data.Symbol), update.Data.Symbol, update.Data.BestAskPrice, update.Data.BestAskQuantity, update.Data.BestBidPrice, update.Data.BestBidQuantity))), TriggerEvent.BestOfferChange, ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Kline client
        SubscribeKlineOptions IKlineSocketClient.SubscribeKlineOptions { get; } = new SubscribeKlineOptions(_exchangeName, false,
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
        async Task<WebSocketResult<UpdateSubscription>> IKlineSocketClient.SubscribeToKlineUpdatesAsync(SubscribeKlineRequest request, Action<DataEvent<SharedKline>> handler, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;

            var validationError = SharedClient.SubscribeKlineOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            ClearSymbolNameIfIncorrect(request);

            var symbols = request.Symbols?.Length > 0 ? request.Symbols.Select(x => x.GetSymbol(FormatSymbol)).ToArray() : [request.Symbol!.GetSymbol(FormatSymbol)];
            var result = await SubscribeToKlineUpdatesAsync(symbols, interval, update =>
            {
                if (update.UpdateType == SocketUpdateType.Snapshot)
                    return;

                foreach(var item in update.Data)
                    handler(update.ToType(new SharedKline(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, item.Symbol), item.Symbol!, item.OpenTime,  item.ClosePrice, item.HighPrice, item.LowPrice, item.OpenPrice, item.Volume)));
            }, false, ct).ConfigureAwait(false);

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
                null,
                update => handler(update.ToType<SharedBalance[]>(update.Data.Select(x =>
                new SharedBalance(KrakenExchange.AssetAliases.ExchangeToCommonName(x.Asset), x.Balance, x.Balance)).ToArray())),
                false,
                ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Spot Order client

        private readonly Dictionary<string, string> _idSymbolMap = new Dictionary<string, string>();
        SubscribeSpotOrderOptions ISpotOrderSocketClient.SubscribeSpotOrderOptions { get; } = new SubscribeSpotOrderOptions(_exchangeName, false);
        async Task<WebSocketResult<UpdateSubscription>> ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrder[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var result = await SubscribeToOrderUpdatesAsync(
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

                    handler(update.ToType<SharedSpotOrder[]>(updateData.Select(
                        x => new SharedSpotOrder(
                            ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol),
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
                                    ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol),
                                    x.Symbol ?? string.Empty,
                                    x.OrderId,
                                    x.LastTradeId.ToString()!,
                                    x.OrderSide == OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
                                    x.LastTradeQuantity ?? 0,
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
