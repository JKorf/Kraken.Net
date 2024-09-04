using Kraken.Net;
using Kraken.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis.Interfaces;
using CryptoExchange.Net.SharedApis.RequestModels;
using CryptoExchange.Net.SharedApis.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kraken.Net.Clients.FuturesApi
{
    internal partial class KrakenRestClientFuturesApi : IKrakenRestClientFuturesApiShared
    {
        public string Exchange => KrakenExchange.ExchangeName;
        public ApiType[] SupportedApiTypes { get; } = new ApiType[] {  };

        // TODO implement after library update

    }
}
