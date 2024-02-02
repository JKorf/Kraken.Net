using Kraken.Net.Clients;
using Kraken.Net.Interfaces.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoExchange.Net.Clients
{
    public static class CryptoExchangeClientExtensions
    {
        public static IKrakenRestClient Kraken(this ICryptoExchangeClient baseClient) => baseClient.TryGet<IKrakenRestClient>() ?? new KrakenRestClient();
    }
}
