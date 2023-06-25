---
title: IKrakenSocketClientSpotApi
has_children: true
parent: Rest API documentation
---
*[generated documentation]*  
`KrakenClient > SpotApi`  
*Spot streams*
  

***

## CancelAllOrdersAfterAsync  

[https://docs.kraken.com/websockets/#message-cancelAllOrdersAfter](https://docs.kraken.com/websockets/#message-cancelAllOrdersAfter)  
<p>

*Cancel all open orders after the timeout*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.CancelAllOrdersAfterAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<KrakenStreamCancelAfterResult>> CancelAllOrdersAfterAsync(string websocketToken, TimeSpan timeout);  
```  

|Parameter|Description|
|---|---|
|websocketToken|The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient|
|timeout||

</p>

***

## CancelAllOrdersAsync  

[https://docs.kraken.com/websockets/#message-cancelAll](https://docs.kraken.com/websockets/#message-cancelAll)  
<p>

*Cancel all open orders*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.CancelAllOrdersAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<KrakenStreamCancelAllResult>> CancelAllOrdersAsync(string websocketToken);  
```  

|Parameter|Description|
|---|---|
|websocketToken|The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient|

</p>

***

## CancelOrderAsync  

[https://docs.kraken.com/websockets/#message-cancelOrder](https://docs.kraken.com/websockets/#message-cancelOrder)  
<p>

*Cancel an order*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.CancelOrderAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<bool>> CancelOrderAsync(string websocketToken, string orderId);  
```  

|Parameter|Description|
|---|---|
|websocketToken|The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient|
|orderId|Id of the order to cancel|

</p>

***

## CancelOrdersAsync  

[https://docs.kraken.com/websockets/#message-cancelOrder](https://docs.kraken.com/websockets/#message-cancelOrder)  
<p>

*Cancel multiple orders*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.CancelOrdersAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<bool>> CancelOrdersAsync(string websocketToken, IEnumerable<string> orderIds);  
```  

|Parameter|Description|
|---|---|
|websocketToken|The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient|
|orderIds|Id of the orders to cancel|

</p>

***

## PlaceOrderAsync  

[https://docs.kraken.com/websockets/#message-addOrder](https://docs.kraken.com/websockets/#message-addOrder)  
<p>

*Place a new order*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.PlaceOrderAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<KrakenStreamPlacedOrder>> PlaceOrderAsync(string websocketToken, string symbol, OrderType type, OrderSide side, decimal quantity, uint? clientOrderId = default, decimal? price = default, decimal? secondaryPrice = default, decimal? leverage = default, DateTime? startTime = default, DateTime? expireTime = default, bool? validateOnly = default, OrderType? closeOrderType = default, decimal? closePrice = default, decimal? secondaryClosePrice = default, IEnumerable<OrderFlags>? flags = default, bool? reduceOnly = default);  
```  

|Parameter|Description|
|---|---|
|websocketToken|The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient|
|symbol|The symbol the order is on|
|type|The type of the order|
|side|The side of the order|
|quantity|The quantity of the order|
|_[Optional]_ clientOrderId|A client id to reference the order by|
|_[Optional]_ price|Price of the order: Limit=limit price, StopLoss=stop loss price, TakeProfit=take profit price, StopLossProfit=stop loss price, StopLossProfitLimit=stop loss price, StopLossLimit=stop loss trigger price, TakeProfitLimit=take profit trigger price, TrailingStop=trailing stop offset, TrailingStopLimit=trailing stop offset, StopLossAndLimit=stop loss price, |
|_[Optional]_ secondaryPrice|Secondary price of an order: StopLossProfit/StopLossProfitLimit=take profit price, StopLossLimit/TakeProfitLimit=triggered limit price, TrailingStopLimit=triggered limit offset, StopLossAndLimit=limit price|
|_[Optional]_ leverage|Desired leverage|
|_[Optional]_ startTime|Scheduled start time|
|_[Optional]_ expireTime|Expiration time|
|_[Optional]_ validateOnly|Only validate inputs, don't actually place the order|
|_[Optional]_ closeOrderType|Close order type|
|_[Optional]_ closePrice|Close order price|
|_[Optional]_ secondaryClosePrice|Close order secondary price|
|_[Optional]_ flags|Order flags|
|_[Optional]_ reduceOnly|Reduce only order|

</p>

***

## SubscribeToKlineUpdatesAsync  

[https://docs.kraken.com/websockets/#message-ohlc](https://docs.kraken.com/websockets/#message-ohlc)  
<p>

*Subscribe to kline updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToKlineUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<DataEvent<KrakenStreamKline>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|Symbol to subscribe to|
|interval|Kline interval|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToKlineUpdatesAsync  

[https://docs.kraken.com/websockets/#message-ohlc](https://docs.kraken.com/websockets/#message-ohlc)  
<p>

*Subscribe to kline updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToKlineUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<DataEvent<KrakenStreamKline>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|Symbols to subscribe to|
|interval|Kline interval|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToOrderBookUpdatesAsync  

[https://docs.kraken.com/websockets/#message-book](https://docs.kraken.com/websockets/#message-book)  
<p>

*Subscribe to depth updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToOrderBookUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int depth, Action<DataEvent<KrakenStreamOrderBook>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|Symbols to subscribe to|
|depth|Depth of the initial order book snapshot. 10, 25, 100, 500 or 1000|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToOrderBookUpdatesAsync  

[https://docs.kraken.com/websockets/#message-book](https://docs.kraken.com/websockets/#message-book)  
<p>

*Subscribe to depth updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToOrderBookUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<KrakenStreamOrderBook>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|Symbol to subscribe to|
|depth|Depth of the initial order book snapshot. 10, 25, 100, 500 or 1000|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToOrderUpdatesAsync  

[https://docs.kraken.com/websockets/#message-openOrders](https://docs.kraken.com/websockets/#message-openOrders)  
<p>

*Subscribe to open order updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToOrderUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string socketToken, Action<DataEvent<KrakenStreamOrder>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|socketToken|The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToSpreadUpdatesAsync  

[https://docs.kraken.com/websockets/#message-spread](https://docs.kraken.com/websockets/#message-spread)  
<p>

*Subscribe to spread updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToSpreadUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamSpread>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|Symbol to subscribe to|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToSpreadUpdatesAsync  

[https://docs.kraken.com/websockets/#message-spread](https://docs.kraken.com/websockets/#message-spread)  
<p>

*Subscribe to spread updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToSpreadUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToSpreadUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenStreamSpread>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|Symbols to subscribe to|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToSystemStatusUpdatesAsync  

[https://docs.kraken.com/websockets/#message-systemStatus](https://docs.kraken.com/websockets/#message-systemStatus)  
<p>

*Subscribe to system status updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToSystemStatusUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToSystemStatusUpdatesAsync(Action<DataEvent<KrakenStreamSystemStatus>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToTickerUpdatesAsync  

[https://docs.kraken.com/websockets/#message-ticker](https://docs.kraken.com/websockets/#message-ticker)  
<p>

*Subscribe to ticker updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToTickerUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<DataEvent<KrakenStreamTick>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|Symbol to subscribe to|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToTickerUpdatesAsync  

[https://docs.kraken.com/websockets/#message-ticker](https://docs.kraken.com/websockets/#message-ticker)  
<p>

*Subscribe to ticker updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToTickerUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenStreamTick>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|Symbols to subscribe to|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToTradeUpdatesAsync  

[https://docs.kraken.com/websockets/#message-trade](https://docs.kraken.com/websockets/#message-trade)  
<p>

*Subscribe to trade updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToTradeUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<IEnumerable<KrakenTrade>>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|Symbol to subscribe to|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToTradeUpdatesAsync  

[https://docs.kraken.com/websockets/#message-trade](https://docs.kraken.com/websockets/#message-trade)  
<p>

*Subscribe to trade updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToTradeUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<IEnumerable<KrakenTrade>>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|Symbols to subscribe to|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToUserTradeUpdatesAsync  

[https://docs.kraken.com/websockets/#message-ownTrades](https://docs.kraken.com/websockets/#message-ownTrades)  
<p>

*Subscribe to own trade updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToUserTradeUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(string socketToken, Action<DataEvent<KrakenStreamUserTrade>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|socketToken|The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToUserTradeUpdatesAsync  

[https://docs.kraken.com/websockets/#message-ownTrades](https://docs.kraken.com/websockets/#message-ownTrades)  
<p>

*Subscribe to own trade updates*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SubscribeToUserTradeUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(string socketToken, bool snapshot, Action<DataEvent<KrakenStreamUserTrade>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|socketToken|The socket token as retrieved by the GetWebsocketTokenAsync method in the KrakenClient|
|snapshot|Whether or not to receive a snapshot of the data upon subscribing|
|handler|Data handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>
