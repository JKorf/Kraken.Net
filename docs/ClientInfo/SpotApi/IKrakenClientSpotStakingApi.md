---
title: IKrakenClientSpotStakingApi
has_children: true
parent: IKrakenClientSpotApi
grand_parent: Rest API documentation
---
*[generated documentation]*  
`KrakenClient > SpotApi > SpotStakingApi`  
*Kraken staking endpoints.*
  

***

## GetPendingTransactionsAsync  

[https://docs.kraken.com/rest/#operation/getStakingPendingDeposits](https://docs.kraken.com/rest/#operation/getStakingPendingDeposits)  
<p>

*Returns the list of pending staking transactions. Once completed, these transactions will be accessible*  
*through <see cref="GetRecentTransactionsAsync"/> and will not be accessible through this API.*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SpotStakingApi.GetPendingTransactionsAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenStakingTransaction>>> GetPendingTransactionsAsync(string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ twoFactorPassword||
|_[Optional]_ ct||

</p>

***

## GetRecentTransactionsAsync  

[https://docs.kraken.com/rest/#operation/getStakingTransactions](https://docs.kraken.com/rest/#operation/getStakingTransactions)  
<p>

*Returns a list of 1000 recent staking transactions from past 90 days.*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SpotStakingApi.GetRecentTransactionsAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenStakingTransaction>>> GetRecentTransactionsAsync(string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetStakableAssets  

[https://docs.kraken.com/rest/#operation/getStakingAssetInfo](https://docs.kraken.com/rest/#operation/getStakingAssetInfo)  
<p>

*Returns the list of assets that you're able to stake.*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SpotStakingApi.GetStakableAssets();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenStakingAsset>>> GetStakableAssets(string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ twoFactorPassword||
|_[Optional]_ ct||

</p>

***

## StakeAsync  

[https://docs.kraken.com/rest/#operation/stake](https://docs.kraken.com/rest/#operation/stake)  
<p>

*Stake an asset from your spot wallet.*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SpotStakingApi.StakeAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenStakeResponse>> StakeAsync(string asset, decimal amount, string method, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|Asset to stake (asset ID or altname) e.g. DOT|
|amount|Amount of the asset to stake|
|method|Name of the staking option to use as returned by <see cref="KrakenStakingAsset.Method"/>|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## UnstakeAsync  

[https://docs.kraken.com/rest/#operation/unstake](https://docs.kraken.com/rest/#operation/unstake)  
<p>

*Unstake an asset from your staking wallet*  

```csharp  
var client = new KrakenClient();  
var result = await client.SpotApi.SpotStakingApi.UnstakeAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenUnstakeResponse>> UnstakeAsync(string asset, decimal amount, string method, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|Asset to unstake (asset ID or altname). Must be a valid staking asset (e.g. XBT.M, XTZ.S, ADA.S)|
|amount|Amount of the asset to stake|
|method|Amount of the asset to stake|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>
