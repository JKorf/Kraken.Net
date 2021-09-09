# Kraken.Net
![Build status](https://travis-ci.com/JKorf/Kraken.Net.svg?branch=master) ![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg)  ![Nuget downloads](https://img.shields.io/nuget/dt/KrakenExchange.Net.svg)

Kraken.Net is a wrapper around the Kraken API as described on [Kraken](https://www.kraken.com/features/api), including all features the API provides using clear and readable objects, both for the REST  as the websocket API's.

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/JKorf/Kraken.Net/issues)**

## CryptoExchange.Net
This library is build upon the CryptoExchange.Net library, make sure to check out the documentation on that for basic usage: [docs](https://github.com/JKorf/CryptoExchange.Net)

## Donations
I develop and maintain this package on my own for free in my spare time. Donations are greatly appreciated. If you prefer to donate any other currency please contact me.

**Btc**:  12KwZk3r2Y3JZ2uMULcjqqBvXmpDwjhhQS  
**Eth**:  0x069176ca1a4b1d6e0b7901a6bc0dbf3bb0bf5cc2  
**Nano**: xrb_1ocs3hbp561ef76eoctjwg85w5ugr8wgimkj8mfhoyqbx4s1pbc74zggw7gs  

## Discord
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). For discussion and/or questions around the CryptoExchange.Net and implementation libraries, feel free to join.

## Getting started
Make sure you have installed the Kraken.Net [Nuget](https://www.nuget.org/packages/KrakenExchange.Net/) package and add `using Kraken.Net` to your usings.  You now have access to 2 clients:  
**KrakenClient**  
The client to interact with the Kraken REST API. Getting prices:
````C#
var client = new KrakenClient(new KrakenClientOptions(){
 // Specify options for the client
});
var callResult = await client.GetTickersAsync();
// Make sure to check if the call was successful
if(!callResult.Success)
{
  // Call failed, check callResult.Error for more info
}
else
{
  // Call succeeded, callResult.Data will have the resulting data
}
````

Placing an order:
````C#
var client = new KrakenClient(new KrakenClientOptions(){
 // Specify options for the client
 ApiCredentials = new ApiCredentials("Key", "Secret")
});
var callResult = await client.PlaceOrderAsync("BTCUSDT", OrderSide.Buy, OrderType.Limit, 10, price: 50);
// Make sure to check if the call was successful
if(!callResult.Success)
{
  // Call failed, check callResult.Error for more info
}
else
{
  // Call succeeded, callResult.Data will have the resulting data
}
````

**KrakenSocketClient**  
The client to interact with the Kraken websocket API. Basic usage:
````C#
var client = new KrakenSocketClient(new KrakenSocketClientOptions()
{
  // Specify options for the client
});
var subscribeResult = client.SubscribeToTickerUpdatesAsync("ETHXBT", data => {
  // Handle data when it is received
});
// Make sure to check if the subscritpion was successful
if(!subscribeResult.Success)
{
  // Subscription failed, check callResult.Error for more info
}
else
{
  // Subscription succeeded, the handler will start receiving data when it is available
}
````

## Client options
For the basic client options see also the CryptoExchange.Net [docs](https://github.com/JKorf/CryptoExchange.Net#client-options). The here listed options are the options specific for Kraken.Net.  
**KrakenClientOptions**  
| Property | Description | Default |
| ----------- | ----------- | ---------|
|`StaticTwoFactorAuthenticationPassword`|The static password to be sent as `otp` parameter in requests |`null`

**KrakenSocketClientOptions**  
| Property | Description | Default |
| ----------- | ----------- | ---------|
|`AuthBaseAddress`|The base address for authenticated subscriptions|`wss://ws-auth.kraken.com/`

## Release notes
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