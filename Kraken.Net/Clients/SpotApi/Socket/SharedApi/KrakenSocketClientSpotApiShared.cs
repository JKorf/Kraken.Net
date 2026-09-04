using Kraken.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Sockets;
using Kraken.Net.Enums;

namespace Kraken.Net.Clients.SpotApi
{
    internal partial class KrakenSocketClientSpotSharedApi :
        SharedApiBase,
        IKrakenSocketClientSpotApiShared,
        IKrakenSocketClientSpotSharedApi
    {
        private readonly KrakenSocketClientSpotApi _api;

        private const string _topicId = "KrakenSpot";
        private const string _exchangeName = "Kraken";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(KrakenExchange.Metadata, this);

        public KrakenSocketClientSpotSharedApi(KrakenSocketClientSpotApi api)
            : base(
                  SharedTransport.Socket,
                  api.Exchange,
                  new[] { TradingMode.Spot },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeTickerOptions,
                SubscribeTradeOptions,
                SubscribeBookTickerOptions,
                SubscribeBalanceOptions,
                SubscribeKlineOptions,
                SubscribeSpotOrderOptions,
                PlaceSpotOrderOptions,
                CancelSpotOrderOptions
            );
        }

        private void ClearSymbolNameIfIncorrect(SharedSymbolRequest request)
        {
            if (request.Symbol?.SymbolName != null && !request.Symbol.SymbolName.Contains('/') && request.Symbol.BaseAsset != null)
                request.Symbol.SymbolName = null;

            if (request.Symbols?.Length > 0)
            {
                foreach (var symbol in request.Symbols)
                {
                    if (symbol?.SymbolName != null && !symbol.SymbolName.Contains('/') && symbol.BaseAsset != null)
                        symbol.SymbolName = null;
                }
            }
        }
    }
}
