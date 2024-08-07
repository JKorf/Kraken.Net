﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Spot API
    /// </summary>
    public interface IKrakenSocketClientSpotApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Subscribe to system status updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-systemStatus" /></para>
        /// </summary>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSystemStatusUpdatesAsync(Action<DataEvent<KrakenStreamSystemStatus>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to ticker updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-ticker" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamTick>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to ticker updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-ticker" /></para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenStreamTick>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-ohlc" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This streamv subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<KrakenStreamKline>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-ohlc" /></para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This streamv subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<KrakenStreamKline>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to trade updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-trade" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<IEnumerable<KrakenTrade>>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to trade updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-trade" /></para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IEnumerable<KrakenTrade>>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to spread updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-spread" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamSpread>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to spread updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-spread" /></para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenStreamSpread>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to depth updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-book" /></para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="depth">Depth of the initial order book snapshot. 10, 25, 100, 500 or 1000</param>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<KrakenStreamOrderBook>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to depth updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-book" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="depth">Depth of the initial order book snapshot. 10, 25, 100, 500 or 1000</param>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenStreamOrderBook>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to open order updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-openOrders" /></para>
        /// </summary>
        /// <param name="socketToken">The socket token as retrieved by the <see cref="IKrakenRestClientSpotApiAccount.GetWebsocketTokenAsync(CancellationToken)">restClient.SpotApi.Account.GetWebsocketTokenAsync</see> method</param>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string socketToken,
            Action<DataEvent<IEnumerable<KrakenStreamOrder>>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to own trade updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-ownTrades" /></para>
        /// </summary>
        /// <param name="socketToken">The socket token as retrieved by the <see cref="IKrakenRestClientSpotApiAccount.GetWebsocketTokenAsync(CancellationToken)">restClient.SpotApi.Account.GetWebsocketTokenAsync</see> method</param>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(string socketToken,
            Action<DataEvent<IEnumerable<KrakenStreamUserTrade>>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to own trade updates
        /// <para><a href="https://docs.kraken.com/websockets/#message-ownTrades" /></para>
        /// </summary>
        /// <param name="socketToken">The socket token as retrieved by the <see cref="IKrakenRestClientSpotApiAccount.GetWebsocketTokenAsync(CancellationToken)">restClient.SpotApi.Account.GetWebsocketTokenAsync</see> method</param>
        /// <param name="snapshot">Whether or not to receive a snapshot of the data upon subscribing</param>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(string socketToken, bool snapshot,
            Action<DataEvent<IEnumerable<KrakenStreamUserTrade>>> handler, CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para><a href="https://docs.kraken.com/websockets/#message-addOrder" /></para>
        /// </summary>
        /// <param name="websocketToken">The socket token as retrieved by the <see cref="IKrakenRestClientSpotApiAccount.GetWebsocketTokenAsync(CancellationToken)">restClient.SpotApi.Account.GetWebsocketTokenAsync</see> method</param>
        /// <param name="symbol">The symbol the order is on, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName"/></param>
        /// <param name="side">The side of the order</param>
        /// <param name="type">The type of the order</param>
        /// <param name="quantity">The quantity of the order</param>
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
        /// <param name="secondaryPrice">Secondary price of an order:<br />
        /// StopLossProfit/StopLossProfitLimit=take profit price<br />
        /// StopLossLimit/TakeProfitLimit=triggered limit price<br />
        /// TrailingStopLimit=triggered limit offset<br />
        /// StopLossAndLimit=limit price</param>
        /// <param name="leverage">Desired leverage</param>
        /// <param name="startTime">Scheduled start time</param>
        /// <param name="expireTime">Expiration time</param>
        /// <param name="validateOnly">Only validate inputs, don't actually place the order</param>
        /// <param name="closeOrderType">Close order type</param>
        /// <param name="closePrice">Close order price</param>
        /// <param name="secondaryClosePrice">Close order secondary price</param>
        /// <param name="flags">Order flags</param>
        /// <param name="reduceOnly">Reduce only order</param>
        /// <param name="margin">Funds the order on margin using the maximum leverage for the pair. Note, absolute max leverage is 5.</param>
        /// <param name="ct">Cancellation token</param>
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
            decimal? secondaryClosePrice = null,
            IEnumerable<OrderFlags>? flags = null,
            bool? reduceOnly = null,
            bool? margin = null, 
            CancellationToken ct = default);

        /// <summary>
        /// Cancel an order
        /// <para><a href="https://docs.kraken.com/websockets/#message-cancelOrder" /></para>
        /// </summary>
        /// <param name="websocketToken">The socket token as retrieved by the <see cref="IKrakenRestClientSpotApiAccount.GetWebsocketTokenAsync(CancellationToken)">restClient.SpotApi.Account.GetWebsocketTokenAsync</see> method</param>
        /// <param name="orderId">Id of the order to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<bool>> CancelOrderAsync(string websocketToken, string orderId, CancellationToken ct = default);

        /// <summary>
        /// Cancel multiple orders
        /// <para><a href="https://docs.kraken.com/websockets/#message-cancelOrder" /></para>
        /// </summary>
        /// <param name="websocketToken">The socket token as retrieved by the <see cref="IKrakenRestClientSpotApiAccount.GetWebsocketTokenAsync(CancellationToken)">restClient.SpotApi.Account.GetWebsocketTokenAsync</see> method</param>
        /// <param name="orderIds">Id of the orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<bool>> CancelOrdersAsync(string websocketToken, IEnumerable<string> orderIds, CancellationToken ct = default);

        /// <summary>
        /// Cancel all open orders
        /// <para><a href="https://docs.kraken.com/websockets/#message-cancelAll" /></para>
        /// </summary>
        /// <param name="websocketToken">The socket token as retrieved by the <see cref="IKrakenRestClientSpotApiAccount.GetWebsocketTokenAsync(CancellationToken)">restClient.SpotApi.Account.GetWebsocketTokenAsync</see> method</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<KrakenStreamCancelAllResult>> CancelAllOrdersAsync(string websocketToken, CancellationToken ct = default);

        /// <summary>
        /// Cancel all open orders after the timeout
        /// <para><a href="https://docs.kraken.com/websockets/#message-cancelAllOrdersAfter" /></para>
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="websocketToken">The socket token as retrieved by the <see cref="IKrakenRestClientSpotApiAccount.GetWebsocketTokenAsync(CancellationToken)">restClient.SpotApi.Account.GetWebsocketTokenAsync</see> method</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<KrakenStreamCancelAfterResult>> CancelAllOrdersAfterAsync(string websocketToken, TimeSpan timeout, CancellationToken ct = default);
    }
}