using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects.Models;
using System.Linq;

namespace Kraken.Net.Clients.SpotApi
{
    internal partial class KrakenRestClientSpotApi : IKrakenRestClientSpotApiShared
    {
        private const string _topicId = "KrakenSpot";
        private const string _exchangeName = "Kraken";

        public TradingMode[] SupportedTradingModes { get; } = new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
        public SharedClientInfo Discover() => SharedUtils.GetClientInfo(KrakenExchange.Metadata, this);

        #region Kline client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(_exchangeName, true, false, true, 720, false,
            SharedKlineInterval.OneMinute,
            SharedKlineInterval.FiveMinutes,
            SharedKlineInterval.FifteenMinutes,
            SharedKlineInterval.ThirtyMinutes,
            SharedKlineInterval.OneHour,
            SharedKlineInterval.FourHours,
            SharedKlineInterval.OneDay,
            SharedKlineInterval.OneWeek)
        {
            MaxTotalDataPoints = 720
        };

        async Task<HttpResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;

            var validationError = SharedClient.GetKlinesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedKline[]>(Exchange, validationError);

            var apiLimit = 720;
            int limit = request.Limit ?? apiLimit;
            if (request.StartTime.HasValue == true)
                limit = (int)Math.Ceiling((DateTime.UtcNow - request.StartTime!.Value).TotalSeconds / (int)request.Interval);

            if (limit > apiLimit)
            {
                // Not available via the API
                var cutoff = DateTime.UtcNow.AddSeconds(-(int)request.Interval * apiLimit);
                return HttpResult.Fail<SharedKline[]>(Exchange, ArgumentError.Invalid(nameof(GetKlinesRequest.Limit), $"Time filter outside of supported range. Can only request the most recent {apiLimit} klines i.e. data later than {cutoff} at this interval"));
            }

            var direction = DataDirection.Ascending;
            var pageParams = Pagination.GetPaginationParameters(direction, request.Limit ?? apiLimit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, false);

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetKlinesAsync(
                symbol,
                interval,
                since: pageParams.StartTime,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedKline[]>(result);

            var data = result.Data.Data.Take(pageParams.Limit);
            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromTime(pageParams, data.Max(x => x.OpenTime).Add(TimeSpan.FromSeconds((int)interval))),
                     data.Count(),
                     data.Select(x => x.OpenTime),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(data, x => x.OpenTime, request.StartTime, request.EndTime, direction)
                    .Select(x => 
                        new SharedKline(request.Symbol, symbol, x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume))
                    .ToArray(), nextPageRequest);
        }

        #endregion

        #region Spot Symbol client

        GetSpotSymbolsOptions ISpotSymbolRestClient.GetSpotSymbolsOptions { get; } = new GetSpotSymbolsOptions(_exchangeName, false)
        {
            OptionalExchangeParameters = [            
                new ParameterDescription(["NewAssetNames", "assetVersion"], typeof(bool), "If true, the response will use the new asset names (e.g. instead of XBT, BTC will be used)", false)
            ]
        };
        async Task<HttpResult<SharedSpotSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotSymbol[]>(Exchange, validationError);

            var useNewAssetResponse = request.GetParamValue<bool?>(Exchange, "NewAssetNames", "assetVersion");
            var result = await ExchangeData.GetSymbolsAsync(newAssetNameResponse: useNewAssetResponse, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(result);

            var response = HttpResult.Ok(result, result.Data.Select(s =>
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
            var symbolRegistrations = response.Data!
                .Concat(response.Data!.Select(x => new SharedSpotSymbol(x.BaseAsset, x.QuoteAsset, x.BaseAsset + "/" + x.QuoteAsset, x.Trading, x.TradingMode))).ToList();
            foreach (var symbol in response.Data!.Where(x => x.Name != x.BaseAsset + x.QuoteAsset))
                symbolRegistrations.Add(new SharedSpotSymbol(symbol.BaseAsset, symbol.QuoteAsset, symbol.BaseAsset + symbol.QuoteAsset, symbol.Trading));

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, EnvironmentName, null, symbolRegistrations.ToArray());
            return response;
        }

        private (string BaseAsset, string QuoteAsset) GetAssets(string name)
        {
            var split = name.Split('/');
            return (KrakenExchange.AssetAliases.ExchangeToCommonName(split[0]), KrakenExchange.AssetAliases.ExchangeToCommonName(split[1]));
        }

        async Task<ExchangeCallResult<SharedSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, EnvironmentName, null, baseAsset));
        }

        async Task<ExchangeCallResult<bool>> ISpotSymbolRestClient.SupportsSpotSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Only Spot symbols allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, EnvironmentName, null, symbol));
        }

        async Task<ExchangeCallResult<bool>> ISpotSymbolRestClient.SupportsSpotSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, EnvironmentName, null, symbolName));
        }
        #endregion

        #region Ticker client

        GetSpotTickerOptions ISpotTickerRestClient.GetSpotTickerOptions { get; } = new GetSpotTickerOptions(_exchangeName, SharedTickerType.Other);
        async Task<HttpResult<SharedSpotTicker>> ISpotTickerRestClient.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker>(Exchange, validationError);

            var result = await ExchangeData.GetTickerAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker>(result);

            var ticker = result.Data.Single();
            return HttpResult.Ok(result, new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, ticker.Value.Symbol), ticker.Value.Symbol, ticker.Value.LastTrade.Price, ticker.Value.High.Value24H, ticker.Value.Low.Value24H, ticker.Value.Volume.Value24H, ticker.Value.OpenPrice == 0 ? null : Math.Round(ticker.Value.LastTrade.Price / ticker.Value.OpenPrice * 100 - 100, 2))
            {
                QuoteVolume = ticker.Value.VolumeWeightedAveragePrice.Value24H * ticker.Value.Volume.Value24H
            });
        }

        GetSpotTickersOptions ISpotTickerRestClient.GetSpotTickersOptions { get; } = new GetSpotTickersOptions(_exchangeName, SharedTickerType.Other);
        async Task<HttpResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker[]>(Exchange, validationError);

            var result = await ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x => new SharedSpotTicker(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Value.Symbol), x.Value.Symbol, x.Value.LastTrade.Price, x.Value.High.Value24H, x.Value.Low.Value24H, x.Value.Volume.Value24H, x.Value.OpenPrice == 0 ? null : Math.Round(x.Value.LastTrade.Price / x.Value.OpenPrice * 100 - 100, 2))
            {
                QuoteVolume = x.Value.VolumeWeightedAveragePrice.Value24H * x.Value.Volume.Value24H
            }).ToArray());
        }

        #endregion

        #region Book Ticker client

        GetBookTickerOptions IBookTickerRestClient.GetBookTickerOptions { get; } = new GetBookTickerOptions(_exchangeName, false);
        async Task<HttpResult<SharedBookTicker>> IBookTickerRestClient.GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBookTicker>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var resultTicker = await ExchangeData.GetTickerAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedBookTicker>(resultTicker);

            return HttpResult.Ok(resultTicker, new SharedBookTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, symbol),
                symbol,
                resultTicker.Data.First().Value.BestAsks.Price,
                resultTicker.Data.First().Value.BestAsks.Quantity,
                resultTicker.Data.First().Value.BestBids.Price,
                resultTicker.Data.First().Value.BestBids.Quantity));
        }

        #endregion

        #region Recent Trade client

        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(_exchangeName, 1000, false);

        async Task<HttpResult<SharedTrade[]>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetRecentTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetTradeHistoryAsync(
                symbol,
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            return HttpResult.Ok(result, result.Data.Data.AsEnumerable().Reverse().Select(x => 
            new SharedTrade(request.Symbol, symbol, x.Quantity, x.Price, x.Timestamp)
            {
                Side = x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell
            }).ToArray());
        }

        #endregion

        #region Balance client
        GetBalancesOptions IBalanceRestClient.GetBalancesOptions { get; } = new GetBalancesOptions(_exchangeName, AccountTypeFilter.Spot);

        async Task<HttpResult<SharedBalance[]>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetBalancesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBalance[]>(Exchange, validationError);

            var result = await Account.GetAvailableBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedBalance[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x => new SharedBalance(KrakenExchange.AssetAliases.ExchangeToCommonName(x.Key), x.Value.Available, x.Value.Total)).ToArray());
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

        PlaceSpotOrderOptions ISpotOrderRestClient.PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions(_exchangeName);
        async Task<HttpResult<SharedId>> ISpotOrderRestClient.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.PlaceSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

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

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderIds.Single().ToString()));
        }

        GetSpotOrderOptions ISpotOrderRestClient.GetSpotOrderOptions { get; } = new GetSpotOrderOptions(_exchangeName, true);
        async Task<HttpResult<SharedSpotOrder>> ISpotOrderRestClient.GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

            var order = await Trading.GetOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedSpotOrder>(order);

            var orderData = order.Data.SingleOrDefault();
            if (orderData.Value == null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(order, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, orderData.Value.OrderDetails.Symbol),
                orderData.Value.OrderDetails.Symbol,
                orderData.Value.Id.ToString(),
                ParseOrderType(orderData.Value.OrderDetails.Type, orderData.Value.Oflags),
                orderData.Value.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseStatus(orderData.Value.Status),
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

        GetOpenSpotOrdersOptions ISpotOrderRestClient.GetOpenSpotOrdersOptions { get; } = new GetOpenSpotOrdersOptions(_exchangeName, true);
        async Task<HttpResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOpenSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var order = await Trading.GetOpenOrdersAsync(ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(order);

            IEnumerable<KrakenOrder> orders = order.Data.Open.Values.AsEnumerable();
            if (symbol != null)
                orders = orders.Where(x => x.OrderDetails.Symbol == symbol);

            return HttpResult.Ok(order, orders.Select(x => new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.OrderDetails.Symbol),
                x.OrderDetails.Symbol,
                x.Id.ToString(),
                ParseOrderType(x.OrderDetails.Type, x.Oflags),
                x.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseStatus(x.Status),
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

        GetSpotClosedOrdersOptions ISpotOrderRestClient.GetClosedSpotOrdersOptions { get; } = new GetSpotClosedOrdersOptions(_exchangeName, false, true, true, 50)
        {
            RequestNotes = "Request always returns up to 50 results"
        };
        async Task<HttpResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetClosedSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            int limit = request.Limit ?? 50;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            var result = await Trading.GetClosedOrdersAsync(
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                resultOffset: pageParams.Offset,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromOffset(pageParams, result.Data.Closed.Count),
                     result.Data.Closed.Count,
                     result.Data.Closed.Select(x => x.Value.CreateTime),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);
            
            var orders = result.Data.Closed.Values.Where(x => x.OrderDetails.Symbol == request.Symbol!.GetSymbol(FormatSymbol));
            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(orders, x => x.CreateTime, request.StartTime, request.EndTime, direction)
                    .Select(x => 
                        new SharedSpotOrder(
                            ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.OrderDetails.Symbol),
                            x.OrderDetails.Symbol,
                            x.Id.ToString(),
                            ParseOrderType(x.OrderDetails.Type, x.Oflags),
                            x.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            ParseStatus(x.Status),
                            x.CreateTime)
                        {
                            ClientOrderId = x.ClientOrderId,
                            Fee = x.Fee,
                            OrderPrice = (x.OrderDetails.Price == 0 || x.OrderDetails.Type == OrderType.TrailingStop) ? null : x.OrderDetails.Price,
                            OrderQuantity = new SharedOrderQuantity(x.Oflags.Contains("viqc") ? null : x.Quantity, x.Oflags.Contains("viqc") ? x.Quantity : null),
                            QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.QuoteQuantityFilled),
                            AveragePrice = x.AveragePrice == 0 ? null: x.AveragePrice,
                            UpdateTime = x.CloseTime
                        })
                    .ToArray(), nextPageRequest);
        }

        GetSpotOrderTradesOptions ISpotOrderRestClient.GetSpotOrderTradesOptions { get; } = new GetSpotOrderTradesOptions(_exchangeName, true);
        async Task<HttpResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotOrderTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            var order = await Trading.GetOrderAsync(request.OrderId, trades: true).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedUserTrade[]>(order);

            var orderData = order.Data.SingleOrDefault();
            if (orderData.Value == null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            var trades = orderData.Value.TradeIds.ToList();
            if (!trades.Any())
                return HttpResult.Ok(order, new SharedUserTrade[] { });

            var tradeInfo = await Trading.GetUserTradeDetailsAsync(trades.Take(20), ct: ct).ConfigureAwait(false);
            if (!tradeInfo.Success)
                return HttpResult.Fail<SharedUserTrade[]>(tradeInfo);

            return HttpResult.Ok(tradeInfo, tradeInfo.Data.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Value.Symbol), 
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

        GetSpotUserTradesOptions ISpotOrderRestClient.GetSpotUserTradesOptions { get; } = new GetSpotUserTradesOptions(_exchangeName, false, true, true, 50)
        {
            RequestNotes = "Request always returns up to 50 results"
        };
        async Task<HttpResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotUserTradesAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotUserTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            int limit = request.Limit ?? 50;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await Trading.GetUserTradesAsync(
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                resultOffset: pageParams.Offset,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedUserTrade[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromOffset(pageParams, result.Data.Trades.Count),
                     result.Data.Trades.Count,
                     result.Data.Trades.Select(x => x.Value.Timestamp),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Trades, x => x.Value.Timestamp, request.StartTime, request.EndTime, direction)
                    .Where(t => t.Value.Symbol == symbol)
                    .Select(x =>
                        new SharedUserTrade(
                            ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Value.Symbol), 
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
                        }).ToArray(), nextPageRequest);
        }

        CancelSpotOrderOptions ISpotOrderRestClient.CancelSpotOrderOptions { get; } = new CancelSpotOrderOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> ISpotOrderRestClient.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.ToString()));
        }

        private SharedOrderStatus ParseStatus(OrderStatus orderStatus)
        {
            if (orderStatus == OrderStatus.Open || orderStatus == OrderStatus.Pending) return SharedOrderStatus.Open;
            if (orderStatus == OrderStatus.Canceled || orderStatus == OrderStatus.Expired) return SharedOrderStatus.Canceled;
            if (orderStatus == OrderStatus.Closed) return SharedOrderStatus.Filled;

            return SharedOrderStatus.Unknown;
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
            if (tif == SharedTimeInForce.FillOrKill) return TimeInForce.FOK;

            return null;
        }

        #endregion

        #region Spot Client Id Order Client

        GetSpotOrderByClientOrderIdOptions ISpotOrderClientIdRestClient.GetSpotOrderByClientOrderIdOptions { get; } = new GetSpotOrderByClientOrderIdOptions(_exchangeName, true);
        async Task<HttpResult<SharedSpotOrder>> ISpotOrderClientIdRestClient.GetSpotOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

            var order = await Trading.GetOpenOrdersAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedSpotOrder>(order);

            var orderData = order.Data.Open.SingleOrDefault().Value;
            if (orderData == null)
            {
                var pageOrder = await Trading.GetClosedOrdersAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
                if (!pageOrder.Success)
                    return HttpResult.Fail<SharedSpotOrder>(pageOrder);

                orderData = pageOrder.Data.Closed.SingleOrDefault().Value;
            }

            if (orderData == null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, new ServerError(new ErrorInfo(ErrorType.UnknownOrder, "Order not found")));

            return HttpResult.Ok(order, new SharedSpotOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, orderData.OrderDetails.Symbol),
                orderData.OrderDetails.Symbol,
                orderData.Id.ToString(),
                ParseOrderType(orderData.OrderDetails.Type, orderData.Oflags),
                orderData.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseStatus(orderData.Status),
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

        CancelSpotOrderByClientOrderIdOptions ISpotOrderClientIdRestClient.CancelSpotOrderByClientOrderIdOptions { get; } = new CancelSpotOrderByClientOrderIdOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> ISpotOrderClientIdRestClient.CancelSpotOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.Pending.FirstOrDefault().ToString() ?? request.OrderId));
        }
        #endregion

        #region Asset client
        GetAssetOptions IAssetsRestClient.GetAssetOptions { get; } = new GetAssetOptions(_exchangeName, true);
        async Task<HttpResult<SharedAsset>> IAssetsRestClient.GetAssetAsync(GetAssetRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetAssetOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset>(Exchange, validationError);

            var assets = await Account.GetWithdrawMethodsAsync(KrakenExchange.AssetAliases.ExchangeToCommonName(request.Asset), ct: ct).ConfigureAwait(false);
            if (!assets.Success)
                return HttpResult.Fail<SharedAsset>(assets);

            if (!assets.Data.Any())
                return HttpResult.Fail<SharedAsset>(assets, new ServerError(new ErrorInfo(ErrorType.UnknownAsset, "Asset not found")));

            return HttpResult.Ok(assets, new SharedAsset(KrakenExchange.AssetAliases.ExchangeToCommonName(request.Asset))
            {
                Networks = assets.Data.Select(x => new SharedAssetNetwork(x.Network)
                {
                    FullName = x.Method,
                    MinWithdrawQuantity = x.Minimum
                }).ToArray()
            });
        }

        GetAssetsOptions IAssetsRestClient.GetAssetsOptions { get; } = new GetAssetsOptions(_exchangeName, false)
        {
            RequestNotes = "If API credentials are set and the NewAssetNames Exchange Parameter is not set to true then withdrawal networks will also be returned",
            OptionalExchangeParameters = new List<ParameterDescription>
            {
                new ParameterDescription(["NewAssetNames", "assetVersion"], typeof(bool), "If true, the response will use the new asset names (e.g. instead of XBT, BTC will be used)", false)
            }
        };
        async Task<HttpResult<SharedAsset[]>> IAssetsRestClient.GetAssetsAsync(GetAssetsRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetAssetsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedAsset[]>(Exchange, validationError);

            var useNewAssetResponse = request.GetParamValue<bool?>(Exchange, "NewAssetNames", "assetVersion");

            if (Authenticated && useNewAssetResponse != true)
            {
                var assets = await Account.GetWithdrawMethodsAsync(ct: ct).ConfigureAwait(false);
                if (!assets.Success)
                    return HttpResult.Fail<SharedAsset[]>(assets);

                return HttpResult.Ok(assets, assets.Data.GroupBy(x => KrakenExchange.AssetAliases.ExchangeToCommonName(x.Asset)).Select(x => new SharedAsset(x.Key)
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
                if (!assets.Success)
                    return HttpResult.Fail<SharedAsset[]>(assets);

                return HttpResult.Ok(assets, assets.Data.Select(x => new SharedAsset(KrakenExchange.AssetAliases.ExchangeToCommonName(x.Key))
                {
                    FullName = x.Value.AlternateName
                }).ToArray());
            }
        }

        #endregion

        #region Deposit client

        GetDepositAddressesOptions IDepositRestClient.GetDepositAddressesOptions { get; } = new GetDepositAddressesOptions(_exchangeName, true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(GetDepositAddressesRequest.Network), typeof(string), "The network the deposit address should be for", "Bitcoin")
            }
        };
        async Task<HttpResult<SharedDepositAddress[]>> IDepositRestClient.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetDepositAddressesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDepositAddress[]>(Exchange, validationError);

            var depositAddresses = await Account.GetDepositAddressesAsync(KrakenExchange.AssetAliases.CommonToExchangeName(request.Asset), request.Network!).ConfigureAwait(false);
            if (!depositAddresses.Success)
                return HttpResult.Fail<SharedDepositAddress[]>(depositAddresses);

            return HttpResult.Ok(depositAddresses, depositAddresses.Data.Select(x => new SharedDepositAddress(request.Asset, x.Address)
            {
                TagOrMemo = x.Tag
            }
            ).ToArray());
        }

        GetDepositsOptions IDepositRestClient.GetDepositsOptions { get; } = new GetDepositsOptions(_exchangeName, false, true, true, 100);
        async Task<HttpResult<SharedDeposit[]>> IDepositRestClient.GetDepositsAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetDepositsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDeposit[]>(Exchange, validationError);

            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await Account.GetDepositHistoryAsync(
                asset: request.Asset != null ? KrakenExchange.AssetAliases.CommonToExchangeName(request.Asset) : null,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                cursor: pageParams.Cursor,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedDeposit[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => result.Data.NextCursor == null ? null : Pagination.NextPageFromCursor(result.Data.NextCursor),
                     result.Data.Items.Length,
                     result.Data.Items.Select(x => x.Timestamp),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Items, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                    .Select(x => 
                        new SharedDeposit(
                            KrakenExchange.AssetAliases.ExchangeToCommonName(x.Asset),
                            x.Quantity,
                            true, 
                            x.Timestamp,
                            x.Status == "SUCCESS" ? SharedTransferStatus.Completed
                            : x.Status == "FAILURE" ? SharedTransferStatus.Failed
                            : SharedTransferStatus.Unknown)
                        {
                            Id = x.ReferenceId,
                            TransactionId = x.TransactionId,
                            Network = x.Method
                        })
                    .ToArray(), nextPageRequest);
        }

        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(_exchangeName, 1, 500, false);
        async Task<HttpResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOrderBook>(Exchange, validationError);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOrderBook>(result);

            return HttpResult.Ok(result, new SharedOrderBook(result.Data.Asks, result.Data.Bids));
        }

        #endregion

        #region Withdrawal client

        GetWithdrawalsOptions IWithdrawalRestClient.GetWithdrawalsOptions { get; } = new GetWithdrawalsOptions(_exchangeName, false, true, true, 500)
        {
            RequestNotes = "The Address field contains the label of the saved withdrawal address, not the actual address"
        };
        async Task<HttpResult<SharedWithdrawal[]>> IWithdrawalRestClient.GetWithdrawalsAsync(GetWithdrawalsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetWithdrawalsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedWithdrawal[]>(Exchange, validationError);

            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await Account.GetWithdrawalHistoryAsync(
                asset: request.Asset != null ? KrakenExchange.AssetAliases.CommonToExchangeName(request.Asset) : null,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                cursor: pageParams.Cursor,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedWithdrawal[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => result.Data.NextCursor == null ? null : Pagination.NextPageFromCursor(result.Data.NextCursor),
                     result.Data.Items.Length,
                     result.Data.Items.Select(x => x.Timestamp),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Items, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedWithdrawal(
                            KrakenExchange.AssetAliases.ExchangeToCommonName(x.Asset),
                            x.Key ?? string.Empty, 
                            x.Quantity,
                            x.Status == "Success", 
                            x.Timestamp,
                            GetWithdrawalStatus(x))
                        {
                            Id = x.ReferenceId,
                            TransactionId = x.TransactionId,
                            Network = x.Method,
                            Fee = x.Fee
                        })
                    .ToArray(), nextPageRequest);
        }

        private SharedTransferStatus GetWithdrawalStatus(KrakenMovementStatus x)
        {
            if (x.Status == "Failure")
                return SharedTransferStatus.Failed;

            if (x.Status == "Success")
                return SharedTransferStatus.Completed;

            if (x.Status == "Initial" || x.Status == "Pending" || x.Status == "Settled")
                return SharedTransferStatus.InProgress;

            return SharedTransferStatus.Unknown;
        }
        #endregion

        #region Withdraw client

        WithdrawOptions IWithdrawRestClient.WithdrawOptions { get; } = new WithdrawOptions(_exchangeName)
        {
            RequiredExchangeParameters = new List<ParameterDescription>
            {
                new ParameterDescription(["KeyName", "key"], typeof(string), "The name of the withdrawal address as defined in the web UI", "KucoinBitcoinAddress1")
            }
        };

        async Task<HttpResult<SharedId>> IWithdrawRestClient.WithdrawAsync(WithdrawRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.WithdrawOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var keyName = request.GetParamValue<string>(Exchange, "keyName", "key");

            // Get data
            var withdrawal = await Account.WithdrawAsync(
                KrakenExchange.AssetAliases.CommonToExchangeName(request.Asset),
                keyName!,
                request.Quantity,
                request.Address,
                ct: ct).ConfigureAwait(false);
            if (!withdrawal.Success)
                return HttpResult.Fail<SharedId>(withdrawal);

            return HttpResult.Ok(withdrawal, new SharedId(withdrawal.Data.ReferenceId));
        }

        #endregion

        #region Fee Client
        GetFeeOptions IFeeRestClient.GetFeeOptions { get; } = new GetFeeOptions(_exchangeName, true);

        async Task<HttpResult<SharedFee>> IFeeRestClient.GetFeesAsync(GetFeeRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFeeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFee>(Exchange, validationError);

            // Get data
            var result = await Account.GetTradeVolumeAsync(
                [request.Symbol!.GetSymbol(FormatSymbol)],
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFee>(result);

            // Return
            return HttpResult.Ok(result, new SharedFee(result.Data.MakerFees.First().Value.Fee, result.Data.Fees.First().Value.Fee));
        }
        #endregion

    }
}
