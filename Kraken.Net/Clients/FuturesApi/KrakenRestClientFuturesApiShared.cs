using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Objects.Models.Futures;

namespace Kraken.Net.Clients.FuturesApi
{
    internal partial class KrakenRestClientFuturesApi : IKrakenRestClientFuturesApiShared
    {
        private const string _topicId = "KrakenFutures";
        private const string _exchangeName = "Kraken";

        public TradingMode[] SupportedTradingModes { get; } = new[] { TradingMode.PerpetualLinear, TradingMode.DeliveryLinear, TradingMode.PerpetualInverse, TradingMode.DeliveryInverse };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
        public SharedClientInfo Discover() => SharedUtils.GetClientInfo(KrakenExchange.Metadata, this);

        #region Balance Client
        GetBalancesOptions IBalanceRestClient.GetBalancesOptions { get; } = new GetBalancesOptions(_exchangeName, AccountTypeFilter.Futures);

        async Task<HttpResult<SharedBalance[]>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetBalancesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBalance[]>(Exchange, validationError);

            var result = await Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedBalance[]>(result);
                        
            var balances = result.Data.MultiCollateralMarginAccount.Currencies.Select(x => new SharedBalance(x.Key, x.Value.Available, x.Value.Quantity)).ToList();
            foreach (var balance in result.Data.MarginAccounts)
            {
                foreach (var currency in balance.Balances)
                    balances.Add(new SharedBalance(currency.Key, currency.Value, currency.Value) { IsolatedMarginSymbol = balance.Symbol });
            }

            return HttpResult.Ok(result, balances.ToArray());            
        }

        #endregion

        #region Klines client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(_exchangeName, false, true, true, 5000, false,
            SharedKlineInterval.OneMinute,
            SharedKlineInterval.FiveMinutes,
            SharedKlineInterval.FifteenMinutes,
            SharedKlineInterval.ThirtyMinutes,
            SharedKlineInterval.OneHour,
            SharedKlineInterval.FourHours,
            SharedKlineInterval.TwelveHours,
            SharedKlineInterval.OneDay,
            SharedKlineInterval.OneWeek);
        async Task<HttpResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var interval = (Enums.FuturesKlineInterval)request.Interval;

            var validationError = SharedClient.GetKlinesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedKline[]>(Exchange, validationError);

            int limit = request.Limit ?? 5000;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, false);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetKlinesAsync(
                Enums.TickType.Trade,
                symbol,
                interval,
                pageParams.StartTime,
                pageParams.EndTime,
                pageParams.Limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedKline[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromTime(pageParams, result.Data.Klines.Min(x => x.Timestamp).Add(TimeSpan.FromSeconds(-(int)interval))),
                     result.Data.Klines.Length,
                     result.Data.Klines.Select(x => x.Timestamp),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Klines, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                    .Select(x => 
                        new SharedKline(request.Symbol, symbol, x.Timestamp, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume))
                    .ToArray(), nextPageRequest);
        }

        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(_exchangeName, 0, 1000, false);
        async Task<HttpResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOrderBook>(Exchange, validationError);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOrderBook>(result);

            var asks = result.Data.Asks;
            var bids = result.Data.Bids;
            if (request.Limit != null)
            {
                asks = asks.Take(request.Limit.Value).ToArray();
                bids = bids.Take(request.Limit.Value).ToArray();
            }

            return HttpResult.Ok(result, new SharedOrderBook(asks, bids));
        }

        #endregion

        #region Recent Trade client

        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(_exchangeName, 100, false);
        async Task<HttpResult<SharedTrade[]>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetRecentTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetTradesAsync(
                symbol,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            var data = result.Data;
            if (request.Limit != null)
                data = data.Take(request.Limit.Value).ToArray();

            return HttpResult.Ok(result, data.Select(x => 
            new SharedTrade(request.Symbol, symbol, x.Quantity!.Value, x.Price, x.Timestamp!.Value)
            {
                Side = x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell
            }).ToArray());
        }

        #endregion

        #region Funding Rate client
        GetFundingRateHistoryOptions IFundingRateRestClient.GetFundingRateHistoryOptions { get; } = new GetFundingRateHistoryOptions(_exchangeName, true, true, false, 1000, false);

        async Task<HttpResult<SharedFundingRate[]>> IFundingRateRestClient.GetFundingRateHistoryAsync(GetFundingRateHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetFundingRateHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFundingRate[]>(Exchange, validationError);

            // Get data
            var result = await ExchangeData.GetHistoricalFundingRatesAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFundingRate[]>(result);

            IEnumerable<KrakenFundingRate> data = result.Data;
            if (request.StartTime != null)
                data = data.Where(x => x.Timestamp >= request.StartTime.Value);
            if (request.EndTime != null)
                data = data.Where(x => x.Timestamp <= request.EndTime.Value);

            // Return
            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, request.Direction ?? DataDirection.Ascending)
                .Select(x =>
                    new SharedFundingRate(x.FundingRate, x.Timestamp))
                .ToArray());
        }
        #endregion

        #region Futures Symbol client

        GetFuturesSymbolsOptions IFuturesSymbolRestClient.GetFuturesSymbolsOptions { get; } = new GetFuturesSymbolsOptions(_exchangeName, false);
        async Task<HttpResult<SharedFuturesSymbol[]>> IFuturesSymbolRestClient.GetFuturesSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesSymbol[]>(Exchange, validationError);

            var result = await ExchangeData.GetSymbolsAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFuturesSymbol[]>(result);

            var data = result.Data.Where(x => x.Type != Enums.SymbolType.SpotIndex && !x.Symbol.StartsWith("rr"));
            var resultData = data.Select(s =>
            {
                var split = s.Symbol.Split('_');
                var assets = split[1];

                return new SharedFuturesSymbol(
                    s.Type == Enums.SymbolType.FlexibleFutures && split.Count() == 2 ? TradingMode.PerpetualLinear :
                    s.Type == Enums.SymbolType.FlexibleFutures && split.Count() > 2 ? TradingMode.DeliveryLinear :
                    s.Type == Enums.SymbolType.InverseFutures && split.Count() == 2 ? TradingMode.PerpetualInverse :
                    TradingMode.DeliveryInverse,
                    s.BaseAsset,
                    s.QuoteAsset,
                    s.Symbol,
                    s.Tradeable)
                {
                    QuantityDecimals = (int?)s.ContractValueTradePrecision,
                    PriceStep = s.TickSize,
                    ContractSize = s.ContractSize,
                    DeliveryTime = split.Count() > 2 ? DateTime.ParseExact(split[2], "yyMMdd", CultureInfo.InvariantCulture) : null
                };
            });

            if (request.TradingMode != null)
                resultData = resultData.Where(x => request.TradingMode == x.TradingMode);

            var response = HttpResult.Ok(result, resultData.ToArray());
            
            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, EnvironmentName, null, response.Data!);
            return response;
        }

        async Task<ExchangeCallResult<SharedSymbol[]>> IFuturesSymbolRestClient.GetFuturesSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, EnvironmentName, null, baseAsset));
        }

        async Task<ExchangeCallResult<bool>> IFuturesSymbolRestClient.SupportsFuturesSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode == TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Spot symbols not allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, EnvironmentName, null, symbol));
        }

        async Task<ExchangeCallResult<bool>> IFuturesSymbolRestClient.SupportsFuturesSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((IFuturesSymbolRestClient)this).GetFuturesSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, EnvironmentName, null, symbolName));
        }
        #endregion

        #region Ticker client

        GetFuturesTickerOptions IFuturesTickerRestClient.GetFuturesTickerOptions { get; } = new GetFuturesTickerOptions(_exchangeName);
        async Task<HttpResult<SharedFuturesTicker>> IFuturesTickerRestClient.GetFuturesTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTicker>(Exchange, validationError);

            var resultTicker = await ExchangeData.GetTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedFuturesTicker>(resultTicker);

            var time = DateTime.UtcNow;

            return HttpResult.Ok(resultTicker, new SharedFuturesTicker(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, resultTicker.Data.Symbol), resultTicker.Data.Symbol, resultTicker.Data.LastPrice, null, null, resultTicker.Data.Volume24h, (resultTicker.Data.OpenPrice24h > 0 && resultTicker.Data.LastPrice > 0) ? Math.Round(resultTicker.Data.LastPrice.Value / resultTicker.Data.OpenPrice24h.Value * 100 - 100, 2) : null)
            {
                MarkPrice = resultTicker.Data.MarkPrice,
                IndexPrice = resultTicker.Data.IndexPrice,
                FundingRate = resultTicker.Data.FundingRate,
                NextFundingTime = resultTicker.Data.FundingRate == null ? null : new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0, DateTimeKind.Utc).AddHours(1)
            });
        }

        GetFuturesTickersOptions IFuturesTickerRestClient.GetFuturesTickersOptions { get; } = new GetFuturesTickersOptions(_exchangeName);
        async Task<HttpResult<SharedFuturesTicker[]>> IFuturesTickerRestClient.GetFuturesTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesTicker[]>(Exchange, validationError);

            var resultTicker = await ExchangeData.GetTickersAsync(ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedFuturesTicker[]>(resultTicker);

            var data = resultTicker.Data.Where(x => !x.Symbol.StartsWith("rr_") && !x.Symbol.StartsWith("in_"));
            if (request.TradingMode != null)
            {
                data = data.Where(x => request.TradingMode == TradingMode.PerpetualLinear ? x.Symbol.StartsWith("PF_") :
                    request.TradingMode == TradingMode.DeliveryLinear ? x.Symbol.StartsWith("FI_") :
                    request.TradingMode == TradingMode.PerpetualInverse ? x.Symbol.StartsWith("PI_") :
                    x.Symbol.StartsWith("FF"));
            }

            var time = DateTime.UtcNow;
            return HttpResult.Ok(resultTicker, data.Select(x => new SharedFuturesTicker(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol), x.Symbol, x.LastPrice, null, null, x.Volume24h, (x.OpenPrice24h > 0 && x.LastPrice > 0) ? Math.Round(x.LastPrice.Value / x.OpenPrice24h.Value * 100 - 100, 2) : null)
            {
                MarkPrice = x.MarkPrice,
                IndexPrice = x.IndexPrice,
                FundingRate = x.FundingRate,
                NextFundingTime = x.FundingRate == null ? null : new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0, DateTimeKind.Utc).AddHours(1)
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
                resultTicker.Data.BestAskPrice ?? 0,
                resultTicker.Data.BestAskQuantity ?? 0,
                resultTicker.Data.BestBidPrice ?? 0,
                resultTicker.Data.BestBidQuantity ?? 0));
        }

        #endregion

        #region Mark Price Klines client

        GetMarkPriceKlinesOptions IMarkPriceKlineRestClient.GetMarkPriceKlinesOptions { get; } = new GetMarkPriceKlinesOptions(_exchangeName, true, false, true, 5000, false);

        async Task<HttpResult<SharedFuturesKline[]>> IMarkPriceKlineRestClient.GetMarkPriceKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var interval = (Enums.FuturesKlineInterval)request.Interval;

            var validationError = SharedClient.GetMarkPriceKlinesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesKline[]>(Exchange, validationError);

            int limit = request.Limit ?? 5000;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, false);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetKlinesAsync(
                Enums.TickType.Mark,
                symbol,
                interval,
                pageParams.StartTime,
                pageParams.EndTime,
                pageParams.Limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFuturesKline[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromTime(pageParams, result.Data.Klines.Min(x => x.Timestamp)),
                     result.Data.Klines.Length,
                     result.Data.Klines.Select(x => x.Timestamp),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data.Klines, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                    .Select(x => 
                        new SharedFuturesKline(request.Symbol, symbol, x.Timestamp, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice))
                    .ToArray(), nextPageRequest);
        }

        #endregion

        #region Open Interest client

        GetOpenInterestOptions IOpenInterestRestClient.GetOpenInterestOptions { get; } = new GetOpenInterestOptions(_exchangeName, true);
        async Task<HttpResult<SharedOpenInterest>> IOpenInterestRestClient.GetOpenInterestAsync(GetOpenInterestRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOpenInterestOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOpenInterest>(Exchange, validationError);

            var result = await ExchangeData.GetTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOpenInterest>(result);

            return HttpResult.Ok(result, new SharedOpenInterest(result.Data.OpenInterest));
        }

        #endregion

        #region Leverage client
        SharedLeverageSettingMode ILeverageRestClient.LeverageSettingType => SharedLeverageSettingMode.PerSymbol;

        GetLeverageOptions ILeverageRestClient.GetLeverageOptions { get; } = new GetLeverageOptions(_exchangeName, true);
        async Task<HttpResult<SharedLeverage>> ILeverageRestClient.GetLeverageAsync(GetLeverageRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetLeverageOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedLeverage>(Exchange, validationError);

            var result = await Trading.GetLeverageAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedLeverage>(result);

            var symbolLeverage = result.Data.SingleOrDefault(x => x.Symbol == request.Symbol!.GetSymbol(FormatSymbol));
            if (symbolLeverage == null)
                return HttpResult.Fail<SharedLeverage>(result, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Symbol leverage not found")));

            return HttpResult.Ok(result, new SharedLeverage(symbolLeverage.MaxLeverage)
            {
                Side = request.PositionSide
            });
        }

        SetLeverageOptions ILeverageRestClient.SetLeverageOptions { get; } = new SetLeverageOptions(_exchangeName);
        async Task<HttpResult<SharedLeverage>> ILeverageRestClient.SetLeverageAsync(SetLeverageRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.SetLeverageOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedLeverage>(Exchange, validationError);

            var result = await Trading.SetLeverageAsync(symbol: request.Symbol!.GetSymbol(FormatSymbol), request.Leverage, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedLeverage>(result);

            return HttpResult.Ok(result, new SharedLeverage(request.Leverage));
        }
        #endregion

        #region Futures Order Client

        SharedFeeDeductionType IFuturesOrderRestClient.FuturesFeeDeductionType => SharedFeeDeductionType.AddToCost;
        SharedFeeAssetType IFuturesOrderRestClient.FuturesFeeAssetType => SharedFeeAssetType.QuoteAsset;

        SharedOrderType[] IFuturesOrderRestClient.FuturesSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market };
        SharedTimeInForce[] IFuturesOrderRestClient.FuturesSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel };
        SharedQuantitySupport IFuturesOrderRestClient.FuturesSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts,
                SharedQuantityType.Contracts);

        string IFuturesOrderRestClient.GenerateClientOrderId() => ExchangeHelpers.RandomString(32);

        PlaceFuturesOrderOptions IFuturesOrderRestClient.PlaceFuturesOrderOptions { get; } = new PlaceFuturesOrderOptions(_exchangeName, false);
        async Task<HttpResult<SharedId>> IFuturesOrderRestClient.PlaceFuturesOrderAsync(PlaceFuturesOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.PlaceFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                GetOrderType(request.OrderType, request.TimeInForce),
                quantity: request.Quantity?.QuantityInContracts ?? 0,
                price: request.Price,
                reduceOnly: request.ReduceOnly,
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        }

        GetFuturesOrderOptions IFuturesOrderRestClient.GetFuturesOrderOptions { get; } = new GetFuturesOrderOptions(_exchangeName, true);
        async Task<HttpResult<SharedFuturesOrder>> IFuturesOrderRestClient.GetFuturesOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, validationError);

            var order = await Trading.GetOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedFuturesOrder>(order);

            return HttpResult.Ok(order, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, order.Data.Order.Symbol),
                order.Data.Order.Symbol,
                order.Data.Order.OrderId,
                SharedOrderType.Other,
                order.Data.Order.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Data.Status),
                order.Data.Order.Timestamp)
            {
                ClientOrderId = order.Data.Order.ClientOrderId,
                OrderPrice = order.Data.Order.Price == 0 ? null : order.Data.Order.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: order.Data.Order.Quantity == 0 ? null : order.Data.Order.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: order.Data.Order.QuantityFilled),
                UpdateTime = order.Data.Order.LastUpdateTime,
                ReduceOnly = order.Data.Order.ReduceOnly
            });
        }

        GetOpenFuturesOrdersOptions IFuturesOrderRestClient.GetOpenFuturesOrdersOptions { get; } = new GetOpenFuturesOrdersOptions(_exchangeName, true);
        async Task<HttpResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetOpenFuturesOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOpenFuturesOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder[]>(Exchange, validationError);

            var symbol = request.Symbol?.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOpenOrdersAsync(ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedFuturesOrder[]>(orders);

            IEnumerable<KrakenFuturesOpenOrder> data = orders.Data;
            if (symbol != null)
                data = data.Where(x => x.Symbol == symbol);

            return HttpResult.Ok(orders, data.Select(x => new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol), 
                x.Symbol,
                x.OrderId.ToString(),
                x.Type == FuturesOrderType.Limit ? SharedOrderType.Limit : SharedOrderType.Other,
                x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                SharedOrderStatus.Open,
                x.Timestamp)
            {
                ClientOrderId = x.ClientOrderId,
                OrderPrice = x.Price == 0 ? null : x.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: x.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: x.QuantityFilled),
                UpdateTime = x.LastUpdateTime,
                ReduceOnly = x.ReduceOnly
            }).ToArray());
        }

        GetFuturesClosedOrdersOptions IFuturesOrderRestClient.GetClosedFuturesOrdersOptions { get; } = new GetFuturesClosedOrdersOptions(_exchangeName, false, false, false, 1000)
        {
            Supported = false,
            RequestNotes = "There is no way to retrieve closed orders without specifying order ids, so this method can never return success"
        };
        Task<HttpResult<SharedFuturesOrder[]>> IFuturesOrderRestClient.GetClosedFuturesOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            return Task.FromResult(HttpResult.Fail<SharedFuturesOrder[]>(Exchange, new InvalidOperationError($"Method not available for {Exchange}")));
        }

        GetFuturesOrderTradesOptions IFuturesOrderRestClient.GetFuturesOrderTradesOptions { get; } = new GetFuturesOrderTradesOptions(_exchangeName, true)
        {
            Supported = false,
            RequestNotes = "There is no way to retrieve trades for a specific order, so this method can never return success"
        };
        Task<HttpResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            return Task.FromResult(HttpResult.Fail<SharedUserTrade[]>(Exchange, new InvalidOperationError($"Method not available for {Exchange}")));
        }

        GetFuturesUserTradesOptions IFuturesOrderRestClient.GetFuturesUserTradesOptions { get; } = new GetFuturesUserTradesOptions(_exchangeName, false, true, true, 100);
        async Task<HttpResult<SharedUserTrade[]>> IFuturesOrderRestClient.GetFuturesUserTradesAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesUserTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            int limit = request.Limit ?? 100;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest);

            // Get data
            var result = await Trading.GetUserTradesAsync(
                startTime: request.EndTime,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedUserTrade[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromTime(pageParams, result.Data.Min(x => x.FillTime)),
                     result.Data.Length,
                     result.Data.Select(x => x.FillTime),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.FillTime, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedUserTrade(
                            ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol), 
                            x.Symbol,
                            x.OrderId.ToString(),
                            x.Id.ToString(),
                            x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            x.Quantity,
                            x.Price,
                            x.FillTime)
                        {
                            ClientOrderId = x.ClientOrderId,
                            Price = x.Price,
                            Quantity = x.Quantity,
                            Role = x.Type == TradeType.Maker ? SharedRole.Maker : SharedRole.Taker
                        }).ToArray(), nextPageRequest);
        }

        CancelFuturesOrderOptions IFuturesOrderRestClient.CancelFuturesOrderOptions { get; } = new CancelFuturesOrderOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> IFuturesOrderRestClient.CancelFuturesOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.OrderId.ToString()));
        }

        GetPositionsOptions IFuturesOrderRestClient.GetPositionsOptions { get; } = new GetPositionsOptions(_exchangeName, true);
        async Task<HttpResult<SharedPosition[]>> IFuturesOrderRestClient.GetPositionsAsync(GetPositionsRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetPositionsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedPosition[]>(Exchange, validationError);

            var result = await Trading.GetOpenPositionsAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedPosition[]>(result);

            IEnumerable<KrakenFuturesPosition> data = result.Data;
            if (request.Symbol != null)
                data = data.Where(x => x.Symbol == request.Symbol.GetSymbol(FormatSymbol));

            if (request.TradingMode.HasValue)
            {
                data = data.Where(x => request.TradingMode == TradingMode.PerpetualLinear ? x.Symbol.StartsWith("PF_") :
                    request.TradingMode == TradingMode.DeliveryLinear ? x.Symbol.StartsWith("FF_") :
                    request.TradingMode == TradingMode.PerpetualInverse ? x.Symbol.StartsWith("PI_") :
                    x.Symbol.StartsWith("FI_"));
            }

            var resultTypes = request.Symbol == null && request.TradingMode == null ? SupportedTradingModes : request.Symbol != null ? new[] { request.Symbol.TradingMode } : new[] { request.TradingMode!.Value };
            return HttpResult.Ok(result, data.Select(x => new SharedPosition(ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol), x.Symbol, Math.Abs(x.Quantity), x.FillTime)
            {
                Leverage = x.MaxFixedLeverage,
                AverageOpenPrice = x.Price,
                PositionMode = SharedPositionMode.OneWay,
                PositionSide = x.Side == PositionSide.Short ? SharedPositionSide.Short : SharedPositionSide.Long
            }).ToArray());
        }

        ClosePositionOptions IFuturesOrderRestClient.ClosePositionOptions { get; } = new ClosePositionOptions(_exchangeName, true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(ClosePositionRequest.PositionSide), typeof(SharedPositionSide), "The position side to close", SharedPositionSide.Long),
                new ParameterDescription(nameof(ClosePositionRequest.Quantity), typeof(decimal), "Quantity of the position is required", 0.1m)
            }
        };
        async Task<HttpResult<SharedId>> IFuturesOrderRestClient.ClosePositionAsync(ClosePositionRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.ClosePositionOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);

            var result = await Trading.PlaceOrderAsync(
                symbol,
                request.PositionSide == SharedPositionSide.Long ? OrderSide.Sell : OrderSide.Buy,
                FuturesOrderType.Market,
                quantity: request.Quantity!.Value,
                reduceOnly: true,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.OrderId.ToString()));
        }

        private SharedOrderStatus ParseOrderStatus(KrakenFuturesOrderActiveStatus status)
        {
            if (status == KrakenFuturesOrderActiveStatus.EnteredBook) return SharedOrderStatus.Open;
            if (status == KrakenFuturesOrderActiveStatus.FullyExecuted) return SharedOrderStatus.Filled;
            if (status == KrakenFuturesOrderActiveStatus.Cancelled
                || status == KrakenFuturesOrderActiveStatus.Rejected
                || status == KrakenFuturesOrderActiveStatus.TriggerActivationFailure)
            {
                return SharedOrderStatus.Canceled;
            }

            return SharedOrderStatus.Unknown;
        }

        private FuturesOrderType GetOrderType(SharedOrderType orderType, SharedTimeInForce? timeInForce)
        {
            if (orderType == SharedOrderType.Market)
                return FuturesOrderType.Market;

            if (timeInForce == SharedTimeInForce.ImmediateOrCancel)
                return FuturesOrderType.ImmediateOrCancel;

            return FuturesOrderType.Limit;
        }

        #endregion

        #region Futures Client Id Order Client

        GetFuturesOrderByClientOrderIdOptions IFuturesOrderClientIdRestClient.GetFuturesOrderByClientOrderIdOptions { get; } = new GetFuturesOrderByClientOrderIdOptions(_exchangeName, true);
        async Task<HttpResult<SharedFuturesOrder>> IFuturesOrderClientIdRestClient.GetFuturesOrderByClientOrderIdAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedFuturesOrder>(Exchange, validationError);

            var order = await Trading.GetOrderAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedFuturesOrder>(order);

            return HttpResult.Ok(order, new SharedFuturesOrder(
                ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, order.Data.Order.Symbol),
                order.Data.Order.Symbol,
                order.Data.Order.OrderId,
                SharedOrderType.Other,
                order.Data.Order.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(order.Data.Status),
                order.Data.Order.Timestamp)
            {
                ClientOrderId = order.Data.Order.ClientOrderId,
                OrderPrice = order.Data.Order.Price == 0 ? null : order.Data.Order.Price,
                OrderQuantity = new SharedOrderQuantity(contractQuantity: order.Data.Order.Quantity == 0 ? null : order.Data.Order.Quantity),
                QuantityFilled = new SharedOrderQuantity(contractQuantity: order.Data.Order.QuantityFilled),
                UpdateTime = order.Data.Order.LastUpdateTime,
                ReduceOnly = order.Data.Order.ReduceOnly
            });
        }

        CancelFuturesOrderByClientOrderIdOptions IFuturesOrderClientIdRestClient.CancelFuturesOrderByClientOrderIdOptions { get; } = new CancelFuturesOrderByClientOrderIdOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> IFuturesOrderClientIdRestClient.CancelFuturesOrderByClientOrderIdAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelFuturesOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var order = await Trading.CancelOrderAsync(clientOrderId: request.OrderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(order.Data.OrderId));
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
            var result = await Account.GetFeeScheduleVolumeAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedFee>(result);

            var result2 = await ExchangeData.GetFeeSchedulesAsync(ct).ConfigureAwait(false);
            if (!result2.Success)
                return HttpResult.Fail<SharedFee>(result2);

            decimal? makerFee = null;
            decimal? takerFee = null;
            var volume = result.Data[result2.Data.First().Uid];
            foreach(var level in result2.Data.First().Tiers)
            {
                if (volume < level.UsdVolume)
                    break;

                makerFee = level.MakerFee;
                takerFee = level.TakerFee;
            }

            if (makerFee == null)
                return HttpResult.Fail<SharedFee>(result, new ServerError(ErrorInfo.Unknown with { Message = "Failed to retrieve" }));

            // Return
            return HttpResult.Ok(result, new SharedFee(makerFee.Value, takerFee!.Value));
        }
        #endregion

        #region Tp/SL Client
        SetFuturesTpSlOptions IFuturesTpSlRestClient.SetFuturesTpSlOptions { get; } = new SetFuturesTpSlOptions(_exchangeName, true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(SetTpSlRequest.Quantity), typeof(decimal), "The quantity to close", 0.123m)
            }
        };

        async Task<HttpResult<SharedId>> IFuturesTpSlRestClient.SetFuturesTpSlAsync(SetTpSlRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.SetFuturesTpSlOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await Trading.PlaceOrderAsync(
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

        CancelFuturesTpSlOptions IFuturesTpSlRestClient.CancelFuturesTpSlOptions { get; } = new CancelFuturesTpSlOptions(_exchangeName, true)
        {
            RequiredOptionalParameters = new List<ParameterDescription>
            {
                new ParameterDescription(nameof(CancelTpSlRequest.OrderId), typeof(string), "Id of the tp/sl order", "123123")
            }
        };

        async Task<HttpResult<bool>> IFuturesTpSlRestClient.CancelFuturesTpSlAsync(CancelTpSlRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelFuturesTpSlOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<bool>(Exchange, validationError);

            var result = await Trading.CancelOrderAsync(
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
