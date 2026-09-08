using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects.Models;
using System.Linq;
using System.Timers;

namespace Kraken.Net.Clients.SpotApi
{
    internal partial class KrakenRestClientSpotSharedApi : 
        SharedApiBase,
        IKrakenRestClientSpotApiShared,
        IKrakenRestClientSpotSharedApi
    {
        private readonly KrakenRestClientSpotApi _api;

        private const string _topicId = "KrakenSpot";
        private const string _exchangeName = "Kraken";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(KrakenExchange.Metadata, this);

        private static readonly HashSet<string> _exchangeFiat = ["USD", "EUR", "GBP", "CHF", "AUD", "CAD", "JPY"];

        public KrakenRestClientSpotSharedApi(KrakenRestClientSpotApi api)
            : base(
                  SharedTransport.Rest,
                  api.Exchange,
                  [TradingMode.Spot],
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetKlinesOptions,
                GetSpotSymbolsOptions,
                GetTickerOptions,
                GetAllTickersOptions,
                GetBookTickerOptions,
                GetRecentTradesOptions,
                GetBalancesOptions,
                PlaceSpotOrderOptions,
                GetSpotOrderOptions,
                GetOpenSpotOrdersOptions,
                GetClosedSpotOrdersOptions,
                GetSpotUserTradeHistoryOptions,
                GetSpotOrderTradesOptions,
                CancelSpotOrderOptions,
                GetSpotOrderByClientOrderIdOptions,
                CancelSpotOrderByClientOrderIdOptions,
                GetAssetOptions,
                GetAllAssetsOptions,
                GetDepositAddressesOptions,
                GetDepositHistoryOptions,
                GetOrderBookOptions,
                GetWithdrawalHistoryOptions,
                WithdrawOptions,
                GetFeeOptions
                );
        }
    }
}
