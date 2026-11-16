---
name: kraken-net
description: Generate and troubleshoot C#/.NET code consuming Kraken.Net for Spot and Futures market data, accounts, trading, Earn, WebSocket v2 requests and subscriptions, and Shared API V2 integrations.
---

# Kraken.Net

Install the consumer package (the package ID differs from the namespace):

```sh
dotnet add package KrakenExchange.Net
```

Use `KrakenRestClient` with `SpotApi.{ExchangeData|Account|Trading|Earn}` or `FuturesApi.{ExchangeData|Account|Trading}`. `KrakenSocketClient.SpotApi` exposes WebSocket v2 subscriptions and order requests directly; `FuturesApi` exposes subscriptions. Reuse and dispose clients, await calls, and retain successful subscriptions for unsubscribe cleanup.

Use `KrakenCredentials(spotCredential, futuresCredential)` with separate `HMACCredential` slots from `CryptoExchange.Net.Authentication`; pass null for an unused product. Avoid the obsolete string constructor, which applies the same key to both products. Public market data needs no credentials. The library obtains Spot WebSocket tokens internally for private streams and socket order requests.

Spot REST examples use `ETHUSDT`; Spot WebSocket v2 uses `ETH/USDT`. Resolve `KrakenSymbol.WebsocketName` from REST symbol metadata when converting. Futures use instrument names such as `PF_ETHUSD`; use metadata to select the contract and quantity representation.

Check `Success` before `Data`: REST returns `HttpResult`, socket requests `QueryResult`, subscriptions `WebSocketResult<UpdateSubscription>`, and some Shared helpers `ExchangeCallResult`, with generic variants where applicable. Check per-item results for batches.

Use `.SharedApi` and narrow V2 capabilities for portable operations. `IKrakenSharedApiClient` groups the four supported surfaces and supports dynamic lookup. Use native APIs for Kraken Earn, 2FA parameters, wallet transfers, and other exchange-specific workflows. Shared Futures orders take quantities in contracts.

## Read when needed

- [API surfaces](references/api-surfaces.md): choose roots, endpoints, Shared capabilities, symbols, catalogs, and quantities.
- [Usage](references/usage.md): setup, separate credentials, account reads, order validation, subscriptions, Shared API V2, and DI.
- [Operational considerations](references/safety.md): credentials, 2FA/nonces, execution, leverage, Earn, movements, and uncertain submissions.

For work on the library itself, use [repository instructions](../../../AGENTS.md) (checkout-only).
