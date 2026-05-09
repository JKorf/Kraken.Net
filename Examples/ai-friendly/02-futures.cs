using CryptoExchange.Net.Authentication;
using Kraken.Net;
using Kraken.Net.Clients;
using Kraken.Net.Enums;

var publicClient = new KrakenRestClient();

var ticker = await publicClient.FuturesApi.ExchangeData.GetTickerAsync("PF_ETHUSD");
if (!ticker.Success)
{
    Console.WriteLine($"Futures ticker request failed: {ticker.Error}");
    return;
}

Console.WriteLine($"Futures ticker returned: {ticker.Data}");

var klines = await publicClient.FuturesApi.ExchangeData.GetKlinesAsync(
    TickType.Trade,
    "PF_ETHUSD",
    FuturesKlineInterval.OneMinute,
    limit: 10);

if (!klines.Success)
{
    Console.WriteLine($"Futures kline request failed: {klines.Error}");
    return;
}

Console.WriteLine($"Futures klines returned: {klines.Data}");

// Private Futures calls need the Futures credential slot.
var futuresClient = new KrakenRestClient(options =>
{
    options.ApiCredentials = new KrakenCredentials(
        null,
        new HMACCredential("FUTURES_KEY", "FUTURES_SECRET"));
});

var balances = await futuresClient.FuturesApi.Account.GetBalancesAsync();
if (!balances.Success)
{
    Console.WriteLine($"Futures balance request failed: {balances.Error}");
    return;
}

Console.WriteLine($"Futures balances returned: {balances.Data}");

var leverage = await futuresClient.FuturesApi.Trading.SetLeverageAsync("PF_ETHUSD", 5);
if (!leverage.Success)
{
    Console.WriteLine($"Set leverage failed: {leverage.Error}");
    return;
}

var order = await futuresClient.FuturesApi.Trading.PlaceOrderAsync(
    symbol: "PF_ETHUSD",
    side: OrderSide.Buy,
    type: FuturesOrderType.Limit,
    quantity: 0.1m,
    price: 1000m);

if (!order.Success)
{
    Console.WriteLine($"Futures order failed: {order.Error}");
    return;
}

Console.WriteLine($"Futures order result: {order.Data}");

var positions = await futuresClient.FuturesApi.Trading.GetOpenPositionsAsync();
if (!positions.Success)
{
    Console.WriteLine($"Open positions request failed: {positions.Error}");
    return;
}

Console.WriteLine($"Open positions returned: {positions.Data.Length}");
