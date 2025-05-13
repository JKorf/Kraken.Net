﻿using Kraken.Net.Clients;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Kraken.Net.Objects.Options;
using Microsoft.Extensions.Options;

// REST
var restClient = new KrakenRestClient();
var ticker = await restClient.SpotApi.ExchangeData.GetTickerAsync("ETHUSDT");
Console.WriteLine($"Rest client ticker price for ETH-USDT: {ticker.Data.Single().Value.LastTrade.Price}");

Console.WriteLine();
Console.WriteLine("Press enter to start websocket subscription");
Console.ReadLine();

// Websocket
// Optional, manually add logging
var logFactory = new LoggerFactory();
logFactory.AddProvider(new TraceLoggerProvider());

var socketClient = new KrakenSocketClient(Options.Create(new KrakenSocketOptions { }), logFactory);
var subscription = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync("ETH/USDT", update =>
{
    Console.WriteLine($"Websocket client ticker price for ETHUSDT: {update.Data.LastPrice}");
});

Console.ReadLine();
