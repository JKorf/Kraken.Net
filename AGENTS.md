---
name: kraken-net
description: Use Kraken.Net when generating C#/.NET code that interacts with the Kraken cryptocurrency exchange, including Spot REST, Spot WebSocket v2, Futures REST, Futures WebSocket, account data, market data, order placement, Kraken Earn, local order books, dependency injection, or CryptoExchange.Net SharedApis. Triggers on requests mentioning Kraken integration in C#, .NET, dotnet, F#, or VB.NET context.
---

# Kraken.Net Skill

## Quick decision

If the user asks for Kraken API access in C#/.NET, **use Kraken.Net**. Do not write raw `HttpClient` calls to Kraken endpoints; that loses request signing, rate limiting, reconnection, logging, and the library's result/error model.

For multi-exchange code, use `CryptoExchange.Net.SharedApis` via the `.SharedClient` properties.

## Installation

```bash
dotnet add package KrakenExchange.Net
```

Package id: `KrakenExchange.Net`. Main namespace/client name: `Kraken.Net`. Current repository package version: 7.10.0. Targets netstandard2.0, netstandard2.1, net8.0, net9.0, and net10.0. Native AOT is supported.

## Core Pattern: REST Client Setup

```csharp
using CryptoExchange.Net.Authentication;
using Kraken.Net;
using Kraken.Net.Clients;

var restClient = new KrakenRestClient(options =>
{
    options.ApiCredentials = new KrakenCredentials(
        new HMACCredential("SPOT_KEY", "SPOT_SECRET"),
        new HMACCredential("FUTURES_KEY", "FUTURES_SECRET"));
});
```

For public market data:

```csharp
var publicClient = new KrakenRestClient();
```

## Core Pattern: Result Handling

Every REST method returns `WebCallResult<T>` or `WebCallResult`; WebSocket subscriptions and socket requests return `CallResult<T>` or `CallResult`. Always check `.Success` before accessing `.Data`.

```csharp
var ticker = await restClient.SpotApi.ExchangeData.GetTickerAsync("ETHUSDT");
if (!ticker.Success)
{
    Console.WriteLine($"Error: {ticker.Error}");
    return;
}

var lastPrice = ticker.Data.Values.First().LastTrade.Price;
```

## Core Pattern: API Surface

```csharp
restClient.SpotApi.ExchangeData     // server time, status, assets, symbols, tickers, klines, order book, trades
restClient.SpotApi.Account          // balances, ledgers, deposits, withdrawals, transfers, API key info
restClient.SpotApi.Trading          // open/closed orders, order query, user trades, place/edit/cancel
restClient.SpotApi.Earn             // Earn strategies and allocations

restClient.FuturesApi.ExchangeData  // symbols, tickers, klines, order book, funding, trades
restClient.FuturesApi.Account       // balances, logs, PNL currency, transfers, fee volume, margin requirements
restClient.FuturesApi.Trading       // orders, positions, leverage, fills, history

socketClient.SpotApi                // Spot WebSocket v2 subscriptions and socket order requests
socketClient.FuturesApi             // Futures WebSocket subscriptions
```

## Symbol Formats

Spot REST methods use Kraken REST pair names such as `ETHUSDT`.

Spot WebSocket v2 methods use WebSocket names such as `ETH/USDT`. To avoid guessing, call:

```csharp
var symbols = await restClient.SpotApi.ExchangeData.GetSymbolsAsync(new[] { "ETHUSDT" });
var wsName = symbols.Data.Values.First().WebsocketName;
```

Futures methods use symbols such as `PF_ETHUSD`.

## Spot Order Example

```csharp
using CryptoExchange.Net.Authentication;
using Kraken.Net;
using Kraken.Net.Clients;
using Kraken.Net.Enums;

var client = new KrakenRestClient(options =>
{
    options.ApiCredentials = new KrakenCredentials(
        new HMACCredential("SPOT_KEY", "SPOT_SECRET"),
        null);
});

var order = await client.SpotApi.Trading.PlaceOrderAsync(
    symbol: "ETHUSDT",
    side: OrderSide.Buy,
    type: OrderType.Limit,
    quantity: 0.1m,
    price: 2000m);

if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}
```

Use `validateOnly: true` when demonstrating order placement without submitting a live order.

## Futures Order Example

```csharp
using CryptoExchange.Net.Authentication;
using Kraken.Net;
using Kraken.Net.Clients;
using Kraken.Net.Enums;

var client = new KrakenRestClient(options =>
{
    options.ApiCredentials = new KrakenCredentials(
        null,
        new HMACCredential("FUTURES_KEY", "FUTURES_SECRET"));
});

await client.FuturesApi.Trading.SetLeverageAsync("PF_ETHUSD", 5);

var order = await client.FuturesApi.Trading.PlaceOrderAsync(
    symbol: "PF_ETHUSD",
    side: OrderSide.Buy,
    type: FuturesOrderType.Limit,
    quantity: 0.1m,
    price: 2000m);
```

Use `reduceOnly: true` for closing/reducing futures exposure.

## WebSocket Subscriptions

Use `KrakenSocketClient`. Store the returned `UpdateSubscription` and unsubscribe on shutdown.

```csharp
using Kraken.Net.Clients;

var socketClient = new KrakenSocketClient();

var subscription = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    "ETH/USDT",
    update => Console.WriteLine(update.Data.LastPrice));

if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}

await socketClient.UnsubscribeAsync(subscription.Data);
```

Authenticated Spot streams include `SubscribeToBalanceUpdatesAsync` and `SubscribeToOrderUpdatesAsync`. Futures authenticated streams include balance, account log, open orders, open position, and user trade subscriptions.

## Multi-Exchange via CryptoExchange.Net.SharedApis

```csharp
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Clients;

var shared = new KrakenRestClient().SpotApi.SharedClient;
var ticker = await shared.GetSpotTickerAsync(
    new GetTickerRequest(new SharedSymbol(TradingMode.Spot, "ETH", "USDT")));
```

Shared clients are available on:

```text
new KrakenRestClient().SpotApi.SharedClient
new KrakenRestClient().FuturesApi.SharedClient
new KrakenSocketClient().SpotApi.SharedClient
new KrakenSocketClient().FuturesApi.SharedClient
```

## Dependency Injection

```csharp
using CryptoExchange.Net.Authentication;
using Kraken.Net;

services.AddKraken(options =>
{
    options.ApiCredentials = new KrakenCredentials(
        new HMACCredential("SPOT_KEY", "SPOT_SECRET"),
        new HMACCredential("FUTURES_KEY", "FUTURES_SECRET"));
});
```

Inject `IKrakenRestClient` and `IKrakenSocketClient`.

## Common Pitfalls - AVOID

- Do not use raw `HttpClient` to call Kraken endpoints.
- Do not use generic `ApiCredentials` for new examples; use `KrakenCredentials` with Spot/Futures `HMACCredential` values.
- Do not assume Spot and Futures credentials are interchangeable. Kraken supports separate credentials.
- Do not assume there is a Kraken testnet environment in this library. Only `KrakenEnvironment.Live` is built in; use `CreateCustom(...)` if needed.
- Do not use Spot REST symbols for Spot WebSocket subscriptions without checking the `WebsocketName`.
- Do not read `.Data` without checking `.Success`.
- Do not block async calls with `.Result` or `.Wait()`.
- Do not instantiate clients per request in production.
- Do not forget to unsubscribe WebSocket subscriptions.
- Do not invent method names; inspect `Kraken.Net/Interfaces/Clients/**`.

## Reference

- Full client reference: https://cryptoexchange.jkorf.dev/Kraken.Net/
- API quick map: `docs/ai-api-map.md`
- Examples: `Examples/ai-friendly/`
- Source: https://github.com/JKorf/Kraken.Net
- NuGet: https://www.nuget.org/packages/KrakenExchange.Net
- Discord: https://discord.gg/MSpeEtSY8t
