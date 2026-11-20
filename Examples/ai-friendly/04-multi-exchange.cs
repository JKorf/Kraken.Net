// Demonstrates portable REST and socket ticker capabilities through Shared API V2.
// Setup: dotnet add package KrakenExchange.Net

using Kraken.Net.Clients;
using CryptoExchange.Net.SharedApis;

using var restClient = new KrakenRestClient();
IGetTickerRest ticker = restClient.SpotApi.SharedApi;
var symbol = new SharedSymbol(TradingMode.Spot, "ETH", "USDT");
await PrintTickerAsync(ticker, symbol);

IGetSpotSymbolsRest symbolsClient = restClient.SpotApi.SharedApi;
var symbols = await symbolsClient.GetSpotSymbolsAsync(new GetSymbolsRequest());
if (!symbols.Success)
{
    Console.WriteLine(symbols.Error);
    return;
}
foreach (var item in symbols.Data.Take(5))
    Console.WriteLine($"{item.Name}: {item.BaseAsset}/{item.QuoteAsset}");
var catalog = symbolsClient.SpotSymbolCatalog;

using var socketClient = new KrakenSocketClient();
ISubscribeTickerSocket tickerSocket = socketClient.SpotApi.SharedApi;
var subscription = await tickerSocket.SubscribeToTickerUpdatesAsync(
    new SubscribeTickerRequest(symbol),
    update => Console.WriteLine(update.Data.LastPrice));
if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}
try
{
    await Task.Delay(TimeSpan.FromSeconds(30));
}
finally
{
    await socketClient.UnsubscribeAsync(subscription.Data);
}

// Another exchange can supply IGetTickerRest without changing this function.
static async Task PrintTickerAsync(IGetTickerRest client, SharedSymbol symbol)
{
    var result = await client.GetTickerAsync(new GetTickerRequest(symbol));
    if (!result.Success)
    {
        Console.WriteLine(result.Error);
        return;
    }
    Console.WriteLine($"{result.Data.Symbol}: {result.Data.LastPrice}");
}
