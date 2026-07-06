using Kraken.Net.Clients;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Kraken.Net.Objects.Options;
using Kraken.Net.SymbolOrderBooks;
using CryptoExchange.Net.Objects.Errors;
using System.Collections.Generic;

namespace Kraken.Net.UnitTests
{
    [NonParallelizable]
    internal class KrakenRestIntegrationTests : RestIntegrationTest<KrakenRestClient>
    {
        public override bool Run { get; set; } = false;

        public KrakenRestIntegrationTests()
        {
        }

        public override KrakenRestClient GetClient(ILoggerFactory loggerFactory)
        {
            var spotKey = Environment.GetEnvironmentVariable("APIKEY");
            var spotSec = Environment.GetEnvironmentVariable("APISECRET");
            var futuresKey = Environment.GetEnvironmentVariable("FUTURESAPIKEY");
            var futuresSec = Environment.GetEnvironmentVariable("FUTURESAPISECRET");

            Authenticated = spotKey != null && spotSec != null && futuresKey != null && futuresSec != null;
            return new KrakenRestClient(null, loggerFactory, Options.Create(new KrakenRestOptions
            {
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new KrakenCredentials()
                                                    .WithSpot(spotKey, spotSec)
                                                    .WithFutures(futuresKey, futuresSec): null
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().SpotApi.ExchangeData.GetTickerAsync("TSTTST", default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.ErrorType, Is.EqualTo(ErrorType.UnknownSymbol));
        }

        [Test]
        public async Task TestSpotAccount()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetBalancesAsync(default, default, default), true, "result");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetAvailableBalancesAsync(default, default), true, "result");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetTradeBalanceAsync(default, default, default), true, "result");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetOpenPositionsAsync(default, default, default), true, "result");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetLedgerInfoAsync(default, default, default, default, default, default, default, default), true, "result");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetTradeVolumeAsync(new[] { "ETHUSDT" }, default, default), true, "result");
            await RunAndCheckResult(client => client.SpotApi.Account.GetDepositMethodsAsync("ETH", default, default, default), true);
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetDepositStatusAsync(default, default, default, default, default), true, "result");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetWithdrawAddressesAsync(default, default, default, default, default, default), true, "result");
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetWithdrawMethodsAsync(default, default, default, default), true, "result");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestSpotExchangeData()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetServerTimeAsync(default), false);
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetSystemStatusAsync(default), false, "result");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetAssetsAsync(default, default, default, default), false, "result");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetSymbolsAsync(default, default, default, default, default, default), false, "result");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetTickerAsync("ETHUSDT", default), false, "result");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetTickersAsync(default, default, default), false, "result");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetKlinesAsync("ETHUSDT", Enums.KlineInterval.OneDay, default, default, default), false, "result.ETHUSDT");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetOrderBookAsync("ETHUSDT", default, default, default), false, "result.ETHUSDT");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetTradeHistoryAsync("ETHUSDT", default, default, default, default), false, "result.ETHUSDT");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetRecentSpreadAsync("ETHUSDT", default, default, default), false, "result.ETHUSDT");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestSpotTrading()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.SpotApi.Trading.GetOpenOrdersAsync(default, default, default, default), true, "result", ignoreProperties: ["price"]);
            await RunAndCheckResult(warnings, client => client.SpotApi.Trading.GetClosedOrdersAsync(default, default, default, default, default, default, default, default, default), true, "result", ignoreProperties: ["price"]);
            await RunAndCheckResult(warnings, client => client.SpotApi.Trading.GetUserTradesAsync(default, default, default, default, default, default), true, "result");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestFuturesAccount()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.FuturesApi.Account.GetBalancesAsync(default), true, "result");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Account.GetPnlCurrencyAsync(default), true, "result");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Account.GetAccountLogAsync(default, default, default, default, default, default, default, default), true, "logs");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestFuturesExchangeData()
        {
            var warnings = new List<Exception>();
            //await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetPlatformNotificationsAsync(default), true);
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetHistoricalFundingRatesAsync("PF_ETHUSD", default), false, "result");
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetSymbolsAsync(default), false, "result");
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetSymbolStatusAsync(default), false, "result");
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetTradesAsync("PF_ETHUSD", default, default), false, "result");
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetOrderBookAsync("PF_ETHUSD", default), false, "result");
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetTickersAsync(default), false, "result");
            await RunAndCheckResult(warnings, client => client.FuturesApi.ExchangeData.GetKlinesAsync(Enums.TickType.Trade, "PF_ETHUSD", Enums.FuturesKlineInterval.OneDay, default, default, default, default), false, "candles");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestFuturesTrading()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetUserTradesAsync(default, default), true, "result");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetSelfTradeStrategyAsync(default), true, "result");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetOpenPositionsAsync(default), true, "result");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetLeverageAsync(default), true, "result");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetOpenOrdersAsync(default), true, "result");
            await RunAndCheckResult(warnings, client => client.FuturesApi.Trading.GetExecutionEventsAsync(default, default, default, default, default, default), true, "elements");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestOrderBooks()
        {
            await TestOrderBook(new KrakenSpotSymbolOrderBook("ETH/USDT"));
            await TestOrderBook(new KrakenFuturesSymbolOrderBook("PF_ETHUSD"));
        }
    }
}
