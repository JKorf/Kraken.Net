using CryptoExchange.Net.Authentication;
using Kraken.Net;
using Kraken.Net.Clients;
using Kraken.Net.Enums;

// Public market data does not need API credentials.
var publicClient = new KrakenRestClient();

var ticker = await publicClient.SpotApi.ExchangeData.GetTickerAsync("ETHUSDT");
if (!ticker.Success)
{
    Console.WriteLine($"Ticker request failed: {ticker.Error}");
    return;
}

var lastTrade = ticker.Data.Values.First().LastTrade.Price;
Console.WriteLine($"ETHUSDT last trade price: {lastTrade}");

// Private account and trading calls need Spot credentials.
var tradingClient = new KrakenRestClient(options =>
{
    options.ApiCredentials = new KrakenCredentials(
        new HMACCredential("SPOT_KEY", "SPOT_SECRET"),
        null);
});

var balances = await tradingClient.SpotApi.Account.GetBalancesAsync();
if (!balances.Success)
{
    Console.WriteLine($"Balance request failed: {balances.Error}");
    return;
}

Console.WriteLine($"Balance assets returned: {balances.Data.Count}");

// validateOnly asks Kraken to validate the order parameters without placing a live order.
var order = await tradingClient.SpotApi.Trading.PlaceOrderAsync(
    symbol: "ETHUSDT",
    side: OrderSide.Buy,
    type: OrderType.Limit,
    quantity: 0.01m,
    price: 1000m,
    validateOnly: true);

if (!order.Success)
{
    Console.WriteLine($"Order validation failed: {order.Error}");
    return;
}

Console.WriteLine("Spot order parameters were accepted by Kraken.");

// Query by order id when you have a live order id. Keep the success check before reading Data.
var orderStatus = await tradingClient.SpotApi.Trading.GetOrderAsync(orderId: "ORDER_ID");
if (!orderStatus.Success)
{
    Console.WriteLine($"Order query failed: {orderStatus.Error}");
    return;
}

Console.WriteLine($"Orders returned: {orderStatus.Data.Count}");
