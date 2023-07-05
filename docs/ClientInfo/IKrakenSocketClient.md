---
title: Socket API documentation
has_children: true
---
*[generated documentation]*  
### KrakenSocketClient  
*Client for accessing the Kraken websocket API.*
  
***
*Futures Api*  
**[IKrakenSocketClientFuturesApi](FuturesApi/IKrakenSocketClientFuturesApi.html) FuturesApi { get; }**  
***
*Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.*  
**void SetApiCredentials(ApiCredentials credentials);**  
***
*Spot Api*  
**[IKrakenSocketClientSpotApi](SpotApi/IKrakenSocketClientSpotApi.html) SpotApi { get; }**  
