using CryptoExchange.Net.SharedApis;
using Kraken.Net.Clients;

// SharedApis let application code target common exchange capabilities.
// The concrete Kraken client still owns transport, auth, rate limiting, and unsubscription.
var restClient = new KrakenRestClient();

ISpotTickerRestClient spotTickerClient = restClient.SpotApi.SharedClient;
var spotSymbol = new SharedSymbol(TradingMode.Spot, "ETH", "USDT");

var ticker = await spotTickerClient.GetSpotTickerAsync(new GetTickerRequest(spotSymbol));
if (!ticker.Success)
{
    Console.WriteLine($"Shared spot ticker failed: {ticker.Error}");
    return;
}

Console.WriteLine($"Shared spot ticker last price: {ticker.Data.LastPrice}");

var socketClient = new KrakenSocketClient();
ITickerSocketClient tickerSocket = socketClient.SpotApi.SharedClient;

var subscription = await tickerSocket.SubscribeToTickerUpdatesAsync(
    new SubscribeTickerRequest(spotSymbol),
    update => Console.WriteLine($"Shared ticker update: {update.Data.LastPrice}"));

if (!subscription.Success)
{
    Console.WriteLine($"Shared ticker subscription failed: {subscription.Error}");
    return;
}

Console.WriteLine("Shared subscription active. Press enter to unsubscribe.");
Console.ReadLine();

await socketClient.UnsubscribeAsync(subscription.Data);
