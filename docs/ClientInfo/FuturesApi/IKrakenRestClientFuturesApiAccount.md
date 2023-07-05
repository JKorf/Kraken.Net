---
title: IKrakenRestClientFuturesApiAccount
has_children: false
parent: IKrakenRestClientFuturesApi
grand_parent: Rest API documentation
---
*[generated documentation]*  
`KrakenRestClient > FuturesApi > Account`  
*Kraken futures account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings*
  

***

## GetAccountLogAsync  

[https://docs.futures.kraken.com/#http-api-history-account-history-get-account-log](https://docs.futures.kraken.com/#http-api-history-account-history-get-account-log)  
<p>

*Get account log entries*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Account.GetAccountLogAsync();  
```  

```csharp  
Task<WebCallResult<KrakenAccountLogResult>> GetAccountLogAsync(DateTime? startTime = default, DateTime? endTime = default, int? fromId = default, int? toId = default, string? sort = default, string? type = default, int? limit = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ startTime|Return results after this time|
|_[Optional]_ endTime|Return results before this time|
|_[Optional]_ fromId|Return results after this id|
|_[Optional]_ toId|Return results before this id|
|_[Optional]_ sort|Sort asc or desc|
|_[Optional]_ type|Type of entry to filter by.|
|_[Optional]_ limit|Amount of entries to be returned.|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetBalancesAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-account-information-get-wallets](https://docs.futures.kraken.com/#http-api-trading-v3-api-account-information-get-wallets)  
<p>

*Get asset balances and margin info*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Account.GetBalancesAsync();  
```  

```csharp  
Task<WebCallResult<Dictionary<string, KrakenBalances>>> GetBalancesAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetFeeScheduleVolumeAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-fee-schedules-get-fee-schedule-volumes](https://docs.futures.kraken.com/#http-api-trading-v3-api-fee-schedules-get-fee-schedule-volumes)  
<p>

*Get fee schedule volume*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Account.GetFeeScheduleVolumeAsync();  
```  

```csharp  
Task<WebCallResult<Dictionary<string, decimal>>> GetFeeScheduleVolumeAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetPnlCurrencyAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-multi-collateral-get-pnl-currency-preference-for-a-market](https://docs.futures.kraken.com/#http-api-trading-v3-api-multi-collateral-get-pnl-currency-preference-for-a-market)  
<p>

*Get the PNL currency preference is used to determine which currency to pay out when realizing PNL gains.*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Account.GetPnlCurrencyAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenFuturesPnlCurrency>>> GetPnlCurrencyAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct|Cancellation token|

</p>

***

## SetPnlCurrencyAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-multi-collateral-set-pnl-currency-preference-for-a-market](https://docs.futures.kraken.com/#http-api-trading-v3-api-multi-collateral-set-pnl-currency-preference-for-a-market)  
<p>

*Set the PNL currency preference is used to determine which currency to pay out when realizing PNL gains.*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Account.SetPnlCurrencyAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult> SetPnlCurrencyAsync(string symbol, string pnlCurrency, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|symbol|Symbol to update|
|pnlCurrency|Currency to use|
|_[Optional]_ ct|Cancellation token|

</p>

***

## TransferAsync  

[https://docs.futures.kraken.com/#http-api-trading-v3-api-transfers-initiate-wallet-transfer](https://docs.futures.kraken.com/#http-api-trading-v3-api-transfers-initiate-wallet-transfer)  
<p>

*Transfer between 2 margin accounts or between margin and cash account*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.FuturesApi.Account.TransferAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult> TransferAsync(string asset, decimal quantity, string fromAccount, string toAccount, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|The asset to transfer|
|quantity|The amount to transfer|
|fromAccount|The wallet (cash or margin account) to which funds should be credited|
|toAccount|The wallet (cash or margin account) from which funds should be debited|
|_[Optional]_ ct|Cancellation token|

</p>
