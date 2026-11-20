# Kraken.Net examples

Each file is an independent console program. Install `KrakenExchange.Net` in a console project, then copy one program to `Program.cs`.

| Program | Workflow |
|---|---|
| [01-spot-quickstart.cs](01-spot-quickstart.cs) | Public ticker, Spot credentials/balances, order validation, and order query |
| [02-futures.cs](02-futures.cs) | Futures data, credentials/balances, leverage, order submission, and positions |
| [03-websocket.cs](03-websocket.cs) | Spot/Futures subscriptions and cleanup |
| [04-multi-exchange.cs](04-multi-exchange.cs) | Shared API V2 ticker capabilities, symbol catalog, and subscription |
| [05-error-handling.cs](05-error-handling.cs) | Result checks, symbol mapping, and bounded public-read retries |

Replace credential placeholders only for the intended account operations. The Futures program changes leverage and submits a live order when executed. The Spot program uses validation-only placement and later queries a placeholder live order id. Review [operational considerations](../../.claude/skills/kraken-net/references/safety.md) before executing account operations.

The repository's `AiExampleCompileTests` compiles all five programs without executing exchange calls. See the [consumer skill](../../.claude/skills/kraken-net/SKILL.md) and [usage snippets](../../.claude/skills/kraken-net/references/usage.md) for focused patterns.
