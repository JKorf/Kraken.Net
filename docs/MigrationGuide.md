---
title: Migrate V2 to V3
nav_order: 4
---

## Migrate from version V2.x.x to V3.x.x

There are a decent amount of breaking changes when moving from version 2.x.x to version 3.x.x. Although the interface has changed, the available endpoints/information have not, so there should be no need to completely rewrite your program.
Most endpoints are now available under a slightly different name or path, and most data models have remained the same, barring a few renames.
In this document most changes will be described. If you have any other questions or issues when updating, feel free to open an issue.

Changes related to `IExchangeClient`, options and client structure are also (partially) covered in the [CryptoExchange.Net Migration Guide](https://jkorf.github.io/CryptoExchange.Net/Migration%20Guide.html)

### Namespaces
There are a few namespace changes:  
|Type|Old|New|
|----|---|---|
|Enums|`Kraken.Net.Objects`|`Kraken.Net.Enums`  |
|Clients|`Kraken.Net`|`Kraken.Net.Clients`  |
|Client interfaces|`Kraken.Net.Interfaces`|`Kraken.Net.Interfaces.Clients`  |
|Objects|`Kraken.Net.Objects`|`Kraken.Net.Objects.Models`  |
|SymbolOrderBook|`Kraken.Net`|`Kraken.Net.SymbolOrderBooks`|

### Client options
The `BaseAddress` and rate limiting options are now under the `SpotApiOptions`.  
*V2*
```csharp
var krakenClient = new KrakenClient(new KrakenClientOptions
{
	ApiCredentials = new ApiCredentials("API-KEY", "API-SECRET"),
	BaseAddress = "ADDRESS",
	RateLimitingBehaviour = RateLimitingBehaviour.Fail
});
```

*V3*
```csharp
var krakenClient = new KrakenClient(new KrakenClientOptions
{
	ApiCredentials = new ApiCredentials("API-KEY", "API-SECRET"),
	SpotApiOptions = new RestApiClientOptions
	{
		BaseAddress = "ADDRESS",
		RateLimitingBehaviour = RateLimitingBehaviour.Fail
	}
});
```

### Client structure
Version 3 adds the `SpotApi` Api client under the `KrakenClient`, and a topic underneath that. This is done to keep the same client structure as other exchange implementations, more info on this [here](https://jkorf.github.io/CryptoExchange.Net/Clients.html).
In the KrakenSocketClient a `SpotStreams` Api client is added. This means all calls will have changed, though most will only need to add `SpotApi.[Topic].XXX`/`SpotStreams.XXX`:

*V2*
```csharp
var balances = await krakenClient.GetBalancesAsync();
var withdrawals = await krakenClient.GetLedgerInfoAsync();

var tickers = await krakenClient.GetTickersAsync();
var symbols = await krakenClient.GetSymbolsAsync();

var order = await krakenClient.PlaceOrderAsync();
var trades = await krakenClient.GetUserTradesAsync();

var sub = krakenSocket.SubscribeToTickerUpdatesAsync("XBT/USD", DataHandler);
```

*V3*  
```csharp
var balances = await krakenClient.SpotApi.Account.GetBalancesAsync();
var withdrawals = await krakenClient.SpotApi.Account.GetLedgerInfoAsync();

var tickers = await krakenClient.SpotApi.ExchangeData.GetTickersAsync();
var symbols = await krakenClient.SpotApi.ExchangeData.GetSymbolsAsync();

var order = await krakenClient.SpotApi.Trading.PlaceOrderAsync();
var trades = await krakenClient.SpotApi.Trading.GetUserTradesAsync();

var sub = krakenSocket.SpotStreams.SubscribeToTickerUpdatesAsync("XBT/USD", DataHandler);
```

### Definitions
Some names have been changed to a common definition. This includes where the name is part of a bigger name  
|Old|New||
|----|---|---|
|`Currency`|`Asset`||
|`Timestamp/Open/High/Low/Close`|`OpenTime/OpenPrice/HighPrice/LowPrice/ClosePrice`||
|`Bid`/`Ask`/`BidVolume`/`AskVolume`|`BestBidPrice`/`BestAskPrice`/`BestBidQuantity`/`BestAskQuantity`||

Some names have slightly changed to be consistent across different libraries   
`ExecutedQuantity` -> `QuantityFilled`  

### KrakenSymbolOrderBook
The `KrakenSymbolOrderBook` has been renamed to KrakenSpotSymbolOrderBook, and parameters have been removed from the constructor. These can be set via the KrakenOrderBookOptions:  
*V2*
```csharp
var book = new KrakenSymbolOrderBook("XBT/USD", 25);
```

*V3*
```csharp
var book = new KrakenSpotSymbolOrderBook("XBT/USD", new KrakenOrderBookOptions
{
	Limit = 25
});
```

### Changed methods

