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

namespace Kraken.Net.UnitTests
{
    [TestFixture]
    public class KrakenClientTests
    {
        //[TestCase()]
        //public void TestConversions()
        //{
        //    var ignoreMethods = new string[]
        //    {
        //        "GetSymbols",
        //        "GetOrderBook",
        //        "GetServerTime",
        //        "GetTickers",
        //        "GetTradeHistory",
        //    };
        //    var defaultParameterValues = new Dictionary<string, object>
        //    {
        //        { "assets", new [] { "XBTUSD" } },
        //        { "symbol", "XBTUSD" },
        //        {"clientOrderId", null }
        //    };

        //    var methods = typeof(KrakenClient).GetMethods(BindingFlags.Public | BindingFlags.Instance);
        //    var callResultMethods = methods.Where(m => m.ReturnType.IsGenericType && m.ReturnType.GetGenericTypeDefinition() == typeof(WebCallResult<>));
        //    foreach (var method in callResultMethods)
        //    {
        //        var name = method.Name;
        //        if (ignoreMethods.Contains(method.Name))
        //            continue;

        //        var expectedType = method.ReturnType.GetGenericArguments()[0];
        //        var expected = typeof(TestHelpers).GetMethod("CreateObjectWithTestParameters").MakeGenericMethod(expectedType).Invoke(null, null);
        //        var parameters = TestHelpers.CreateParametersForMethod(method, defaultParameterValues);
        //        var client = TestHelpers.CreateResponseClient(SerializeExpected(expected), new KrakenClientOptions() { ApiCredentials = new ApiCredentials("Test", "Test"), LogLevel = LogLevel.Debug });

        //        act
        //       var result = method.Invoke(client, parameters);
        //        var callResult = result.GetType().GetProperty("Success").GetValue(result);
        //        var data = result.GetType().GetProperty("Data").GetValue(result);

        //        assert
        //        Assert.AreEqual(true, callResult);
        //        Assert.IsTrue(TestHelpers.AreEqual(expected, data), method.Name);
        //    }
        //}


        [TestCase()]
        public async Task TestErrorResult_Should_ResultInFailedCall()
        {
            // arrange
            var client = TestHelpers.CreateAuthResponseClient($"{{\"error\": [\"first error\", \"another error\"], \"result\": null}}");

            // act
            var result = await client.SpotApi.ExchangeData.GetSymbolsAsync();

            // assert
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.Error!.Message.Contains("first error"));
            Assert.IsTrue(result.Error!.Message.Contains("another error"));
        }

        [TestCase()]
        public async Task TestHttpError_Should_ResultInFailedCall()
        {
            // arrange
            var client = TestHelpers.CreateAuthResponseClient($"Error request", System.Net.HttpStatusCode.BadRequest);

            // act
            var result = await client.SpotApi.ExchangeData.GetSymbolsAsync();

            // assert
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.ResponseStatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.IsTrue(result.Error!.ToString().Contains("Error request"));
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

        [TestCase("BTC-USDT", false)]
        [TestCase("NANO-USDT", false)]
        [TestCase("NANO-BTC", false)]
        [TestCase("ETH-BTC", false)]
        [TestCase("BE-ETC", false)]
        [TestCase("NANO-USDTD", false)]
        [TestCase("BTCUSDT", true)]
        [TestCase("BTCUSDTA", true)]
        [TestCase("BTCUSDTAF", true)]
        [TestCase("BTCUSD", true)]
        [TestCase("NANOUSDT.d", true)]
        public void CheckValidKrakenSymbol(string symbol, bool isValid)
        {
            if (isValid)
                Assert.DoesNotThrow(() => symbol.ValidateKrakenSymbol());
            else
                Assert.Throws(typeof(ArgumentException), () => symbol.ValidateKrakenSymbol());
        }

        [TestCase("BTC/USDT", true)]
        [TestCase("NANO/USDT", true)]
        [TestCase("NANO/BTC", true)]
        [TestCase("ETH/BTC", true)]
        [TestCase("BE/ETC", true)]
        [TestCase("B/ETC", true)]
        [TestCase("/ETC", false)]
        [TestCase("NANO/USDTD", true)]
        [TestCase("BTCUSDT", false)]
        [TestCase("BTCUSD", false)]
        public void CheckValidKrakenWebsocketSymbol(string symbol, bool isValid)
        {
            if (isValid)
                Assert.DoesNotThrow(() => symbol.ValidateKrakenWebsocketSymbol());
            else
                Assert.Throws(typeof(ArgumentException), () => symbol.ValidateKrakenWebsocketSymbol());
        }

        [Test]
        public void CheckRestInterfaces()
        {
            var assembly = Assembly.GetAssembly(typeof(KrakenRestClientSpotApi));
            var ignore = new string[] { "IKrakenRestClientSpot" };
            var clientInterfaces = assembly.GetTypes().Where(t => t.Name.StartsWith("IKrakenRestClientSpot") && !ignore.Contains(t.Name));

            foreach (var clientInterface in clientInterfaces)
            {
                var implementation = assembly.GetTypes().Single(t => t.IsAssignableTo(clientInterface) && t != clientInterface);
                int methods = 0;
                foreach (var method in implementation.GetMethods().Where(m => m.ReturnType.IsAssignableTo(typeof(Task))))
                {
                    var interfaceMethod = clientInterface.GetMethod(method.Name, method.GetParameters().Select(p => p.ParameterType).ToArray());
                    Assert.NotNull(interfaceMethod, $"Missing interface for method {method.Name} in {implementation.Name} implementing interface {clientInterface.Name}");
                    methods++;
                }
                Debug.WriteLine($"{clientInterface.Name} {methods} methods validated");
            }
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
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual(System.Net.HttpStatusCode.BadGateway, result.ResponseStatusCode);
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
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Error);
            Assert.IsTrue(result.Error.Message == "Error occured");
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
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual(System.Net.HttpStatusCode.BadGateway, result.ResponseStatusCode);
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
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Error);
            Assert.IsTrue(result.Error.Message == "Error occured");
        }
    }
}
