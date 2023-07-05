using Kraken.Net.Interfaces;
using Kraken.Net.UnitTests.TestImplementations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;
using Kraken.Net.Objects;
using Kraken.Net.Interfaces.Clients;

namespace Kraken.Net.UnitTests
{
    [TestFixture]
    public class JsonTests
    {
        private JsonToObjectComparer<IKrakenRestClient> _comparer = new JsonToObjectComparer<IKrakenRestClient>((json) => TestHelpers.CreateResponseClient(json, x =>
        {
            x.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("1234", "1234");
            x.OutputOriginalData = true;
            x.SpotOptions.RateLimiters = new List<IRateLimiter>();
            x.FuturesOptions.RateLimiters = new List<IRateLimiter>();
        }));
        
        [Test]
        public async Task ValidateSpotAccountCalls()
        {   
            await _comparer.ProcessSubject("SpotAccount", c => c.SpotApi.Account,
                useNestedJsonPropertyForAllCompare: new List<string> { "result" },
                useNestedJsonPropertyForCompare: new Dictionary<string, string> {
                    { "GetOrderBookAsync", "XXBTZUSD" } ,
                }
                );
        }

        [Test]
        public async Task ValidateSpotExchangeDataCalls()
        {
            await _comparer.ProcessSubject("SpotExchangeData", c => c.SpotApi.ExchangeData,
                useNestedJsonPropertyForAllCompare: new List<string> { "result" },
                useNestedJsonPropertyForCompare: new Dictionary<string, string> {
                    { "GetOrderBookAsync", "XXBTZUSD" } ,
                }
                );
        }

        [Test]
        public async Task ValidateSpotTradingCalls()
        {
            await _comparer.ProcessSubject("SpotTrading", c => c.SpotApi.Trading,
                useNestedJsonPropertyForAllCompare: new List<string> { "result" },
                useNestedJsonPropertyForCompare: new Dictionary<string, string> {
                    { "GetOrderBookAsync", "XXBTZUSD" } ,
                }
                );
        }

        [Test]
        public async Task ValidateFuturesAccountCalls()
        {
            await _comparer.ProcessSubject("FuturesAccount", c => c.FuturesApi.Account,
                useNestedJsonPropertyForCompare: new Dictionary<string, string>
                {
                    { "GetBalancesAsync", "accounts" },
                    { "GetFeeScheduleVolumeAsync", "volumesByFeeSchedule" },
                    { "GetPnlCurrencyAsync", "preferences" }
                });
        }

        [Test]
        public async Task ValidateFuturesExchangeDataCalls()
        {
            await _comparer.ProcessSubject("FuturesExchangeData", c => c.FuturesApi.ExchangeData,
                useNestedJsonPropertyForCompare: new Dictionary<string, string>
                {
                    { "GetFeeSchedulesAsync", "feeSchedules" },
                    { "GetHistoricalFundingRatesAsync", "rates" },
                    { "GetOrderBookAsync", "orderBook" },
                    { "GetSymbolsAsync", "instruments" },
                    { "GetSymbolStatusAsync", "instrumentStatus" },
                    { "GetTickersAsync", "tickers" },
                    { "GetTradesAsync", "history" }
                });
        }

        [Test]
        public async Task ValidateFuturesTradingDataCalls()
        {
            await _comparer.ProcessSubject("FuturesTrading", c => c.FuturesApi.Trading,
                useNestedJsonPropertyForCompare: new Dictionary<string, string>
                {
                    { "CancelAllOrderAfterAsync", "status" },
                    { "CancelAllOrdersAsync", "cancelStatus" },
                    { "CancelOrderAsync", "cancelStatus" },
                    { "EditOrderAsync", "editStatus" },
                    { "GetLeverageAsync", "leveragePreferences" },
                    { "GetOpenOrdersAsync", "openOrders" },
                    { "GetOpenPositionsAsync", "openPositions" },
                    { "GetOrdersAsync", "orders" },
                    { "GetSelfTradeStrategyAsync", "strategy" },
                    { "GetUserTradesAsync", "fills" },
                    { "PlaceOrderAsync", "sendStatus" },
                },
                ignoreProperties: new Dictionary<string, List<string>>
                {
                    { "PlaceOrderAsync", new List<string>{ "takerReducedQuantity" } }
                });
        }
    }
}
