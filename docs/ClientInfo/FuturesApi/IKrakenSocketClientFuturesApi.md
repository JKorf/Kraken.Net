---
title: IKrakenSocketClientFuturesApi
has_children: true
parent: Socket API documentation
---
*[generated documentation]*  
`KrakenSocketClient > FuturesApi`  
*Futures API*
  

***

## SubscribeToAccountLogUpdatesAsync  

[https://docs.futures.kraken.com/#websocket-api-private-feeds-account-log](https://docs.futures.kraken.com/#websocket-api-private-feeds-account-log)  
<p>

*Subscribe to account updates*  

```csharp  
var client = new KrakenSocketClient();  
var result = await client.FuturesApi.SubscribeToAccountLogUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToAccountLogUpdatesAsync(Action<DataEvent<KrakenFuturesAccountLogsSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesAccountLogsUpdate>> updateHandler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|snapshotHandler|Handler for the initial snapshot data received when (re)connecting the stream|
|updateHandler|Update handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToBalanceUpdatesAsync  

[https://docs.futures.kraken.com/#websocket-api-private-feeds-balances](https://docs.futures.kraken.com/#websocket-api-private-feeds-balances)  
<p>

*Subscribe to balance updates*  

```csharp  
var client = new KrakenSocketClient();  
var result = await client.FuturesApi.SubscribeToBalanceUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<KrakenFuturesBalancesUpdate>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|handler|Update handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToHeartbeatUpdatesAsync  

[https://docs.futures.kraken.com/#websocket-api-public-feeds-heartbeat](https://docs.futures.kraken.com/#websocket-api-public-feeds-heartbeat)  
<p>

*Subscribe to heartbeat updates*  

```csharp  
var client = new KrakenSocketClient();  
var result = await client.FuturesApi.SubscribeToHeartbeatUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToHeartbeatUpdatesAsync(Action<DataEvent<KrakenFuturesHeartbeatUpdate>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|handler|Update handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToMiniTickerUpdatesAsync  

[https://docs.futures.kraken.com/#websocket-api-public-feeds-ticker-lite](https://docs.futures.kraken.com/#websocket-api-public-feeds-ticker-lite)  
<p>

*Subscribe to mini ticker updates*  

```csharp  
var client = new KrakenSocketClient();  
var result = await client.FuturesApi.SubscribeToMiniTickerUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesMiniTickerUpdate>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|The symbols to subscribe|
|handler|Update handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToNotificationUpdatesAsync  

[https://docs.futures.kraken.com/#websocket-api-private-feeds-notifications](https://docs.futures.kraken.com/#websocket-api-private-feeds-notifications)  
<p>

*Subscribe to notification updates*  

```csharp  
var client = new KrakenSocketClient();  
var result = await client.FuturesApi.SubscribeToNotificationUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToNotificationUpdatesAsync(Action<DataEvent<KrakenFuturesNotificationUpdate>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|handler|Update handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToOpenOrdersUpdatesAsync  

[https://docs.futures.kraken.com/#websocket-api-private-feeds-open-orders](https://docs.futures.kraken.com/#websocket-api-private-feeds-open-orders)  
[https://docs.futures.kraken.com/#websocket-api-private-feeds-open-orders-verbose](https://docs.futures.kraken.com/#websocket-api-private-feeds-open-orders-verbose)  
<p>

*Subscribe to open order updates*  

```csharp  
var client = new KrakenSocketClient();  
var result = await client.FuturesApi.SubscribeToOpenOrdersUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToOpenOrdersUpdatesAsync(bool verbose, Action<DataEvent<KrakenFuturesOpenOrdersSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesOpenOrdersUpdate>> updateHandler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|verbose|Whether to connect to the verbose stream or not. The verbose feed adds extra information about all the post-only orders that failed to cross the book.|
|snapshotHandler|Handler for the initial snapshot data received when (re)connecting the stream|
|updateHandler|Update handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToOpenPositionUpdatesAsync  

[https://docs.futures.kraken.com/#websocket-api-private-feeds-open-positions](https://docs.futures.kraken.com/#websocket-api-private-feeds-open-positions)  
<p>

*Subscribe to open position updates*  

```csharp  
var client = new KrakenSocketClient();  
var result = await client.FuturesApi.SubscribeToOpenPositionUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToOpenPositionUpdatesAsync(Action<DataEvent<KrakenFuturesOpenPositionUpdate>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|handler|Update handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToOrderBookUpdatesAsync  

[https://docs.futures.kraken.com/#websocket-api-public-feeds-book](https://docs.futures.kraken.com/#websocket-api-public-feeds-book)  
<p>

*Subscribe to order book updates*  

```csharp  
var client = new KrakenSocketClient();  
var result = await client.FuturesApi.SubscribeToOrderBookUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesBookSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesBookUpdate>> updateHandler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|The symbols to subscribe|
|snapshotHandler|Handler for the initial snapshot data received when (re)connecting the stream|
|updateHandler|Update handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToTickerUpdatesAsync  

[https://docs.futures.kraken.com/#websocket-api-public-feeds-ticker](https://docs.futures.kraken.com/#websocket-api-public-feeds-ticker)  
<p>

*Subscribe to ticker updates*  

```csharp  
var client = new KrakenSocketClient();  
var result = await client.FuturesApi.SubscribeToTickerUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesTickerUpdate>> handler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|The symbols to subscribe|
|handler|Update handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToTradeUpdatesAsync  

[https://docs.futures.kraken.com/#websocket-api-public-feeds-trade](https://docs.futures.kraken.com/#websocket-api-public-feeds-trade)  
<p>

*Subscribe to public trade updates*  

```csharp  
var client = new KrakenSocketClient();  
var result = await client.FuturesApi.SubscribeToTradeUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<DataEvent<KrakenFuturesTradesSnapshotUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesTradeUpdate>> updateHandler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|The symbols to subscribe|
|snapshotHandler|Handler for the initial snapshot data received when (re)connecting the stream|
|updateHandler|Update handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>

***

## SubscribeToUserTradeUpdatesAsync  

[https://docs.futures.kraken.com/#websocket-api-private-feeds-fills](https://docs.futures.kraken.com/#websocket-api-private-feeds-fills)  
<p>

*Subscribe to user trades updates*  

```csharp  
var client = new KrakenSocketClient();  
var result = await client.FuturesApi.SubscribeToUserTradeUpdatesAsync(/* parameters */);  
```  

```csharp  
Task<CallResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(Action<DataEvent<KrakenFuturesUserTradesUpdate>> snapshotHandler, Action<DataEvent<KrakenFuturesUserTradesUpdate>> updateHandler, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|snapshotHandler|Handler for the initial snapshot data received when (re)connecting the stream|
|updateHandler|Update handler|
|_[Optional]_ ct|Cancellation token for closing this subscription|

</p>
