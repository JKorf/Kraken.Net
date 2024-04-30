using Newtonsoft.Json;
using NUnit.Framework;
using System;
using Kraken.Net.Objects;
using Kraken.Net.UnitTests.TestImplementations;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Objects;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Clients;
using Kraken.Net.Clients.SpotApi;
using Kraken.Net.ExtensionMethods;
using CryptoExchange.Net.Objects.Sockets;
using System.Collections.Generic;
using Kraken.Net.Objects.Models.Futures;
using NUnit.Framework.Legacy;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Authentication;
using System.Net.Http;
using CryptoExchange.Net.Testing.Implementations;

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
            Assert.That(result.Error!.ToString().Contains("Error request"));
        }

        public string SerializeExpected<T>(T data)
        {
            var result = new KrakenResult<T>()
            {
                Result = data,
                Error = new string[] {}
            };

            return JsonConvert.SerializeObject(result);
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
                Error = new List<string>
                {
                    "Error occured"
                }
            };

            TestHelpers.SetResponse((KrakenRestClient)client, JsonConvert.SerializeObject(resultObj));

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

            TestHelpers.SetResponse((KrakenRestClient)client, JsonConvert.SerializeObject(resultObj), System.Net.HttpStatusCode.BadRequest);

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
    }
}
