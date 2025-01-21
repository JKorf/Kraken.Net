# ![.Kraken.Net](https://github.com/JKorf/Kraken.Net/blob/master/Kraken.Net/Icon/icon.png?raw=true) Kraken.Net

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/Kraken.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/Kraken.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/Kraken.Net?style=for-the-badge)

Kraken.Net is a strongly typed client library for accessing the [Kraken REST and Websocket API](https://www.kraken.com/features/api).
## Features
* Response data is mapped to descriptive models
* Input parameters and response values are mapped to discriptive enum values where possible
* Automatic websocket (re)connection management 
* Client side rate limiting 
* Client side order book implementation
* Extensive logging
* Support for different environments
* Easy integration with other exchange client based on the CryptoExchange.Net base library

## Supported Frameworks
The library is targeting both `.NET Standard 2.0` and `.NET Standard 2.1` for optimal compatibility

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
*REST Endpoints*  

```csharp
// Get the ETH/USD ticker via rest request
var restClient = new KrakenRestClient();
var tickerResult = await restClient.SpotApi.ExchangeData.GetTickerAsync("ETHUSD");
var lastPrice = tickerResult.Data.First().Value.LastTrade.Price;
```

*Websocket streams*  

```csharp
// Subscribe to ETH/USD ticker updates via the websocket API
var socketClient = new KrakenSocketClient();
var tickerSubscriptionResult = socketClient.SpotApi.SubscribeToTickerUpdatesAsync("ETH/USD", (update) =>
{
	var lastPrice = update.Data.LastPrice;
});
```

For information on the clients, dependency injection, response processing and more see the [Kraken.Net documentation](https://jkorf.github.io/Kraken.Net), [CryptoExchange.Net documentation](https://jkorf.github.io/CryptoExchange.Net), or have a look at the examples [here](https://github.com/JKorf/Kraken.Net/tree/master/Examples) or [here](https://github.com/JKorf/CryptoExchange.Net/tree/master/Examples).

## CryptoExchange.Net
Kraken.Net is based on the [CryptoExchange.Net](https://github.com/JKorf/CryptoExchange.Net) base library. Other exchange API implementations based on the CryptoExchange.Net base library are available and follow the same logic.

CryptoExchange.Net also allows for [easy access to different exchange API's](https://jkorf.github.io/CryptoExchange.Net#idocs_shared).

|Exchange|Repository|Nuget|
|--|--|--|
|Binance|[JKorf/Binance.Net](https://github.com/JKorf/Binance.Net)|[![Nuget version](https://img.shields.io/nuget/v/Binance.net.svg?style=flat-square)](https://www.nuget.org/packages/Binance.Net)|
|BingX|[JKorf/BingX.Net](https://github.com/JKorf/BingX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.BingX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.BingX.Net)|
|Kraken|[JKorf/Kraken.Net](https://github.com/JKorf/Kraken.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kraken.net.svg?style=flat-square)](https://www.nuget.org/packages/Kraken.Net)|
|Bitget|[JKorf/Bitget.Net](https://github.com/JKorf/Bitget.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Bitget.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Bitget.Net)|
|BitMart|[JKorf/BitMart.Net](https://github.com/JKorf/BitMart.Net)|[![Nuget version](https://img.shields.io/nuget/v/BitMart.net.svg?style=flat-square)](https://www.nuget.org/packages/BitMart.Net)|
|Bybit|[JKorf/Bybit.Net](https://github.com/JKorf/Bybit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bybit.net.svg?style=flat-square)](https://www.nuget.org/packages/Bybit.Net)|
|Coinbase|[JKorf/Coinbase.Net](https://github.com/JKorf/Coinbase.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Coinbase.Net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Coinbase.Net)|
|CoinEx|[JKorf/CoinEx.Net](https://github.com/JKorf/CoinEx.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinEx.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinEx.Net)|
|CoinGecko|[JKorf/CoinGecko.Net](https://github.com/JKorf/CoinGecko.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinGecko.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinGecko.Net)|
|Crypto.com|[JKorf/CryptoCom.Net](https://github.com/JKorf/CryptoCom.Net)|[![Nuget version](https://img.shields.io/nuget/v/CryptoCom.net.svg?style=flat-square)](https://www.nuget.org/packages/CryptoCom.Net)|
|Gate.io|[JKorf/GateIo.Net](https://github.com/JKorf/GateIo.Net)|[![Nuget version](https://img.shields.io/nuget/v/GateIo.net.svg?style=flat-square)](https://www.nuget.org/packages/GateIo.Net)|
|HTX|[JKorf/HTX.Net](https://github.com/JKorf/HTX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.HTX.Net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.HTX.Net)|
|HyperLiquid|[JKorf/HyperLiquid.Net](https://github.com/JKorf/HyperLiquid.Net)|[![Nuget version](https://img.shields.io/nuget/v/HyperLiquid.Net.svg?style=flat-square)](https://www.nuget.org/packages/HyperLiquid.Net)|
|Kucoin|[JKorf/Kucoin.Net](https://github.com/JKorf/Kucoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kucoin.net.svg?style=flat-square)](https://www.nuget.org/packages/Kucoin.Net)|
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|
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
* Version 5.5.3 - 18 Jan 2025
    * Fix restClient.FuturesApi.Trading.GetUserTradesAsync startTime parameter not being applied

* Version 5.5.2 - 07 Jan 2025
    * Updated CryptoExchange.Net version
    * Added Type property to KrakenExchange class

* Version 5.5.1 - 23 Dec 2024
    * Fixed reconnection bug for query connections without active subscription

* Version 5.5.0 - 23 Dec 2024
    * Updated CryptoExchange.Net to version 8.5.0, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Added SetOptions methods on Rest and Socket clients
    * Added setting of DefaultProxyCredentials to CredentialCache.DefaultCredentials on the DI http client
    * Improved websocket disconnect detection

* Version 5.4.2 - 15 Dec 2024
    * Added newAssetNameResponse parameter to restClient.SpotApi.Account.GetBalancesAsync

* Version 5.4.1 - 03 Dec 2024
    * Updated CryptoExchange.Net to version 8.4.3, see https://github.com/JKorf/CryptoExchange.Net/releases/
    * Fixed orderbook creation via KrakenOrderBookFactory

* Version 5.4.0 - 28 Nov 2024
    * Updated CryptoExchange.Net to version 8.4.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/8.4.0
    * Added GetFeesAsync Shared REST client implementations
    * Updated BinanceOptions to LibraryOptions implementation
    * Updated test and analyzer package versions
    * CryptoExchange update, fixed restClient.FuturesApi.Account.GetFeeScheduleVolumeAsync deserialization

* Version 5.3.0 - 19 Nov 2024
    * Updated CryptoExchange.Net to version 8.3.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/8.3.0
    * Added support for loading client settings from IConfiguration
    * Added DI registration method for configuring Rest and Socket options at the same time
    * Added DisplayName and ImageUrl properties to KrakenExchange class
    * Added newAssetNameResponse parameter to restClient.SpotApi.ExchangeData.GetSymbolsAsync and restClient.SpotApi.ExchangeData.GetAssetsAsync
    * Updated client constructors to accept IOptions from DI
    * Removed redundant KrakenSocketClient constructor

* Version 5.2.0 - 06 Nov 2024
    * Updated CryptoExchange.Net to version 8.2.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/8.2.0

* Version 5.1.1 - 01 Nov 2024
    * Updated CryptoExchange.Net version to fix exception during websocket reconnection when using websocket requests

* Version 5.1.0 - 28 Oct 2024
    * Updated CryptoExchange.Net to version 8.1.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/8.1.0
    * Moved FormatSymbol to KrakenExchange class
    * Added support Side setting on SharedTrade model
    * Added KrakenTrackerFactory for creating trackers
    * Added overload to Create method on KrakenOrderBookFactory support SharedSymbol parameter
    * Fixed websocket Unsubscribe for orderbook subscriptions

* Version 5.0.2 - 22 Oct 2024
    * Fixed websocket subscription request revitalization throwing an exception

* Version 5.0.1 - 21 Oct 2024
    * Fixed socketClient.SpotApi.SubscribeToAggregatedOrderBookUpdatesAsync and SubscribeToInvidualOrderBookUpdatesAsync not passing the depth parameter to the server
    * Fixed userReference parameter incorrectly set at restClient.SpotApi.Trading.PlaceOrderAsync
    * Fixed timestamp serialization for socketClient.SpotApi queries

* Version 5.0.0 - 14 Oct 2024
    * Updated CryptoExchange.Net to version 8.0.3, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/8.0.3
    * Updated the library to use System.Text.Json for (de)serialization instead of Json.Net
    * Updated Spot websocket implementation from V1 to V2
        * Moved requesting of WebSocket token for private endpoints from user endpoint to internal
        * Removed automatic mapping of BTC to XBT (V2 API used BTC as symbol instead of the previous XBT)
        * Respone models have been updated to V2
        * Spread subscription has been removed, part of Ticker stream now
        * Added individual order book subscription
        * Added instrument subscription
        * Added user balances subscription
        * UserTrade subscription has been removed, part of Order stream now
        * Added socketClient.SpotApi.PlaceMultipleOrdersAsync endpoint
        * Added socketClient.SpotApi.EditOrderAsync endpoint
    * Added socketClient.SpotApi.ReplaceOrderAsync endpoint
    * Added Shared implementation for Futures WebSocket and REST APIs
    * Extended Shared implementation for Spot WebSocket API
    * Added restClient.SpotApi.Trading.CancelAllOrdersAfterAsync endpoint
    * Added restClient.FuturesApi.ExchangeData.GetTickerAsync endpoint
    * Added restClient.FuturesApi.Trading.GetOrdrAync endpoint
    * Renamed clientOrderId to userReference parameters in Spot orders as it was implemented with the `userref` field
    * Added new clientOrderId parameter to Spot orders using the correct `cl_ord_id` field
    * Updated Shared Spot REST implementation to use new clientOrderId property
    * Updated restClient.FuturesApi.GetBalancesAsync response so it's more discoverable
    * Updated AssetStatus Enum values
    * Updated SymbolStatus Enum values
    * Fixed deserialization issue FuturesApi.Trading.GetOpenPositionsAsync

* Version 4.12.0 - 27 Sep 2024
    * Updated CryptoExchange.Net to version 8.0.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/8.0.0
    * Added partial Shared client interfaces implementation for Spot and FuturesApi Rest and Socket clients
    * Added SpotApi.Account.GetDepositHistoryAsync endpoint
    * Added SpotApi.Account.GetWithdrawalHistoryAsync endpoint
    * Added trades parameter to SpotApi.Trading.GetOrderAsync and GetOrdersAsync endpoints
    * Added Maker property on KrakenUserTrade model
    * Renamed Decimals to PriceDecimals on KrakenSymbol model
    * Updated Status property type from string? to SymbolStatus on KrakenSymbol model
    * Updated Sourcelink package version
    * Marked ISpotClient references as deprecated

* Version 4.11.1 - 28 Aug 2024
    * Updated CryptoExchange.Net to version 7.11.2, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/7.11.2
    * Added pricePrefixOperator, priceSuffixOperator, secondaryPricePrefixOperator and secondaryPriceSuffixOperator parameters to SpotApi.Trading.PlaceOrderAsync and EditOrderAsync endpoints

* Version 4.11.0 - 07 Aug 2024
    * Updated CryptoExchange.Net to version 7.11.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/7.11.0
    * Updated XML code comments

* Version 4.10.0 - 27 Jul 2024
    * Updated CryptoExchange.Net to version 7.10.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/7.10.0

* Version 4.9.0 - 16 Jul 2024
    * Updated CryptoExchange.Net to version 7.9.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/7.9.0

* Version 4.8.1 - 02 Jul 2024
    * Updated CryptoExchange.Net to V7.8.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/7.8.0
    * Updated KrakenAllocatedAmount model

* Version 4.8.0 - 23 Jun 2024
    * Updated CryptoExchange.Net to version 7.7.0, see https://github.com/JKorf/CryptoExchange.Net/releases/tag/7.7.0
    * Updated response models from classes to records
    * Added CancellationToken optional parameter to websocket requests

* Version 4.7.0 - 11 Jun 2024
    * Fix Asset not set on response model in SpotApi.Account.GetAvailableBalancesAsync
    * Updated CryptoExchange.Net to v7.6.0, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes

* Version 4.6.6 - 02 Jun 2024
    * Added margin parameter to websocket SpotApi.PlaceOrderAsync
    * Added countryCode parameter to SpotApi.ExchangeData.GetSymbolsAsync

* Version 4.6.5 - 07 May 2024
    * Updated CryptoExchange.Net to v7.5.2, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes

* Version 4.6.4 - 03 May 2024
    * Updated various models
    * Fixed deserialization issue in SpotApi.ExchangeData.GetSymbolsAsync endpoint

* Version 4.6.3 - 01 May 2024
    * Updated CryptoExchange.Net to v7.5.0, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes

* Version 4.6.2 - 28 Apr 2024
    * Added Url and ApiDocsUrl to KrakenExchange static info class
    * Added KrakenOrderBookFactory book creation method
    * Fixed KrakenOrderBookFactory injection issue
    * Updated CryptoExchange.Net to v7.4.0, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes

* Version 4.6.1 - 23 Apr 2024
    * Updated CryptoExchange.Net to 7.3.3, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes

* Version 4.6.0 - 18 Apr 2024
    * Updated CryptoExchange.Net to 7.3.1, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes
    * (Re)implemented client side ratelimiting
    * Added Payout to Allocations response model

* Version 4.5.0 - 01 Apr 2024
    * Added Kraken Earn endpoints
    * Added missing LedgerEntryType enum values
    * Removed deprecated Staking endpoints

* Version 4.4.3 - 25 Mar 2024
    * Fixed parameter serialization for socket client SpotApi.PlaceOrderAsync

* Version 4.4.2 - 24 Mar 2024
    * Fixed websocket deserialization int overflow issue

* Version 4.4.1 - 24 Mar 2024
	* Updated CryptoExchange.Net to 7.2.0, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes

* Version 4.4.0 - 16 Mar 2024
    * Updated CryptoExchange.Net to 7.1.0, see https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes for release notes
	
* Version 4.3.0 - 25 Feb 2024
    * Updated CryptoExchange.Net and implemented reworked websocket message handling. For release notes for the CryptoExchange.Net base library see: https://github.com/JKorf/CryptoExchange.Net?tab=readme-ov-file#release-notes
    * Fixed issue in DI registration causing http client to not be correctly injected
    * Added single symbol overloads to Futures websocket subscriptions
    * Removed redundant KrakenRestClient constructor overload
    * Updated some namespaces

* Version 4.2.2 - 04 Jan 2024
    * Fixed issue deserializing DateTime value in user order updates when running .net framework

* Version 4.2.1 - 03 Jan 2024
    * Added SpotApi.Account.GetWithdrawAddressesAsync
    * Added SpotApi.Account.GetWithdrawMethodsAsync
    * Added missing otp parameter SpotApi.Trading.EditOrderAsync
    * Fixed SpotApi.Trading.EditOrderAsync response deserialization

* Version 4.2.0 - 02 Dec 2023
    * Added SpotApi.Trading.AddMultipleOrdersAsync
    * Added SpotApi.Trading.EditOrderAsync
    * Added FuturesApi.Account.GetInitialMarginRequirementsAsync
    * Added FuturesApi.Account.GetMaxOrderQuantityAsync
    * Changed futures quantity parameter from int to decimal

* Version 4.1.5 - 24 Oct 2023
    * Updated CryptoExchange.Net

* Version 4.1.4 - 09 Oct 2023
    * Updated CryptoExchange.Net version
    * Added ISpotClient to DI injection

* Version 4.1.3 - 25 Aug 2023
    * Updated CryptoExchange.Net

* Version 4.1.2 - 23 Jul 2023
    * Fix for missing Symbol property on futures order book stream update

* Version 4.1.1 - 11 Jul 2023
    * Added amount parameter to GetDepositAddresses endpoint
    * Added limit parameter to SpotApi GetTradeHistory endpoint
    * Added address parameter to Withdraw endpoint
    * Added missing stop order properties on futures OpenOrders model
    * Fixed duplice parameter issue on SpotApi PlaceOrder
    * Fixed TickSize property parsing on SpotApi GetSymbols endpoint

* Version 4.1.0 - 05 Jul 2023
    * Added support for Kraken Futures

* Version 4.0.0 - 25 Jun 2023
    * Updated CryptoExchange.Net to version 6.0.0
    * Renamed KrakenClient to KrakenRestClient
    * Renamed **Streams to **Api on the KrakenSocketClient
    * Updated endpoints to consistently use a base url without any path as basis to make switching environments/base urls clearer
    * Added IKrakenOrderBookFactory and implementation for creating order books
    * Updated dependency injection register method (AddKraken)

* Version 3.1.10 - 19 Jun 2023
    * Fixed close ordertype parameter being sent even when not provided

* Version 3.1.9 - 14 May 2023
    * Added close parameters to rest PlaceOrder endpoint

* Version 3.1.8 - 15 Apr 2023
    * Added GetWithdrawalStatusAsync endpoint
    * Added CancelWithdrawalAsync endpoint
    * Added TransferAsync endpoint
    * Updated various response models and endpoint parameters

* Version 3.1.7 - 18 Mar 2023
    * Added reduceOnly and selfTradePreventionType parameters to place order endpoints
    * Updated CryptoExchange.Net

* Version 3.1.6 - 14 Feb 2023
    * Updated CryptoExchange.Net
    * Added retry on nonce error

* Version 3.1.5 - 05 Feb 2023
    * Added CancelAllOrdersAsync endpoint

* Version 3.1.4 - 13 Jan 2023
    * Added support for Trade In Force

* Version 3.1.3 - 03 Jan 2023
    * Fixed stackoverflow on SubscribeToTradeUpdatesAsync

* Version 3.1.2 - 29 Dec 2022
    * Added multi-symbol overloads for socket client subscribe methods

* Version 3.1.1 - 21 Nov 2022
    * Fixed reconnect url

* Version 3.1.0 - 17 Nov 2022
    * Updated CryptoExchange.Net
    * Fixed authenticated socket subscription not being able to reconnect

* Version 3.0.15 - 17 Aug 2022
    * Added support for viqc oflags

* Version 3.0.14 - 18 Jul 2022
    * Updated CryptoExchange.Net

* Version 3.0.13 - 16 Jul 2022
    * Updated CryptoExchange.Net

* Version 3.0.12 - 10 Jul 2022
    * Fixed exception when trying to access result from PlaceOrderAsync when validateOnly is set to true
    * Updated CryptoExchange.Net

* Version 3.0.11 - 12 Jun 2022
    * Added staking endpoints
    * Updated CryptoExchange.Net

* Version 3.0.10 - 24 May 2022
    * Updated CryptoExchange.Net

* Version 3.0.9 - 22 May 2022
    * Updated CryptoExchange.Net

* Version 3.0.8 - 08 May 2022
    * Fixed symbol validation not allowing T/EUR
    * Updated CryptoExchange.Net

* Version 3.0.7 - 01 May 2022
    * Updated CryptoExchange.Net which fixed an timing related issue in the websocket reconnection logic
    * Added seconds representation to KlineInterval enum
    * Added flags parameter to socket PlaceOrderAsync

* Version 3.0.6 - 14 Apr 2022
    * Updated CryptoExchange.Net

* Version 3.0.5 - 10 Mar 2022
    * Updated CryptoExchange.Net

* Version 3.0.4 - 08 Mar 2022
    * Fixed startTime/endTime parameters in socket PlaceOrderAsync
    * Fixed socket CancelOrderAsync not returning error on if failed
    * Updated CryptoExchange.Net

* Version 3.0.3 - 01 Mar 2022
    * Updated CryptoExchange.Net improving the websocket reconnection robustness

* Version 3.0.2 - 27 Feb 2022
    * Updated CryptoExchange.Net to fix timestamping issue when request is ratelimiter

* Version 3.0.1 - 24 Feb 2022
    * Updated CryptoExchange.Net

* Version 3.0.0 - 18 Feb 2022
	* Added Github.io page for documentation: https://jkorf.github.io/Kraken.Net/
	* Added unit tests for parsing the returned JSON for each endpoint and subscription
	* Added AddKraken extension method on IServiceCollection for easy dependency injection
	* Added URL reference to API endpoint documentation for each endpoint
	* Added default rate limiter

	* Refactored client structure to be consistent across exchange implementations
	* Renamed various properties to be consistent across exchange implementations

	* Cleaned up project structure
	* Fixed various models

	* Updated CryptoExchange.Net, see https://github.com/JKorf/CryptoExchange.Net#release-notes
	* See https://jkorf.github.io/Kraken.Net/MigrationGuide.html for additional notes for updating from V2 to V3

* Version 2.2.3 - 08 Oct 2021
    * Updated CryptoExchange.Net to fix some socket issues

* Version 2.2.2 - 06 Oct 2021
    * Updated CryptoExchange.Net, fixing socket issue when calling from .Net Framework

* Version 2.2.1 - 05 Oct 2021
    * Updated CryptoExchange.Net

* Version 2.2.0 - 29 Sep 2021
    * Renamed SubscribeToDepthUpdatesAsync to SubscribeToOrderBookUpdatesAsync
    * Updated CryptoExchange.Net

* Version 2.1.3 - 22 Sep 2021
    * Fixed nonce provider when running multiple program instances

* Version 2.1.2 - 22 Sep 2021
    * Added trace output for nonce

* Version 2.1.1 - 21 Sep 2021
    * Fix for nonce provider not working correctly in combination with other exchanges

* Version 2.1.0 - 20 Sep 2021
    * Fix for not recognizing DOGE/BTC because Kraken renames them to XDG/XBT in update messages
    * Added custom nonce provider support
    * Added missing SetApiCredentials method
    * Updated CryptoExchange.Net

* Version 2.0.11 - 15 Sep 2021
    * Updated CryptoExchange.Net

* Version 2.0.10 - 09 Sep 2021
    * Removed invalid check GetOrdersAsync

* Version 2.0.8 - 02 Sep 2021
    * Fix for disposing order book closing socket even if there are other connections

* Version 2.0.7 - 02 Sep 2021
    * Fixed secondaryPrice parameter serialization in PlaceOrderAsync
    * Added snapshot parameter to SubscribeToOwnTradeUpdatesAsync

* Version 2.0.6 - 01 Sep 2021
    * Fix for user trades subscription topic

* Version 2.0.5 - 26 Aug 2021
    * Updated CryptoExchange.Net

* Version 2.0.4 - 26 Aug 2021
    * Added SequenceNumber to order/trade socket updates
    * Changed all clientOrderId parameters to uint

* Version 2.0.3 - 24 Aug 2021
    * Updated CryptoExchange.Net, improving websocket and SymbolOrderBook performance
    * Fix for order book subscription

* Version 2.0.2 - 18 Aug 2021
    * Added GetSystemStatusAsync endpoint
    * Added SubscribeToSystemStatusUpdatesAsync subscription
    * Added Place/Cancel order requests on socket client
    * Fixed multiple symbols ticker subscriptions

* Version 2.0.1 - 13 Aug 2021
    * Fix for OperationCancelledException being thrown when closing a socket from a .net framework project
    * Fixed deserialization issue in KrakenTradeVolume

* Version 2.0.0 - 12 Aug 2021
	* Release version with new CryptoExchange.Net version 4.0.0
		* Multiple changes regarding logging and socket connection, see [CryptoExchange.Net release notes](https://github.com/JKorf/CryptoExchange.Net#release-notes)
		
* Version 2.0.0-beta3 - 09 Aug 2021
    * Fixed deserialization error in GetTradeVolumeAsync
    * Fixed error message invalid symbol
    * Now uses IEnumerable<T> instead of params T in various methods
    * Renamed GetRecentTradesAsync to GetTradeHistoryAsync
    * Renamed GetTradeHistoryAsync to GetUserTradesAsync
    * Renamed GetTradesAsync to GetUserTradeDetailsAsync
    * Renamed WithdrawFundsAsync to WithdrawAsync

* Version 2.0.0-beta2 - 26 Jul 2021
    * Updated CryptoExchange.Net

* Version 2.0.0-beta1 - 09 Jul 2021
    * Fixed unsubscribing user streams
    * Added Async postfix for async methods
    * Updated CryptoExchange.Net

* Version 1.4.3 - 04 mei 2021
    * Added GetAvailableBalances endpoint
	
* Version 1.4.2 - 28 apr 2021
    * Updated CryptoExchange.Net

* Version 1.4.1 - 19 apr 2021
	* Fixed ICommonSymbol.CommonName implementation on KrakenSymbol
    * Updated CryptoExchange.Net

* Version 1.4.0 - 12 apr 2021
    * Added GetWithdrawInfo endpoint
    * Added authenticated SubscribeToOrderUpdates and SubscribeToOwnTradeUpdates subscriptions on socket client

* Version 1.3.2 - 30 mrt 2021
    * Updated CryptoExchange.Net

* Version 1.3.1 - 01 mrt 2021
    * Added Nuget SymbolPackage

* Version 1.3.0 - 01 mrt 2021
    * Added config for deterministic build
    * Updated CryptoExchange.Net

* Version 1.2.3 - 22 jan 2021
    * Updated for ICommonKline

* Version 1.2.2 - 14 jan 2021
    * Updated CryptoExchange.Net

* Version 1.2.1 - 22 dec 2020
    * Added missing SetDefaultOptions for socket client
    * Fixed symbol name check for ETH2.S/ETH

* Version 1.2.0 - 21 dec 2020
    * Update CryptoExchange.Net
    * Updated to latest IExchangeClient

* Version 1.1.9 - 11 dec 2020
    * Updated CryptoExchange.Net
    * Implemented IExchangeClient

* Version 1.1.8 - 19 nov 2020
    * Updated CryptoExchange.Net

* Version 1.1.7 - 09 nov 2020
    * Fix string values for order book checksum

* Version 1.1.6 - 09 nov 2020
    * Fixed symbol validation
    * Added string value properties to orderbook for checksum validation

* Version 1.1.5 - 08 Oct 2020
    * Fixed withdraw endpoint

* Version 1.1.4 - 08 Oct 2020
    * Added withdraw method
    * Fix close timestamp orders
    * Added OrderMin property on pair
    * Updated CryptoExchange.Net

* Version 1.1.3 - 28 Aug 2020
    * Updated CryptoExchange.Net

* Version 1.1.2 - 12 Aug 2020
    * Updated CryptoExchange.Net

* Version 1.1.1 - 21 Jul 2020
    * Added checksum validation for KrakenSymbolOrderBook

* Version 1.1.0 - 20 Jul 2020
    * Added two-factor authentication support

* Version 1.0.8 - 21 Jun 2020
    * Updated CryptoExchange

* Version 1.0.7 - 16 Jun 2020
    * Fix for KrakenSymbolOrderBook

* Version 1.0.6 - 07 Jun 2020
	* Updated CryptoExchange.Net to fix order book desync

* Version 1.0.5 - 03 Mar 2020
    * Fixed since parameter in GetRecentTrades endpoint

* Version 1.0.4 - 27 Jan 2020
    * Updated CryptoExchange.Net

* Version 1.0.3 - 12 Nov 2019
    * Added TradingAgreement parameter for placing orders for German accounts

* Version 1.0.2 - 24 Oct 2019
	* Fixed order deserialization

* Version 1.0.1 - 23 Oct 2019
	* Fixed validation length symbols
	
* Version 1.0.0 - 23 Oct 2019
	* See CryptoExchange.Net 3.0 release notes
	* Added input validation
	* Added CancellationToken support to all requests
	* Now using IEnumerable<> for collections
	* Renamed Market -> Symbol
	* Renamed GetAccountBalance -> GetBalances

* Version 0.0.4 - 15 Oct 2019
    * Fixed placing orders
    * Fixed possible missmatch in stream subscriptions

* Version 0.0.3 - 24 Sep 2019
    * Added missing order type, added missing ledger transfer types

* Version 0.0.2 - 10 Sep 2019
    * Added missing SetDefaultOptions and SetApiCredentials methods

* Version 0.0.1 - 29 Aug 2019
	* Initial release