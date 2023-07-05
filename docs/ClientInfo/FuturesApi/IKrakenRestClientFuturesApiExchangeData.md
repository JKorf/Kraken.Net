---
title: IKrakenRestClientFuturesApiExchangeData
has_children: false
parent: IKrakenRestClientFuturesApi
grand_parent: Rest API documentation
---
*[generated documentation]*  
`KrakenRestClient > FuturesApi > ExchangeData`  
*Kraken futures exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.*
  

***

## GetFeeSchedulesAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-fee-schedules-get-fee-schedules](https://docs.futures.kraken.com/#http-api-trading-v3-api-fee-schedules-get-fee-schedules)  
<p>

*Get fee schedules*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.ExchangeData.GetFeeSchedulesAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenFeeSchedule>>> GetFeeSchedulesAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetHistoricalFundingRatesAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-historical-funding-rates-historical-funding-rates](https://docs.futures.kraken.com/#http-api-trading-v3-api-historical-funding-rates-historical-funding-rates)  
<p>

*Get historical funding rates*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.ExchangeData.GetHistoricalFundingRatesAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenFundingRate>>> GetHistoricalFundingRatesAsync(string symbol, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetKlinesAsync  

[https://docs.futures.kraken.com/#http-api-charts-candles-market-candles](https://docs.futures.kraken.com/#http-api-charts-candles-market-candles)  
<p>

*Get klines/candle data*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.ExchangeData.GetKlinesAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenFuturesKlines>> GetKlinesAsync(TickType tickType, string symbol, FuturesKlineInterval interval, DateTime? startTime = default, DateTime? endTime = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|tickType|Type of price tick|
|symbol|The symbol|
|interval|Interval of the klines|
|_[Optional]_ startTime|Start time|
|_[Optional]_ endTime|End time|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOrderBookAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-market-data-get-orderbook](https://docs.futures.kraken.com/#http-api-trading-v3-api-market-data-get-orderbook)  
<p>

*Get the orderbook*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.ExchangeData.GetOrderBookAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenFuturesOrderBook>> GetOrderBookAsync(string symbol, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetPlatformNotificationsAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-general-get-notifications](https://docs.futures.kraken.com/#http-api-trading-v3-api-general-get-notifications)  
<p>

*Get platform notifications*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.ExchangeData.GetPlatformNotificationsAsync();  
```  

```csharp  
Task<WebCallResult<KrakenFuturesPlatfromNotificationResult>> GetPlatformNotificationsAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetSymbolsAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-instrument-details-get-instruments](https://docs.futures.kraken.com/#http-api-trading-v3-api-instrument-details-get-instruments)  
<p>

*Get a list of symbols*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.ExchangeData.GetSymbolsAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenFuturesSymbol>>> GetSymbolsAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetSymbolStatusAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-instrument-details-get-instrument-status-list](https://docs.futures.kraken.com/#http-api-trading-v3-api-instrument-details-get-instrument-status-list)  
<p>

*Get a list of symbols statuses*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.ExchangeData.GetSymbolStatusAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenFuturesSymbolStatus>>> GetSymbolStatusAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetTickersAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-market-data-get-tickers](https://docs.futures.kraken.com/#http-api-trading-v3-api-market-data-get-tickers)  
<p>

*Get tickers*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.ExchangeData.GetTickersAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenFuturesTicker>>> GetTickersAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetTradesAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-market-data-get-trade-history](https://docs.futures.kraken.com/#http-api-trading-v3-api-market-data-get-trade-history)  
<p>

*Get list of recent trades*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.ExchangeData.GetTradesAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenFuturesTrade>>> GetTradesAsync(string symbol, DateTime? startTime = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol|
|_[Optional]_ startTime|Filter by start time|
|_[Optional]_ ct|Cancellation token|

</p>
