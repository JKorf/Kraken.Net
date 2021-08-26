using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Socket;

namespace Kraken.Net.Interfaces
{
    /// <summary>
    /// Interface for the Kraken socket client
    /// </summary>
    public interface IKrakenSocketClient: ISocketClient
    {
        /// <summary>
        /// Subscribe to ticker updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamTick>> handler);

        /// <summary>
        /// Subscribe to kline updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<KrakenStreamKline>> handler);

        /// <summary>
        /// Subscribe to trade updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<IEnumerable<KrakenTrade>>> handler);

        /// <summary>
        /// Subscribe to spread updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamSpread>> handler);

        /// <summary>
        /// Subscribe to depth updates
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to</param>
        /// <param name="depth">Depth of the initial order book snapshot</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToDepthUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenStreamOrderBook>> handler);

        /// <summary>
        /// Subscribe to open order updates
        /// </summary>
        /// <param name="socketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string socketToken,
            Action<DataEvent<KrakenStreamOrder>> handler);

        /// <summary>
        /// Subscribe to own trade updates
        /// </summary>
        /// <param name="socketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <param name="handler">Data handler</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOwnTradeUpdatesAsync(string socketToken,
            Action<DataEvent<KrakenStreamUserTrade>> handler);

        /// <summary>
        /// Place a new order
        /// </summary>
        /// <param name="websocketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <param name="symbol">The symbol the order is on</param>
        /// <param name="side">The side of the order</param>
        /// <param name="type">The type of the order</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="clientOrderId">A client id to reference the order by</param>
        /// <param name="price">Price of the order:
        /// Limit=limit price,
        /// StopLoss=stop loss price,
        /// TakeProfit=take profit price,
        /// StopLossProfit=stop loss price,
        /// StopLossProfitLimit=stop loss price,
        /// StopLossLimit=stop loss trigger price,
        /// TakeProfitLimit=take profit trigger price,
        /// TrailingStop=trailing stop offset,
        /// TrailingStopLimit=trailing stop offset,
        /// StopLossAndLimit=stop loss price,
        /// </param>
        /// <param name="secondaryPrice">Secondary price of an order:
        /// StopLossProfit/StopLossProfitLimit=take profit price,
        /// StopLossLimit/TakeProfitLimit=triggered limit price,
        /// TrailingStopLimit=triggered limit offset,
        /// StopLossAndLimit=limit price</param>
        /// <param name="leverage">Desired leverage</param>
        /// <param name="startTime">Scheduled start time</param>
        /// <param name="expireTime">Expiration time</param>
        /// <param name="validateOnly">Only validate inputs, don't actually place the order</param>
        /// <param name="closeOrderType">Close order type</param>
        /// <param name="closePrice">Close order price</param>
        /// <param name="secondaryClosePrice">Close order secondary price</param>
        /// <returns></returns>
        Task<CallResult<KrakenStreamPlacedOrder>> PlaceOrderAsync(
            string websocketToken,
            string symbol,
            OrderType type,
            OrderSide side,
            decimal quantity,
            uint? clientOrderId = null,
            decimal? price = null,
            decimal? secondaryPrice = null,
            decimal? leverage = null,
            DateTime? startTime = null,
            DateTime? expireTime = null,
            bool? validateOnly = null,
            OrderType? closeOrderType = null,
            decimal? closePrice = null,
            decimal? secondaryClosePrice = null);

        /// <summary>
        /// Cancel an order
        /// </summary>
        /// <param name="websocketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <param name="orderId">Id of the order to cancel</param>
        /// <returns></returns>
        Task<CallResult<bool>> CancelOrderAsync(string websocketToken, string orderId);

        /// <summary>
        /// Cancel multiple orders
        /// </summary>
        /// <param name="websocketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <param name="orderIds">Id of the orders to cancel</param>
        /// <returns></returns>
        Task<CallResult<bool>> CancelOrdersAsync(string websocketToken, IEnumerable<string> orderIds);

        /// <summary>
        /// Cancel all open orders
        /// </summary>
        /// <param name="websocketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <returns></returns>
        Task<CallResult<KrakenStreamCancelAllResult>> CancelAllOrdersAsync(string websocketToken);

        /// <summary>
        /// Cancel all open orders after the timeout
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="websocketToken">The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient</param>
        /// <returns></returns>
        Task<CallResult<KrakenStreamCancelAfterResult>> CancelAllOrdersAfterAsync(string websocketToken, TimeSpan timeout);
    }
}