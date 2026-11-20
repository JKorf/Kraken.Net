# Kraken.Net usage

## Imports

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.SharedApis;
using Kraken.Net;
using Kraken.Net.Clients;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients;
using Microsoft.Extensions.DependencyInjection;
```

## Public market data and results

```csharp
using var client = new KrakenRestClient();
var ticker = await client.SpotApi.ExchangeData.GetTickerAsync("ETHUSDT");
if (!ticker.Success)
{
    Console.WriteLine(ticker.Error);
    return;
}
foreach (var item in ticker.Data)
    Console.WriteLine($"{item.Key}: {item.Value.LastTrade.Price}");
```

## Separate Spot and Futures credentials

Spot-only configuration and a balance read:

```csharp
var key = Environment.GetEnvironmentVariable("KRAKEN_SPOT_KEY")
    ?? throw new InvalidOperationException("Spot key is required");
var secret = Environment.GetEnvironmentVariable("KRAKEN_SPOT_SECRET")
    ?? throw new InvalidOperationException("Spot secret is required");
using var client = new KrakenRestClient(options =>
{
    options.ApiCredentials = new KrakenCredentials(new HMACCredential(key, secret), null);
});
var balances = await client.SpotApi.Account.GetBalancesAsync();
if (!balances.Success)
{
    Console.WriteLine(balances.Error);
    return;
}
Console.WriteLine(balances.Data.Count);
```

Futures-only configuration uses the second slot:

```csharp
var key = Environment.GetEnvironmentVariable("KRAKEN_FUTURES_KEY")
    ?? throw new InvalidOperationException("Futures key is required");
var secret = Environment.GetEnvironmentVariable("KRAKEN_FUTURES_SECRET")
    ?? throw new InvalidOperationException("Futures secret is required");
using var client = new KrakenRestClient(options =>
{
    options.ApiCredentials = new KrakenCredentials(null, new HMACCredential(key, secret));
});
var positions = await client.FuturesApi.Trading.GetOpenPositionsAsync();
if (!positions.Success)
{
    Console.WriteLine(positions.Error);
    return;
}
Console.WriteLine(positions.Data.Length);
```

Supply both slots when an application needs both products. `new KrakenCredentials().WithSpot(key, secret).WithFutures(futuresKey, futuresSecret)` is also supported.

## Spot order validation

This snippet assumes the Spot-authenticated client above. It validates parameters without submitting a live order. Read [execution considerations](safety.md#orders-amendments-and-cancellation) when adapting it for trading.

```csharp
var order = await client.SpotApi.Trading.PlaceOrderAsync(
    symbol: "ETHUSDT", side: OrderSide.Buy, type: OrderType.Limit,
    quantity: 0.01m, price: 1000m, validateOnly: true);
if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}
Console.WriteLine("Order parameters validated");
```

## Futures order submission

This changes account state when executed and assumes the Futures-authenticated client above. Quantity is instrument size in contracts; choose precision and size from metadata. Futures placement has no validation-only switch.

```csharp
var order = await client.FuturesApi.Trading.PlaceOrderAsync(
    symbol: "PF_ETHUSD", side: OrderSide.Buy, type: FuturesOrderType.Limit,
    quantity: 0.1m, price: 1000m);
if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}
Console.WriteLine($"{order.Data.OrderId}: {order.Data.Status}");
```

## Spot WebSocket lifecycle and symbol mapping

```csharp
using var client = new KrakenRestClient();
var symbols = await client.SpotApi.ExchangeData.GetSymbolsAsync(new[] { "ETHUSDT" });
if (!symbols.Success)
{
    Console.WriteLine(symbols.Error);
    return;
}
if (symbols.Data.Count == 0)
    return;
var wsSymbol = symbols.Data.Values.First().WebsocketName;
using var socketClient = new KrakenSocketClient();
var subscription = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    wsSymbol, update => Console.WriteLine(update.Data.LastPrice));
if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}
try
{
    await Task.Delay(TimeSpan.FromSeconds(30));
}
finally
{
    await socketClient.UnsubscribeAsync(subscription.Data);
}
```

Futures subscriptions use native instrument identifiers directly, for example `socketClient.FuturesApi.SubscribeToTickerUpdatesAsync("PF_ETHUSD", handler)`. Private streams use the corresponding credential slot.

## Spot WebSocket order request

Socket order requests return `QueryResult<T>` rather than subscription handles. This example also uses validation-only placement:

```csharp
var key = Environment.GetEnvironmentVariable("KRAKEN_SPOT_KEY")
    ?? throw new InvalidOperationException("Spot key is required");
var secret = Environment.GetEnvironmentVariable("KRAKEN_SPOT_SECRET")
    ?? throw new InvalidOperationException("Spot secret is required");
using var socketClient = new KrakenSocketClient(options =>
{
    options.ApiCredentials = new KrakenCredentials(new HMACCredential(key, secret), null);
});
var result = await socketClient.SpotApi.PlaceOrderAsync(
    symbol: "ETH/USDT", side: OrderSide.Buy, type: OrderType.Limit,
    quantity: 0.01m, limitPrice: 1000m, validateOnly: true);
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine("Socket order parameters validated");
```

## Shared API V2

```csharp
IGetTickerRest ticker = client.SpotApi.SharedApi;
var result = await ticker.GetTickerAsync(new GetTickerRequest(
    new SharedSymbol(TradingMode.Spot, "ETH", "USDT")));
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.LastPrice);
```

Load symbols successfully before reading the catalog:

```csharp
IGetSpotSymbolsRest symbolsClient = client.SpotApi.SharedApi;
var result = await symbolsClient.GetSpotSymbolsAsync(new GetSymbolsRequest());
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
foreach (var symbol in result.Data.Take(5))
    Console.WriteLine($"{symbol.Name}: {symbol.BaseAsset}/{symbol.QuoteAsset}");
var catalog = symbolsClient.SpotSymbolCatalog;
```

## Dependency injection and dynamic lookup

```csharp
var services = new ServiceCollection();
services.AddKraken();
using var provider = services.BuildServiceProvider();
var aggregate = provider.GetRequiredService<IKrakenSharedApiClient>();
var match = aggregate.GetCapability(
    SharedCapabilities.Tickers.GetTicker.Rest, tradingMode: TradingMode.Spot);
if (match is null)
    return;
var result = await match.Capability.GetTickerAsync(new GetTickerRequest(
    new SharedSymbol(TradingMode.Spot, "ETH", "USDT")));
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.LastPrice);
```

Configure `services.AddKraken(options => { options.ApiCredentials = credentials; })` for authenticated consumers, or set `options.Rest.ApiCredentials` and `options.Socket.ApiCredentials` separately. Inject `IKrakenRestClient`, `IKrakenSocketClient`, the aggregate, or a registered narrow capability. See [API routing](api-surfaces.md#shared-api-v2) for selection when multiple exchanges are registered.

## Complete programs (checkout-only)

[Examples](../../../../Examples/ai-friendly/README.md) contains independently compilable Spot, Futures, WebSocket, Shared API V2, and public-read retry programs. These links require a checkout; the snippets above work with the installed package.
