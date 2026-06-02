using Kraken.Net;
using Kraken.Net.Clients;
using Kraken.Net.Enums;

const string spotSymbol = "BTCUSDT";
const string futuresSymbol = "PF_ETHUSD";

// Replace with valid credentials or order placement will always fail
var spotApiKey = "KEY";
var spotApiSecret = "SECRET";
var futuresApiKey = "FUTURESKEY";
var futuresApiSecret = "FUTURESSECRET";

Console.WriteLine("Kraken.Net order placement example");
Console.WriteLine();
Console.WriteLine("This example can place real orders when valid credentials are configured.");
Console.WriteLine();

var client = new KrakenRestClient(options =>
{
    options.ApiCredentials = new KrakenCredentials()
        .WithSpot(spotApiKey, spotApiSecret)
        .WithFutures(futuresApiKey, futuresApiSecret);
});

await PlaceSpotLimitOrderAsync(client);
Console.WriteLine();
await PlaceFuturesReduceOnlyOrderExampleAsync(client);

static async Task PlaceSpotLimitOrderAsync(KrakenRestClient client)
{
    Console.WriteLine($"Placing spot limit buy order for {spotSymbol}...");

    var ticker = await client.SpotApi.ExchangeData.GetTickerAsync(spotSymbol);
    if (!ticker.Success)
    {
        Console.WriteLine($"Failed to get spot ticker: {ticker.Error}");
        return;
    }

    var lastPrice = ticker.Data.Values.First().LastTrade.Price;
    var safePrice = Math.Round(lastPrice * 0.95m, 2);
    var order = await client.SpotApi.Trading.PlaceOrderAsync(
        symbol: spotSymbol,
        side: OrderSide.Buy,
        type: OrderType.Limit,
        quantity: 0.001m,
        price: safePrice,
        timeInForce: TimeInForce.GTC);

    if (!order.Success)
    {
        Console.WriteLine($"Failed to place spot order: {order.Error}");
        return;
    }

    var orderId = order.Data.OrderIds.FirstOrDefault();
    if (orderId == null)
    {
        Console.WriteLine("No spot order id returned.");
        return;
    }

    Console.WriteLine($"Placed spot order {orderId}: {order.Data.Descriptions.OrderDescription}");

    var orderStatus = await client.SpotApi.Trading.GetOrderAsync(orderId: orderId);
    if (orderStatus.Success && orderStatus.Data.TryGetValue(orderId, out var orderInfo))
        Console.WriteLine($"Spot order status: {orderInfo.Status}, filled: {orderInfo.QuantityFilled}");
    else
        Console.WriteLine($"Failed to query spot order: {orderStatus.Error}");

    var cancel = await client.SpotApi.Trading.CancelOrderAsync(orderId: orderId);
    Console.WriteLine(cancel.Success
        ? $"Cancelled spot order {orderId}"
        : $"Failed to cancel spot order: {cancel.Error}");
}

static async Task PlaceFuturesReduceOnlyOrderExampleAsync(KrakenRestClient client)
{
    Console.WriteLine($"Placing futures reduce-only limit sell order for {futuresSymbol}...");

    var ticker = await client.FuturesApi.ExchangeData.GetTickerAsync(futuresSymbol);
    if (!ticker.Success)
    {
        Console.WriteLine($"Failed to get futures ticker: {ticker.Error}");
        return;
    }

    var referencePrice = ticker.Data.LastPrice ?? ticker.Data.MarkPrice;
    var safePrice = Math.Round(referencePrice * 1.05m, 2);
    var order = await client.FuturesApi.Trading.PlaceOrderAsync(
        symbol: futuresSymbol,
        side: OrderSide.Sell,
        type: FuturesOrderType.Limit,
        quantity: 0.01m,
        price: safePrice,
        reduceOnly: true);

    if (!order.Success)
    {
        Console.WriteLine($"Failed to place futures order: {order.Error}");
        return;
    }

    Console.WriteLine($"Placed futures order {order.Data.OrderId}, status: {order.Data.Status}");

    var orderStatus = await client.FuturesApi.Trading.GetOrderAsync(orderId: order.Data.OrderId);
    if (orderStatus.Success)
        Console.WriteLine($"Futures order status: {orderStatus.Data.Status}, update reason: {orderStatus.Data.UpdateReason ?? "none"}");
    else
        Console.WriteLine($"Failed to query futures order: {orderStatus.Error}");

    var cancel = await client.FuturesApi.Trading.CancelOrderAsync(orderId: order.Data.OrderId);
    Console.WriteLine(cancel.Success
        ? $"Cancelled futures order {order.Data.OrderId}"
        : $"Failed to cancel futures order: {cancel.Error}");
}
