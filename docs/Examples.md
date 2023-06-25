---
title: Examples
nav_order: 3
---

## Basic operations
Make sure to read the [CryptoExchange.Net documentation](https://jkorf.github.io/CryptoExchange.Net/Clients.html#processing-request-responses) on processing responses.

### Get market data
```csharp
// Getting info on all symbols
var symbolData = await krakenClient.SpotApi.ExchangeData.GetSymbolsAsync();

// Getting ticker
var tickerData = await krakenClient.SpotApi.ExchangeData.GetTickerAsync("XBTUSD");

// Getting the order book of a symbol
var orderBookData = await krakenClient.SpotApi.ExchangeData.GetOrderBookAsync("XBTUSD");

// Getting recent trades of a symbol
var tradeHistoryData = await krakenClient.SpotApi.ExchangeData.GetTradeHistoryAsync("XBTUSD");
```

### Requesting balances
```csharp
var balanceData = await krakenClient.SpotApi.Account.GetBalancesAsync();
```
### Placing order
```csharp
// Placing a buy limit order for 0.001 BTC at a price of 50000USDT each
var orderData = await krakenClient.SpotApi.Trading.PlaceOrderAsync(
                "BTC-USDT",
                OrderSide.Buy,
                OrderType.Limit,
                0.001m,
                50000);
                                                            
// Place a stop loss order, place a limit order of 0.001 BTC at 39000USDT each when the last trade price drops below 40000USDT
var orderData = await krakenClient.SpotApi.Trading.PlaceOrderAsync(
                "BTC-USDT",
                OrderSide.Sell,
                OrderType.StopLossLimit,
                0.001m,
                40000,
                secondaryPrice: 39000);
```

### Requesting a specific order
```csharp
// Request info on order with id `1234`
var orderData = await krakenClient.SpotApi.Trading.GetOrderAsync("1234");
```

### Requesting order history
```csharp
// Get all orders conform the parameters
 var ordersData = await krakenClient.SpotApi.Trading.GetOrdersAsync();
```

### Cancel order
```csharp
// Cancel order with id `1234`
var orderData = await krakenClient.SpotApi.Trading.CancelOrderAsync("1234");
```

### Get user trades
```csharp
var userTradesResult = await krakenClient.SpotApi.Trading.GetUserTradesAsync();
```

### Subscribing to market data updates
```csharp
var subscribeResult = await krakenSocket.SpotApi.SubscribeToTickerUpdatesAsync("XBT/USD", data =>
{
    // Handle ticker data
});
```

### Subscribing to order updates
```csharp
var socketToken = await krakenClient.SpotApi.Account.GetWebsocketTokenAsync();
if(!socketToken.Success)
{
    // Handle error
    return;
}
var symbolData = await krakenSocket.SpotApi.SubscribeToOrderUpdatesAsync(socketToken.Data.Token, data =>
{
    // Handle update
});
```
