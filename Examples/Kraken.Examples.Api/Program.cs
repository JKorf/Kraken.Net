using Kraken.Net;
using Kraken.Net.Interfaces.Clients;
using CryptoExchange.Net.Authentication;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the Kraken services
builder.Services.AddKraken();

// OR to provide API credentials for accessing private endpoints, or setting other options:
/*
builder.Services.AddKraken(options =>
{    
   options.ApiCredentials = new KrakenCredentials(
       new HMACCredential("SPOT_API_KEY", "SPOT_API_SECRET"),
       new HMACCredential("FUTURES_API_KEY", "FUTURES_API_SECRET"));
   options.Rest.RequestTimeout = TimeSpan.FromSeconds(5);
});
*/

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Map the endpoints and inject the Kraken rest client
app.MapGet("/{Symbol}", async ([FromServices] IKrakenRestClient client, string symbol) =>
{
    var result = await client.SpotApi.ExchangeData.GetTickerAsync(symbol);
    return result.Success
        ? Results.Ok(result.Data)
        : Results.Problem(result.Error?.Message, statusCode: 502);
})
.WithOpenApi();

app.MapGet("/Balances", async ([FromServices] IKrakenRestClient client) =>
{
    var result = await client.SpotApi.Account.GetBalancesAsync();
    return result.Success
        ? Results.Ok(result.Data)
        : Results.Problem(result.Error?.Message, statusCode: 502);
})
.WithOpenApi();

app.Run();
