using Kraken.Net;
using Kraken.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis.Interfaces;
using CryptoExchange.Net.SharedApis.RequestModels;
using CryptoExchange.Net.SharedApis.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.SharedApis.Models.Rest;
using CryptoExchange.Net.SharedApis.Enums;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;

namespace Kraken.Net.Clients.SpotApi
{
    internal partial class KrakenRestClientSpotApi : IKrakenRestClientSpotApiShared
    {
        public string Exchange => KrakenExchange.ExchangeName;

        public IEnumerable<SharedOrderType> SupportedOrderType { get; } = new[]
        {
            SharedOrderType.Limit,
            SharedOrderType.Market,
            SharedOrderType.LimitMaker
        };

        public IEnumerable<SharedTimeInForce> SupportedTimeInForce { get; } = new[]
        {
            SharedTimeInForce.GoodTillCanceled,
            SharedTimeInForce.ImmediateOrCancel,
            SharedTimeInForce.FillOrKill
        };

        public SharedQuantitySupport OrderQuantitySupport { get; } =
            new SharedQuantitySupport(
                SharedQuantityType.BaseAssetQuantity,
                SharedQuantityType.BaseAssetQuantity,
                SharedQuantityType.Both,
                SharedQuantityType.BaseAssetQuantity);

        async Task<WebCallResult<IEnumerable<SharedKline>>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval.TotalSeconds;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new WebCallResult<IEnumerable<SharedKline>>(new ArgumentError("Interval not supported"));

            var result = await ExchangeData.GetKlinesAsync(
                FormatSymbol(request.BaseAsset, request.QuoteAsset, request.ApiType),
                interval,
                request.StartTime,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<SharedKline>>(default);

            return result.As(result.Data.Data.Select(x => new SharedKline(x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume)));
        }

        async Task<WebCallResult<IEnumerable<SharedSpotSymbol>>> ISpotSymbolRestClient.GetSymbolsAsync(SharedRequest request, CancellationToken ct)
        {
            var result = await ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<SharedSpotSymbol>>(default);

            return result.As(result.Data.Select(s => new SharedSpotSymbol(s.Value.BaseAsset, s.Value.QuoteAsset, s.Key)));
        }

        async Task<WebCallResult<SharedTicker>> ITickerRestClient.GetTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var result = await ExchangeData.GetTickerAsync(
                FormatSymbol(request.BaseAsset, request.QuoteAsset, request.ApiType),
                ct).ConfigureAwait(false);
            if (!result)
                return result.As<SharedTicker>(default);

            var ticker = result.Data.Single();
            return result.As(new SharedTicker(ticker.Value.Symbol, ticker.Value.LastTrade.Price, ticker.Value.High.Value24H, ticker.Value.Low.Value24H));
        }

        async Task<WebCallResult<IEnumerable<SharedTicker>>> ITickerRestClient.GetTickersAsync(SharedRequest request, CancellationToken ct)
        {
            var result = await ExchangeData.GetTickersAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<SharedTicker>>(default);

            return result.As(result.Data.Select(x => new SharedTicker(x.Value.Symbol, x.Value.LastTrade.Price, x.Value.High.Value24H, x.Value.Low.Value24H)));
        }

        async Task<WebCallResult<IEnumerable<SharedTrade>>> ITradeRestClient.GetTradesAsync(GetTradesRequest request, CancellationToken ct)
        {
            var result = await ExchangeData.GetTradeHistoryAsync(
                FormatSymbol(request.BaseAsset, request.QuoteAsset, request.ApiType),
                since: request.StartTime,
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<SharedTrade>>(default);

            return result.As(result.Data.Data.Select(x => new SharedTrade(x.Quantity, x.Price, x.Timestamp)));
        }

        async Task<WebCallResult<IEnumerable<SharedBalance>>> IBalanceRestClient.GetBalancesAsync(SharedRequest request, CancellationToken ct)
        {
            var result = await Account.GetAvailableBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result)
                return result.As<IEnumerable<SharedBalance>>(default);

            return result.As(result.Data.Select(x => new SharedBalance(x.Key, x.Value.Available, x.Value.Total)));
        }

        async Task<WebCallResult<SharedOrderId>> ISpotOrderRestClient.PlaceOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            uint cid = 0;
            if (request.ClientOrderId != null && !uint.TryParse(request.ClientOrderId, out cid))
                return new WebCallResult<SharedOrderId>(new ArgumentError("Client order id needs to be a positive integer if specified"));

            var result = await Trading.PlaceOrderAsync(
                FormatSymbol(request.BaseAsset, request.QuoteAsset, request.ApiType),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                GetPlaceOrderType(request.OrderType),
                request.Quantity ?? request.QuoteQuantity ?? 0,
                request.Price,
                clientOrderId: request.ClientOrderId != null ? cid : null,
                orderFlags: GetOrderFlags(request.OrderType, request.QuoteQuantity),
                timeInForce: GetTimeInForce(request.TimeInForce)).ConfigureAwait(false);

            if (!result)
                return result.As<SharedOrderId>(default);

            return result.As(new SharedOrderId(result.Data.OrderIds.Single().ToString()));
        }

        async Task<WebCallResult<SharedSpotOrder>> ISpotOrderRestClient.GetOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var order = await Trading.GetOrderAsync(request.OrderId).ConfigureAwait(false);
            if (!order)
                return order.As<SharedSpotOrder>(default);

            var orderData = order.Data.SingleOrDefault();
            if (orderData.Value == null)
                return new WebCallResult<SharedSpotOrder>(new ServerError("Order not found"));

            return order.As(new SharedSpotOrder(
                orderData.Value.OrderDetails.Symbol,
                orderData.Value.Id.ToString(),
                ParseOrderType(orderData.Value.OrderDetails.Type, orderData.Value.Oflags),
                orderData.Value.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(orderData.Value.Status),
                orderData.Value.CreateTime)
            {
                ClientOrderId = orderData.Value.ClientOrderId,
                Fee = orderData.Value.Fee,
                Price = orderData.Value.OrderDetails.Price,
                Quantity = orderData.Value.Quantity,
                QuantityFilled = orderData.Value.QuantityFilled,
                QuoteQuantityFilled = orderData.Value.QuoteQuantityFilled,
                AveragePrice = orderData.Value.AveragePrice,
                UpdateTime = orderData.Value.CloseTime
            });
        }

        async Task<WebCallResult<IEnumerable<SharedSpotOrder>>> ISpotOrderRestClient.GetOpenOrdersAsync(GetSpotOpenOrdersRequest request, CancellationToken ct)
        {
            string? symbol = null;
            if (request.BaseAsset != null && request.QuoteAsset != null)
                symbol = FormatSymbol(request.BaseAsset, request.QuoteAsset, request.ApiType);

            var order = await Trading.GetOpenOrdersAsync().ConfigureAwait(false);
            if (!order)
                return order.As<IEnumerable<SharedSpotOrder>>(default);

            IEnumerable<KrakenOrder> orders = order.Data.Open.Values.AsEnumerable();
            if (symbol != null)
                orders = orders.Where(x => x.OrderDetails.Symbol == symbol);

            return order.As(orders.Select(x => new SharedSpotOrder(
                x.OrderDetails.Symbol,
                x.Id.ToString(),
                ParseOrderType(x.OrderDetails.Type, x.Oflags),
                x.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                Fee = x.Fee,
                Price = x.OrderDetails.Price,
                Quantity = x.Quantity,
                QuantityFilled = x.QuantityFilled,
                QuoteQuantityFilled = x.QuoteQuantityFilled,
                AveragePrice = x.AveragePrice,
                UpdateTime = x.CloseTime
            }));
        }

        async Task<WebCallResult<IEnumerable<SharedSpotOrder>>> ISpotOrderRestClient.GetClosedOrdersAsync(GetSpotClosedOrdersRequest request, CancellationToken ct)
        {
            string? symbol = null;
            if (request.BaseAsset != null && request.QuoteAsset != null)
                symbol = FormatSymbol(request.BaseAsset, request.QuoteAsset, request.ApiType);

            var order = await Trading.GetClosedOrdersAsync(startTime: request.StartTime, endTime: request.EndTime).ConfigureAwait(false);
            if (!order)
                return order.As<IEnumerable<SharedSpotOrder>>(default);

            IEnumerable<KrakenOrder> orders = order.Data.Closed.Values.AsEnumerable();
            if (symbol != null)
                orders = orders.Where(x => x.OrderDetails.Symbol == symbol);

            return order.As(orders.Select(x => new SharedSpotOrder(
                x.OrderDetails.Symbol,
                x.Id.ToString(),
                ParseOrderType(x.OrderDetails.Type, x.Oflags),
                x.OrderDetails.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                ParseOrderStatus(x.Status),
                x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                Fee = x.Fee,
                Price = x.OrderDetails.Price,
                Quantity = x.Quantity,
                QuantityFilled = x.QuantityFilled,
                QuoteQuantityFilled = x.QuoteQuantityFilled,
                AveragePrice = x.AveragePrice,
                UpdateTime = x.CloseTime
            }));
        }

        async Task<WebCallResult<IEnumerable<SharedUserTrade>>> ISpotOrderRestClient.GetOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var order = await Trading.GetOrderAsync(request.OrderId, trades: true).ConfigureAwait(false);
            if (!order)
                return order.As<IEnumerable<SharedUserTrade>>(default);

            var orderData = order.Data.SingleOrDefault();
            if (orderData.Value == null)
                return new WebCallResult<IEnumerable<SharedUserTrade>>(new ServerError("Order not found"));

            var trades = orderData.Value.TradeIds.ToList();
            if (!trades.Any())
                return order.As<IEnumerable<SharedUserTrade>>(new SharedUserTrade[] { });

            var tradeInfo = await Trading.GetUserTradeDetailsAsync(trades.Take(20), ct: ct).ConfigureAwait(false);
            if (!tradeInfo)
                return tradeInfo.As<IEnumerable<SharedUserTrade>>(default);

            return tradeInfo.As(tradeInfo.Data.Select(x => new SharedUserTrade(
                x.Value.Symbol,
                x.Value.OrderId.ToString(),
                x.Value.Id.ToString(),
                x.Value.Quantity,
                x.Value.Price,
                x.Value.Timestamp)
            {
                Fee = x.Value.Fee,
                Role = x.Value.Maker ? SharedRole.Maker : SharedRole.Taker
            }));
        }

        async Task<WebCallResult<IEnumerable<SharedUserTrade>>> ISpotOrderRestClient.GetUserTradesAsync(GetUserTradesRequest request, CancellationToken ct)
        {
            var order = await Trading.GetUserTradesAsync(startTime: request.StartTime, endTime: request.EndTime).ConfigureAwait(false);
            if (!order)
                return order.As<IEnumerable<SharedUserTrade>>(default);

            var symbol = FormatSymbol(request.BaseAsset, request.QuoteAsset, request.ApiType);
            return order.As(order.Data.Trades.Where(t => t.Value.Symbol == symbol).Select(x => new SharedUserTrade(
                x.Value.Symbol,
                x.Value.OrderId.ToString(),
                x.Value.Id.ToString(),
                x.Value.Quantity,
                x.Value.Price,
                x.Value.Timestamp)
            {
                Fee = x.Value.Fee,
                Role = x.Value.Maker ? SharedRole.Maker : SharedRole.Taker
            }));
        }

        async Task<WebCallResult<SharedOrderId>> ISpotOrderRestClient.CancelOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var order = await Trading.CancelOrderAsync(request.OrderId).ConfigureAwait(false);
            if (!order)
                return order.As<SharedOrderId>(default);

            return order.As(new SharedOrderId(order.Data.ToString()));
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
    }
}
