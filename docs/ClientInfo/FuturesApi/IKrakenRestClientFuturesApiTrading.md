---
title: IKrakenRestClientFuturesApiTrading
has_children: false
parent: IKrakenRestClientFuturesApi
grand_parent: Rest API documentation
---
*[generated documentation]*  
`KrakenRestClient > FuturesApi > Trading`  
*Kraken futures trading endpoints, placing and mananging orders.*
  

***

## CancelAllOrderAfterAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-cancel-all-orders](https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-cancel-all-orders)  
<p>

*This endpoint provides a Dead Man's Switch mechanism to protect the user from network malfunctions. The user can send a request with a timeout in seconds which will trigger a countdown timer that will cancel all user orders when timeout expires. The user has to keep sending request to push back the timeout expiration or they can deactivate the mechanism by specifying a timeout of zero (0).*  
*The recommended mechanism usage is making a call every 15 to 20 seconds and provide a timeout of 60 seconds.This allows the user to keep the orders in place on a brief network failure, while keeping them safe in case of a network breakdown.*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.CancelAllOrderAfterAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenFuturesCancelAfter>> CancelAllOrderAfterAsync(TimeSpan cancelAfter, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|cancelAfter|Cancel after this time. TimeSpan.Zero to cancel a previous cancel after.|
|_[Optional]_ ct|Cancellation token|

</p>

***

## CancelAllOrdersAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-cancel-all-orders](https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-cancel-all-orders)  
<p>

*Cancel all orders*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.CancelAllOrdersAsync();  
```  

```csharp  
Task<WebCallResult<KrakenFuturesCancelledOrders>> CancelAllOrdersAsync(string? symbol = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ symbol|Only cancel on this symbol|
|_[Optional]_ ct|Cancellation token|

</p>

***

## CancelOrderAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-cancel-order](https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-cancel-order)  
<p>

*Cancel an order*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.CancelOrderAsync();  
```  

```csharp  
Task<WebCallResult<KrakenFuturesOrderResult>> CancelOrderAsync(string? orderId = default, string? clientOrderId = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ orderId|Filter by order id|
|_[Optional]_ clientOrderId|Filter by client order id|
|_[Optional]_ ct|Cancellation token|

</p>

***

## EditOrderAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-edit-order](https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-edit-order)  
<p>

*Edit an existing order*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.EditOrderAsync();  
```  

```csharp  
Task<WebCallResult<KrakenFuturesOrderResult>> EditOrderAsync(string? orderId = default, string? clientOrderId = default, int? quantity = default, decimal? price = default, decimal? stopPrice = default, TrailingStopDeviationUnit? trailingStopDeviationUnit = default, decimal? trailingStopMaxDeviation = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ orderId|Order id of order to edit|
|_[Optional]_ clientOrderId|Client order id of order to edit|
|_[Optional]_ quantity|New quantity|
|_[Optional]_ price|New price|
|_[Optional]_ stopPrice|New stop price|
|_[Optional]_ trailingStopDeviationUnit|New trailing stop deviation unit|
|_[Optional]_ trailingStopMaxDeviation|New trailing stop max deviation|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetExecutionEventsAsync  

[https://docs.futures.kraken.com/#http-api-history-account-history-get-execution-events](https://docs.futures.kraken.com/#http-api-history-account-history-get-execution-events)  
<p>

*Get execution events*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.GetExecutionEventsAsync();  
```  

```csharp  
Task<WebCallResult<KrakenFuturesUserExecutionEvents>> GetExecutionEventsAsync(DateTime? startTime = default, DateTime? endTime = default, string? sort = default, string? tradeable = default, string? continuationToken = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ startTime|Filter by start time|
|_[Optional]_ endTime|Filter by end time|
|_[Optional]_ sort|Sort asc or desc|
|_[Optional]_ tradeable|If present events of other tradeables are filtered out.	|
|_[Optional]_ continuationToken|Opaque token from the Next-Continuation-Token header used to continue listing events. The sort parameter must be the same as in the previous request to continue listing in the same direction.|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetLeverageAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-multi-collateral-get-the-leverage-setting-for-a-market](https://docs.futures.kraken.com/#http-api-trading-v3-api-multi-collateral-get-the-leverage-setting-for-a-market)  
<p>

*Get the leverage settings*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.GetLeverageAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenFuturesLeverage>>> GetLeverageAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOpenOrdersAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-get-open-orders](https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-get-open-orders)  
<p>

*Get open orders*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.GetOpenOrdersAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenFuturesOpenOrder>>> GetOpenOrdersAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOpenPositionsAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-account-information-get-open-positions](https://docs.futures.kraken.com/#http-api-trading-v3-api-account-information-get-open-positions)  
<p>

*Get open positions*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.GetOpenPositionsAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenFuturesPosition>>> GetOpenPositionsAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOrdersAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-get-the-current-status-for-specific-orders](https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-get-the-current-status-for-specific-orders)  
<p>

*Get orders by ids*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.GetOrdersAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenFuturesOrderStatus>>> GetOrdersAsync(IEnumerable<string>? orderIds = default, IEnumerable<string>? clientOrderIds = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ orderIds|Order ids list|
|_[Optional]_ clientOrderIds|Client order ids list|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetSelfTradeStrategyAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-trading-settings-get-current-self-trade-strategy](https://docs.futures.kraken.com/#http-api-trading-v3-api-trading-settings-get-current-self-trade-strategy)  
<p>

*Get current self trade strategy*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.GetSelfTradeStrategyAsync();  
```  

```csharp  
Task<WebCallResult<SelfTradeStrategy>> GetSelfTradeStrategyAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetUserTradesAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-historical-data-get-your-fills](https://docs.futures.kraken.com/#http-api-trading-v3-api-historical-data-get-your-fills)  
<p>

*Get list of user trades*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.GetUserTradesAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenFuturesUserTrade>>> GetUserTradesAsync(DateTime? startTime = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ startTime|Filter by start time|
|_[Optional]_ ct|Cancellation token|

</p>

***

## PlaceOrderAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-send-order](https://docs.futures.kraken.com/#http-api-trading-v3-api-order-management-send-order)  
<p>

*Place a new order*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.PlaceOrderAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenFuturesOrderResult>> PlaceOrderAsync(string symbol, OrderSide side, FuturesOrderType type, int quantity, decimal? price = default, decimal? stopPrice = default, bool? reduceOnly = default, TrailingStopDeviationUnit? trailingStopDeviationUnit = default, decimal? trailingStopMaxDeviation = default, TriggerSignal? triggerSignal = default, string? clientOrderId = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol|
|side|Order side|
|type|Order type|
|quantity|Order quantity|
|_[Optional]_ price|Limit price|
|_[Optional]_ stopPrice|Stop price|
|_[Optional]_ reduceOnly|Is reduce only|
|_[Optional]_ trailingStopDeviationUnit|Trailing stop deviation unit|
|_[Optional]_ trailingStopMaxDeviation|Trailing stop max deviation|
|_[Optional]_ triggerSignal|Trigger signal|
|_[Optional]_ clientOrderId|Client order id|
|_[Optional]_ ct|Cancellation token|

</p>

***

## SetLeverageAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-multi-collateral-set-the-leverage-setting-for-a-market](https://docs.futures.kraken.com/#http-api-trading-v3-api-multi-collateral-set-the-leverage-setting-for-a-market)  
<p>

*Set max leverage for a symbol*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.SetLeverageAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult> SetLeverageAsync(string symbol, decimal maxLeverage, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol|
|maxLeverage|Max leverage|
|_[Optional]_ ct|Cancellation token|

</p>

***

## SetSelfTradeStrategyAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-trading-settings-update-self-trade-strategy](https://docs.futures.kraken.com/#http-api-trading-v3-api-trading-settings-update-self-trade-strategy)  
<p>

*Set self trading strategy*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Trading.SetSelfTradeStrategyAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<SelfTradeStrategy>> SetSelfTradeStrategyAsync(SelfTradeStrategy strategy, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|strategy|The strategy|
|_[Optional]_ ct|Cancellation token|

</p>
