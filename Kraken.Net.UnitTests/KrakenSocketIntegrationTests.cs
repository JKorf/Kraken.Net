using Kraken.Net.Clients;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Options;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Objects.Models.Socket.Futures;

namespace Kraken.Net.UnitTests
{
    [NonParallelizable]
    internal class KrakenSocketIntegrationTests : SocketIntegrationTest<KrakenSocketClient>
    {
        public override bool Run { get; set; } = true;

        public KrakenSocketIntegrationTests()
        {
        }

        public override KrakenSocketClient GetClient(ILoggerFactory loggerFactory, bool useUpdatedDeserialization)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new KrakenSocketClient(Options.Create(new KrakenSocketOptions
            {
                OutputOriginalData = true,
                UseUpdatedDeserialization = useUpdatedDeserialization,
                ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec) : null
            }), loggerFactory);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task TestSubscriptions(bool useUpdatedDeserialization)
        {
            await RunAndCheckUpdate<KrakenTickerUpdate>(useUpdatedDeserialization, (client, updateHandler) => client.SpotApi.SubscribeToBalanceUpdatesAsync(default , default, default, default), false, true);
            await RunAndCheckUpdate<KrakenTickerUpdate>(useUpdatedDeserialization, (client, updateHandler) => client.SpotApi.SubscribeToTickerUpdatesAsync("ETH/USD", updateHandler, default, default, default), true, false);

            await RunAndCheckUpdate<KrakenFuturesTickerUpdate>(useUpdatedDeserialization, (client, updateHandler) => client.FuturesApi.SubscribeToTickerUpdatesAsync("PF_ETHUSD", updateHandler, default), true, false);
        } 
    }
}
