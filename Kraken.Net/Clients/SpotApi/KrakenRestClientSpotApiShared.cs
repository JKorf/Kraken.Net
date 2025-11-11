using Kraken.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;
using System;
using CryptoExchange.Net.Objects.Errors;

namespace Kraken.Net.Clients.SpotApi
{
    internal partial class KrakenRestClientSpotApi : IKrakenRestClientSpotApiShared
    {
        private const string _topicId = "KrakenSpot";

        public string Exchange => KrakenExchange.ExchangeName;
        public TradingMode[] SupportedTradingModes { get; } = new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        #region Kline client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(SharedPaginationSupport.NotSupported, true, 720, false,
            SharedKlineInterval.OneMinute,
            SharedKlineInterval.FiveMinutes,
            SharedKlineInterval.FifteenMinutes,
            SharedKlineInterval.ThirtyMinutes,
            SharedKlineInterval.OneHour,
            SharedKlineInterval.FourHours,
            SharedKlineInterval.OneDay,
            SharedKlineInterval.OneWeek);

        async Task<ExchangeWebResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new ExchangeWebResult<SharedKline[]>(Exchange, ArgumentError.Invalid(nameof(GetKlinesRequest.Interval), "Interval not supported"));

            var validationError = ((IKlineRestClient)this).GetKlinesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedKline[]>(Exchange, validationError);

            var apiLimit = 720;
            int limit = request.Limit ?? apiLimit;
            if (request.StartTime.HasValue == true)
                limit = (int)Math.Ceiling((DateTime.UtcNow - request.StartTime!.Value).TotalSeconds / (int)request.Interval);

            if (limit > apiLimit)
            {
                // Not available via the API
                var cutoff = DateTime.UtcNow.AddSeconds(-(int)request.Interval * apiLimit);
                return new ExchangeWebResult<SharedKline[]>(Exchange, ArgumentError.Invalid(nameof(GetKlinesRequest.Limit), $"Time filter outside of supported range. Can only request the most recent {apiLimit} klines i.e. data later than {cutoff} at this interval"));
            }

            // Pagination not supported, no time filter available (always most recent)

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetKlinesAsync(
                symbol,
                interval,
                DateTime.UtcNow.AddSeconds(-(int)request.Interval * limit),
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedKline[]>(Exchange, null, default);

            // Filter the data based on requested timestamps
            IEnumerable<KrakenKline> data = result.Data.Data;
            if (request.StartTime.HasValue == true)
                data = data.Where(d => d.OpenTime >= request.StartTime.Value);
            if (request.EndTime.HasValue == true)
                data = data.Where(d => d.OpenTime < request.EndTime.Value);
            if (request.Limit.HasValue == true)
                data = data.Take(request.Limit.Value);

            return result.AsExchangeResult<SharedKline[]>(Exchange, request.Symbol.TradingMode, data.Reverse().Select(x => 
                new SharedKline(request.Symbol, symbol, x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume)).ToArray());
        }

        #endregion

        #region Spot Symbol client

        EndpointOptions<GetSymbolsRequest> ISpotSymbolRestClient.GetSpotSymbolsOptions { get; } = new EndpointOptions<GetSymbolsRequest>(false);
        async Task<ExchangeWebResult<SharedSpotSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotSymbolRestClient)this).GetSpotSymbolsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotSymbol[]>(Exchange, validationError);

            var useNewAssetResponse = ExchangeParameters.GetValue<bool?>(request.ExchangeParameters, Exchange, "NewAssetNames");
            var result = await ExchangeData.GetSymbolsAsync(newAssetNameResponse: useNewAssetResponse, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotSymbol[]>(Exchange, null, default);

            var response = result.AsExchangeResult<SharedSpotSymbol[]>(Exchange, TradingMode.Spot, result.Data.Select(s =>
            {
                var assets = GetAssets(s.Value.WebsocketName);
                return new SharedSpotSymbol(assets.BaseAsset, assets.QuoteAsset, s.Key, s.Value.Status == SymbolStatus.Online)
                {
                    PriceDecimals = s.Value.PriceDecimals,
                    QuantityDecimals = s.Value.LotDecimals,
                    MinTradeQuantity = s.Value.OrderMin,
                    PriceStep = s.Value.TickSize,
                    MinNotionalValue = s.Value.MinValue
                };
            }).ToArray());

            // Also register [BaseAsset]/[QuoteAsset] as names for websocket and [BaseAsset][QuoteAsset]
            var symbolRegistrations = response.Data
                .Concat(response.Data.Select(x => new SharedSpotSymbol(x.BaseAsset, x.QuoteAsset, x.BaseAsset + "/" + x.QuoteAsset, x.Trading, x.TradingMode))).ToList();
            foreach (var symbol in response.Data.Where(x => x.Name != x.BaseAsset + x.QuoteAsset))
                symbolRegistrations.Add(new SharedSpotSymbol(symbol.BaseAsset, symbol.QuoteAsset, symbol.BaseAsset + symbol.QuoteAsset, symbol.Trading));

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, symbolRegistrations.ToArray());
            return response;
        }

        private (string BaseAsset, string QuoteAsset) GetAssets(string name)
        {
            var split = name.Split('/');
            return (KrakenExchange.AssetAliases.ExchangeToCommonName(split[0]), KrakenExchange.AssetAliases.ExchangeToCommonName(split[1]));
        }

        #endregion

        #region Ticker client

        GetTickerOptions ISpotTickerRestClient.GetSpotTickerOptions { get; } = new GetTickerOptions(SharedTickerType.Other);
        async Task<ExchangeWebResult<SharedSpotTicker>> ISpotTickerRestClient.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTickerRestClient)this).GetSpotTickerOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotTicker>(Exchange, validationError);

            var result = await ExchangeData.GetTickerAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotTicker>(Exchange, null, default);

            var ticker = result.Data.Single();
            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, ticker.Value.Symbol), ticker.Value.Symbol, ticker.Value.LastTrade.Price, ticker.Value.High.Value24H, ticker.Value.Low.Value24H, ticker.Value.Volume.Value24H, ticker.Value.OpenPrice == 0 ? null : Math.Round(ticker.Value.LastTrade.Price / ticker.Value.OpenPrice * 100 - 100, 2))
            {
                QuoteVolume = ticker.Value.VolumeWeightedAveragePrice.Value24H * ticker.Value.Volume.Value24H
            });
        }

        GetTickersOptions ISpotTickerRestClient.GetSpotTickersOptions { get; } = new GetTickersOptions(SharedTickerType.Other);
        async Task<ExchangeWebResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTickerRestClient)this).GetSpotTickersOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotTicker[]>(Exchange, validationError);

            var result = await ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotTicker[]>(Exchange, null, default);

            return result.AsExchangeResult<SharedSpotTicker[]>(Exchange, TradingMode.Spot, result.Data.Select(x => new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, x.Value.Symbol), x.Value.Symbol, x.Value.LastTrade.Price, x.Value.High.Value24H, x.Value.Low.Value24H, x.Value.Volume.Value24H, x.Value.OpenPrice == 0 ? null : Math.Round(x.Value.LastTrade.Price / x.Value.OpenPrice * 100 - 100, 2))
            {
                QuoteVolume = x.Value.VolumeWeightedAveragePrice.Value24H * x.Value.Volume.Value24H
            }).ToArray());
        }

        #endregion

        #region Book Ticker client

        EndpointOptions<GetBookTickerRequest> IBookTickerRestClient.GetBookTickerOptions { get; } = new EndpointOptions<GetBookTickerRequest>(false);
        async Task<ExchangeWebResult<SharedBookTicker>> IBookTickerRestClient.GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
        {
            var validationError = ((IBookTickerRestClient)this).GetBookTickerOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedBookTicker>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var resultTicker = await ExchangeData.GetTickerAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!resultTicker)
                return resultTicker.AsExchangeResult<SharedBookTicker>(Exchange, null, default);

            return resultTicker.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedBookTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, symbol),
                symbol,
                resultTicker.Data.First().Value.BestAsks.Price,
                resultTicker.Data.First().Value.BestAsks.Quantity,
                resultTicker.Data.First().Value.BestBids.Price,
                resultTicker.Data.First().Value.BestBids.Quantity));
        }

        #endregion

        #region Recent Trade client

        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(1000, false);

        async Task<ExchangeWebResult<SharedTrade[]>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = ((IRecentTradeRestClient)this).GetRecentTradesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedTrade[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetTradeHistoryAsync(
                symbol,
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedTrade[]>(Exchange, null, default);

            return result.AsExchangeResult<SharedTrade[]>(Exchange, request.Symbol.TradingMode, result.Data.Data.Reverse().Select(x => 
            new SharedTrade(request.Symbol, symbol, x.Quantity, x.Price, x.Timestamp)
            {
                Side = x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell
            }).ToArray());
        }

        #endregion

        #region Balance client
        GetBalancesOptions IBalanceRestClient.GetBalancesOptions { get; } = new GetBalancesOptions(AccountTypeFilter.Spot);

        async Task<ExchangeWebResult<SharedBalance[]>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = ((IBalanceRestClient)this).GetBalancesOptions.ValidateRequest(Exchange, request, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedBalance[]>(Exchange, validationError);

            var result = await Account.GetAvailableBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedBalance[]>(Exchange, null, default);

            return result.AsExchangeResult<SharedBalance[]>(Exchange, TradingMode.Spot, result.Data.Select(x => new SharedBalance(KrakenExchange.AssetAliases.ExchangeToCommonName(x.Key), x.Value.Available, x.Value.Total)).ToArray());
        }

        #endregion

        #region Spot Order client

        SharedFeeDeductionType ISpotOrderRestClient.SpotFeeDeductionType => SharedFeeDeductionType.DeductFromOutput;
        SharedFeeAssetType ISpotOrderRestClient.SpotFeeAssetType => SharedFeeAssetType.QuoteAsset;
        SharedOrderType[] ISpotOrderRestClient.SpotSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market, SharedOrderType.LimitMaker };
        SharedTimeInForce[] ISpotOrderRestClient.SpotSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel, SharedTimeInForce.FillOrKill };

        SharedQuantitySupport ISpotOrderRestClient.SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAndQuoteAsset,
                SharedQuantityType.BaseAsset);

        string ISpotOrderRestClient.GenerateClientOrderId() => ExchangeHelpers.RandomString(18);

        PlaceSpotOrderOptions ISpotOrderRestClient.PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions();
        async Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).PlaceSpotOrderOptions.ValidateRequest(
                Exchange,
                request,
                request.TradingMode,
                SupportedTradingModes,
                ((ISpotOrderRestClient)this).SpotSupportedOrderTypes,
                ((ISpotOrderRestClient)this).SpotSupportedTimeInForce,
                ((ISpotOrderRestClient)this).SpotSupportedOrderQuantity);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var result = await Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                GetPlaceOrderType(request.OrderType),
                request.Quantity?.QuantityInBaseAsset ?? request.Quantity?.QuantityInQuoteAsset ?? 0,
                request.Price,
                clientOrderId: request.ClientOrderId,
                orderFlags: GetOrderFlags(request.OrderType, request.Quantity?.QuantityInQuoteAsset),
                timeInForce: GetTimeInForce(request.TimeInForce),
                ct: ct).ConfigureAwait(false);

            if (!result)
                return result.AsExchangeResult<SharedId>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedId(result.Data.OrderIds.Single().ToString()));
        }

        EndpointOptions<GetOrderRequest> ISpotOrderRestClient.GetSpotOrderOptions { get; } = new EndpointOptions<GetOrderRequest>(true);
        async Task<ExchangeWebResult<SharedSpotOrder>> ISpotOrderRestClient.GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotOrder>(Exchange, validationError);

            var order = await Trading.GetOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedSpotOrder>(Exchange, null, default);

            var orderData = order.Data.SingleOrDefault();
            if (orderData.Value == null)
                return new ExchangeWebResult<SharedSpotOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return order.AsExchangeResult(Exchange, TradingMode.Spot, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, orderData.Value.OrderDetails.Symbol),
                orderData.Value.OrderDetails.Symbol,
                orderData.Value.Id.ToString(),
                ParseOrderType(orderData.Value.OrderDetails.Type, orderData.Value.Oflags),
                orderData.Value.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(orderData.Value.Status),
                orderData.Value.CreateTime)
            {
                ClientOrderId = orderData.Value.ClientOrderId,
                Fee = orderData.Value.Fee,
                OrderPrice = (orderData.Value.OrderDetails.Price == 0 || orderData.Value.OrderDetails.Type == OrderType.TrailingStop) ? null : orderData.Value.OrderDetails.Price,
                OrderQuantity = new SharedOrderQuantity(orderData.Value.Oflags.Contains("viqc") ? null : orderData.Value.Quantity, orderData.Value.Oflags.Contains("viqc") ? orderData.Value.Quantity : null),
                QuantityFilled = new SharedOrderQuantity(orderData.Value.QuantityFilled, orderData.Value.QuoteQuantityFilled),
                AveragePrice = orderData.Value.AveragePrice == 0 ? null : orderData.Value.AveragePrice,
                UpdateTime = orderData.Value.CloseTime
            });
        }

        EndpointOptions<GetOpenOrdersRequest> ISpotOrderRestClient.GetOpenSpotOrdersOptions { get; } = new EndpointOptions<GetOpenOrdersRequest>(true);
        async Task<ExchangeWebResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetOpenSpotOrdersOptions.ValidateRequest(Exchange, request, request.Symbol?.TradingMode ?? request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotOrder[]>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var order = await Trading.GetOpenOrdersAsync(ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedSpotOrder[]>(Exchange, null, default);

            IEnumerable<KrakenOrder> orders = order.Data.Open.Values.AsEnumerable();
            if (symbol != null)
                orders = orders.Where(x => x.OrderDetails.Symbol == symbol);

            return order.AsExchangeResult<SharedSpotOrder[]>(Exchange, TradingMode.Spot, orders.Select(x => new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.OrderDetails.Symbol),
                x.OrderDetails.Symbol,
                x.Id.ToString(),
                ParseOrderType(x.OrderDetails.Type, x.Oflags),
                x.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                Fee = x.Fee,
                OrderPrice = (x.OrderDetails.Price == 0 || x.OrderDetails.Type == OrderType.TrailingStop) ? null : x.OrderDetails.Price,
                OrderQuantity = new SharedOrderQuantity(x.Oflags.Contains("viqc") ? null : x.Quantity, x.Oflags.Contains("viqc") ? x.Quantity : null),
                QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.QuoteQuantityFilled),
                AveragePrice = x.AveragePrice == 0 ? null: x.AveragePrice,
                UpdateTime = x.CloseTime
            }).ToArray());
        }

        PaginatedEndpointOptions<GetClosedOrdersRequest> ISpotOrderRestClient.GetClosedSpotOrdersOptions { get; } = new PaginatedEndpointOptions<GetClosedOrdersRequest>(SharedPaginationSupport.Descending, true, 1000, true)
        {
            RequestNotes = "Request always returns up to 50 results"
        };
        async Task<ExchangeWebResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetClosedSpotOrdersOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotOrder[]>(Exchange, validationError);

            // Determine page token
            int? offset = null;
            if (pageToken is OffsetToken token)
                offset = token.Offset;

            var order = await Trading.GetClosedOrdersAsync(
                startTime: request.StartTime,
                endTime: request.EndTime,
                resultOffset: offset,
                ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedSpotOrder[]>(Exchange, null, default);

            // Get next token
            OffsetToken? nextToken = null;
            if (order.Data.Count > (order.Data.Closed.Count + (offset ?? 0)))
                nextToken = new OffsetToken((offset ?? 0) + order.Data.Closed.Count);

            var orders = order.Data.Closed.Values.Where(x => x.OrderDetails.Symbol == request.Symbol!.GetSymbol(FormatSymbol));
            return order.AsExchangeResult<SharedSpotOrder[]>(Exchange, TradingMode.Spot, orders.Select(x => new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.OrderDetails.Symbol),
                x.OrderDetails.Symbol,
                x.Id.ToString(),
                ParseOrderType(x.OrderDetails.Type, x.Oflags),
                x.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                Fee = x.Fee,
                OrderPrice = (x.OrderDetails.Price == 0 || x.OrderDetails.Type == OrderType.TrailingStop) ? null : x.OrderDetails.Price,
                OrderQuantity = new SharedOrderQuantity(x.Oflags.Contains("viqc") ? null : x.Quantity, x.Oflags.Contains("viqc") ? x.Quantity : null),
                QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.QuoteQuantityFilled),
                AveragePrice = x.AveragePrice == 0 ? null: x.AveragePrice,
                UpdateTime = x.CloseTime
            }).ToArray(), nextToken);
        }

        EndpointOptions<GetOrderTradesRequest> ISpotOrderRestClient.GetSpotOrderTradesOptions { get; } = new EndpointOptions<GetOrderTradesRequest>(true);
        async Task<ExchangeWebResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotOrderTradesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, validationError);

            var order = await Trading.GetOrderAsync(request.OrderId, trades: true).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedUserTrade[]>(Exchange, null, default);

            var orderData = order.Data.SingleOrDefault();
            if (orderData.Value == null)
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            var trades = orderData.Value.TradeIds.ToList();
            if (!trades.Any())
                return order.AsExchangeResult<SharedUserTrade[]>(Exchange, null, new SharedUserTrade[] { });

            var tradeInfo = await Trading.GetUserTradeDetailsAsync(trades.Take(20), ct: ct).ConfigureAwait(false);
            if (!tradeInfo)
                return tradeInfo.AsExchangeResult<SharedUserTrade[]>(Exchange, null, default);

            return tradeInfo.AsExchangeResult<SharedUserTrade[]>(Exchange, TradingMode.Spot, tradeInfo.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.Value.Symbol), 
                x.Value.Symbol,
                x.Value.OrderId.ToString(),
                x.Value.Id.ToString(),
                x.Value.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.Value.Quantity,
                x.Value.Price,
                x.Value.Timestamp)
            {
                Fee = x.Value.Fee,
                Role = x.Value.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());
        }

        PaginatedEndpointOptions<GetUserTradesRequest> ISpotOrderRestClient.GetSpotUserTradesOptions { get; } = new PaginatedEndpointOptions<GetUserTradesRequest>(SharedPaginationSupport.Descending, true, 1000, true)
        {
            RequestNotes = "Request always returns up to 50 results"
        };
        async Task<ExchangeWebResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotUserTradesAsync(GetUserTradesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotUserTradesOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedUserTrade[]>(Exchange, validationError);

            // Determine page token
            int? offset = null;
            if (pageToken is OffsetToken token)
                offset = token.Offset;

            // Get data
            var order = await Trading.GetUserTradesAsync(
                startTime: request.StartTime,
                endTime: request.EndTime,
                resultOffset: offset, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedUserTrade[]>(Exchange, null, default);

            // Get next token
            OffsetToken? nextToken = null;
            if (order.Data.Count > order.Data.Trades.Count)
                nextToken = new OffsetToken((offset ?? 0) + order.Data.Trades.Count);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            return order.AsExchangeResult<SharedUserTrade[]>(Exchange, TradingMode.Spot, order.Data.Trades.Where(t => t.Value.Symbol == symbol).Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicId, x.Value.Symbol), 
                x.Value.Symbol,
                x.Value.OrderId.ToString(),
                x.Value.Id.ToString(),
                x.Value.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.Value.Quantity,
                x.Value.Price,
                x.Value.Timestamp)
            {
                Fee = x.Value.Fee,
                Role = x.Value.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray(), nextToken);
        }

        EndpointOptions<CancelOrderRequest> ISpotOrderRestClient.CancelSpotOrderOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> ISpotOrderRestClient.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).CancelSpotOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, request.TradingMode, new SharedId(order.Data.ToString()));
        }

        private SharedOrderStatus ParseOrderStatus(OrderStatus status)
        {
            if (status == OrderStatus.Open || status == OrderStatus.Pending) return SharedOrderStatus.Open;
            if (status == OrderStatus.Canceled || status == OrderStatus.Expired) return SharedOrderStatus.Canceled;
            return SharedOrderStatus.Filled;
        }

        private SharedOrderType ParseOrderType(OrderType type, string? flags)
        {
            if (type == OrderType.Market) return SharedOrderType.Market;
            if (type == OrderType.Limit && flags?.Contains("post") == true) return SharedOrderType.LimitMaker;
            if (type == OrderType.Limit) return SharedOrderType.Limit;

            return SharedOrderType.Other;
        }

        private OrderType GetPlaceOrderType(SharedOrderType type)
        {
            if (type == SharedOrderType.Market) return OrderType.Market;

            return OrderType.Limit;
        }

        private IEnumerable<OrderFlags>? GetOrderFlags(SharedOrderType type, decimal? quoteQuantity)
        {
            List<OrderFlags>? result = null;
            if (type == SharedOrderType.LimitMaker)
            {
                result ??= new List<OrderFlags>();
                result.Add(OrderFlags.PostOnly);
            }
            if (quoteQuantity > 0)
            {
                result ??= new List<OrderFlags>();
                result.Add(OrderFlags.OrderVolumeExpressedInQuoteAsset);
            }
            return result;
        }

        private TimeInForce? GetTimeInForce(SharedTimeInForce? tif)
        {
            if (tif == SharedTimeInForce.ImmediateOrCancel) return TimeInForce.IOC;
            if (tif == SharedTimeInForce.GoodTillCanceled) return TimeInForce.GTC;

            return null;
        }

        #endregion

        #region Spot Client Id Order Client

        EndpointOptions<GetOrderRequest> ISpotOrderClientIdRestClient.GetSpotOrderByClientOrderIdOptions { get; } = new EndpointOptions<GetOrderRequest>(true);
        async Task<ExchangeWebResult<SharedSpotOrder>> ISpotOrderClientIdRestClient.GetSpotOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).GetSpotOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotOrder>(Exchange, validationError);

            var order = await Trading.GetOpenOrdersAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedSpotOrder>(Exchange, null, default);

            var orderData = order.Data.Open.SingleOrDefault().Value;
            if (orderData == null)
            {
                var pageOrder = await Trading.GetClosedOrdersAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
                orderData = pageOrder.Data.Closed.SingleOrDefault().Value;
            }

            if (orderData == null)
                return new ExchangeWebResult<SharedSpotOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return order.AsExchangeResult(Exchange, TradingMode.Spot, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, orderData.OrderDetails.Symbol),
                orderData.OrderDetails.Symbol,
                orderData.Id.ToString(),
                ParseOrderType(orderData.OrderDetails.Type, orderData.Oflags),
                orderData.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(orderData.Status),
                orderData.CreateTime)
            {
                ClientOrderId = orderData.ClientOrderId,
                Fee = orderData.Fee,
                OrderPrice = (orderData.OrderDetails.Price == 0 || orderData.OrderDetails.Type == OrderType.TrailingStop) ? null : orderData.OrderDetails.Price,
                OrderQuantity = new SharedOrderQuantity(orderData.Oflags.Contains("viqc") ? null : orderData.Quantity, orderData.Oflags.Contains("viqc") ? orderData.Quantity : null),
                QuantityFilled = new SharedOrderQuantity(orderData.QuantityFilled, orderData.QuoteQuantityFilled),
                AveragePrice = orderData.AveragePrice == 0 ? null : orderData.AveragePrice,
                UpdateTime = orderData.CloseTime
            });
        }

        EndpointOptions<CancelOrderRequest> ISpotOrderClientIdRestClient.CancelSpotOrderByClientOrderIdOptions { get; } = new EndpointOptions<CancelOrderRequest>(true);
        async Task<ExchangeWebResult<SharedId>> ISpotOrderClientIdRestClient.CancelSpotOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotOrderRestClient)this).CancelSpotOrderOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order)
                return order.AsExchangeResult<SharedId>(Exchange, null, default);

            return order.AsExchangeResult(Exchange, TradingMode.Spot, new SharedId(order.Data.Pending.FirstOrDefault().ToString() ?? request.OrderId));
        }
        #endregion

        #region Asset client
        EndpointOptions<GetAssetRequest> IAssetsRestClient.GetAssetOptions { get; } = new EndpointOptions<GetAssetRequest>(true);
        async Task<ExchangeWebResult<SharedAsset>> IAssetsRestClient.GetAssetAsync(GetAssetRequest request, CancellationToken ct)
        {
            var validationError = ((IAssetsRestClient)this).GetAssetOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedAsset>(Exchange, validationError);

            var assets = await Account.GetWithdrawMethodsAsync(KrakenExchange.AssetAliases.ExchangeToCommonName(request.Asset), ct: ct).ConfigureAwait(false);
            if (!assets)
                return assets.AsExchangeResult<SharedAsset>(Exchange, null, default);

            if (!assets.Data.Any())
                return assets.AsExchangeError<SharedAsset>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownAsset, "Asset not found")));

            return assets.AsExchangeResult(Exchange, TradingMode.Spot, new SharedAsset(KrakenExchange.AssetAliases.ExchangeToCommonName(request.Asset))
            {
                Networks = assets.Data.Select(x => new SharedAssetNetwork(x.Network)
                {
                    FullName = x.Method,
                    MinWithdrawQuantity = x.Minimum
                }).ToArray()
            });
        }

        EndpointOptions<GetAssetsRequest> IAssetsRestClient.GetAssetsOptions { get; } = new EndpointOptions<GetAssetsRequest>(false)
        {
            RequestNotes = "If API credentials are set and the NewAssetNames Exchange Parameter is not set to true then withdrawal networks will also be returned"
        };
        async Task<ExchangeWebResult<SharedAsset[]>> IAssetsRestClient.GetAssetsAsync(GetAssetsRequest request, CancellationToken ct)
        {
            var validationError = ((IAssetsRestClient)this).GetAssetsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedAsset[]>(Exchange, validationError);

            var useNewAssetResponse = ExchangeParameters.GetValue<bool?>(request.ExchangeParameters, Exchange, "NewAssetNames");

            if (Authenticated && useNewAssetResponse != true)
            {
                var assets = await Account.GetWithdrawMethodsAsync(ct: ct).ConfigureAwait(false);
                if (!assets)
                    return assets.AsExchangeResult<SharedAsset[]>(Exchange, null, default);

                return assets.AsExchangeResult<SharedAsset[]>(Exchange, TradingMode.Spot, assets.Data.GroupBy(x => KrakenExchange.AssetAliases.ExchangeToCommonName(x.Asset)).Select(x => new SharedAsset(x.Key)
                {
                    Networks = x.Select(x => new SharedAssetNetwork(x.Network)
                    {
                        FullName = x.Method,
                        MinWithdrawQuantity = x.Minimum
                    }).ToArray()
                }).ToArray());
            }
            else
            {
                var assets = await ExchangeData.GetAssetsAsync(newAssetNameResponse: useNewAssetResponse, ct: ct).ConfigureAwait(false);
                if (!assets)
                    return assets.AsExchangeResult<SharedAsset[]>(Exchange, null, default);

                return assets.AsExchangeResult<SharedAsset[]>(Exchange, TradingMode.Spot, assets.Data.Select(x => new SharedAsset(KrakenExchange.AssetAliases.ExchangeToCommonName(x.Key))
                {
                    FullName = x.Value.AlternateName
                }).ToArray());
            }
        }

        #endregion

        #region Deposit client

        EndpointOptions<GetDepositAddressesRequest> IDepositRestClient.GetDepositAddressesOptions { get; } = new EndpointOptions<GetDepositAddressesRequest>(true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(GetDepositAddressesRequest.Network), typeof(string), "The network the deposit address should be for", "Bitcoin")
            }
        };
        async Task<ExchangeWebResult<SharedDepositAddress[]>> IDepositRestClient.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            var validationError = ((IDepositRestClient)this).GetDepositAddressesOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedDepositAddress[]>(Exchange, validationError);

            var depositAddresses = await Account.GetDepositAddressesAsync(KrakenExchange.AssetAliases.CommonToExchangeName(request.Asset), request.Network!).ConfigureAwait(false);
            if (!depositAddresses)
                return depositAddresses.AsExchangeResult<SharedDepositAddress[]>(Exchange, null, default);

            return depositAddresses.AsExchangeResult<SharedDepositAddress[]>(Exchange, TradingMode.Spot, depositAddresses.Data.Select(x => new SharedDepositAddress(request.Asset, x.Address)
            {
                TagOrMemo = x.Tag
            }
            ).ToArray());
        }

        GetDepositsOptions IDepositRestClient.GetDepositsOptions { get; } = new GetDepositsOptions(SharedPaginationSupport.Descending, true, 100);
        async Task<ExchangeWebResult<SharedDeposit[]>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IDepositRestClient)this).GetDepositsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedDeposit[]>(Exchange, validationError);

            // Determine page token
            string? cursor = null;
            if (pageToken is CursorToken cursorToken)
                cursor = cursorToken.Cursor;

            // Get data
            var deposits = await Account.GetDepositHistoryAsync(
                asset: request.Asset != null ? KrakenExchange.AssetAliases.CommonToExchangeName(request.Asset) : null,
                startTime: request.StartTime,
                endTime: request.EndTime,
                limit: request.Limit,
                cursor: cursor,
                ct: ct).ConfigureAwait(false);
            if (!deposits)
                return deposits.AsExchangeResult<SharedDeposit[]>(Exchange, null, default);

            // Determine next token
            CursorToken? nextToken = null;
            if (!string.IsNullOrEmpty(deposits.Data.NextCursor))
                nextToken = new CursorToken(deposits.Data.NextCursor!);

            return deposits.AsExchangeResult<SharedDeposit[]>(Exchange, TradingMode.Spot, deposits.Data.Items.Select(x => new SharedDeposit(KrakenExchange.AssetAliases.ExchangeToCommonName(x.Asset), x.Quantity, true, x.Timestamp)
            {
                Id = x.ReferenceId,
                TransactionId = x.TransactionId,
                Network = x.Method
            }).ToArray(), nextToken);
        }

        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(1, 500, false);
        async Task<ExchangeWebResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = ((IOrderBookRestClient)this).GetOrderBookOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedOrderBook>(Exchange, validationError);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedOrderBook>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, request.Symbol.TradingMode, new SharedOrderBook(result.Data.Asks, result.Data.Bids));
        }

        #endregion

        #region Withdrawal client

        GetWithdrawalsOptions IWithdrawalRestClient.GetWithdrawalsOptions { get; } = new GetWithdrawalsOptions(SharedPaginationSupport.Descending, true, 500)
        {
            RequestNotes = "The Address field contains the label of the saved withdrawal address, not the actual address"
        };
        async Task<ExchangeWebResult<SharedWithdrawal[]>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((IWithdrawalRestClient)this).GetWithdrawalsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedWithdrawal[]>(Exchange, validationError);

            // Determine page token
            string? cursor = null;
            if (pageToken is CursorToken cursorToken)
                cursor = cursorToken.Cursor;

            // Get data
            var withdrawals = await Account.GetWithdrawalHistoryAsync(
                asset: request.Asset != null ? KrakenExchange.AssetAliases.CommonToExchangeName(request.Asset) : null,
                startTime: request.StartTime,
                endTime: request.EndTime,
                limit: request.Limit,
                cursor: cursor,
                ct: ct).ConfigureAwait(false);
            if (!withdrawals)
                return withdrawals.AsExchangeResult<SharedWithdrawal[]>(Exchange, null, default);

            // Determine next token
            CursorToken? nextToken = null;
            if (!string.IsNullOrEmpty(withdrawals.Data.NextCursor))
                nextToken = new CursorToken(withdrawals.Data.NextCursor!);

            return withdrawals.AsExchangeResult<SharedWithdrawal[]>(Exchange, TradingMode.Spot, withdrawals.Data.Items.Select(x => new SharedWithdrawal(KrakenExchange.AssetAliases.ExchangeToCommonName(x.Asset), x.Key ?? string.Empty, x.Quantity, x.Status == "Success", x.Timestamp)
            {
                Id = x.ReferenceId,
                TransactionId = x.TransactionId,
                Network = x.Method,
                Fee = x.Fee
            }).ToArray(), nextToken);
        }

        #endregion

        #region Withdraw client

        WithdrawOptions IWithdrawRestClient.WithdrawOptions { get; } = new WithdrawOptions()
        {
            RequiredExchangeParameters = new List<ParameterDescription>
            {
                new ParameterDescription("keyName", typeof(string), "The name of the withdrawal address as defined in the web UI", "KucoinBitcoinAddress1")
            }
        };

        async Task<ExchangeWebResult<SharedId>> IWithdrawRestClient.WithdrawAsync(WithdrawRequest request, CancellationToken ct)
        {
            var validationError = ((IWithdrawRestClient)this).WithdrawOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedId>(Exchange, validationError);

            var keyName = ExchangeParameters.GetValue<string>(request.ExchangeParameters, Exchange, "keyName");

            // Get data
            var withdrawal = await Account.WithdrawAsync(
                KrakenExchange.AssetAliases.CommonToExchangeName(request.Asset),
                keyName!,
                request.Quantity,
                request.Address,
                ct: ct).ConfigureAwait(false);
            if (!withdrawal)
                return withdrawal.AsExchangeResult<SharedId>(Exchange, null, default);

            return withdrawal.AsExchangeResult(Exchange, TradingMode.Spot, new SharedId(withdrawal.Data.ReferenceId));
        }

        #endregion

        #region Fee Client
        EndpointOptions<GetFeeRequest> IFeeRestClient.GetFeeOptions { get; } = new EndpointOptions<GetFeeRequest>(true);

        async Task<ExchangeWebResult<SharedFee>> IFeeRestClient.GetFeesAsync(GetFeeRequest request, CancellationToken ct)
        {
            var validationError = ((IFeeRestClient)this).GetFeeOptions.ValidateRequest(Exchange, request, request.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedFee>(Exchange, validationError);

            // Get data
            var result = await Account.GetTradeVolumeAsync(
                [request.Symbol!.GetSymbol(FormatSymbol)],
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedFee>(Exchange, null, default);

            // Return
            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedFee(result.Data.MakerFees.First().Value.Fee, result.Data.Fees.First().Value.Fee));
        }
        #endregion

    }
}
