# AI-Friendly Examples

These examples are optimized for AI coding assistants and quick onboarding. Each file is:

- **Compilable** - drop into a console project with `dotnet add package KrakenExchange.Net` and it builds.
- **Self-contained** - single file, no external setup, no shared helpers.
- **Heavily commented** - explains why each line is there, not just what it does.
- **Idiomatic** - follows current Kraken.Net 7.x patterns.

## Files

| File | What it shows |
|---|---|
| `01-spot-quickstart.cs` | Client setup, public ticker, authenticated balances, validate-only limit order, order lookup |
| `02-futures.cs` | Futures market data, balances, leverage, order placement shape, open positions |
| `03-websocket.cs` | Spot and Futures WebSocket subscriptions with proper teardown |
| `04-multi-exchange.cs` | `CryptoExchange.Net.SharedApis` pattern and shared capability discovery |
| `05-error-handling.cs` | `HttpResult`, `WebSocketResult`, `QueryResult`, and `ExchangeCallResult` handling patterns |

## Running

```bash
dotnet new console -n MyKrakenApp
cd MyKrakenApp
dotnet add package KrakenExchange.Net
# Copy the example .cs file content into Program.cs
# Replace API key placeholders for authenticated examples
dotnet run
```
