using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Testing;
using Kraken.Net.Clients;
using Kraken.Net.Objects.Models.Socket;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kraken.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSpotSubscriptions()
        {
            var client = new KrakenSocketClient(opts =>
            {
                opts.ApiCredentials = new ApiCredentials("MTIz", "MTIz");
            });
            var tester = new SocketSubscriptionValidator<KrakenSocketClient>(client, "Subscriptions/Spot", "wss://ws-auth.kraken.com", "data");
            await tester.ValidateAsync<KrakenTickerUpdate>((client, handler) => client.SpotApi.SubscribeToTickerUpdatesAsync("ETH/USDT", handler), "Ticker");
            await tester.ValidateAsync<KrakenKlineUpdate[]>((client, handler) => client.SpotApi.SubscribeToKlineUpdatesAsync("ETH/USDT", Enums.KlineInterval.FiveMinutes, handler), "Kline", ignoreProperties: new List<string> { "timestamp" });
            await tester.ValidateAsync<KrakenTradeUpdate[]>((client, handler) => client.SpotApi.SubscribeToTradeUpdatesAsync("ETH/USDT", handler), "Trades");
            await tester.ValidateAsync<KrakenBookUpdate>((client, handler) => client.SpotApi.SubscribeToAggregatedOrderBookUpdatesAsync("ETH/USDT", 10, handler), "AggBook");
        }
    }
}
