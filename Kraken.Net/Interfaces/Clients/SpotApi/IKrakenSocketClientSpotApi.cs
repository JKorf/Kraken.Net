using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects.Sockets;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Objects.Models.Socket.Futures;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Spot V2 websocket API
    /// </summary>
    public interface IKrakenSocketClientSpotApi : ISocketApiClient, IDisposable
    {
        /// <summary>
        /// Get the shared socket subscription client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        IKrakenSocketClientSpotApiShared SharedClient { get; }

        /// <summary>
        /// Subscribe to system status updates
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/status" /></para>
        /// </summary>
        /// <param name="handler">Data handler</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToSystemStatusUpdatesAsync(Action<DataEvent<KrakenStreamSystemStatus>> handler, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to ticker updates
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/ticker" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, AClass?, bool?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="handler">Data handler</param>
        /// <param name="eventTrigger">The update event trigger</param>
        /// <param name="snapshot">Whether to receive an initial snapshot after subscribing</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenTickerUpdate>> handler, TriggerEvent? eventTrigger = null, bool? snapshot = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to ticker updates
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/ticker" /></para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, AClass?, bool?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="handler">Data handler</param>
        /// <param name="eventTrigger">The update event trigger</param>
        /// <param name="snapshot">Whether to receive an initial snapshot after subscribing</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenTickerUpdate>> handler, TriggerEvent? eventTrigger = null, bool? snapshot = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline updates
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/ohlc" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, AClass?, bool?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="handler">Data handler</param>
        /// <param name="snapshot">Whether or not a snapshot of the last trades should be send after subscribing</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<KrakenKlineUpdate[]>> handler, bool? snapshot = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to kline updates
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/ohlc" /></para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, AClass?, bool?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="interval">Kline interval</param>
        /// <param name="handler">Data handler</param>
        /// <param name="snapshot">Whether or not a snapshot of the last trades should be send after subscribing</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<KrakenKlineUpdate[]>> handler, bool? snapshot = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to trade updates
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/trade" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, AClass?, bool?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="handler">Data handler</param>
        /// <param name="snapshot">Whether or not a snapshot of the last trades should be send after subscribing</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<KrakenTradeUpdate[]>> handler, bool? snapshot = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to trade updates
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/trade" /></para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, AClass?, bool?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="handler">Data handler</param>
        /// <param name="snapshot">Whether or not a snapshot of the last trades should be send after subscribing</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenTradeUpdate[]>> handler, bool? snapshot = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates. Order book entries are aggregated per price level
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/book" /></para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, AClass?, bool?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="depth">Depth of the initial order book snapshot. 10, 25, 100, 500 or 1000</param>
        /// <param name="handler">Data handler</param>
        /// <param name="snapshot">Whether or not a snapshot of the last book state should be send after subscribing</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAggregatedOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<KrakenBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book updates. Order book entries are aggregated per price level
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/book" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, AClass?, bool?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="depth">Depth of the initial order book snapshot. 10, 25, 100, 500 or 1000</param>
        /// <param name="handler">Data handler</param>
        /// <param name="snapshot">Whether or not a snapshot of the last book state should be send after subscribing</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToAggregatedOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default);


        /// <summary>
        /// Subscribe to the full order book with individual orders. Requires authentication.
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/level3" /></para>
        /// </summary>
        /// <param name="symbols">Symbols to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, AClass?, bool?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="depth">Depth of the initial order book snapshot. 10, 100 or 1000</param>
        /// <param name="handler">Data handler</param>
        /// <param name="snapshot">Whether or not a snapshot of the last book state should be send after subscribing</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToIndividualOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<KrakenIndividualBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to the full order book with individual orders. Requires authentication.
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/level3" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to subscribe to, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, AClass?, bool?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName">WebsocketName</see> property</param>
        /// <param name="depth">Depth of the initial order book snapshot. 10, 100 or 1000</param>
        /// <param name="handler">Data handler</param>
        /// <param name="snapshot">Whether or not a snapshot of the last book state should be send after subscribing</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToIndividualOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenIndividualBookUpdate>> handler, bool? snapshot = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to instrument (asset and symbol) updates
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/instrument" /></para>
        /// </summary>
        /// <param name="handler">Data handler</param>
        /// <param name="snapshot">Whether or not a snapshot of the current instruments state should be send after subscribing</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToInstrumentUpdatesAsync(Action<DataEvent<KrakenInstrumentUpdate>> handler, bool? snapshot = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user balances updates
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/balances" /></para>
        /// </summary>
        /// <param name="snapshotHandler">Handler for the initial snapshot data send after subscribing</param>
        /// <param name="updateHandler">Handler for update data when changes occur</param>
        /// <param name="snapshot">Whether or not a snapshot of the current balances should be send after subscribing</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<KrakenBalanceSnapshot[]>>? snapshotHandler, Action<DataEvent<KrakenBalanceUpdate[]>> updateHandler, bool? snapshot = null, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user order updates
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/executions" /></para>
        /// </summary>
        /// <param name="updateHandler">Data handler. Depending on the event not all fields on the update event are filled</param>
        /// <param name="snapshotOrder">Whether or not a snapshot of the current open orders should be send after subscribing</param>
        /// <param name="snapshotTrades">Whether or not a snapshot of the last 50 user trades should be send after subscribing</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(
            Action<DataEvent<KrakenOrderUpdate[]>> updateHandler,
            bool? snapshotOrder = null,
            bool? snapshotTrades = null,
            CancellationToken ct = default);

        /// <summary>
        /// Place a new order
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/add_order" /></para>
        /// </summary>
        /// <param name="symbol">The symbol the order is on, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, AClass?, bool?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName"/></param>
        /// <param name="side">The side of the order</param>
        /// <param name="type">The type of the order</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="clientOrderId">Client order id</param>
        /// <param name="userReference">User reference id</param>
        /// <param name="limitPrice">Order limit price</param>
        /// <param name="limitPriceType">Limit price type</param>
        /// <param name="timeInForce">Time in force</param>
        /// <param name="reduceOnly">Reduce only order</param>
        /// <param name="margin">Funds the order on margin using the maximum leverage for the pair. Note, absolute max leverage is 5.</param>
        /// <param name="postOnly">Post only order flag</param>
        /// <param name="startTime">Scheduled start time</param>
        /// <param name="expireTime">Expiration time</param>
        /// <param name="deadline">Deadline time, the engine will prevent this order from matching after this time, it provides protection against latency on time sensitive orders.</param>
        /// <param name="icebergQuantity">Display size for iceberg orders</param>
        /// <param name="feePreference">Fee preference</param>
        /// <param name="noMarketPriceProtection">Execute without market price protection</param>
        /// <param name="selfTradePreventionType">Self trade prevention type</param>
        /// <param name="quoteQuantity">Quantity in quote asset (buy market orders only)</param>
        /// <param name="validateOnly">Only validate inputs, don't actually place the order</param>
        /// <param name="triggerPriceReference">Order trigger price reference</param>
        /// <param name="triggerPrice">Order trigger price</param>
        /// <param name="triggerPriceType">Order trigger price type</param>
        /// <param name="conditionalOrderType">Conditional order type</param>
        /// <param name="conditionalLimitPrice">Conditional order limit price</param>
        /// <param name="conditionalLimitPriceType">Conditional order limit price type</param>
        /// <param name="conditionalTriggerPrice">Conditional order trigger price</param>
        /// <param name="conditionalTriggerPriceType">Conditional order trigger price type</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<KrakenOrderResult>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal quantity,
            string? clientOrderId = null,
            uint? userReference = null,
            decimal? limitPrice = null,
            PriceType? limitPriceType = null,
            TimeInForce? timeInForce = null,
            bool? reduceOnly = null,
            bool? margin = null,
            bool? postOnly = null,
            DateTime? startTime = null,
            DateTime? expireTime = null,
            DateTime? deadline = null,
            decimal? icebergQuantity = null,
            FeePreference? feePreference = null,
            bool? noMarketPriceProtection = null,
            SelfTradePreventionType? selfTradePreventionType = null,
            decimal? quoteQuantity = null,
            bool? validateOnly = null,

            Trigger? triggerPriceReference = null,
            decimal? triggerPrice = null,
            PriceType? triggerPriceType = null,

            OrderType? conditionalOrderType = null,
            decimal? conditionalLimitPrice = null,
            PriceType? conditionalLimitPriceType = null,
            decimal? conditionalTriggerPrice = null,
            PriceType? conditionalTriggerPriceType = null,

            CancellationToken ct = default);

        /// <summary>
        /// Edit an existing order
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/amend_order" /></para>
        /// </summary>
        /// <param name="orderId">Order id, either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">Client order id, either this or orderId should be provided</param>
        /// <param name="limitPrice">New limit price</param>
        /// <param name="limitPriceType">New limit price type</param>
        /// <param name="quantity">New quantity</param>
        /// <param name="icebergQuantity">New iceberg quantity</param>
        /// <param name="postOnly">New post only flag</param>
        /// <param name="triggerPrice">New trigger price</param>
        /// <param name="triggerPriceType">New trigger price type</param>
        /// <param name="deadline">Deadline</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<KrakenSocketAmendOrderResult>> EditOrderAsync(
            string? orderId = null,
            string? clientOrderId = null,
            decimal? limitPrice = null,
            PriceType? limitPriceType = null,
            decimal? quantity = null,
            decimal? icebergQuantity = null,
            bool? postOnly = null,
            decimal? triggerPrice = null,
            PriceType? triggerPriceType = null,
            DateTime? deadline = null,
            CancellationToken ct = default);

        /// <summary>
        /// Replace an existing order
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/edit_order" /></para>
        /// </summary>
        /// <param name="symbol">The symbol the order is on, for example `ETH/USDT`. Websocket name of a symbol can be obtained via <see cref="IKrakenRestClientSpotApiExchangeData.GetSymbolsAsync(IEnumerable{string}?, string?, AClass?, bool?, CancellationToken)">restClient.SpotApi.ExchangeData.GetSymbolsAsync</see> using the <see cref="KrakenSymbol.WebsocketName"/></param>
        /// <param name="orderId">Order id, either this or clientOrderId should be provided</param>
        /// <param name="clientOrderId">Client order id, either this or orderId should be provided</param>
        /// <param name="quantity">The quantity of the order</param>
        /// <param name="userReference">User reference id</param>
        /// <param name="limitPrice">Order limit price</param>
        /// <param name="reduceOnly">Reduce only order</param>
        /// <param name="postOnly">Post only order flag</param>
        /// <param name="deadline">Deadline time, the engine will prevent this order from matching after this time, it provides protection against latency on time sensitive orders.</param>
        /// <param name="icebergQuantity">Display size for iceberg orders</param>
        /// <param name="feePreference">Fee preference</param>
        /// <param name="noMarketPriceProtection">Execute without market price protection</param>
        /// <param name="validateOnly">Only validate inputs, don't actually place the order</param>
        /// <param name="triggerPriceReference">Order trigger price reference</param>
        /// <param name="triggerPrice">Order trigger price</param>
        /// <param name="triggerPriceType">Order trigger price type</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<KrakenSocketReplaceOrderResult>> ReplaceOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            decimal? quantity = null,
            uint? userReference = null,
            decimal? limitPrice = null,
            bool? reduceOnly = null,
            bool? postOnly = null,
            DateTime? deadline = null,
            decimal? icebergQuantity = null,
            FeePreference? feePreference = null,
            bool? noMarketPriceProtection = null,
            bool? validateOnly = null,

            Trigger? triggerPriceReference = null,
            decimal? triggerPrice = null,
            PriceType? triggerPriceType = null,
            CancellationToken ct = default);

        /// <summary>
        /// Place multiple orders in a single request
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/batch_add" /></para>
        /// </summary>
        /// <param name="symbol">Symbol to place the orders on</param>
        /// <param name="orders">Order info</param>
        /// <param name="deadline">Deadline time, the engine will prevent this order from matching after this time, it provides protection against latency on time sensitive orders.</param>
        /// <param name="validateOnly">Only validate inputs, don't actually place the order</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<CallResult<KrakenOrderResult>[]>> PlaceMultipleOrdersAsync(
            string symbol,
            IEnumerable<KrakenSocketOrderRequest> orders,
            DateTime? deadline = null,
            bool? validateOnly = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel an order
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/cancel_order" /></para>
        /// </summary>
        /// <param name="orderId">Id of the order to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<KrakenOrderResult>> CancelOrderAsync(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Cancel multiple orders
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/cancel_order" /></para>
        /// </summary>
        /// <param name="orderIds">Id of the orders to cancel</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<KrakenOrderResult>> CancelOrdersAsync(IEnumerable<string> orderIds, CancellationToken ct = default);

        /// <summary>
        /// Cancel all open orders
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/cancel_all" /></para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<KrakenStreamCancelAllResult>> CancelAllOrdersAsync(CancellationToken ct = default);

        /// <summary>
        /// Cancel all open orders after the timeout
        /// <para><a href="https://docs.kraken.com/api/docs/websocket-v2/cancel_after" /></para>
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<CallResult<KrakenCancelAfterResult>> CancelAllOrdersAfterAsync(TimeSpan timeout, CancellationToken ct = default);
    }
}
