using CryptoExchange.Net.SharedApis.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    public interface IKrakenRestClientSpotApiShared :
        ITickerRestClient,
        ISpotSymbolRestClient,
        IKlineRestClient,
        ITradeRestClient,
        IBalanceRestClient
    {
    }
}
