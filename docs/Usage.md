---
title: Getting started
nav_order: 2
---

## Creating client
There are 2 clients available to interact with the Kraken API, the `KrakenClient` and `KrakenSocketClient`.

*Create a new rest client*
```csharp
var krakenClient = new KrakenClient(new KrakenClientOptions()
{
	// Set options here for this client
});
```

*Create a new socket client*
```csharp
var krakenSocketClient = new KrakenSocketClient(new KrakenSocketClientOptions()
{
	// Set options here for this client
});
```

Different options are available to set on the clients, see this example
```csharp
var krakenClient = new KrakenClient(new KrakenClientOptions()
{
	ApiCredentials = new ApiCredentials("API-KEY", "API-SECRET"),
	LogLevel = LogLevel.Trace,
	RequestTimeout = TimeSpan.FromSeconds(60)
});
```
Alternatively, options can be provided before creating clients by using `SetDefaultOptions`:
```csharp
KrakenClient.SetDefaultOptions(new KrakenClientOptions{
	// Set options here for all new clients
});
var krakenClient = new KrakenClient();
```
More info on the specific options can be found in the [CryptoExchange.Net documentation](https://jkorf.github.io/CryptoExchange.Net/Options.html)

### Dependency injection
See [CryptoExchange.Net documentation](https://jkorf.github.io/CryptoExchange.Net/Clients.html#dependency-injection)