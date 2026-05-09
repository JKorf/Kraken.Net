# Copilot Instructions for Kraken.Net

This repository is **Kraken.Net** - a strongly typed C#/.NET client library for the Kraken Spot and Futures REST and WebSocket APIs. It is part of the CryptoExchange.Net ecosystem.

When generating code that consumes Kraken.Net, follow these conventions:

## Use Kraken.Net, not raw HTTP

Never generate raw `HttpClient` calls to Kraken endpoints. Always use `KrakenRestClient` or `KrakenSocketClient` so request signing, rate limiting, WebSocket reconnection, and result handling stay consistent with the library.

## Client setup

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

For public market data, credentials are not required: `new KrakenRestClient()`.

## Result handling

REST methods return `WebCallResult<T>` or `WebCallResult`. WebSocket subscriptions and socket API requests return `CallResult<T>` or `CallResult`. Always check `.Success` before reading `.Data`; error details are on `.Error`.

## API structure

- `restClient.SpotApi.ExchangeData` - spot market data, assets, symbols, tickers, klines, order books, trades, spreads
- `restClient.SpotApi.Account` - balances, ledgers, deposits, withdrawals, transfers, API key info
- `restClient.SpotApi.Trading` - spot open/closed orders, order query, trade history, place/edit/cancel orders
- `restClient.SpotApi.Earn` - Kraken Earn strategies, allocations, allocate/deallocate funds
- `restClient.FuturesApi.ExchangeData` - futures symbols, tickers, klines, order books, funding rates, trades
- `restClient.FuturesApi.Account` - futures balances, account log, PNL currency, transfers, fees, margin requirements
- `restClient.FuturesApi.Trading` - futures orders, positions, leverage, user trades, order history
- `socketClient.SpotApi` - spot WebSocket v2 subscriptions and socket order requests
- `socketClient.FuturesApi` - futures WebSocket subscriptions

## Symbol formats

REST spot examples commonly use symbols such as `ETHUSDT`. Spot WebSocket v2 subscriptions use Kraken WebSocket names such as `ETH/USDT`; obtain them from `restClient.SpotApi.ExchangeData.GetSymbolsAsync()` via `KrakenSymbol.WebsocketName`.

Futures examples use symbols such as `PF_ETHUSD`.

## Cross-exchange

For code that needs to work across multiple exchanges, use `CryptoExchange.Net.SharedApis` from `.SharedClient` properties. Kraken exposes shared clients on Spot and Futures REST and socket APIs.

## Avoid

- Legacy or non-existent `KrakenClient` classes; use `KrakenRestClient` and `KrakenSocketClient`.
- Generic `ApiCredentials` for new code; use `KrakenCredentials` with separate Spot and Futures `HMACCredential` values when needed.
- Assuming Kraken has testnet environments in this library; `KrakenEnvironment.Live` is the built-in environment, with `KrakenEnvironment.CreateCustom(...)` for custom addresses.
- Reading `.Data` without checking `.Success`.
- Blocking async calls with `.Result` or `.Wait()`.
- Instantiating clients per request in production; use dependency injection and reuse clients.
- Forgetting to unsubscribe WebSocket subscriptions via the concrete socket client.

## Reference

For detailed patterns and pitfalls see `AGENTS.md`, `llms.txt`, and `llms-full.txt` in the repository root, plus `docs/ai-api-map.md` and `Examples/ai-friendly/` for compilable examples.
