using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Testing;
using CryptoExchange.Net.Testing.Implementations;
using Kraken.Net.Clients;
using Kraken.Net.Clients.SpotApi;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Interfaces.Clients.SpotApi;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Futures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kraken.Net.UnitTests
{
    [TestFixture]
    public class KrakenClientTests
    {
        [Test]
        public void CheckSignatureExample()
        {
            var authProvider = new KrakenAuthenticationProvider(
                new KrakenCredentials().WithSpot("XX", "kQH5HW/8p1uGOVjbgWA7FunAmGO8lsSUXNsu3eow76sz84Q18fWxnyRzBHCd3pd5nE9qa99HAZtuZuj6F1huXg=="),
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
                new Parameters(KrakenExchange._parameterSerializationSettings)
                {
                    { "ordertype", "limit" },
                    { "pair", "XBTUSD" },
                    { "price", "37500" },
                    { "type", "buy" },
                    { "volume", "1.25" },
                });
        }

        [Test]
        public void ProcessRequest_ReusedParameters_ReplacesNonce()
        {
            var nonceProvider = new Mock<INonceProvider>();
            nonceProvider.SetupSequence(x => x.GetNonce())
                .Returns(1)
                .Returns(2);
            var authProvider = new KrakenAuthenticationProvider(
                new KrakenCredentials().WithSpot("key", "kQH5HW/8p1uGOVjbgWA7FunAmGO8lsSUXNsu3eow76sz84Q18fWxnyRzBHCd3pd5nE9qa99HAZtuZuj6F1huXg=="),
                nonceProvider.Object);
            var client = (RestApiClient)new KrakenRestClient().SpotApi;
            var parameters = new Parameters(KrakenExchange._parameterSerializationSettings);
            var requestDefinition = new RequestDefinition("https://api.kraken.com", "/0/private/QueryOrders", HttpMethod.Post)
            {
                Authenticated = true
            };

            RestRequestConfiguration CreateRequest() => new(
                requestDefinition,
                null,
                parameters,
                new Dictionary<string, string>(),
                HttpMethodParameterPosition.InBody,
                RequestBodyFormat.FormData);

            authProvider.ProcessRequest(client, CreateRequest());

            Assert.That(parameters["nonce"], Is.EqualTo(1));
            Assert.DoesNotThrow(() => authProvider.ProcessRequest(client, CreateRequest()));
            Assert.That(parameters["nonce"], Is.EqualTo(2));
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
                    { "ApiCredentials:Spot:Key", "123" },
                    { "ApiCredentials:Spot:Secret", "4/dpxb3iT4tp/ZCVEwSnEsLxx0bqyhLpdfOpc6fn7OR8+UClSV5n9E6aSS8MPtnRfp32bAb0nmbRn6H8ndwLUQ==" },
                    { "Socket:ApiCredentials:Spot:Key", "456" },
                    { "Socket:ApiCredentials:Spot:Secret", "4/dpxb3iT4tp/ZCVEwSnEsLxx0bqyhLpdfOpc6fn7OR8+UClSV5n9E6aSS8MPtnRfp32bAb0nmbRn6H8ndwLUQ==" },
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
            Assert.That(((KrakenRestClientSpotApi)restClient.SpotApi).AuthenticationProvider.Key, Is.EqualTo("123"));
            Assert.That(((KrakenSocketClientSpotApi)socketClient.SpotApi).AuthenticationProvider.Key, Is.EqualTo("456"));
            Assert.That(((BaseApiClient)restClient.SpotApi).ClientOptions.Proxy.Host, Is.EqualTo("host"));
            Assert.That(((BaseApiClient)restClient.SpotApi).ClientOptions.Proxy.Port, Is.EqualTo(80));
            Assert.That(((BaseApiClient)socketClient.SpotApi).ClientOptions.Proxy.Host, Is.EqualTo("host2"));
            Assert.That(((BaseApiClient)socketClient.SpotApi).ClientOptions.Proxy.Port, Is.EqualTo(81));
        }

        [Test]
        public void TestSpotSharedApiDiscoveryMatchesAggregate()
        {
            var (missingOptions, missingInterfaces) = TestHelpers.ValidateSharedApi(new KrakenRestClient().SpotApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }

        [Test]
        public void TestFuturesSharedApiDiscoveryMatchesAggregate()
        {
            var (missingOptions, missingInterfaces) = TestHelpers.ValidateSharedApi(new KrakenRestClient().FuturesApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }

        [Test]
        public void TestSpotRestSharedApiDoesntHaveUnsupportedCapabilities()
        {
            var unsupported = TestHelpers.ValidateUnsupportedCapabilities(new KrakenRestClient().SpotApi.SharedApi);

            Assert.That(unsupported, Is.Empty);
        }

        [Test]
        public void TestSpotSocketSharedApiDoesntHaveUnsupportedCapabilities()
        {
            var unsupported = TestHelpers.ValidateUnsupportedCapabilities(new KrakenSocketClient().SpotApi.SharedApi);

            Assert.That(unsupported, Is.Empty);
        }

        [Test]
        public void TestFuturesRestSharedApiDoesntHaveUnsupportedCapabilities()
        {
            var unsupported = TestHelpers.ValidateUnsupportedCapabilities(new KrakenRestClient().FuturesApi.SharedApi);

            Assert.That(unsupported, Is.Empty);
        }

        [Test]
        public void TestFuturesSocketSharedApiDoesntHaveUnsupportedCapabilities()
        {
            var unsupported = TestHelpers.ValidateUnsupportedCapabilities(new KrakenSocketClient().FuturesApi.SharedApi);

            Assert.That(unsupported, Is.Empty);
        }
    }
}
