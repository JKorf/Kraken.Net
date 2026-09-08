using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Clients.SpotApi;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Models.Futures;

namespace Kraken.Net.Clients.FuturesApi
{
    internal partial class KrakenRestClientFuturesSharedApi :
        SharedApiBase,
        IKrakenRestClientFuturesApiShared,
        IKrakenRestClientFuturesSharedApi
    {
        private readonly KrakenRestClientFuturesApi _api;

        private const string _topicId = "KrakenFutures";
        private const string _exchangeName = "Kraken";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(KrakenExchange.Metadata, this);

        public KrakenRestClientFuturesSharedApi(KrakenRestClientFuturesApi api) 
            : base(
                  SharedTransport.Rest,
                  api.Exchange,
                  new[] { TradingMode.PerpetualLinear, TradingMode.DeliveryLinear, TradingMode.PerpetualInverse, TradingMode.DeliveryInverse },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetBalancesOptions,
                GetKlinesOptions,
                GetOrderBookOptions,
                GetRecentTradesOptions,
                GetFundingRateHistoryOptions,
                GetFuturesSymbolsOptions,
                GetTickerOptions,
                GetAllTickersOptions,
                GetBookTickerOptions,
                GetMarkPriceKlinesOptions,
                GetOpenInterestOptions,
                GetLeverageOptions,
                SetLeverageOptions,
                PlaceFuturesOrderOptions,
                GetFuturesOrderOptions,
                GetOpenFuturesOrdersOptions,
                GetFuturesUserTradeHistoryOptions,
                CancelFuturesOrderOptions,
                GetPositionsOptions,
                GetFuturesOrderByClientOrderIdOptions,
                CancelFuturesOrderByClientOrderIdOptions,
                GetFeeOptions,
                SetFuturesTpSlOptions,
                CancelFuturesTpSlOptions
                );
        }
    }
}
