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
        }));
        
        [Test]
        public async Task ValidateSpotAccountCalls()
        {   
            await _comparer.ProcessSubject("Account", c => c.SpotApi.Account,
                useNestedJsonPropertyForAllCompare: new List<string> { "result" },
                useNestedJsonPropertyForCompare: new Dictionary<string, string> {
                    { "GetOrderBookAsync", "XXBTZUSD" } ,
                }
                );
        }

        [Test]
        public async Task ValidateSpotExchangeDataCalls()
        {
            await _comparer.ProcessSubject("ExchangeData", c => c.SpotApi.ExchangeData,
                useNestedJsonPropertyForAllCompare: new List<string> { "result" },
                useNestedJsonPropertyForCompare: new Dictionary<string, string> {
                    { "GetOrderBookAsync", "XXBTZUSD" } ,
                }
                );
        }

        [Test]
        public async Task ValidateSpotTradingCalls()
        {
            await _comparer.ProcessSubject("Trading", c => c.SpotApi.Trading,
                useNestedJsonPropertyForAllCompare: new List<string> { "result" },
                useNestedJsonPropertyForCompare: new Dictionary<string, string> {
                    { "GetOrderBookAsync", "XXBTZUSD" } ,
                }
                );
        }

    }
}
