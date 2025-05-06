using NUnit.Framework;
using Kraken.Net.UnitTests.TestImplementations;
using System.Threading.Tasks;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Clients;
using System.Collections.Generic;
using Kraken.Net.Objects.Models.Futures;
using NUnit.Framework.Legacy;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Authentication;
using System.Net.Http;
using CryptoExchange.Net.Testing.Implementations;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CryptoExchange.Net.Objects;
using Kraken.Net.Interfaces.Clients;

namespace Kraken.Net.UnitTests
{
    [TestFixture]
    public class KrakenClientTests
    {
        [TestCase()]
        public async Task TestErrorResult_Should_ResultInFailedCall()
        {
            // arrange
            var client = TestHelpers.CreateAuthResponseClient($"{{\"error\": [\"first error\", \"another error\"], \"result\": null}}");

            // act
            var result = await client.SpotApi.ExchangeData.GetSymbolsAsync();

            // assert
            ClassicAssert.IsFalse(result.Success);
            Assert.That(result.Error!.Message.Contains("first error"));
            Assert.That(result.Error!.Message.Contains("another error"));
        }

        [TestCase()]
        public async Task TestHttpError_Should_ResultInFailedCall()
        {
            // arrange
            var client = TestHelpers.CreateAuthResponseClient($"Error request", System.Net.HttpStatusCode.BadRequest);

            // act
            var result = await client.SpotApi.ExchangeData.GetSymbolsAsync();

            // assert
            ClassicAssert.IsFalse(result.Success);
            Assert.That(result.ResponseStatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        public string SerializeExpected<T>(T data)
        {
            var result = new KrakenResult<T>()
            {
                Result = data,
                Error = new string[] {}
            };

            return JsonSerializer.Serialize(result);
        }

        [TestCase()]
        public async Task ReceivingHttpErrorWithNoJsonOnSpotApi_Should_ReturnErrorAndNotSuccess()
        {
            // arrange
            var client = TestHelpers.CreateClient();
            TestHelpers.SetResponse((KrakenRestClient)client, "", System.Net.HttpStatusCode.BadGateway);

            // act
            var result = await client.SpotApi.ExchangeData.GetTickersAsync();

            // assert
            ClassicAssert.IsFalse(result.Success);
            ClassicAssert.IsNotNull(result.Error);
            Assert.That(System.Net.HttpStatusCode.BadGateway == result.ResponseStatusCode);
        }

        [TestCase()]
        public async Task ReceivingErrorOnSpotApi_Should_ReturnErrorAndNotSuccess()
        {
            // arrange
            var client = TestHelpers.CreateClient();
            var resultObj = new KrakenResult()
            {
                Error = new string[]
                {
                    "Error occured"
                }
            };

            TestHelpers.SetResponse((KrakenRestClient)client, JsonSerializer.Serialize(resultObj));

            // act
            var result = await client.SpotApi.ExchangeData.GetAssetsAsync();

            // assert
            ClassicAssert.IsFalse(result.Success);
            ClassicAssert.IsNotNull(result.Error);
            Assert.That(result.Error.Message == "Error occured");
        }

        [TestCase()]
        public async Task ReceivingHttpErrorWithNoJsonOnFuturesApi_Should_ReturnErrorAndNotSuccess()
        {
            // arrange
            var client = TestHelpers.CreateClient();
            TestHelpers.SetResponse((KrakenRestClient)client, "", System.Net.HttpStatusCode.BadGateway);

            // act
            var result = await client.FuturesApi.ExchangeData.GetTickersAsync();

            // assert
            ClassicAssert.IsFalse(result.Success);
            ClassicAssert.IsNotNull(result.Error);
            Assert.That(System.Net.HttpStatusCode.BadGateway == result.ResponseStatusCode);
        }

        [TestCase()]
        public async Task ReceivingErrorOnFuturesApi_Should_ReturnErrorAndNotSuccess()
        {
            // arrange
            var client = TestHelpers.CreateClient();
            var resultObj = new KrakenFuturesResult()
            {
                Error = "Error occured"
            };

            TestHelpers.SetResponse((KrakenRestClient)client, JsonSerializer.Serialize(resultObj), System.Net.HttpStatusCode.BadRequest);

            // act
            var result = await client.FuturesApi.ExchangeData.GetTickersAsync();

            // assert
            ClassicAssert.IsFalse(result.Success);
            ClassicAssert.IsNotNull(result.Error);
            Assert.That(result.Error.Message == "Error occured");
        }

        [Test]
        public void CheckSignatureExample()
        {
            var authProvider = new KrakenAuthenticationProvider(
                new ApiCredentials("XX", "kQH5HW/8p1uGOVjbgWA7FunAmGO8lsSUXNsu3eow76sz84Q18fWxnyRzBHCd3pd5nE9qa99HAZtuZuj6F1huXg=="),
                new TestNonceProvider(1616492376594)
                );
            var client = (RestApiClient)new KrakenRestClient().SpotApi;

            CryptoExchange.Net.Testing.TestHelpers.CheckSignature(
                client,
                authProvider,
                HttpMethod.Get,
                "/0/private/AddOrder",
                (uriParams, bodyParams, headers) =>
                {
                    return headers["API-Sign"].ToString();
                },
                "4/dpxb3iT4tp/ZCVEwSnEsLxx0bqyhLpdfOpc6fn7OR8+UClSV5n9E6aSS8MPtnRfp32bAb0nmbRn6H8ndwLUQ==",
                new Dictionary<string, object>
                {
                    { "ordertype", "limit" },
                    { "pair", "XBTUSD" },
                    { "price", "37500" },
                    { "type", "buy" },
                    { "volume", "1.25" },
                });
        }

        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<KrakenRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<KrakenSocketClient>();
        }

        [Test]
        [TestCase(TradeEnvironmentNames.Live, "https://api.kraken.com")]
        [TestCase("", "https://api.kraken.com")]
        public void TestConstructorEnvironments(string environmentName, string expected)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Kraken:Environment:Name", environmentName },
                }).Build();

            var collection = new ServiceCollection();
            collection.AddKraken(configuration.GetSection("Kraken"));
            var provider = collection.BuildServiceProvider();

            var client = provider.GetRequiredService<IKrakenRestClient>();

            var address = client.SpotApi.BaseAddress;

            Assert.That(address, Is.EqualTo(expected));
        }

        [Test]
        public void TestConstructorNullEnvironment()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Kraken", null },
                }).Build();

            var collection = new ServiceCollection();
            collection.AddKraken(configuration.GetSection("Kraken"));
            var provider = collection.BuildServiceProvider();

            var client = provider.GetRequiredService<IKrakenRestClient>();

            var address = client.SpotApi.BaseAddress;

            Assert.That(address, Is.EqualTo("https://api.kraken.com"));
        }

        [Test]
        public void TestConstructorApiOverwriteEnvironment()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Kraken:Environment:Name", "test" },
                    { "Kraken:Rest:Environment:Name", "live" },
                }).Build();

            var collection = new ServiceCollection();
            collection.AddKraken(configuration.GetSection("Kraken"));
            var provider = collection.BuildServiceProvider();

            var client = provider.GetRequiredService<IKrakenRestClient>();

            var address = client.SpotApi.BaseAddress;

            Assert.That(address, Is.EqualTo("https://api.kraken.com"));
        }

        [Test]
        public void TestConstructorConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ApiCredentials:Key", "123" },
                    { "ApiCredentials:Secret", "4/dpxb3iT4tp/ZCVEwSnEsLxx0bqyhLpdfOpc6fn7OR8+UClSV5n9E6aSS8MPtnRfp32bAb0nmbRn6H8ndwLUQ==" },
                    { "Socket:ApiCredentials:Key", "456" },
                    { "Socket:ApiCredentials:Secret", "4/dpxb3iT4tp/ZCVEwSnEsLxx0bqyhLpdfOpc6fn7OR8+UClSV5n9E6aSS8MPtnRfp32bAb0nmbRn6H8ndwLUQ==" },
                    { "Rest:OutputOriginalData", "true" },
                    { "Socket:OutputOriginalData", "false" },
                    { "Rest:Proxy:Host", "host" },
                    { "Rest:Proxy:Port", "80" },
                    { "Socket:Proxy:Host", "host2" },
                    { "Socket:Proxy:Port", "81" },
                }).Build();

            var collection = new ServiceCollection();
            collection.AddKraken(configuration);
            var provider = collection.BuildServiceProvider();

            var restClient = provider.GetRequiredService<IKrakenRestClient>();
            var socketClient = provider.GetRequiredService<IKrakenSocketClient>();

            Assert.That(((BaseApiClient)restClient.SpotApi).OutputOriginalData, Is.True);
            Assert.That(((BaseApiClient)socketClient.SpotApi).OutputOriginalData, Is.False);
            Assert.That(((BaseApiClient)restClient.SpotApi).AuthenticationProvider.ApiKey, Is.EqualTo("123"));
            Assert.That(((BaseApiClient)socketClient.SpotApi).AuthenticationProvider.ApiKey, Is.EqualTo("456"));
            Assert.That(((BaseApiClient)restClient.SpotApi).ClientOptions.Proxy.Host, Is.EqualTo("host"));
            Assert.That(((BaseApiClient)restClient.SpotApi).ClientOptions.Proxy.Port, Is.EqualTo(80));
            Assert.That(((BaseApiClient)socketClient.SpotApi).ClientOptions.Proxy.Host, Is.EqualTo("host2"));
            Assert.That(((BaseApiClient)socketClient.SpotApi).ClientOptions.Proxy.Port, Is.EqualTo(81));
        }
    }
}
