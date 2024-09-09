using CryptoExchange.Net.SharedApis.Interfaces.Socket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Interfaces.Clients.SpotApi
{
    public interface IKrakenSocketClientSpotApiShared :
        ITickerSocketClient,
        ITradeSocketClient,
        IBookTickerSocketClient,
        IKlineSocketClient
        //Can be implemented with V2 websockets
        //IBalanceSocketClient, 
        //ISpotOrderSocketClient
    {
    }
}
