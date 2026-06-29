using Kraken.Net.Clients;
using Kraken.Net.Enums;

var socketClient = new KrakenSocketClient();

// Subscription methods return WebSocketResult<UpdateSubscription>.
// Spot WebSocket v2 uses WebSocket symbol names such as ETH/USDT.
// When in doubt, fetch Spot symbols via REST and use KrakenSymbol.WebsocketName.
var spotTicker = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    "ETH/USDT",
    update => Console.WriteLine($"Spot ticker update: {update.Data.LastPrice}"));

if (!spotTicker.Success)
{
    Console.WriteLine($"Spot ticker subscription failed: {spotTicker.Error}");
    return;
}

var spotKlines = await socketClient.SpotApi.SubscribeToKlineUpdatesAsync(
    "ETH/USDT",
    KlineInterval.OneMinute,
    update => Console.WriteLine($"Spot kline update count: {update.Data.Length}"));

if (!spotKlines.Success)
{
    await socketClient.UnsubscribeAsync(spotTicker.Data);
    Console.WriteLine($"Spot kline subscription failed: {spotKlines.Error}");
    return;
}

var futuresTicker = await socketClient.FuturesApi.SubscribeToTickerUpdatesAsync(
    "PF_ETHUSD",
    update => Console.WriteLine($"Futures ticker update: {update.Data}"));

if (!futuresTicker.Success)
{
    await socketClient.UnsubscribeAsync(spotTicker.Data);
    await socketClient.UnsubscribeAsync(spotKlines.Data);
    Console.WriteLine($"Futures ticker subscription failed: {futuresTicker.Error}");
    return;
}

Console.WriteLine("Subscriptions are active. Press enter to unsubscribe.");
Console.ReadLine();

await socketClient.UnsubscribeAsync(spotTicker.Data);
await socketClient.UnsubscribeAsync(spotKlines.Data);
await socketClient.UnsubscribeAsync(futuresTicker.Data);
