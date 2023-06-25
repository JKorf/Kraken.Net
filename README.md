# Kraken.Net
[![.NET](https://github.com/JKorf/Kraken.Net/actions/workflows/dotnet.yml/badge.svg)](https://github.com/JKorf/Kraken.Net/actions/workflows/dotnet.yml) ![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg)  ![Nuget downloads](https://img.shields.io/nuget/dt/KrakenExchange.Net.svg)

Kraken.Net is a wrapper around the Kraken API as described on [Kraken](https://www.kraken.com/features/api), including all features the API provides using clear and readable objects, both for the REST  as the websocket API's.

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/JKorf/Kraken.Net/issues)**

[Documentation](https://jkorf.github.io/Kraken.Net/)

## Support the project
I develop and maintain this package on my own for free in my spare time, any support is greatly appreciated.

### Donate
Make a one time donation in a crypto currency of your choice. If you prefer to donate a currency not listed here please contact me.

**Btc**:  12KwZk3r2Y3JZ2uMULcjqqBvXmpDwjhhQS  
**Eth**:  0x069176ca1a4b1d6e0b7901a6bc0dbf3bb0bf5cc2  
**Nano**: xrb_1ocs3hbp561ef76eoctjwg85w5ugr8wgimkj8mfhoyqbx4s1pbc74zggw7gs  

### Sponsor
Alternatively, sponsor me on Github using [Github Sponsors](https://github.com/sponsors/JKorf). 

## Discord
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). For discussion and/or questions around the CryptoExchange.Net and implementation libraries, feel free to join.

## Release notes
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