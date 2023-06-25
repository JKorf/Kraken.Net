using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces.CommonClients;
using CryptoExchange.Net.Objects;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.Logging;

namespace Kraken.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IKrakenClientSpotApi" />
    public class KrakenRestClientSpotApi : RestApiClient, IKrakenClientSpotApi, ISpotClient
    {
        #region fields

        /// <inheritdoc />
        public new KrakenRestOptions ClientOptions => (KrakenRestOptions)base.ClientOptions;

        internal static TimeSyncState _timeSyncState = new TimeSyncState("Spot Api");
        #endregion

        #region Api clients

        /// <inheritdoc />
        public IKrakenClientSpotApiAccount Account { get; }
        /// <inheritdoc />
        public IKrakenClientSpotApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IKrakenClientSpotApiTrading Trading { get; }

        /// <inheritdoc />
        public IKrakenClientSpotStakingApi Staking { get; }

        /// <inheritdoc />
        public string ExchangeName => "Kraken";
        #endregion

        /// <summary>
        /// Event triggered when an order is placed via this client
        /// </summary>
        public event Action<OrderId>? OnOrderPlaced;
        /// <summary>
        /// Event triggered when an order is canceled via this client
        /// </summary>
        public event Action<OrderId>? OnOrderCanceled;

        #region ctor
        internal KrakenRestClientSpotApi(ILogger logger, HttpClient? httpClient, KrakenRestOptions options)
            : base(logger, httpClient, options.Environment.SpotRestBaseAddress, options, options.SpotOptions)
        {
            Account = new KrakenRestClientSpotApiAccount(this);
            ExchangeData = new KrakenRestClientSpotApiExchangeData(this); 
            Trading = new KrakenRestClientSpotApiTrading(this);
            Staking = new KrakenRestClientSpotStakingApi(this);

            requestBodyFormat = RequestBodyFormat.FormData;
        }
        #endregion

        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new KrakenAuthenticationProvider(credentials, ClientOptions.NonceProvider ?? new KrakenNonceProvider());

        #region common interface

        async Task<WebCallResult<IEnumerable<Symbol>>> IBaseRestClient.GetSymbolsAsync(CancellationToken ct)
        {
            var exchangeInfo = await ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!exchangeInfo)
                return exchangeInfo.As<IEnumerable<Symbol>>(null);

            return exchangeInfo.As(exchangeInfo.Data.Select(s => new Symbol
            {
                SourceObject = s,
                Name = s.Key,
                MinTradeQuantity = s.Value.OrderMin,
                QuantityDecimals = s.Value.LotDecimals,
                PriceDecimals = s.Value.Decimals
            }));
        }

        async Task<WebCallResult<Ticker>> IBaseRestClient.GetTickerAsync(string symbol, CancellationToken ct)
        {
            var tickers = await ExchangeData.GetTickerAsync(symbol, ct).ConfigureAwait(false);
            if (!tickers)
                return tickers.As<Ticker>(null);

            if (!tickers.Data.Any())
                return tickers.AsError<Ticker>(new ServerError("No symbol found"));

            var ticker = tickers.Data.First();
            return tickers.As(new Ticker
            {
                SourceObject = ticker.Value,
                HighPrice = ticker.Value.High.Value24H,
                LastPrice = ticker.Value.LastTrade.Price,
                LowPrice = ticker.Value.Low.Value24H,
                Price24H = ticker.Value.OpenPrice,
                Symbol = ticker.Key,
                Volume = ticker.Value.Volume.Value24H
            });
        }

        async Task<WebCallResult<IEnumerable<Ticker>>> IBaseRestClient.GetTickersAsync(CancellationToken ct)
        {
            var assets = await ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!assets)
                return assets.As<IEnumerable<Ticker>>(null);

            var ticker = await ExchangeData.GetTickersAsync(assets.Data.Select(d => d.Key).ToArray(), ct).ConfigureAwait(false);
            if (!ticker)
                return ticker.As<IEnumerable<Ticker>>(null);

            return ticker.As(ticker.Data.Select(t => new Ticker
            {
                 SourceObject = t,
                 HighPrice = t.Value.High.Value24H,
                 LastPrice = t.Value.LastTrade.Price,
                 LowPrice = t.Value.Low.Value24H,
                 Price24H = t.Value.OpenPrice,
                 Symbol = t.Key,
                 Volume = t.Value.Volume.Value24H
            }));
        }

        async Task<WebCallResult<IEnumerable<Kline>>> IBaseRestClient.GetKlinesAsync(string symbol, TimeSpan timespan, DateTime? startTime, DateTime? endTime, int? limit, CancellationToken ct)
        {
            if (endTime != null)
                throw new ArgumentException($"Kraken doesn't support the {nameof(endTime)} parameter for the method {nameof(IBaseRestClient.GetKlinesAsync)}", nameof(endTime));

            if (limit != null)
                throw new ArgumentException($"Kraken doesn't support the {nameof(limit)} parameter for the method {nameof(IBaseRestClient.GetKlinesAsync)}", nameof(limit));

            var klines = await ExchangeData.GetKlinesAsync(symbol, GetKlineIntervalFromTimespan(timespan), since: startTime, ct: ct).ConfigureAwait(false);
            if (!klines)
                return klines.As<IEnumerable<Kline>>(null);

            return klines.As(klines.Data.Data.Select(k => new Kline
            {
                SourceObject = k,
                ClosePrice = k.ClosePrice,
                HighPrice = k.HighPrice,
                LowPrice = k.LowPrice,
                OpenPrice = k.OpenPrice,
                OpenTime = k.OpenTime,
                Volume = k.Volume
            }));
        }

        async Task<WebCallResult<OrderBook>> IBaseRestClient.GetOrderBookAsync(string symbol, CancellationToken ct)
        {
            var book = await ExchangeData.GetOrderBookAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!book)
                return book.As<OrderBook>(null);

            return book.As(new OrderBook
            {
                SourceObject = book.Data,
                Asks = book.Data.Asks.Select(a => new OrderBookEntry { Price = a.Price, Quantity = a.Quantity }),
                Bids = book.Data.Bids.Select(b => new OrderBookEntry { Price = b.Price, Quantity = b.Quantity })
            });
        }

        async Task<WebCallResult<IEnumerable<Trade>>> IBaseRestClient.GetRecentTradesAsync(string symbol, CancellationToken ct)
        {
            var tradesResult = await ExchangeData.GetTradeHistoryAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!tradesResult)
                return tradesResult.As<IEnumerable<Trade>>(null);

            return tradesResult.As(tradesResult.Data.Data.Select(d => new Trade
            {
                SourceObject = d,
                Price = d.Price,
                Quantity = d.Quantity,
                Symbol = symbol,
                Timestamp = d.Timestamp
            }));
        }

        async Task<WebCallResult<OrderId>> ISpotClient.PlaceOrderAsync(string symbol, CommonOrderSide side, CommonOrderType type, decimal quantity, decimal? price, string? accountId, string? clientOrderId, CancellationToken ct)
        {
            uint? clientId = null;
            if (clientOrderId != null)
            {
                if (!uint.TryParse(clientOrderId, out var id))
                    throw new ArgumentException("ClientOrderId for Bitfinex should be parsable to int");
                else
                    clientId = id;
            }

            var result = await Trading.PlaceOrderAsync(symbol, GetOrderSide(side), GetOrderType(type), quantity, price: price, clientOrderId: clientId, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<OrderId>(null);

            return result.As(new OrderId
            {
                SourceObject = result.Data,
                Id = result.Data.OrderIds.First()
            });
        }

        async Task<WebCallResult<Order>> IBaseRestClient.GetOrderAsync(string orderId, string? symbol, CancellationToken ct)
        {
            var result = await Trading.GetOrderAsync(orderId, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<Order>(null);

            if (!result.Data.Any())
                return result.AsError<Order>(new ServerError("Order not found"));
                
            var order = result.Data.First();
            return result.As(new Order
            {
                SourceObject = order,
                Id = order.Key,
                Price = order.Value.Price,
                Quantity = order.Value.Quantity,
                QuantityFilled = order.Value.QuantityFilled,
                Symbol = order.Value.OrderDetails.Symbol,
                Timestamp = order.Value.CreateTime,
                Side = order.Value.OrderDetails.Side == OrderSide.Buy ? CommonOrderSide.Buy: CommonOrderSide.Sell,
                Type = order.Value.OrderDetails.Type == OrderType.Limit ? CommonOrderType.Limit: order.Value.OrderDetails.Type == Enums.OrderType.Market? CommonOrderType.Market: CommonOrderType.Other,
                Status = order.Value.Status == OrderStatus.Canceled ? CommonOrderStatus.Canceled: order.Value.Status == OrderStatus.Closed || order.Value.Status == Enums.OrderStatus.Pending ? CommonOrderStatus.Active: CommonOrderStatus.Filled
            });
        }

        async Task<WebCallResult<IEnumerable<UserTrade>>> IBaseRestClient.GetOrderTradesAsync(string orderId, string? symbol, CancellationToken ct)
        {
            var result = await Trading.GetUserTradesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<UserTrade>>(null);

            return result.As(result.Data.Trades.Where(t => t.Value.OrderId == orderId).Select(t => new UserTrade
            {
                SourceObject = t,
                Id = t.Key,
                Fee = t.Value.Fee,
                OrderId = t.Value.OrderId,
                Price = t.Value.Price,
                Quantity = t.Value.Quantity,
                Symbol = t.Value.Symbol,
                Timestamp = t.Value.Timestamp
            }));
        }

        async Task<WebCallResult<IEnumerable<Order>>> IBaseRestClient.GetOpenOrdersAsync(string? symbol, CancellationToken ct)
        {
            var result = await Trading.GetOpenOrdersAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<Order>>(null);

            return result.As(result.Data.Open.Select(order => new Order
            {
                SourceObject = order,
                Id = order.Key,
                Price = order.Value.Price,
                Quantity = order.Value.Quantity,
                QuantityFilled = order.Value.QuantityFilled,
                Symbol = order.Value.OrderDetails.Symbol,
                Timestamp = order.Value.CreateTime,
                Side = order.Value.OrderDetails.Side == OrderSide.Buy ? CommonOrderSide.Buy : CommonOrderSide.Sell,
                Type = order.Value.OrderDetails.Type == OrderType.Limit ? CommonOrderType.Limit : order.Value.OrderDetails.Type == Enums.OrderType.Market ? CommonOrderType.Market : CommonOrderType.Other,
                Status = order.Value.Status == OrderStatus.Canceled ? CommonOrderStatus.Canceled : order.Value.Status == OrderStatus.Closed || order.Value.Status == Enums.OrderStatus.Pending ? CommonOrderStatus.Active : CommonOrderStatus.Filled
            }));
        }

        async Task<WebCallResult<IEnumerable<Order>>> IBaseRestClient.GetClosedOrdersAsync(string? symbol, CancellationToken ct)
        {
            var result = await Trading.GetClosedOrdersAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<Order>>(null);

            return result.As(result.Data.Closed.Select(order => new Order
            {
                SourceObject = order,
                Id = order.Key,
                Price = order.Value.Price,
                Quantity = order.Value.Quantity,
                QuantityFilled = order.Value.QuantityFilled,
                Symbol = order.Value.OrderDetails.Symbol,
                Timestamp = order.Value.CreateTime,
                Side = order.Value.OrderDetails.Side == OrderSide.Buy ? CommonOrderSide.Buy : CommonOrderSide.Sell,
                Type = order.Value.OrderDetails.Type == OrderType.Limit ? CommonOrderType.Limit : order.Value.OrderDetails.Type == Enums.OrderType.Market ? CommonOrderType.Market : CommonOrderType.Other,
                Status = order.Value.Status == OrderStatus.Canceled ? CommonOrderStatus.Canceled : order.Value.Status == OrderStatus.Closed || order.Value.Status == Enums.OrderStatus.Pending ? CommonOrderStatus.Active : CommonOrderStatus.Filled
            }));
        }

        async Task<WebCallResult<OrderId>> IBaseRestClient.CancelOrderAsync(string orderId, string? symbol, CancellationToken ct)
        {
            var result = await Trading.CancelOrderAsync(orderId, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<OrderId>(null);

            if (!result.Data.Pending.Any() && result.Data.Count == 0)
                return result.AsError<OrderId>(new ServerError("No orders canceled"));

            return result.As(new OrderId
            {
                SourceObject = result.Data,
                Id = orderId
            });
        }

        async Task<WebCallResult<IEnumerable<Balance>>> IBaseRestClient.GetBalancesAsync(string? accountId, CancellationToken ct)
        {
            var result = await Account.GetAvailableBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<Balance>>(null);

            return result.As(result.Data.Values.Select(b => new Balance
            {
                SourceObject = b,
                Asset = b.Asset,
                Available = b.Available,
                Total = b.Total
            }));
        }
        #endregion

        internal Uri GetUri(string endpoint)
        {
            return new Uri(BaseAddress.AppendPath(endpoint));
        }

        internal void InvokeOrderPlaced(OrderId id)
        {
            OnOrderPlaced?.Invoke(id);
        }

        internal void InvokeOrderCanceled(OrderId id)
        {
            OnOrderCanceled?.Invoke(id);
        }

        /// <inheritdoc />
        protected override async Task<bool> ShouldRetryRequestAsync<T>(WebCallResult<T> callResult, int tries)
        {
            if (!callResult.Success)
                return false;

            var krakenResult = (KrakenResult)(object)callResult.Data!;
            if (krakenResult.Error.FirstOrDefault() == "EAPI:Invalid nonce")
            {
                if (tries <= 3)
                {
                    _logger.Log(LogLevel.Warning, "Received nonce error; retrying request");
                    await Task.Delay(25).ConfigureAwait(false);
                    return true;
                }
            }

            return false;
        }

        internal async Task<WebCallResult<T>> Execute<T>(Uri url, HttpMethod method, CancellationToken ct, Dictionary<string, object>? parameters = null, bool signed = false, int weight = 1, bool ignoreRatelimit = false)
        {
            var result = await SendRequestAsync<KrakenResult<T>>(url, method, ct, parameters, signed, requestWeight: weight, ignoreRatelimit: ignoreRatelimit).ConfigureAwait(false);
            if (!result)
                return result.AsError<T>(result.Error!);

            if (result.Data.Error.Any())
                return result.AsError<T>(new ServerError(string.Join(", ", result.Data.Error)));

            return result.As(result.Data.Result);
        }

        /// <summary>
        /// Get the name of a symbol for Kraken based on the base and quote asset
        /// </summary>
        /// <param name="baseAsset"></param>
        /// <param name="quoteAsset"></param>
        /// <returns></returns>
        public string GetSymbolName(string baseAsset, string quoteAsset) => (baseAsset + quoteAsset).ToUpperInvariant();

        private static KlineInterval GetKlineIntervalFromTimespan(TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.FromMinutes(1)) return KlineInterval.OneMinute;
            if (timeSpan == TimeSpan.FromMinutes(5)) return KlineInterval.FiveMinutes;
            if (timeSpan == TimeSpan.FromMinutes(15)) return KlineInterval.FifteenMinutes;
            if (timeSpan == TimeSpan.FromMinutes(30)) return KlineInterval.ThirtyMinutes;
            if (timeSpan == TimeSpan.FromHours(1)) return KlineInterval.OneHour;
            if (timeSpan == TimeSpan.FromHours(4)) return KlineInterval.FourHour;
            if (timeSpan == TimeSpan.FromDays(1)) return KlineInterval.OneDay;
            if (timeSpan == TimeSpan.FromDays(7)) return KlineInterval.OneWeek;
            if (timeSpan == TimeSpan.FromDays(15)) return KlineInterval.FifteenDays;

            throw new ArgumentException("Unsupported timespan for Kraken Klines, check supported intervals using Kraken.Net.Objects.KlineInterval");
        }

        private static OrderSide GetOrderSide(CommonOrderSide side)
        {
            if (side == CommonOrderSide.Sell) return OrderSide.Sell;
            if (side == CommonOrderSide.Buy) return OrderSide.Buy;

            throw new ArgumentException("Unsupported order side for Kraken order: " + side);
        }

        private static OrderType GetOrderType(CommonOrderType type)
        {
            if (type == CommonOrderType.Limit) return OrderType.Limit;
            if (type == CommonOrderType.Market) return OrderType.Market;

            throw new ArgumentException("Unsupported order type for Kraken order: " + type);
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, ClientOptions.AutoTimestamp, ClientOptions.TimestampRecalculationInterval, _timeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => _timeSyncState.TimeOffset;

        /// <inheritdoc />
        public ISpotClient CommonSpotClient => this;
    }
}
