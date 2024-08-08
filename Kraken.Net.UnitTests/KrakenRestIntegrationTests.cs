using Kraken.Net.Clients;
using Kraken.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.UnitTests
{
    [NonParallelizable]
    internal class KrakenRestIntegrationTests : RestIntergrationTest<KrakenRestClient>
    {
        public override bool Run { get; set; }

        public KrakenRestIntegrationTests()
        {
        }

        public override KrakenRestClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new KrakenRestClient(null, loggerFactory, opts =>
            {
                opts.OutputOriginalData = true;
                opts.ApiCredentials = Authenticated ? new ApiCredentials(key, sec) : null;
            });
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().SpotApi.ExchangeData.GetTickerAsync("TSTTST", default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.Message, Contains.Substring("EQuery:Unknown asset pair"));
        }

        [Test]
        public async Task TestSpotAccount()
        {
            await RunAndCheckResult(client => client.SpotApi.Account.GetBalancesAsync(default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetAvailableBalancesAsync(default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetTradeBalanceAsync(default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetOpenPositionsAsync(default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetLedgerInfoAsync(default, default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetTradeVolumeAsync(default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetDepositMethodsAsync("ETH", default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetDepositStatusAsync(default, default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetWithdrawAddressesAsync(default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Account.GetWithdrawMethodsAsync(default, default, default, default), true);
        }

        [Test]
        public async Task TestSpotExchangeData()
        {
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetServerTimeAsync(default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetSystemStatusAsync(default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetAssetsAsync(default, default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetSymbolsAsync(default, default, default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetTickerAsync("ETHUSDT", default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetTickersAsync(default, default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetKlinesAsync("ETHUSDT", Enums.KlineInterval.OneDay, default, default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetOrderBookAsync("ETHUSDT", default, default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetTradeHistoryAsync("ETHUSDT", default, default, default), false);
            await RunAndCheckResult(client => client.SpotApi.ExchangeData.GetRecentSpreadAsync("ETHUSDT", default, default), false);
        }

        [Test]
        public async Task TestSpotTrading()
        {
            await RunAndCheckResult(client => client.SpotApi.Trading.GetOpenOrdersAsync(default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Trading.GetClosedOrdersAsync(default, default, default, default, default, default), true);
            await RunAndCheckResult(client => client.SpotApi.Trading.GetUserTradesAsync(default, default, default, default, default, default), true);
        }

        [Test]
        public async Task TestFuturesAccount()
        {
            // Needs different API key then SPOT, currently not supported
            //await RunAndCheckResult(client => client.FuturesApi.Account.GetBalancesAsync(default), true);
            //await RunAndCheckResult(client => client.FuturesApi.Account.GetPnlCurrencyAsync(default), true);
            //await RunAndCheckResult(client => client.FuturesApi.Account.GetAccountLogAsync(default, default, default, default, default, default, default, default), true);
            //await RunAndCheckResult(client => client.FuturesApi.Account.GetFeeScheduleVolumeAsync(default), true);
            //await RunAndCheckResult(client => client.FuturesApi.Account.GetInitialMarginRequirementsAsync("PF_ETHUSD", Enums.FuturesOrderType.Market, Enums.OrderSide.Buy, 1, default, default), true);
            //await RunAndCheckResult(client => client.FuturesApi.Account.GetMaxOrderQuantityAsync("PF_ETHUSD", Enums.FuturesOrderType.Market, default, default), true);
            await Task.CompletedTask;
        }

        [Test]
        public async Task TestFuturesExchangeData()
        {
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetPlatformNotificationsAsync(default), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetHistoricalFundingRatesAsync("PF_ETHUSD", default), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetFeeSchedulesAsync(default), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetSymbolsAsync(default), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetSymbolStatusAsync(default), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetTradesAsync("PF_ETHUSD", default, default), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetOrderBookAsync("PF_ETHUSD", default), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetTickersAsync(default), false);
            await RunAndCheckResult(client => client.FuturesApi.ExchangeData.GetKlinesAsync(Enums.TickType.Trade, "PF_ETHUSD", Enums.FuturesKlineInterval.OneDay, default, default, default), false);
        }

        [Test]
        public async Task TestFuturesTrading()
        {
            // Needs different API key then SPOT, currently not supported
            //await RunAndCheckResult(client => client.FuturesApi.Trading.GetUserTradesAsync(default, default), true);
            //await RunAndCheckResult(client => client.FuturesApi.Trading.GetSelfTradeStrategyAsync(default), true);
            //await RunAndCheckResult(client => client.FuturesApi.Trading.GetOpenPositionsAsync(default), true);
            //await RunAndCheckResult(client => client.FuturesApi.Trading.GetLeverageAsync(default), true);
            //await RunAndCheckResult(client => client.FuturesApi.Trading.GetOpenOrdersAsync(default), true);
            //await RunAndCheckResult(client => client.FuturesApi.Trading.GetExecutionEventsAsync(default, default, default, default, default, default), true);
            await Task.CompletedTask;
        }
    }
}
