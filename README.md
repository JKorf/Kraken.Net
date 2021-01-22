# ![Icon](https://github.com/JKorf/Kraken.Net/blob/master/Kraken.Net/Icon/icon.png?raw=true) Kraken.Net 

![Build status](https://travis-ci.org/JKorf/Kraken.Net.svg?branch=master)

A .Net wrapper for the Kraken API as described on [Kraken](https://www.kraken.com/features/api), including all features the API provides using clear and readable objects.

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/JKorf/Kraken.Net/issues)**

## CryptoExchange.Net
Implementation is build upon the CryptoExchange.Net library, make sure to also check out the documentation on that: [docs](https://github.com/JKorf/CryptoExchange.Net)

Other CryptoExchange.Net implementations:
<table>
<tr>
<td><a href="https://github.com/JKorf/Bittrex.Net"><img src="https://github.com/JKorf/Bittrex.Net/blob/master/Bittrex.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Bittrex.Net">Bittrex</a>
</td>
<td><a href="https://github.com/JKorf/Bitfinex.Net"><img src="https://github.com/JKorf/Bitfinex.Net/blob/master/Bitfinex.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Bitfinex.Net">Bitfinex</a>
</td>
<td><a href="https://github.com/JKorf/Binance.Net"><img src="https://github.com/JKorf/Binance.Net/blob/master/Binance.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Binance.Net">Binance</a>
</td>
<td><a href="https://github.com/JKorf/CoinEx.Net"><img src="https://github.com/JKorf/CoinEx.Net/blob/master/CoinEx.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/CoinEx.Net">CoinEx</a>
</td>
<td><a href="https://github.com/JKorf/Huobi.Net"><img src="https://github.com/JKorf/Huobi.Net/blob/master/Huobi.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Huobi.Net">Huobi</a>
</td>
<td><a href="https://github.com/JKorf/Kucoin.Net"><img src="https://github.com/JKorf/Kucoin.Net/blob/master/Kucoin.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/JKorf/Kucoin.Net">Kucoin</a>
</td>
</tr>
</table>

Implementations from third parties:
<table>
<tr>
<td><a href="https://github.com/Zaliro/Switcheo.Net"><img src="https://github.com/Zaliro/Switcheo.Net/blob/master/Resources/switcheo-coin.png?raw=true"></a>
<br />
<a href="https://github.com/Zaliro/Switcheo.Net">Switcheo</a>
</td>
<td><a href="https://github.com/ridicoulous/LiquidQuoine.Net"><img src="https://github.com/ridicoulous/LiquidQuoine.Net/blob/master/Resources/icon.png?raw=true"></a>
<br />
<a href="https://github.com/ridicoulous/LiquidQuoine.Net">Liquid</a>
</td>
<td><a href="https://github.com/ridicoulous/Bitmex.Net"><img src="https://github.com/ridicoulous/Bitmex.Net/blob/master/Bitmex.Net/Icon/icon.png"></a>
<br />
<a href="https://github.com/ridicoulous/Bitmex.Net">Bitmex</a>
</td>
<td><a href="https://github.com/intelligences/HitBTC.Net"><img src="https://github.com/intelligences/HitBTC.Net/blob/master/src/HitBTC.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/intelligences/HitBTC.Net">HitBTC</a>
</td>
<td><a href="https://github.com/EricGarnier/LiveCoin.Net"><img src="https://github.com/EricGarnier/LiveCoin.Net/blob/master/LiveCoin.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/EricGarnier/LiveCoin.Net">LiveCoin</a>
</td>
<td><a href="https://github.com/burakoner/OKEx.Net"><img src="https://github.com/burakoner/OKEx.Net/blob/master/Okex.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/burakoner/OKEx.Net">OKEx</a>
</td>
<td><a href="https://github.com/burakoner/Chiliz.Net"><img src="https://github.com/burakoner/Chiliz.Net/blob/master/Chiliz.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/burakoner/Chiliz.Net">Chiliz</a>
</td>
<td><a href="https://github.com/burakoner/BtcTurk.Net"><img src="https://github.com/burakoner/BtcTurk.Net/blob/master/BtcTurk.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/burakoner/BtcTurk.Net">BtcTurk</a>
</td>
<td><a href="https://github.com/burakoner/Thodex.Net"><img src="https://github.com/burakoner/Thodex.Net/blob/main/Thodex.Net/Icon/icon.png?raw=true"></a>
<br />
<a href="https://github.com/burakoner/Thodex.Net">Thodex</a>
</td>
</tr>
</table>


## Donations
Donations are greatly appreciated and a motivation to keep improving.

**Btc**:  12KwZk3r2Y3JZ2uMULcjqqBvXmpDwjhhQS  
**Eth**:  0x069176ca1a4b1d6e0b7901a6bc0dbf3bb0bf5cc2  
**Nano**: xrb_1ocs3hbp561ef76eoctjwg85w5ugr8wgimkj8mfhoyqbx4s1pbc74zggw7gs  


## Installation
![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg)  ![Nuget downloads](https://img.shields.io/nuget/dt/KrakenExchange.Net.svg)
Available on [Nuget](https://www.nuget.org/packages/KrakenExchange.Net/).
```
pm> Install-Package KrakenExchange.Net
```
To get started with Kraken.Net first you will need to get the library itself. The easiest way to do this is to install the package into your project using [NuGet](https://www.nuget.org/packages/KrakenExchange.Net/). Using Visual Studio this can be done in two ways.

### Using the package manager
In Visual Studio right click on your solution and select 'Manage NuGet Packages for solution...'. A screen will appear which initially shows the currently installed packages. In the top bit select 'Browse'. This will let you download packages from the NuGet server. In the search box type 'KrakenExchange.Net' and hit enter. The KrakenExchange.Net package should come up in the results. After selecting the package you can then on the right hand side select in which projects in your solution the package should install. After you've selected all project you wish to install and use Kraken.Net in hit 'Install' and the package will be downloaded and added to you projects.

### Using the package manager console
In Visual Studio in the top menu select 'Tools' -> 'NuGet Package Manager' -> 'Package Manager Console'. This should open up a command line interface. On top of the interface there is a dropdown menu where you can select the Default Project. This is the project that Kraken.Net will be installed in. After selecting the correct project type `Install-Package KrakenExchange.Net` in the command line interface. This should install the latest version of the package in your project.

After doing either of above steps you should now be ready to actually start using Kraken.Net.
## Getting started
After installing it's time to actually use it. To get started you have to add the Kraken.Net namespace: `using Kraken.Net;`.

Kraken.Net provides two clients to interact with the Kraken API. The `KrakenClient` provides all rest API calls. The  `KrakenSocketClient`  provides functions to interact with the websocket provided by the Kraken API. Both clients are disposable and as such can be used in a `using` statement.

## Release notes
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
