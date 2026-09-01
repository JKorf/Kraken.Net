using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using Kraken.Net.Clients.SpotApi;
using Kraken.Net.Enums;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Objects.Models.Socket.Futures;

namespace Kraken.Net.Clients.FuturesApi
{
    internal partial class KrakenSocketClientFuturesSharedApi : 
        SharedApiBase,
        IKrakenSocketClientFuturesApiShared,
        IKrakenSocketClientFuturesSharedApi
    {
        private readonly KrakenSocketClientFuturesApi _api;

        private const string _topicId = "KrakenFutures";
        private const string _exchangeName = "Kraken";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(KrakenExchange.Metadata, this);

        public KrakenSocketClientFuturesSharedApi(KrakenSocketClientFuturesApi api)
            : base(
                  SharedTransport.Socket,
                  api.Exchange,
                  new[] { TradingMode.PerpetualLinear, TradingMode.DeliveryLinear, TradingMode.PerpetualInverse, TradingMode.DeliveryInverse },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeTickerOptions,
                SubscribeTradeOptions,
                SubscribeBookTickerOptions,
                SubscribeBalanceOptions,
                SubscribeFuturesOrderOptions,
                SubscribeUserTradeOptions,
                SubscribePositionOptions
                );
        }







    }
}
