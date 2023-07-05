---
title: IKrakenRestClientSpotApiAccount
has_children: false
parent: IKrakenRestClientSpotApi
grand_parent: Rest API documentation
---
*[generated documentation]*  
`KrakenRestClient > SpotApi > Account`  
*Kraken account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings*
  

***

## CancelWithdrawalAsync  

[https://docs.kraken.com/rest/#tag/User-Funding/operation/cancelWithdrawal](https://docs.kraken.com/rest/#tag/User-Funding/operation/cancelWithdrawal)  
<p>

*Cancel an active withdrawal*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.CancelWithdrawalAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<bool>> CancelWithdrawalAsync(string asset, string referenceId, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|Asset|
|referenceId|Reference id|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetAvailableBalancesAsync  

[https://docs.kraken.com/rest/#operation/getTradeBalance](https://docs.kraken.com/rest/#operation/getTradeBalance)  
<p>

*Get balances including quantity in holding*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.GetAvailableBalancesAsync();  
```  

```csharp  
Task<WebCallResult<Dictionary<string, KrakenBalanceAvailable>>> GetAvailableBalancesAsync(string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetBalancesAsync  

[https://docs.kraken.com/rest/#operation/getAccountBalance](https://docs.kraken.com/rest/#operation/getAccountBalance)  
<p>

*Get balances*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.GetBalancesAsync();  
```  

```csharp  
Task<WebCallResult<Dictionary<string, decimal>>> GetBalancesAsync(string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetDepositAddressesAsync  

[https://docs.kraken.com/rest/#operation/getDepositAddresses](https://docs.kraken.com/rest/#operation/getDepositAddresses)  
<p>

*Get deposit addresses for an asset*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.GetDepositAddressesAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenDepositAddress>>> GetDepositAddressesAsync(string asset, string depositMethod, bool generateNew, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|The asset to get the deposit address for|
|depositMethod|The method of deposit|
|generateNew|Whether to generate a new address|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetDepositMethodsAsync  

[https://docs.kraken.com/rest/#operation/getDepositMethods](https://docs.kraken.com/rest/#operation/getDepositMethods)  
<p>

*Get deposit methods*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.GetDepositMethodsAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenDepositMethod>>> GetDepositMethodsAsync(string asset, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|Asset to get methods for|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetDepositStatusAsync  

[https://docs.kraken.com/rest/#operation/getStatusRecentDeposits](https://docs.kraken.com/rest/#operation/getStatusRecentDeposits)  
<p>

*Get status of deposits*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.GetDepositStatusAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenMovementStatus>>> GetDepositStatusAsync(string? asset = default, string? depositMethod = default, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ asset|Asset to get deposit info for|
|_[Optional]_ depositMethod|The deposit method|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetLedgerInfoAsync  

[https://docs.kraken.com/rest/#operation/getLedgers](https://docs.kraken.com/rest/#operation/getLedgers)  
<p>

*Get ledger entries info*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.GetLedgerInfoAsync();  
```  

```csharp  
Task<WebCallResult<KrakenLedgerPage>> GetLedgerInfoAsync(IEnumerable<string>? assets = default, IEnumerable<LedgerEntryType>? entryTypes = default, DateTime? startTime = default, DateTime? endTime = default, int? resultOffset = default, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ assets|Filter list by asset names|
|_[Optional]_ entryTypes|Filter list by entry types|
|_[Optional]_ startTime|Return data after this time|
|_[Optional]_ endTime|Return data before this time|
|_[Optional]_ resultOffset|Offset the results by|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetLedgersEntryAsync  

[https://docs.kraken.com/rest/#operation/getLedgersInfo](https://docs.kraken.com/rest/#operation/getLedgersInfo)  
<p>

*Get info on specific ledger entries*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.GetLedgersEntryAsync();  
```  

```csharp  
Task<WebCallResult<Dictionary<string, KrakenLedgerEntry>>> GetLedgersEntryAsync(IEnumerable<string>? ledgerIds = default, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ledgerIds|The ids to get info for|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetOpenPositionsAsync  

[https://docs.kraken.com/rest/#operation/getOpenPositions](https://docs.kraken.com/rest/#operation/getOpenPositions)  
<p>

*Get a list of open positions*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.GetOpenPositionsAsync();  
```  

```csharp  
Task<WebCallResult<Dictionary<string, KrakenPosition>>> GetOpenPositionsAsync(IEnumerable<string>? transactionIds = default, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ transactionIds|Filter by transaction ids|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetTradeBalanceAsync  

<p>

*Get trade balance*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.GetTradeBalanceAsync();  
```  

```csharp  
Task<WebCallResult<KrakenTradeBalance>> GetTradeBalanceAsync(string? baseAsset = default, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ baseAsset|Base asset to get trade balance for|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetTradeVolumeAsync  

[https://docs.kraken.com/rest/#operation/getTradeVolume](https://docs.kraken.com/rest/#operation/getTradeVolume)  
<p>

*Get trade volume*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.GetTradeVolumeAsync();  
```  

```csharp  
Task<WebCallResult<KrakenTradeVolume>> GetTradeVolumeAsync(IEnumerable<string>? symbols = default, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ symbols|Symbols to get data for|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetWebsocketTokenAsync  

[https://docs.kraken.com/rest/#operation/getWebsocketsToken](https://docs.kraken.com/rest/#operation/getWebsocketsToken)  
<p>

*Get the token to connect to the private websocket streams*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.GetWebsocketTokenAsync();  
```  

```csharp  
Task<WebCallResult<KrakenWebSocketToken>> GetWebsocketTokenAsync(CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ ct||

</p>

***

## GetWithdrawalStatusAsync  

[https://docs.kraken.com/rest/#tag/User-Funding/operation/getStatusRecentWithdrawals](https://docs.kraken.com/rest/#tag/User-Funding/operation/getStatusRecentWithdrawals)  
<p>

*Get status of withdrawals*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.GetWithdrawalStatusAsync();  
```  

```csharp  
Task<WebCallResult<IEnumerable<KrakenMovementStatus>>> GetWithdrawalStatusAsync(string? asset = default, string? withdrawalMethod = default, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|_[Optional]_ asset|Filter by asset|
|_[Optional]_ withdrawalMethod|Filter by method|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## GetWithdrawInfoAsync  

[https://docs.kraken.com/rest/#operation/getWithdrawalInformation](https://docs.kraken.com/rest/#operation/getWithdrawalInformation)  
<p>

*Retrieve fee information about potential withdrawals for a particular asset, key and amount.*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.GetWithdrawInfoAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenWithdrawInfo>> GetWithdrawInfoAsync(string asset, string key, decimal quantity, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|The asset|
|key|The withdrawal key name|
|quantity|The quantity to withdraw|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## TransferAsync  

[https://docs.kraken.com/rest/#tag/User-Funding/operation/walletTransfer](https://docs.kraken.com/rest/#tag/User-Funding/operation/walletTransfer)  
<p>

*Transfer funds between wallets*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.TransferAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenReferenceId>> TransferAsync(string asset, decimal quantity, string fromWallet, string toWallet, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|Asset|
|quantity|Quantity|
|fromWallet|Source wallet|
|toWallet|Target wallet|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>

***

## WithdrawAsync  

[https://docs.kraken.com/rest/#operation/withdrawFunds](https://docs.kraken.com/rest/#operation/withdrawFunds)  
<p>

*Withdraw funds*  

```csharp  
var client = new KrakenRestClient();  
var result = await client.SpotApi.Account.WithdrawAsync(/* parameters */);  
```  

```csharp  
Task<WebCallResult<KrakenWithdraw>> WithdrawAsync(string asset, string key, decimal quantity, string? twoFactorPassword = default, CancellationToken ct = default);  
```  

|Parameter|Description|
|---|---|
|asset|The asset being withdrawn|
|key|The withdrawal key name, as set up on your account|
|quantity|The quantity to withdraw, including fees|
|_[Optional]_ twoFactorPassword|Password or authentication app code if enabled|
|_[Optional]_ ct|Cancellation token|

</p>
