# ![.Kraken.Net](https://github.com/JKorf/Kraken.Net/blob/master/Kraken.Net/Icon/icon.png?raw=true) Kraken.Net

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/Kraken.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/Kraken.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/Kraken.Net?style=for-the-badge)
![Since](https://img.shields.io/badge/since-2019-brightgreen?style=for-the-badge)

Kraken.Net is a strongly typed client library for accessing the [Kraken REST and Websocket API](https://www.kraken.com/features/api).
## Features
* Response data is mapped to descriptive models
* Input parameters and response values are mapped to discriptive enum values where possible
* High performance
* Automatic websocket (re)connection management 
* Client side rate limiting 
* Client side order book implementation
* Support for managing different accounts
* Extensive logging
* Support for different environments
* Easy integration with other exchange client based on the CryptoExchange.Net base library
* Native AOT support

## Supported Frameworks
The library is targeting both `.NET Standard 2.0` and `.NET Standard 2.1` for optimal compatibility, as well as the latest dotnet versions to use the latest framework features.

|.NET implementation|Version Support|
|--|--|
|.NET Core|`2.0` and higher|
|.NET Framework|`4.6.1` and higher|
|Mono|`5.4` and higher|
|Xamarin.iOS|`10.14` and higher|
|Xamarin.Android|`8.0` and higher|
|UWP|`10.0.16299` and higher|
|Unity|`2018.1` and higher|

## Install the library

### NuGet 
[![NuGet version](https://img.shields.io/nuget/v/KrakenExchange.net.svg?style=for-the-badge)](https://www.nuget.org/packages/KrakenExchange.Net)  [![Nuget downloads](https://img.shields.io/nuget/dt/KrakenExchange.Net.svg?style=for-the-badge)](https://www.nuget.org/packages/KrakenExchange.Net)

	dotnet add package KrakenExchange.Net
	
### GitHub packages
Kraken.Net is available on [GitHub packages](https://github.com/JKorf/Kraken.Net/pkgs/nuget/KrakenExchange.Net). You'll need to add `https://nuget.pkg.github.com/JKorf/index.json` as a NuGet package source.

### Download release
[![GitHub Release](https://img.shields.io/github/v/release/JKorf/Kraken.Net?style=for-the-badge&label=GitHub)](https://github.com/JKorf/Kraken.Net/releases)

The NuGet package files are added along side the source with the latest GitHub release which can found [here](https://github.com/JKorf/Kraken.Net/releases).

## How to use
*Basic request:*  

```csharp
// Get the ETH/USD ticker via rest request
var restClient = new KrakenRestClient();
var tickerResult = await restClient.SpotApi.ExchangeData.GetTickerAsync("ETHUSD");
var lastPrice = tickerResult.Data.First().Value.LastTrade.Price;
```

*Place order:*
```csharp
var restClient = new KrakenRestClient(opts => {
	opts.ApiCredentials = new KrakenCredentials(
		new HMACCredential("SPOTKEY", "SPOTSECRET"),
		new HMACCredential("FUTURESKEY", "FUTURESSECRET")
	);
});

// Place Limit order to go long for 0.1 ETH at 2000
var orderResult = await restClient.FuturesApi.Trading.PlaceOrderAsync(
    "PF_ETHUSD",
    OrderSide.Buy,
    FuturesOrderType.Limit,
    0.1m,
    2000
    );
```

*WebSocket subscription:*

```csharp
// Subscribe to ETH/USD ticker updates via the websocket API
var socketClient = new KrakenSocketClient();
var tickerSubscriptionResult = socketClient.SpotApi.SubscribeToTickerUpdatesAsync("ETH/USD", (update) =>
{
	var lastPrice = update.Data.LastPrice;
});
```

For information on the clients, dependency injection, response processing and more see the [Kraken.Net documentation](https://cryptoexchange.jkorf.dev?library=Kraken.Net) or have a look at the examples [here](https://github.com/JKorf/Kraken.Net/tree/master/Examples) or [here](https://github.com/JKorf/CryptoExchange.Net/tree/master/Examples).

## AI / LLM documentation

Kraken.Net includes AI-oriented documentation and examples for code generation tools:

|File|Purpose|
|--|--|
|[`AGENTS.md`](AGENTS.md)|Assistant skill with core Kraken.Net patterns, pitfalls, and examples|
|[`llms.txt`](llms.txt)|Short LLM index with links to docs, examples, and critical usage rules|
|[`llms-full.txt`](llms-full.txt)|Detailed LLM context with endpoint routing, code patterns, and anti-hallucination checks|
|[`docs/ai-api-map.md`](docs/ai-api-map.md)|Table-style intent-to-method map for Spot, Futures, WebSocket, Earn, and SharedApis|
|[`Examples/ai-friendly`](Examples/ai-friendly)|Compilable single-file examples for common REST, WebSocket, shared API, and error handling workflows|

See [cryptoexchange-skills-hub](https://github.com/JKorf/cryptoexchange-skills-hub) for installable skills.

## CryptoExchange.Net
Kraken.Net is based on the [CryptoExchange.Net](https://github.com/JKorf/CryptoExchange.Net) base library. Other exchange API implementations based on the CryptoExchange.Net base library are available and follow the same logic.

CryptoExchange.Net also allows for [easy access to different exchange API's](https://cryptoexchange.jkorf.dev/client-libs/shared).

|Exchange|Repository|Nuget|
|--|--|--|
|Aster|[JKorf/Aster.Net](https://github.com/JKorf/Aster.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Aster.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Aster.Net)|
|Binance|[JKorf/Binance.Net](https://github.com/JKorf/Binance.Net)|[![Nuget version](https://img.shields.io/nuget/v/Binance.net.svg?style=flat-square)](https://www.nuget.org/packages/Binance.Net)|
|BingX|[JKorf/BingX.Net](https://github.com/JKorf/BingX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.BingX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.BingX.Net)|
|Kraken|[JKorf/Kraken.Net](https://github.com/JKorf/Kraken.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kraken.net.svg?style=flat-square)](https://www.nuget.org/packages/Kraken.Net)|
|Bitget|[JKorf/Bitget.Net](https://github.com/JKorf/Bitget.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Bitget.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Bitget.Net)|
|BitMart|[JKorf/BitMart.Net](https://github.com/JKorf/BitMart.Net)|[![Nuget version](https://img.shields.io/nuget/v/BitMart.net.svg?style=flat-square)](https://www.nuget.org/packages/BitMart.Net)|
|BitMEX|[JKorf/BitMEX.Net](https://github.com/JKorf/BitMEX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.BitMEX.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.BitMEX.Net)|
|Bitstamp|[JKorf/Bitstamp.Net](https://github.com/JKorf/Bitstamp.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitstamp.Net.svg?style=flat-square)](https://www.nuget.org/packages/Bitstamp.Net)|
|BloFin|[JKorf/BloFin.Net](https://github.com/JKorf/BloFin.Net)|[![Nuget version](https://img.shields.io/nuget/v/BloFin.net.svg?style=flat-square)](https://www.nuget.org/packages/BloFin.Net)|
|Bybit|[JKorf/Bybit.Net](https://github.com/JKorf/Bybit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bybit.net.svg?style=flat-square)](https://www.nuget.org/packages/Bybit.Net)|
|Coinbase|[JKorf/Coinbase.Net](https://github.com/JKorf/Coinbase.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Coinbase.Net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Coinbase.Net)|
|CoinEx|[JKorf/CoinEx.Net](https://github.com/JKorf/CoinEx.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinEx.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinEx.Net)|
|CoinGecko|[JKorf/CoinGecko.Net](https://github.com/JKorf/CoinGecko.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinGecko.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinGecko.Net)|
|CoinW|[JKorf/CoinW.Net](https://github.com/JKorf/CoinW.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinW.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinW.Net)|
|Crypto.com|[JKorf/CryptoCom.Net](https://github.com/JKorf/CryptoCom.Net)|[![Nuget version](https://img.shields.io/nuget/v/CryptoCom.net.svg?style=flat-square)](https://www.nuget.org/packages/CryptoCom.Net)|
|DeepCoin|[JKorf/DeepCoin.Net](https://github.com/JKorf/DeepCoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/DeepCoin.net.svg?style=flat-square)](https://www.nuget.org/packages/DeepCoin.Net)|
|Gate.io|[JKorf/GateIo.Net](https://github.com/JKorf/GateIo.Net)|[![Nuget version](https://img.shields.io/nuget/v/GateIo.net.svg?style=flat-square)](https://www.nuget.org/packages/GateIo.Net)|
|HTX|[JKorf/HTX.Net](https://github.com/JKorf/HTX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.HTX.Net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.HTX.Net)|
|HyperLiquid|[JKorf/HyperLiquid.Net](https://github.com/JKorf/HyperLiquid.Net)|[![Nuget version](https://img.shields.io/nuget/v/HyperLiquid.Net.svg?style=flat-square)](https://www.nuget.org/packages/HyperLiquid.Net)|
|Kucoin|[JKorf/Kucoin.Net](https://github.com/JKorf/Kucoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kucoin.net.svg?style=flat-square)](https://www.nuget.org/packages/Kucoin.Net)|
|Lighter|[JKorf/Lighter.Net](https://github.com/JKorf/Lighter.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Lighter.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Lighter.Net)|
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|
|Pionex|[JKorf/Pionex.Net](https://github.com/JKorf/Pionex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Pionex.net.svg?style=flat-square)](https://www.nuget.org/packages/Pionex.Net)|
|Polymarket|[JKorf/Polymarket.Net](https://github.com/JKorf/Polymarket.Net)|[![Nuget version](https://img.shields.io/nuget/v/Polymarket.net.svg?style=flat-square)](https://www.nuget.org/packages/Polymarket.Net)|
|Toobit|[JKorf/Toobit.Net](https://github.com/JKorf/Toobit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Toobit.net.svg?style=flat-square)](https://www.nuget.org/packages/Toobit.Net)|
|Upbit|[JKorf/Upbit.Net](https://github.com/JKorf/Upbit.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Upbit.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Upbit.Net)|
|Weex|[JKorf/Weex.Net](https://github.com/JKorf/Weex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Weex.net.svg?style=flat-square)](https://www.nuget.org/packages/Weex.Net)|
|WhiteBit|[JKorf/WhiteBit.Net](https://github.com/JKorf/WhiteBit.Net)|[![Nuget version](https://img.shields.io/nuget/v/WhiteBit.net.svg?style=flat-square)](https://www.nuget.org/packages/WhiteBit.Net)|
|XT|[JKorf/XT.Net](https://github.com/JKorf/XT.Net)|[![Nuget version](https://img.shields.io/nuget/v/XT.net.svg?style=flat-square)](https://www.nuget.org/packages/XT.Net)|

## Discord
[![Nuget version](https://img.shields.io/discord/847020490588422145?style=for-the-badge)](https://discord.gg/MSpeEtSY8t)  
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). Feel free to join for discussion and/or questions around the CryptoExchange.Net and implementation libraries.

## Supported functionality

### Spot REST
|API|Supported|Location|
|--|--:|--|
|Market Data|✓|`restClient.SpotApi.ExchangeData`|
|Account Data|✓|`restClient.SpotApi.Account` / `restClient.SpotApi.Trading`|
|Trading|✓|`restClient.SpotApi.Trading`|
|Funding|✓|`restClient.SpotApi.Account`|
|Subaccounts|X||
|Earn|✓|`restClient.SpotApi.Earn`|
|NFT Market Data|X||
|NFT Trading|X||

### Spot Api Websocket V2
|API|Supported|Location|
|--|--:|--|
|User Trading|✓|`socketClient.SpotApi`|
|User Data|✓|`socketClient.SpotApi`|
|Market Data|✓|`socketClient.SpotApi`|
|Admin|✓|`socketClient.SpotApi`|

### Spot FIX
|API|Supported|Location|
|--|--:|--|
|*|X||

### Futures REST
|API|Supported|Location|
|--|--:|--|
|Trading Market Data|✓|`restClient.FuturesApi.ExchangeData`|
|Trading Instrument Details|✓|`restClient.FuturesApi.ExchangeData`|
|Trading Fee Schedules|✓|`restClient.FuturesApi.ExchangeData`|
|Trading Account Info|✓|`restClient.FuturesApi.Account` / `restClient.FuturesApi.Trading`|
|Trading Order Management|✓|`restClient.FuturesApi.Trading`|
|Subaccounts|X||
|Transfers|✓|`restClient.FuturesApi.Account`|
|Assignment Program|X||
|Multi-Collateral|✓|`restClient.FuturesApi.Account` / `restClient.FuturesApi.Trading`|
|General|✓|`restClient.FuturesApi.ExchangeData`|
|Historical data|✓|`restClient.FuturesApi.Trading`|
|Historical Funding Rates|✓|`restClient.FuturesApi.ExchangeData`|
|History Account History|✓|`restClient.FuturesApi.Account` / `restClient.FuturesApi.Trading`|
|History Market History|✓|`restClient.FuturesApi.ExchangeData`|
|Charts Candles|✓|`restClient.FuturesApi.ExchangeData`|
|Charts Analytics|X||

### Futures Websocket
|API|Supported|Location|
|--|--:|--|
|Trading Market Data|✓|`restClient.FuturesApi.ExchangeData`|
|User Data|✓|`socketClient.FuturesApi`|
|Market Data|✓|`socketClient.FuturesApi`|
|Admin|✓|`socketClient.FuturesApi`|

## Support the project
Any support is greatly appreciated.

### Donate
Make a one time donation in a crypto currency of your choice. If you prefer to donate a currency not listed here please contact me.

**Btc**:  bc1q277a5n54s2l2mzlu778ef7lpkwhjhyvghuv8qf  
**Eth**:  0xcb1b63aCF9fef2755eBf4a0506250074496Ad5b7   
**USDT (TRX)**  TKigKeJPXZYyMVDgMyXxMf17MWYia92Rjd

### Sponsor
Alternatively, sponsor me on Github using [Github Sponsors](https://github.com/sponsors/JKorf). 

## Release notes
* Version 8.2.0 - 21 Jul 2026
    * Updated CryptoExchange.Net to v12.2.0 
    * Added SpotSymbolCatalog to Shared ISpotSymbolRestClient interface
    * Added FuturesSymbolCatalog to Shared IFuturesSymbolRestClient interface
    * Added BaseAssetType, BaseAssetSubType, QuoteAssetType and QuoteAssetSubType to GetSymbolsRequest model
    * Added DisplayName to SharedSpotSymbol and SharedFuturesSymbol models
    * Added BaseAssetType, BaseAssetSubType, QuoteAssetType and QuoteAssetSubType to SharedSpotSymbol and SharedFuturesSymbol models
    * Added DebuggerDisplay attributes to Shared models
    * Fixed restClient.FuturesApi.Trading.GetOrderAsync exception if not order returned

* Version 8.1.1 - 13 Jul 2026
    * Fixed exception during authentication when retrying requests

* Version 8.1.0 - 09 Jul 2026
    * Updated CryptoExchange.Net to v12.1.0
    * Added MethodId, NetworkId, Fee to KrakenWithdrawMethod model
    * Added TradeOrderType, AssetClass, Leverage and TradeId to KrakenUserTrade
    * Added AssetClass, Inputs to KrakenTradeVolume model
    * Added MarginFreeForOrders,UnexecutedValue to KrakenTradeBalance model
    * Added TimeInForce to KrakenOrder model, added AssetClass to KrakenOrderInfo model
    * Added MarginRate to KrakenAssetInfo model
    * Added OpenPrice, HighPrice, LowPrice to KrakenFuturesTickerUpdate model
    * Added HighPrice and LowPrice mapping to Shared Futures SubscribeToTickerUpdatesAsync events
    * Added QuantityRemaining property to KrakenFuturesUserTradesUpdate model
    * Updated KrakenFuturesSnapshotOpenOrders.Reason to enum
    * Updated TimeInForce mapping
    * Updated SpotApi.Account.GetTradeVolumeAsync endpoint
    * Updated futures GetFeesAsync implementation
    * Fixed OrderId not set on Shared Futures SubscribeToFuturesOrderUpdatesAsync updates
    * Fixed incorrect order status mapping Shared Futures SubscribeToFuturesOrderUpdatesAsync updates
    * Fixed Premium deserialization on KrakenFuturesTickerUpdate model
    * Fixed futures orders/user trades subscription missing updates
    * Removed deprecated FuturesApi GetFeeScheduleVolumeAsync, GetInitialMarginRequirementsAsync, GetFeeSchedulesAsync and GetMaxOrderQuantityAsync endpoints

* Version 8.0.0 - 29 Jun 2026
    * Result types:
      * (Web)CallResult types are replaced by HttpResult, WebSocketResult and QueryResult with the same logic
      * WebSocketResult and QueryResult now return additional info for websocket operations
      * Updated result types to record type
      * Removed implicit result type conversion to bool, `if (result)` no longer works, instead use `if (result.Success)`
      * Fixed result object nullability hinting, for example Data might be null if Success isn't checked for true
    * Clients:
      * Added ToString overrides on base API types
      * Added Exchange property on BaseApiClient
      * Added ApiCredentials property on Api clients
      * Updated ILogger source from client name to topic specific client name
      * Removed logging from client creation
      * Fixed issue in SocketApiClient.GetSocketConnection causing requests to always wait the full max 10 seconds when there was a reconnecting socket
    * Shared APIs:
      * Added missing dedicated option types
      * Added Discover method on ISharedClient interface, returning info on supported capabilities and operations
      * Added ResetStaticExchangeParameters method on ExchangeParameters
      * Added Status property to SharedWithdrawal model
      * Added TradingModes property to SharedBalance model
      * Added REST Shared GetPositionsAsync UnrealizedPnl mapping
      * Updated Shared ExchangeParameters parameter names to be case insensitive
      * Updated code comments
      * Replaced ExchangeResult with ExchangeCallResult type
      * Removed TradingMode from the response model, only maintained on models where it makes sense
      * Fixed Shared REST GetFundingRateHistoryAsync not allowing time filtering
    * Added async streaming on UserDataTracker items with StreamUpdatesAsync
    * Added cancellation token support to UserDataTracker starting
    * Added SupportedEnvironments property to PlatformInfo
    * Added Clear() method on UserClientProvider to clear all cached clients
    * Added support for setting API credentials one by one for spot/futures
    * Added UnrealizedPnl to KrakenFuturesPosition model 
    * Updated internal websocket token management
    * Various small performance improvements
    * Fixed websocket connection attempts counting towards rate limit even when server could not be reached
    * Fixed exception for credentials when accessing public api
    * Fixed socketClient.SpotApi.EditOrderAsync parameter serialization when not using orderId
