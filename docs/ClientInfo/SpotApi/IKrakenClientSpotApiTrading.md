---
title: IKrakenClientSpotApiTrading
has_children: false
parent: IKrakenClientSpotApi
grand_parent: Rest API documentation
---
*[generated documentation]*  
`KrakenClient > SpotApi > Trading`  
*Kraken trading endpoints, placing and mananging orders.*
  

***

## CancelAllOrdersAsync  

[https://docs.kraken.com/rest/#operation/cancelAllOrders](https://docs.kraken.com/rest/#operation/cancelAllOrders)  
<p>

*Cancel all orders*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.Trading.CancelAllOrdersAsync();  
```  

```csharp  
Task<WebCallResult<KrakenCancelResult>> CancelAllOrdersAsync(string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## CancelOrderAsync  

[https://docs.kraken.com/rest/#operation/cancelOrder](https://docs.kraken.com/rest/#operation/cancelOrder)  
<p>

*Cancel an order*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.Trading.CancelOrderAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenCancelResult>> CancelOrderAsync(string orderId, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|orderId|The id of the order to cancel|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetClosedOrdersAsync  

[https://docs.kraken.com/rest/#operation/getClosedOrders](https://docs.kraken.com/rest/#operation/getClosedOrders)  
<p>

*Get a list of closed orders*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.Trading.GetClosedOrdersAsync();  
```  

```csharp  
Task<WebCallResult<KrakenClosedOrdersPage>> GetClosedOrdersAsync(uint? clientOrderId = default, DateTime? startTime = default, DateTime? endTime = default, int? resultOffset = default, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ clientOrderId|Filter by client order id|
|_[Optional]_ startTime|Return data after this time|
|_[Optional]_ endTime|Return data before this time|
|_[Optional]_ resultOffset|Offset the results by|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOpenOrdersAsync  

[https://docs.kraken.com/rest/#operation/getOpenOrders](https://docs.kraken.com/rest/#operation/getOpenOrders)  
<p>

*Get a list of open orders*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.Trading.GetOpenOrdersAsync();  
```  

```csharp  
Task<WebCallResult<OpenOrdersPage>> GetOpenOrdersAsync(uint? clientOrderId = default, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ clientOrderId|Filter by client order id|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOrderAsync  

[https://docs.kraken.com/rest/#operation/getOrdersInfo](https://docs.kraken.com/rest/#operation/getOrdersInfo)  
<p>

*Get info on specific order*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.Trading.GetOrderAsync();  
```  

```csharp  
Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrderAsync(string? orderId = default, uint? clientOrderId = default, bool? consolidateTaker = default, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ orderId|Get order by its order id|
|_[Optional]_ clientOrderId|Get orders by clientOrderId|
|_[Optional]_ consolidateTaker|Whether or not to consolidate trades by individual taker trades|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOrdersAsync  

[https://docs.kraken.com/rest/#operation/getOrdersInfo](https://docs.kraken.com/rest/#operation/getOrdersInfo)  
<p>

*Get info on specific orders*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.Trading.GetOrdersAsync();  
```  

```csharp  
Task<WebCallResult<Dictionary<string, KrakenOrder>>> GetOrdersAsync(IEnumerable<string>? orderIds = default, uint? clientOrderId = default, bool? consolidateTaker = default, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ orderIds|Get orders by their order ids|
|_[Optional]_ clientOrderId|Get orders by clientOrderId|
|_[Optional]_ consolidateTaker|Whether or not to consolidate trades by individual taker trades|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetUserTradeDetailsAsync  

[https://docs.kraken.com/rest/#operation/getTradesInfo](https://docs.kraken.com/rest/#operation/getTradesInfo)  
<p>

*Get info on specific trades*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.Trading.GetUserTradeDetailsAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(string tradeId, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|tradeId|The trade to get info on|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetUserTradeDetailsAsync  

[https://docs.kraken.com/rest/#operation/getTradesInfo](https://docs.kraken.com/rest/#operation/getTradesInfo)  
<p>

*Get info on specific trades*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.Trading.GetUserTradeDetailsAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<Dictionary<string, KrakenUserTrade>>> GetUserTradeDetailsAsync(IEnumerable<string> tradeIds, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|tradeIds|The trades to get info on|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetUserTradesAsync  

[https://docs.kraken.com/rest/#operation/getTradeHistory](https://docs.kraken.com/rest/#operation/getTradeHistory)  
<p>

*Get trade history*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.Trading.GetUserTradesAsync();  
```  

```csharp  
Task<WebCallResult<KrakenUserTradesPage>> GetUserTradesAsync(DateTime? startTime = default, DateTime? endTime = default, int? resultOffset = default, bool? consolidateTaker = default, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ startTime|Return data after this time|
|_[Optional]_ endTime|Return data before this time|
|_[Optional]_ resultOffset|Offset the results by|
|_[Optional]_ consolidateTaker|Whether or not to consolidate trades by individual taker trades|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## PlaceOrderAsync  

[https://docs.kraken.com/rest/#operation/addOrder](https://docs.kraken.com/rest/#operation/addOrder)  
<p>

*Place a new order*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.Trading.PlaceOrderAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenPlacedOrder>> PlaceOrderAsync(string symbol, OrderSide side, OrderType type, decimal quantity, decimal? price = default, decimal? secondaryPrice = default, decimal? leverage = default, DateTime? startTime = default, DateTime? expireTime = default, bool? validateOnly = default, uint? clientOrderId = default, IEnumerable<OrderFlags>? orderFlags = default, string? twoFactorPassword = default, TimeInForce? timeInForce = default, bool? reduceOnly = default, decimal? icebergQuanty = default, Trigger? trigger = default, SelfTradePreventionType? selfTradePreventionType = default, OrderType? closeOrderType = default, decimal? closePrice = default, decimal? secondaryClosePrice = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol the order is on|
|side|The side of the order|
|type|The type of the order|
|quantity|The quantity of the order|
|_[Optional]_ price|Price of the order: Limit=limit price, StopLoss=stop loss price, TakeProfit=take profit price, StopLossProfit=stop loss price, StopLossProfitLimit=stop loss price, StopLossLimit=stop loss trigger price, TakeProfitLimit=take profit trigger price, TrailingStop=trailing stop offset, TrailingStopLimit=trailing stop offset, StopLossAndLimit=stop loss price, |
|_[Optional]_ secondaryPrice|Secondary price of an order: StopLossProfit/StopLossProfitLimit=take profit price, StopLossLimit/TakeProfitLimit=triggered limit price, TrailingStopLimit=triggered limit offset, StopLossAndLimit=limit price|
|_[Optional]_ leverage|Desired leverage|
|_[Optional]_ startTime|Scheduled start time|
|_[Optional]_ expireTime|Expiration time|
|_[Optional]_ validateOnly|Only validate inputs, don't actually place the order|
|_[Optional]_ clientOrderId|A client id to reference the order by|
|_[Optional]_ orderFlags|Flags for the order|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ timeInForce|Time-in-force of the order to specify how long it should remain in the order book before being cancelled|
|_[Optional]_ reduceOnly|Reduce only order|
|_[Optional]_ icebergQuanty|Iceberg visible quantity|
|_[Optional]_ trigger|Price signal|
|_[Optional]_ selfTradePreventionType|Self trade prevention type|
|_[Optional]_ closeOrderType|Close order type|
|_[Optional]_ closePrice|Close order price|
|_[Optional]_ secondaryClosePrice|Close order secondary price|
|_[Optional]_ ct|Cancellation token|

</p>
