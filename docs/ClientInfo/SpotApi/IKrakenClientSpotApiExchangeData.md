---
title: IKrakenClientSpotApiExchangeData
has_children: false
parent: IKrakenClientSpotApi
grand_parent: Rest API documentation
---
*[generated documentation]*  
`KrakenClient > SpotApi > ExchangeData`  
*Kraken exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.*
  

***

## GetAssetsAsync  

[https://docs.kraken.com/rest/#operation/getAssetInfo](https://docs.kraken.com/rest/#operation/getAssetInfo)  
<p>

*Get a list of assets and info about them*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.ExchangeData.GetAssetsAsync();  
```  

```csharp  
Task<WebCallResult<Dictionary<string, KrakenAssetInfo>>> GetAssetsAsync(IEnumerable<string>? assets = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ assets|Filter list for specific assets|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetKlinesAsync  

[https://docs.kraken.com/rest/#operation/getOHLCData](https://docs.kraken.com/rest/#operation/getOHLCData)  
<p>

*Gets kline data for a symbol*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.ExchangeData.GetKlinesAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenKlinesResult>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? since = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|The symbol to get data for|
|interval|The interval of the klines|
|_[Optional]_ since|Return klines since a specific time|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOrderBookAsync  

[https://docs.kraken.com/rest/#operation/getOrderBook](https://docs.kraken.com/rest/#operation/getOrderBook)  
<p>

*Get the order book for a symbol*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.ExchangeData.GetOrderBookAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenOrderBook>> GetOrderBookAsync(string symbol, int? limit = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|Symbol to get the book for|
|_[Optional]_ limit|Limit to book to the best x bids/asks|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetRecentSpreadAsync  

[https://docs.kraken.com/rest/#operation/getRecentSpreads](https://docs.kraken.com/rest/#operation/getRecentSpreads)  
<p>

*Get spread data for a symbol*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.ExchangeData.GetRecentSpreadAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenSpreadsResult>> GetRecentSpreadAsync(string symbol, DateTime? since = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|Symbol to get spread data for|
|_[Optional]_ since|Return spread data since a specific time|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetServerTimeAsync  

[https://docs.kraken.com/rest/#operation/getServerTime](https://docs.kraken.com/rest/#operation/getServerTime)  
<p>

*Get the server time*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.ExchangeData.GetServerTimeAsync();  
```  

```csharp  
Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetSymbolsAsync  

[https://docs.kraken.com/rest/#operation/getTradableAssetPairs](https://docs.kraken.com/rest/#operation/getTradableAssetPairs)  
<p>

*Get a list of symbols and info about them*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.ExchangeData.GetSymbolsAsync();  
```  

```csharp  
Task<WebCallResult<Dictionary<string, KrakenSymbol>>> GetSymbolsAsync(IEnumerable<string>? symbols = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ symbols|Filter list for specific symbols|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetSystemStatusAsync  

[https://docs.kraken.com/rest/#operation/getSystemStatus](https://docs.kraken.com/rest/#operation/getSystemStatus)  
<p>

*Get the system status*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.ExchangeData.GetSystemStatusAsync();  
```  

```csharp  
Task<WebCallResult<KrakenSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetTickerAsync  

[https://docs.kraken.com/rest/#operation/getTickerInformation](https://docs.kraken.com/rest/#operation/getTickerInformation)  
<p>

*Get tickers for symbol*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.ExchangeData.GetTickerAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickerAsync(string symbol, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|Symbol to get tickers for|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetTickersAsync  

[https://docs.kraken.com/rest/#operation/getTickerInformation](https://docs.kraken.com/rest/#operation/getTickerInformation)  
<p>

*Get tickers for symbols*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.ExchangeData.GetTickersAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<Dictionary<string, KrakenRestTick>>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbols|Symbols to get tickers for|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetTradeHistoryAsync  

[https://docs.kraken.com/rest/#operation/getRecentTrades](https://docs.kraken.com/rest/#operation/getRecentTrades)  
<p>

*Get a list of recent trades for a symbol*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.ExchangeData.GetTradeHistoryAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenTradesResult>> GetTradeHistoryAsync(string symbol, DateTime? since = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|Symbol to get trades for|
|_[Optional]_ since|Return trades since a specific time|
|_[Optional]_ ct|Cancellation token|

</p>
