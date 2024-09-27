using CryptoExchange.Net.SharedApis;
using Kraken.Net.Interfaces.Clients.SpotApi;

namespace Kraken.Net.Clients.FuturesApi
{
    internal partial class KrakenRestClientFuturesApi : IKrakenRestClientFuturesApiShared
    {
        public string Exchange => KrakenExchange.ExchangeName;
        public TradingMode[] SupportedTradingModes { get; } = new TradingMode[] {  };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        // TODO implement after library update

    }
}
