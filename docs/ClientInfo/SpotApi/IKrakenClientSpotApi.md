---
title: IKrakenClientSpotApi
has_children: true
parent: Rest API documentation
---
*[generated documentation]*  
`KrakenRestClient > SpotApi`  
*Spot API endpoints*
  
***
*Get the ISpotClient for this client. This is a common interface which allows for some basic operations without knowing any details of the exchange.*  
**ISpotClient CommonSpotClient { get; }**  
***
*Endpoints related to account settings, info or actions*  
**IKrakenClientSpotApiAccount Account { get; }**  
***
*Endpoints related to retrieving market and system data*  
**IKrakenClientSpotApiExchangeData ExchangeData { get; }**  
***
*Endpoints related to staking assets*  
**IKrakenClientSpotStakingApi Staking { get; }**  
***
*Endpoints related to orders and trades*  
**IKrakenClientSpotApiTrading Trading { get; }**  
