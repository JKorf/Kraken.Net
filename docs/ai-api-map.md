# Kraken.Net AI API Quick Map

Use this file to route common user intents to the correct Kraken.Net client member. If a method name or parameter is not listed here, inspect `Kraken.Net/Interfaces/Clients/**` before generating code.

## Client Roots

| Intent | Use |
|---|---|
| REST calls | `new KrakenRestClient()` |
| WebSocket streams and socket API requests | `new KrakenSocketClient()` |
| API key authentication | `options.ApiCredentials = new KrakenCredentials(spotCredential, futuresCredential)` |
| Spot credentials | `new HMACCredential("SPOT_KEY", "SPOT_SECRET")` as first `KrakenCredentials` argument |
| Futures credentials | `new HMACCredential("FUTURES_KEY", "FUTURES_SECRET")` as second `KrakenCredentials` argument |
| Live environment | `KrakenEnvironment.Live` |
| Custom environment | `KrakenEnvironment.CreateCustom(...)` |
| Dependency injection | `services.AddKraken(options => { ... })` |

## Spot REST

| User intent | Kraken.Net member |
|---|---|
| Get server time | `client.SpotApi.ExchangeData.GetServerTimeAsync()` |
| Get system status | `client.SpotApi.ExchangeData.GetSystemStatusAsync()` |
| Get assets | `client.SpotApi.ExchangeData.GetAssetsAsync()` |
| Get spot symbols / asset pairs | `client.SpotApi.ExchangeData.GetSymbolsAsync()` |
| Get info for one or more spot symbols | `client.SpotApi.ExchangeData.GetSymbolsAsync(new[] { "ETHUSDT" })` |
| Get latest spot ticker | `client.SpotApi.ExchangeData.GetTickerAsync("ETHUSDT")` |
| Get all or multiple spot tickers | `client.SpotApi.ExchangeData.GetTickersAsync(...)` |
| Get spot klines / candles | `client.SpotApi.ExchangeData.GetKlinesAsync("ETHUSDT", KlineInterval.OneMinute)` |
| Get spot order book | `client.SpotApi.ExchangeData.GetOrderBookAsync("ETHUSDT")` |
| Get spot trade history / recent trades | `client.SpotApi.ExchangeData.GetTradeHistoryAsync("ETHUSDT")` |
| Get recent spreads | `client.SpotApi.ExchangeData.GetRecentSpreadAsync("ETHUSDT")` |
| Get spot balances | `client.SpotApi.Account.GetBalancesAsync()` |
| Get available balances | `client.SpotApi.Account.GetAvailableBalancesAsync()` |
| Get trade balance | `client.SpotApi.Account.GetTradeBalanceAsync()` |
| Get spot open positions | `client.SpotApi.Account.GetOpenPositionsAsync()` |
| Get ledgers | `client.SpotApi.Account.GetLedgerInfoAsync(...)` |
| Get specific ledger entries | `client.SpotApi.Account.GetLedgersEntryAsync(...)` |
| Get trade volume / fees | `client.SpotApi.Account.GetTradeVolumeAsync(...)` |
| Get deposit methods | `client.SpotApi.Account.GetDepositMethodsAsync(asset)` |
| Get deposit addresses | `client.SpotApi.Account.GetDepositAddressesAsync(asset, depositMethod)` |
| Get deposit status | `client.SpotApi.Account.GetDepositStatusAsync(...)` |
| Get deposit history | `client.SpotApi.Account.GetDepositHistoryAsync(...)` |
| Get withdrawal info | `client.SpotApi.Account.GetWithdrawInfoAsync(asset, key, quantity)` |
| Withdraw asset | `client.SpotApi.Account.WithdrawAsync(asset, key, quantity, ...)` |
| Get withdrawal addresses | `client.SpotApi.Account.GetWithdrawAddressesAsync(...)` |
| Get withdrawal methods | `client.SpotApi.Account.GetWithdrawMethodsAsync(...)` |
| Get withdrawal status | `client.SpotApi.Account.GetWithdrawalStatusAsync(...)` |
| Get withdrawal history | `client.SpotApi.Account.GetWithdrawalHistoryAsync(...)` |
| Cancel withdrawal | `client.SpotApi.Account.CancelWithdrawalAsync(asset, referenceId)` |
| Transfer between wallets | `client.SpotApi.Account.TransferAsync(asset, quantity, fromWallet, toWallet)` |
| Get API key info | `client.SpotApi.Account.GetApiKeyInfoAsync()` |
| Get WebSocket token | `client.SpotApi.Account.GetWebsocketTokenAsync()` |
| Get open spot orders | `client.SpotApi.Trading.GetOpenOrdersAsync()` |
| Get closed spot orders | `client.SpotApi.Trading.GetClosedOrdersAsync(...)` |
| Query spot order | `client.SpotApi.Trading.GetOrderAsync(orderId: orderId)` |
| Query multiple spot orders | `client.SpotApi.Trading.GetOrdersAsync(orderIds: ids)` |
| Get user trades | `client.SpotApi.Trading.GetUserTradesAsync(...)` |
| Get user trade details | `client.SpotApi.Trading.GetUserTradeDetailsAsync(...)` |
| Place spot order | `client.SpotApi.Trading.PlaceOrderAsync(...)` |
| Place multiple spot orders | `client.SpotApi.Trading.PlaceMultipleOrdersAsync(...)` |
| Edit spot order | `client.SpotApi.Trading.EditOrderAsync(...)` |
| Cancel spot order | `client.SpotApi.Trading.CancelOrderAsync(orderId: orderId)` |
| Cancel all spot orders | `client.SpotApi.Trading.CancelAllOrdersAsync()` |
| Cancel all spot orders after timeout | `client.SpotApi.Trading.CancelAllOrdersAfterAsync(timeout)` |
| Cancel multiple spot orders | `client.SpotApi.Trading.CancelMultipleOrdersAsync(...)` |

## Kraken Earn REST

| User intent | Kraken.Net member |
|---|---|
| Get Earn strategies | `client.SpotApi.Earn.GetStrategiesAsync(...)` |
| Get Earn allocations | `client.SpotApi.Earn.GetAllocationsAsync(...)` |
| Get allocation status | `client.SpotApi.Earn.GetAllocationStatusAsync(strategyId)` |
| Get deallocation status | `client.SpotApi.Earn.GetDeallocationStatusAsync(strategyId)` |
| Allocate Earn funds | `client.SpotApi.Earn.AllocateEarnFundsAsync(strategyId, quantity)` |
| Deallocate Earn funds | `client.SpotApi.Earn.DeallocateEarnFundsAsync(strategyId, quantity)` |

## Futures REST

| User intent | Kraken.Net member |
|---|---|
| Get futures fee schedules | `client.FuturesApi.ExchangeData.GetFeeSchedulesAsync()` |
| Get historical funding rates | `client.FuturesApi.ExchangeData.GetHistoricalFundingRatesAsync("PF_ETHUSD")` |
| Get futures klines / candles | `client.FuturesApi.ExchangeData.GetKlinesAsync(TickType.Trade, "PF_ETHUSD", FuturesKlineInterval.OneMinute)` |
| Get futures order book | `client.FuturesApi.ExchangeData.GetOrderBookAsync("PF_ETHUSD")` |
| Get platform notifications | `client.FuturesApi.ExchangeData.GetPlatformNotificationsAsync()` |
| Get futures symbols / instruments | `client.FuturesApi.ExchangeData.GetSymbolsAsync()` |
| Get futures symbol statuses | `client.FuturesApi.ExchangeData.GetSymbolStatusAsync()` |
| Get one futures ticker | `client.FuturesApi.ExchangeData.GetTickerAsync("PF_ETHUSD")` |
| Get all futures tickers | `client.FuturesApi.ExchangeData.GetTickersAsync()` |
| Get futures trades | `client.FuturesApi.ExchangeData.GetTradesAsync("PF_ETHUSD")` |
| Get futures account log | `client.FuturesApi.Account.GetAccountLogAsync(...)` |
| Get futures balances | `client.FuturesApi.Account.GetBalancesAsync()` |
| Get PNL currency preference | `client.FuturesApi.Account.GetPnlCurrencyAsync()` |
| Set PNL currency preference | `client.FuturesApi.Account.SetPnlCurrencyAsync(symbol, pnlCurrency)` |
| Transfer futures funds | `client.FuturesApi.Account.TransferAsync(asset, quantity, fromAccount, toAccount)` |
| Get futures fee schedule volume | `client.FuturesApi.Account.GetFeeScheduleVolumeAsync()` |
| Get initial margin requirement | `client.FuturesApi.Account.GetInitialMarginRequirementsAsync(...)` |
| Get max order quantity | `client.FuturesApi.Account.GetMaxOrderQuantityAsync(...)` |
| Cancel futures dead-man switch | `client.FuturesApi.Trading.CancelAllOrderAfterAsync(TimeSpan.Zero)` |
| Set futures dead-man switch | `client.FuturesApi.Trading.CancelAllOrderAfterAsync(cancelAfter)` |
| Cancel all futures orders | `client.FuturesApi.Trading.CancelAllOrdersAsync(symbol)` |
| Cancel futures order | `client.FuturesApi.Trading.CancelOrderAsync(orderId: orderId)` |
| Edit futures order | `client.FuturesApi.Trading.EditOrderAsync(...)` |
| Get execution events | `client.FuturesApi.Trading.GetExecutionEventsAsync(...)` |
| Get leverage settings | `client.FuturesApi.Trading.GetLeverageAsync()` |
| Set leverage | `client.FuturesApi.Trading.SetLeverageAsync("PF_ETHUSD", 5)` |
| Get open futures orders | `client.FuturesApi.Trading.GetOpenOrdersAsync()` |
| Get open futures positions | `client.FuturesApi.Trading.GetOpenPositionsAsync()` |
| Query futures order | `client.FuturesApi.Trading.GetOrderAsync(orderId: orderId)` |
| Query multiple futures orders | `client.FuturesApi.Trading.GetOrdersAsync(orderIds: ids)` |
| Get self-trade strategy | `client.FuturesApi.Trading.GetSelfTradeStrategyAsync()` |
| Set self-trade strategy | `client.FuturesApi.Trading.SetSelfTradeStrategyAsync(strategy)` |
| Get futures user trades / fills | `client.FuturesApi.Trading.GetUserTradesAsync(...)` |
| Place futures order | `client.FuturesApi.Trading.PlaceOrderAsync(...)` |
| Get futures order history | `client.FuturesApi.Trading.GetOrderHistoryAsync(...)` |

## Spot WebSocket

| User intent | Kraken.Net member |
|---|---|
| Subscribe system status | `socketClient.SpotApi.SubscribeToSystemStatusUpdatesAsync(handler)` |
| Subscribe spot ticker | `socketClient.SpotApi.SubscribeToTickerUpdatesAsync("ETH/USDT", handler)` |
| Subscribe many spot tickers | `socketClient.SpotApi.SubscribeToTickerUpdatesAsync(symbols, handler)` |
| Subscribe spot klines | `socketClient.SpotApi.SubscribeToKlineUpdatesAsync("ETH/USDT", KlineInterval.OneMinute, handler)` |
| Subscribe spot trades | `socketClient.SpotApi.SubscribeToTradeUpdatesAsync("ETH/USDT", handler)` |
| Subscribe aggregated order book | `socketClient.SpotApi.SubscribeToAggregatedOrderBookUpdatesAsync("ETH/USDT", 10, handler)` |
| Subscribe individual order book | `socketClient.SpotApi.SubscribeToIndividualOrderBookUpdatesAsync("ETH/USDT", 10, handler)` |
| Subscribe instrument updates | `socketClient.SpotApi.SubscribeToInstrumentUpdatesAsync(handler)` |
| Subscribe balance updates | `socketClient.SpotApi.SubscribeToBalanceUpdatesAsync(snapshotHandler, updateHandler)` |
| Subscribe order updates | `socketClient.SpotApi.SubscribeToOrderUpdatesAsync(updateHandler)` |
| Socket API place spot order | `socketClient.SpotApi.PlaceOrderAsync(...)` |
| Socket API edit/amend spot order | `socketClient.SpotApi.EditOrderAsync(...)` |
| Socket API replace spot order | `socketClient.SpotApi.ReplaceOrderAsync(...)` |
| Socket API place multiple spot orders | `socketClient.SpotApi.PlaceMultipleOrdersAsync(...)` |
| Socket API cancel spot order | `socketClient.SpotApi.CancelOrderAsync(orderId)` |
| Socket API cancel multiple spot orders | `socketClient.SpotApi.CancelOrdersAsync(orderIds)` |
| Socket API cancel all spot orders | `socketClient.SpotApi.CancelAllOrdersAsync()` |
| Socket API cancel all spot orders after timeout | `socketClient.SpotApi.CancelAllOrdersAfterAsync(timeout)` |

## Futures WebSocket

| User intent | Kraken.Net member |
|---|---|
| Subscribe futures account log | `socketClient.FuturesApi.SubscribeToAccountLogUpdatesAsync(snapshotHandler, updateHandler)` |
| Subscribe futures balances | `socketClient.FuturesApi.SubscribeToBalanceUpdatesAsync(handler)` |
| Subscribe futures heartbeat | `socketClient.FuturesApi.SubscribeToHeartbeatUpdatesAsync(handler)` |
| Subscribe futures mini ticker | `socketClient.FuturesApi.SubscribeToMiniTickerUpdatesAsync("PF_ETHUSD", handler)` |
| Subscribe futures notifications | `socketClient.FuturesApi.SubscribeToNotificationUpdatesAsync(handler)` |
| Subscribe futures open orders | `socketClient.FuturesApi.SubscribeToOpenOrdersUpdatesAsync(verbose, snapshotHandler, updateHandler)` |
| Subscribe futures open positions | `socketClient.FuturesApi.SubscribeToOpenPositionUpdatesAsync(handler)` |
| Subscribe futures order book | `socketClient.FuturesApi.SubscribeToOrderBookUpdatesAsync("PF_ETHUSD", snapshotHandler, updateHandler)` |
| Subscribe futures ticker | `socketClient.FuturesApi.SubscribeToTickerUpdatesAsync("PF_ETHUSD", handler)` |
| Subscribe futures trades | `socketClient.FuturesApi.SubscribeToTradeUpdatesAsync("PF_ETHUSD", handler)` |
| Subscribe futures user trades | `socketClient.FuturesApi.SubscribeToUserTradeUpdatesAsync(handler)` |

## SharedApis

| User intent | Kraken.Net member or interface |
|---|---|
| Shared spot REST client | `new KrakenRestClient().SpotApi.SharedClient` |
| Shared futures REST client | `new KrakenRestClient().FuturesApi.SharedClient` |
| Shared spot socket client | `new KrakenSocketClient().SpotApi.SharedClient` |
| Shared futures socket client | `new KrakenSocketClient().FuturesApi.SharedClient` |
| Discover shared capabilities | `client.SpotApi.SharedClient.Discover()` |
| Shared spot ticker REST | `ISpotTickerRestClient.GetSpotTickerAsync(new GetTickerRequest(symbol))` |
| Shared spot order REST | `ISpotOrderRestClient.PlaceSpotOrderAsync(...)` |
| Shared futures order REST | `IFuturesOrderRestClient.PlaceFuturesOrderAsync(...)` |
| Shared balance REST | `IBalanceRestClient.GetBalancesAsync(...)` |
| Shared ticker socket | `ITickerSocketClient.SubscribeToTickerUpdatesAsync(...)` |
| Shared order book socket | `IOrderBookSocketClient.SubscribeToOrderBookUpdatesAsync(...)` |

Shared REST methods return `HttpResult<T>` / `HttpResult`; shared socket subscriptions return `WebSocketResult<UpdateSubscription>`; shared symbol/cache helper methods such as `SupportsSpotSymbolAsync` can return `ExchangeCallResult<T>`.

For shared socket subscriptions, keep the concrete socket client and unsubscribe with `await socketClient.UnsubscribeAsync(subscription.Data)`.

## Result Handling

| Situation | Pattern |
|---|---|
| REST success check | `if (!result.Success) { Console.WriteLine(result.Error); return; }` |
| Socket subscription success check | `if (!sub.Success) { Console.WriteLine(sub.Error); return; }` where `sub` is `WebSocketResult<UpdateSubscription>` |
| Spot socket request success check | `if (!query.Success) { Console.WriteLine(query.Error); return; }` where `query` is `QueryResult<T>` |
| Read REST data | Read `result.Data` only after `result.Success` |
| Read shared helper data | Read `ExchangeCallResult<T>.Data` only after `.Success` |
| Retry decision | Retry only when `result.Error?.IsTransient == true` |
| Cancellation | Pass `ct: cancellationToken` |

## Common Routing Pitfalls

| Do not use | Use instead |
|---|---|
| `KrakenClient` | `KrakenRestClient` |
| raw `HttpClient` calls | `KrakenRestClient` / `KrakenSocketClient` |
| generic `ApiCredentials` in new code | `KrakenCredentials` |
| one key assumed for both APIs | separate Spot/Futures `HMACCredential` values |
| Spot REST symbol for WebSocket without checking | `KrakenSymbol.WebsocketName` from `GetSymbolsAsync` |
| `SpotApi.Margin` | Spot margin/order parameters on `SpotApi.Trading` and positions on `SpotApi.Account` |
| `FuturesApi.Positions` | `FuturesApi.Trading.GetOpenPositionsAsync()` |
| `FuturesApi.MarketData` | `FuturesApi.ExchangeData` |
| `.Data` without `.Success` check | Check `.Success` first |
| `ITickerSocketClient.UnsubscribeAsync(...)` | Keep concrete socket client and call `socketClient.UnsubscribeAsync(subscription.Data)` |
