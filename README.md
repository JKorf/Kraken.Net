# ![.Kraken.Net](https://github.com/JKorf/Kraken.Net/blob/master/Kraken.Net/Icon/icon.png?raw=true) Kraken.Net

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/Kraken.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/Kraken.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/Kraken.Net?style=for-the-badge)

Kraken.Net is a strongly typed client library for accessing the [Kraken REST and Websocket API](https://www.kraken.com/features/api). All data is mapped to readable models and enum values. Additional features include an implementation for maintaining a client side order book, easy integration with other exchange client libraries and more.

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

## Get the library
[![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg?style=for-the-badge)](https://www.nuget.org/packages/KrakenExchange.Net)  [![Nuget downloads](https://img.shields.io/nuget/dt/KrakenExchange.Net.svg?style=for-the-badge)](https://www.nuget.org/packages/KrakenExchange.Net)

	dotnet add package KrakenExchange.Net

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
	var lastPrice = update.Data.LastTrade.Price;
});
```

For information on the clients, dependency injection, response processing and more see the [documentation](https://jkorf.github.io/CryptoExchange.Net), or have a look at the examples  [here](https://github.com/JKorf/CryptoExchange.Net/tree/master/Examples).

## CryptoExchange.Net
Kraken.Net is based on the [CryptoExchange.Net](https://github.com/JKorf/CryptoExchange.Net) base library. Other exchange API implementations based on the CryptoExchange.Net base library are available and follow the same logic.

CryptoExchange.Net also allows for [easy access to different exchange API's](https://jkorf.github.io/CryptoExchange.Net#idocs_common).

|Exchange|Repository|Nuget|
|--|--|--|
|Binance|[JKorf/Binance.Net](https://github.com/JKorf/Binance.Net)|[![Nuget version](https://img.shields.io/nuget/v/Binance.net.svg?style=flat-square)](https://www.nuget.org/packages/Binance.Net)|
|Bitfinex|[JKorf/Bitfinex.Net](https://github.com/JKorf/Bitfinex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitfinex.net.svg?style=flat-square)](https://www.nuget.org/packages/Bitfinex.Net)|
|Bitget|[JKorf/Bitget.Net](https://github.com/JKorf/Bitget.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Bitget.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Bitget.Net)|
|Bybit|[JKorf/Bybit.Net](https://github.com/JKorf/Bybit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bybit.net.svg?style=flat-square)](https://www.nuget.org/packages/Bybit.Net)|
|CoinEx|[JKorf/CoinEx.Net](https://github.com/JKorf/CoinEx.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinEx.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinEx.Net)|
|CoinGecko|[JKorf/CoinGecko.Net](https://github.com/JKorf/CoinGecko.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinGecko.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinGecko.Net)|
|Huobi|[JKorf/Huobi.Net](https://github.com/JKorf/Huobi.Net)|[![Nuget version](https://img.shields.io/nuget/v/Huobi.net.svg?style=flat-square)](https://www.nuget.org/packages/Huobi.Net)|
|Kucoin|[JKorf/Kucoin.Net](https://github.com/JKorf/Kucoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kucoin.net.svg?style=flat-square)](https://www.nuget.org/packages/Kucoin.Net)|
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|

## Discord
[![Nuget version](https://img.shields.io/discord/847020490588422145?style=for-the-badge)](https://discord.gg/MSpeEtSY8t)  
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). Feel free to join for discussion and/or questions around the CryptoExchange.Net and implementation libraries.

## Supported functionality

### Spot Api
|API|Supported|Location|
|--|--:|--|
|Market Data|✓|`restClient.SpotApi.ExchangeData`|
|Account Data|✓|`restClient.SpotApi.Account` / `restClient.SpotApi.Trading`|
|Trading|✓|`restClient.SpotApi.Trading`|
|Funding|✓|`restClient.SpotApi.Account`|
|Subaccounts|X||
|Earn|X||
|Websocket Public Messages|✓|`socketClient.SpotApi`|
|Websocket Private Messages|✓|`socketClient.SpotApi`|

### Futures Api
|API|Supported|Location|
|--|--:|--|
|Account Information|✓|`restClient.FuturesApi.Account`|
|Assignment Program|X||
|Fee Schedules|✓|`restClient.FuturesApi.ExchangeData`|
|General|✓|`restClient.FuturesApi.ExchangeData`|
|Historical data|✓|`restClient.FuturesApi.Trading`|
|Historical Funding Rates|✓|`restClient.FuturesApi.ExchangeData`|
|Instrument Details|✓|`restClient.FuturesApi.ExchangeData`|
|Market Data|✓|`restClient.FuturesApi.ExchangeData`|
|Multi-Collateral|✓|`restClient.FuturesApi.Account` / `restClient.FuturesApi.Trading`|
|Order Management|✓|`restClient.FuturesApi.Trading`|
|Subaccounts|X||
|Trading settings|X||
|Transfers|✓|`restClient.FuturesApi.Account`|
|Account History|✓|`restClient.FuturesApi.Account` / `restClient.FuturesApi.Trading`|
|Market History|✓|`restClient.FuturesApi.ExchangeData`|
|Analytics|X||
|Candles|✓|`restClient.FuturesApi.ExchangeData`|
|Websocket Public Feeds|✓|`socketClient.FuturesApi`|
|Websocket Private Feeds|✓|`socketClient.FuturesApi`|

## Support the project
I develop and maintain this package on my own for free in my spare time, any support is greatly appreciated.

### Donate
Make a one time donation in a crypto currency of your choice. If you prefer to donate a currency not listed here please contact me.

**Btc**:  bc1qz0jv0my7fc60rxeupr23e75x95qmlq6489n8gh  
**Eth**:  0xcb1b63aCF9fef2755eBf4a0506250074496Ad5b7  

### Sponsor
Alternatively, sponsor me on Github using [Github Sponsors](https://github.com/sponsors/JKorf). 

## Release notes
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