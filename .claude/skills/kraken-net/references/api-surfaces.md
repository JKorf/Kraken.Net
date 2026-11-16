# Kraken.Net API surfaces

## Client roots

`KrakenRestClient` / `IKrakenRestClient` expose `SpotApi` with `ExchangeData`, `Account`, `Trading`, and `Earn`, and `FuturesApi` with `ExchangeData`, `Account`, and `Trading`. `KrakenSocketClient` / `IKrakenSocketClient` expose methods directly on `SpotApi` and `FuturesApi`; socket methods are not under REST-style account/trading groups. `IKrakenTrackerFactory` supplies trackers.

`KrakenEnvironment.Live` is the built-in environment; `KrakenEnvironment.CreateCustom(...)` configures explicit addresses. Do not invent a built-in `Testnet` property.

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
| Get futures position history | `client.FuturesApi.Account.GetPositionEventsAsync(...)` |
| Get futures balances | `client.FuturesApi.Account.GetBalancesAsync()` |
| Get PNL currency preference | `client.FuturesApi.Account.GetPnlCurrencyAsync()` |
| Set PNL currency preference | `client.FuturesApi.Account.SetPnlCurrencyAsync(symbol, pnlCurrency)` |
| Withdraw Futures funds to Spot | `client.FuturesApi.Account.WithdrawToSpotWalletAsync(asset, quantity, sourceWallet)` |
| Transfer futures funds | `client.FuturesApi.Account.TransferAsync(asset, quantity, fromAccount, toAccount)` |
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

## Shared API V2

`IKrakenSharedApiClient` groups these properties; the corresponding native surface exposes `.SharedApi`. Listed interfaces are V2 capabilities, not legacy topic clients.

- `SpotRest` (`IKrakenRestClientSpotSharedApi`): `IGetAssetRest`, `IGetAllAssetsRest`, `IGetBalancesRest`, `IGetDepositHistoryRest`, `IGetDepositAddressesRest`, `IGetKlinesRest`, `IGetOrderBookRest`, `IGetRecentTradesRest`, `IPlaceSpotOrderRest`, `IGetSpotOrderRest`, `IGetSpotOrderByClientOrderIdRest`, `IGetOpenSpotOrdersRest`, `IGetClosedSpotOrdersRest`, `IGetSpotOrderTradesRest`, `IGetSpotUserTradeHistoryRest`, `ICancelSpotOrderRest`, `ICancelSpotOrderByClientOrderIdRest`, `IGetSpotSymbolsRest`, `IGetTickerRest`, `IGetAllTickersRest`, `IGetWithdrawalHistoryRest`, `IWithdrawRest`, `IGetFeesRest`, `IGetBookTickerRest`.
- `FuturesRest` (`IKrakenRestClientFuturesSharedApi`): `IGetBalancesRest`, `IGetKlinesRest`, `IGetOrderBookRest`, `IGetRecentTradesRest`, `IGetBookTickerRest`, `IGetFundingRateHistoryRest`, `IGetFuturesSymbolsRest`, `IGetTickerRest`, `IGetAllTickersRest`, `IGetMarkPriceKlinesRest`, `IGetOpenInterestRest`, `IGetLeverageRest`, `ISetLeverageRest`, `IPlaceFuturesOrderRest`, `IGetFuturesOrderRest`, `IGetOpenFuturesOrdersRest`, `IGetFuturesUserTradeHistoryRest`, `ICancelFuturesOrderRest`, `IGetPositionsRest`, `IGetFeesRest`, `IGetFuturesOrderByClientOrderIdRest`, `ICancelFuturesOrderByClientOrderIdRest`, `ISetFuturesTpSlRest`, `ICancelFuturesTpSlRest`.
- `SpotSocket` (`IKrakenSocketClientSpotSharedApi`): `ISubscribeTickerSocket`, `ISubscribeTradesSocket`, `ISubscribeBookTickerSocket`, `ISubscribeKlinesSocket`, `ISubscribeBalancesSocket`, `ISubscribeSpotOrdersSocket`, `IPlaceSpotOrderSocket`, `ICancelSpotOrderSocket`.
- `FuturesSocket` (`IKrakenSocketClientFuturesSharedApi`): `ISubscribeTickerSocket`, `ISubscribeTradesSocket`, `ISubscribeBookTickerSocket`, `ISubscribeBalancesSocket`, `ISubscribeFuturesOrdersSocket`, `ISubscribeUserTradesSocket`, `ISubscribePositionsSocket`.

For known workflows use an aggregate property or inject the narrow capability. For dynamic selection, call `aggregate.GetCapability(SharedCapabilities.Tickers.GetTicker.Rest, tradingMode: TradingMode.Spot)` and check for null before using the match's `Capability` and `Options`. Descriptors identify operations; they do not guarantee support. `GetCapabilities` enumerates all matches and `Discover` summarizes metadata. When several exchanges are registered, inject `IEnumerable<TCapability>` and select by exchange, transport, and supported trading mode.

## Symbols, catalogs, and quantities

Native Spot REST accepts pair names such as `ETHUSDT`; Spot WebSocket v2 uses `ETH/USDT`. `SpotApi.ExchangeData.GetSymbolsAsync()` returns REST pair metadata, including `WebsocketName`. Resolve the WebSocket name rather than inserting separators into arbitrary exchange aliases. Futures use instrument identifiers such as `PF_ETHUSD`; preserve the product prefix and delivery suffix when applicable. `KrakenExchange.FormatSymbol` and `SharedSymbol` support reusable formatting; choose the actual market from metadata.

Call `IGetSpotSymbolsRest.GetSpotSymbolsAsync(new GetSymbolsRequest())` or `IGetFuturesSymbolsRest.GetFuturesSymbolsAsync(new GetSymbolsRequest())`, check success, then inspect `SpotSymbolCatalog` or `FuturesSymbolCatalog`. Symbol requests honor filters. Spot discovery includes currency and tokenized-asset pairs; metadata includes display names, crypto/fiat/TradFi asset types, stablecoin subtypes, and tokenized equity classifications where populated. Native `AClass` and execution venue filters distinguish relevant markets.

Futures discovery covers linear and inverse perpetuals and delivery contracts, excluding spot indices. Inspect trading mode, `ContractSize`, `QuantityDecimals`, price steps, delivery time, and tradeability. Shared Futures order quantities use `SharedQuantityType.Contracts` and `SharedOrderQuantity.QuantityInContracts`; the adapter sends this value as native `quantity` (`size`). Do not substitute base-asset quantity or assume all futures contracts have equal value. Spot order quantities normally represent base assets; inspect supported quantity options when using quote-volume features.

## Checkout-only sources

- [Public interfaces](../../../../Kraken.Net/Interfaces/Clients/): signatures, parameters, and cancellation tokens.
- [Client implementations](../../../../Kraken.Net/Clients/): Shared options, symbol classification, and conversions.
- [Complete examples](../../../../Examples/ai-friendly/): independent consumer programs.
