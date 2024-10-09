using CryptoExchange.Net.Objects;
using Kraken.Net.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kraken.Net.Objects.Models;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Kraken trading endpoints, placing and mananging orders.
    /// </summary>
    public interface IKrakenRestClientSpotApiTrading
    {
        /// <summary>
        /// Get a list of open orders
        /// <para><a href="https://docs.kraken.com/rest/#operation/getOpenOrders" /></para>
        /// </summary>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of open orders</returns>
        Task<WebCallResult<OpenOrdersPage>> GetOpenOrdersAsync(uint? clientOrderId = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get a list of closed orders
        /// <para><a href="https://docs.kraken.com/rest/#operation/getClosedOrders" /></para>
        /// </summary>
        /// <param name="clientOrderId">Filter by client order id</param>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Closed orders page</returns>
        Task<WebCallResult<KrakenClosedOrdersPage>> GetClosedOrdersAsync(uint? clientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific order
        /// <para><a href="https://docs.kraken.com/rest/#operation/getOrdersInfo" /></para>
        /// </summary>
        /// <param name="clientOrderId">Get orders by clientOrderId</param>
        /// <param name="orderId">Get order by its order id</param>
        /// <param name="consolidateTaker">Whether or not to consolidate trades by individual taker trades</param>
        /// <param name="trades">Whether to include trades in the response</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with order info</returns>
        Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrderAsync(string? orderId = null, uint? clientOrderId = null, bool? consolidateTaker = null, bool? trades = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific orders
        /// <para><a href="https://docs.kraken.com/rest/#operation/getOrdersInfo" /></para>
        /// </summary>
        /// <param name="clientOrderId">Get orders by clientOrderId</param>
        /// <param name="orderIds">Get orders by their order ids</param>
        /// <param name="consolidateTaker">Whether or not to consolidate trades by individual taker trades</param>
        /// <param name="trades">Whether to include trades in the response</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with order info</returns>
        Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrdersAsync(IEnumerable<string>? orderIds = null, uint? clientOrderId = null, bool? consolidateTaker = null, bool? trades = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get trade history
        /// <para><a href="https://docs.kraken.com/rest/#operation/getTradeHistory" /></para>
        /// </summary>
        /// <param name="startTime">Return data after this time</param>
        /// <param name="endTime">Return data before this time</param>
        /// <param name="resultOffset">Offset the results by</param>
        /// <param name="consolidateTaker">Whether or not to consolidate trades by individual taker trades</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Trade history page</returns>
        Task<WebCallResult<KrakenUserTradesPage>> GetUserTradesAsync(DateTime? startTime = null, DateTime? endTime = null, int? resultOffset = null, bool? consolidateTaker = null, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific trades
        /// <para><a href="https://docs.kraken.com/rest/#operation/getTradesInfo" /></para>
        /// </summary>
        /// <param name="tradeId">The trade to get info on</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with trade info</returns>
        Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(string tradeId, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Get info on specific trades
        /// <para><a href="https://docs.kraken.com/rest/#operation/getTradesInfo" /></para>
        /// </summary>
        /// <param name="tradeIds">The trades to get info on</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Dictionary with trade info</returns>
        Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(IEnumerable<string> tradeIds, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Place multiple new orders
        /// </summary>
        /// <param name="symbol">The symbol the order is on, for example `ETHUSDT`</param>
        /// <param name="orders">The orders to place</param>
        /// <param name="deadline">Deadline after which the orders will be rejected</param>
        /// <param name="validateOnly">Only validate inputs, don't actually place the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenBatchOrderResult>> PlaceMultipleOrdersAsync(string symbol, IEnumerable<KrakenOrderRequest> orders, DateTime? deadline = null, bool? validateOnly = null, CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para><a href="https://docs.kraken.com/rest/#operation/addOrder" /></para>
        /// </summary>
        /// <param name="symbol">The symbol the order is on, for example `ETHUSDT`</param>
        /// <param name="side">The side of the order</param>
        /// <param name="type">The type of the order</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="userReference">A numeric id to reference the order by</param>
        /// <param name="clientOrderId">A client id to reference the order by</param>
        /// <param name="price">Price of the order:<br />
        /// Limit=limit price<br />
        /// StopLoss=stop loss price<br />
        /// TakeProfit=take profit price<br />
        /// StopLossProfit=stop loss price<br />
        /// StopLossProfitLimit=stop loss price<br />
        /// StopLossLimit=stop loss trigger price<br />
        /// TakeProfitLimit=take profit trigger price<br />
        /// TrailingStop=trailing stop offset<br />
        /// TrailingStopLimit=trailing stop offset<br />
        /// StopLossAndLimit=stop loss price
        /// </param>
        /// <param name="secondaryPrice">Secondary price of an order:
        /// StopLossProfit/StopLossProfitLimit=take profit price<br />
        /// StopLossLimit/TakeProfitLimit=triggered limit price<br />
        /// TrailingStopLimit=triggered limit offset<br />
        /// StopLossAndLimit=limit price</param>
        /// <param name="leverage">Desired leverage</param>
        /// <param name="startTime">Scheduled start time</param>
        /// <param name="expireTime">Expiration time</param>
        /// <param name="validateOnly">Only validate inputs, don't actually place the order</param>
        /// <param name="orderFlags">Flags for the order</param>
        /// <param name="reduceOnly">Reduce only order</param>
        /// <param name="icebergQuanty">Iceberg visible quantity</param>
        /// <param name="trigger">Price signal</param>
        /// <param name="selfTradePreventionType">Self trade prevention type</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="timeInForce">Time-in-force of the order to specify how long it should remain in the order book before being cancelled</param>
        /// <param name="closeOrderType">Close order type</param>
        /// <param name="closePrice">Close order price</param>
        /// <param name="secondaryClosePrice">Close order secondary price</param>
        /// <param name="pricePrefixOperator">Prefix operator for the price field, `+` or `-` for a relative offset to the last traded price, `#` will either add or subtract the amount to the last traded price, depending on the direction and order type used. For trailing stop orders always `+` can be used for an absolute price</param>
        /// <param name="priceSuffixOperator">Suffix operator for the price field. Only option is `%` to specify a relative percentage offset for trailing orders</param>
        /// <param name="secondaryPricePrefixOperator">Prefix operator for the secondary price field, `+` or `-` for a relative offset to the last traded price, `#` will either add or subtract the amount to the last traded price, depending on the direction and order type used.</param>
        /// <param name="secondaryPriceSuffixOperator">Suffix operator for the price field. Only option is `%` to specify a relative percentage offset for trailing orders</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Placed order info</returns>
        Task<WebCallResult<KrakenPlacedOrder>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal quantity,
            decimal? price = null,
            decimal? secondaryPrice = null,
            decimal? leverage = null,
            DateTime? startTime = null,
            DateTime? expireTime = null,
            bool? validateOnly = null,
            uint? userReference = null,
            string? clientOrderId = null,
            IEnumerable<OrderFlags>? orderFlags = null,
            string? twoFactorPassword = null,
            TimeInForce? timeInForce = null,
            bool? reduceOnly = null,
            decimal? icebergQuanty = null,
            Trigger? trigger = null,
            SelfTradePreventionType? selfTradePreventionType = null,
            OrderType? closeOrderType = null,
            decimal? closePrice = null,
            decimal? secondaryClosePrice = null,
            string? pricePrefixOperator = null,
            string? priceSuffixOperator = null,
            string? secondaryPricePrefixOperator = null,
            string? secondaryPriceSuffixOperator = null,
            CancellationToken ct = default);

        /// <summary>
        /// Edit an order
        /// <para><a href="https://docs.kraken.com/rest/#operation/editOrder" /></para>
        /// </summary>
        /// <param name="symbol">Symbol, for example `ETHUSDT`</param>
        /// <param name="orderId">Order id or client order id of the order to edit</param>
        /// <param name="quantity">New quantity</param>
        /// <param name="icebergQuanty">Iceberg quantity</param>
        /// <param name="price">Price</param>
        /// <param name="secondaryPrice">Secondary price</param>
        /// <param name="flags">Flags</param>
        /// <param name="deadline">Deadline</param>
        /// <param name="cancelResponse">Used to interpret if client wants to receive pending replace, before the order is completely replaced</param>
        /// <param name="validateOnly">Only validate inputs, don't actually place the order</param>
        /// <param name="newClientOrderId">New client order id</param>
        /// <param name="pricePrefixOperator">Prefix operator for the price field, `+` or `-` for a relative offset to the last traded price, `#` will either add or subtract the amount to the last traded price, depending on the direction and order type used. For trailing stop orders always `+` can be used for an absolute price</param>
        /// <param name="priceSuffixOperator">Suffix operator for the price field. Only option is `%` to specify a relative percentage offset for trailing orders</param>
        /// <param name="secondaryPricePrefixOperator">Prefix operator for the secondary price field, `+` or `-` for a relative offset to the last traded price, `#` will either add or subtract the amount to the last traded price, depending on the direction and order type used.</param>
        /// <param name="secondaryPriceSuffixOperator">Suffix operator for the price field. Only option is `%` to specify a relative percentage offset for trailing orders</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<WebCallResult<KrakenEditOrder>> EditOrderAsync(
            string symbol,
            string orderId,
            decimal? quantity = null,
            decimal? icebergQuanty = null,
            decimal? price = null,
            decimal? secondaryPrice = null,
            IEnumerable<OrderFlags>? flags = null,
            DateTime? deadline = null,
            bool? cancelResponse = null,
            bool? validateOnly = null,
            uint? newClientOrderId = null,
            string? twoFactorPassword = null,
            string? pricePrefixOperator = null,
            string? priceSuffixOperator = null,
            string? secondaryPricePrefixOperator = null,
            string? secondaryPriceSuffixOperator = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel an order
        /// <para><a href="https://docs.kraken.com/rest/#operation/cancelOrder" /></para>
        /// </summary>
        /// <param name="orderId">The id of the order to cancel</param>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Cancel result</returns>
        Task<WebCallResult<KrakenCancelResult>> CancelOrderAsync(string orderId, string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders
        /// <para><a href="https://docs.kraken.com/rest/#operation/cancelAllOrders" /></para>
        /// </summary>
        /// <param name="twoFactorPassword">Password or authentication app code if enabled</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Cancel result</returns>
        Task<WebCallResult<KrakenCancelResult>> CancelAllOrdersAsync(string? twoFactorPassword = null, CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders after the timeout expires. Can be called at an interval to keep extending the timeout and word as a dead-man switch. Timeout can be disabled by setting timeout to 0.
        /// <para><a href="https://docs.kraken.com/api/docs/rest-api/cancel-all-orders-after" /></para>
        /// </summary>
        /// <param name="cancelAfter"></param>
        /// <param name="twoFactorPassword"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<KrakenCancelAfterResult>> CancelAllOrdersAfterAsync(TimeSpan cancelAfter, string? twoFactorPassword = null, CancellationToken ct = default);
    }
}
