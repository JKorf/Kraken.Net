---
title: Rest API documentation
has_children: true
---
*[generated documentation]*  
### KrakenRestClient  
*Client for accessing the Kraken API.*
  
***
*Futures API endpoints*  
**KrakenRestClientFuturesApi FuturesApi { get; }**  
***
*Set the API credentials for this client. All Api clients in this client will use the new credentials, regardless of earlier set options.*  
**void SetApiCredentials(ApiCredentials credentials);**  
***
*Spot API endpoints*  
**IKrakenClientSpotApi SpotApi { get; }**  
