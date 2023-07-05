---
title: IKrakenRestClientSpotApi
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
**[IKrakenRestClientSpotApiAccount](IKrakenRestClientSpotApiAccount.html) Account { get; }**  
***
*Endpoints related to retrieving market and system data*  
**[IKrakenRestClientSpotApiExchangeData](IKrakenRestClientSpotApiExchangeData.html) ExchangeData { get; }**  
***
*Endpoints related to staking assets*  
**[IKrakenRestClientSpotStakingApi](SpotStakingApi/IKrakenRestClientSpotStakingApi.html) Staking { get; }**  
***
*Endpoints related to orders and trades*  
**[IKrakenRestClientSpotApiTrading](IKrakenRestClientSpotApiTrading.html) Trading { get; }**  
