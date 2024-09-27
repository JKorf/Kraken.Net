using CryptoExchange.Net.SharedApis;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Clients.FuturesApi
{
    internal partial class KrakenSocketClientFuturesApi : IKrakenSocketClientFuturesApiShared
    {
        public string Exchange => KrakenExchange.ExchangeName;
        public TradingMode[] SupportedTradingModes { get; } = new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        // Can be implemented with V2 websockets
    }
}
