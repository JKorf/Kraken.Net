using CryptoExchange.Net.Objects;
using Kraken.Net.Clients;
using Kraken.Net.Enums;

var client = new KrakenRestClient();

var symbols = await client.SpotApi.ExchangeData.GetSymbolsAsync(new[] { "ETHUSDT" });
if (!symbols.Success || symbols.Data.Count == 0)
{
    Console.WriteLine($"Could not load symbol metadata: {symbols.Error}");
    return;
}

var symbol = symbols.Data.Values.First();
Console.WriteLine($"REST symbol ETHUSDT maps to WebSocket name {symbol.WebsocketName}");

var ticker = await WithRetryAsync(
    () => client.SpotApi.ExchangeData.GetTickerAsync("ETHUSDT"));

if (!ticker.Success)
{
    Console.WriteLine($"Ticker failed after retry handling: {ticker.Error}");
    return;
}

Console.WriteLine($"Ticker entries returned: {ticker.Data.Count}");

var futuresOrderBook = await WithRetryAsync(
    () => client.FuturesApi.ExchangeData.GetOrderBookAsync("PF_ETHUSD"));

if (!futuresOrderBook.Success)
{
    Console.WriteLine($"Futures order book failed: {futuresOrderBook.Error}");
    return;
}

Console.WriteLine($"Futures order book returned: {futuresOrderBook.Data}");

static async Task<HttpResult<T>> WithRetryAsync<T>(
    Func<Task<HttpResult<T>>> call,
    int maxAttempts = 3)
{
    HttpResult<T> last = default!;

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        last = await call();
        if (last.Success)
            return last;

        if (last.Error?.IsTransient != true)
            return last;

        await Task.Delay(TimeSpan.FromMilliseconds(250 * Math.Pow(2, attempt)));
    }

    return last;
}
