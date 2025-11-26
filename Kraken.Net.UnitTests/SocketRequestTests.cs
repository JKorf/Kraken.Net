using Kraken.Net.Clients;
using Kraken.Net.Enums;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Options;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.UnitTests
{
    [TestFixture]
    public class SocketRequestTests
    {
        private KrakenSocketClient CreateClient(bool useUpdatedDeserialization)
        {
            var fact = new LoggerFactory();
            fact.AddProvider(new TraceLoggerProvider());
            var client = new KrakenSocketClient(Options.Create(new KrakenSocketOptions
            {
                OutputOriginalData = true,
                UseUpdatedDeserialization = useUpdatedDeserialization,
                RequestTimeout = TimeSpan.FromSeconds(5),
                ApiCredentials = new ApiCredentials("MTIz", "MTIz"),
                Environment = new KrakenEnvironment("UnitTest", "https://localhost", "wss://localhost", "wss://localhost", "http://localhost", "wss://localhost")
            }), fact);
            return client;
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ValidateExchangeApiCalls(bool useUpdatedDeserialization)
        {
            var tester = new SocketRequestValidator<KrakenSocketClient>("Socket/SpotApi");

            await tester.ValidateAsync(CreateClient(useUpdatedDeserialization), client => client.SpotApi.PlaceOrderAsync("ETH/USDT", OrderSide.Buy, OrderType.Limit, 1), "PlaceOrder", nestedJsonProperty: "result", ignoreProperties: [ ]);
            await tester.ValidateAsync(CreateClient(useUpdatedDeserialization), client => client.SpotApi.PlaceMultipleOrdersAsync("ETH/USDT", new[] { new KrakenSocketOrderRequest() }), "PlaceMultipleOrders", nestedJsonProperty: "result", skipResponseValidation: true);
            await tester.ValidateAsync(CreateClient(useUpdatedDeserialization), client => client.SpotApi.EditOrderAsync("123"), "EditOrder", nestedJsonProperty: "result", ignoreProperties: [ ]);
            await tester.ValidateAsync(CreateClient(useUpdatedDeserialization), client => client.SpotApi.ReplaceOrderAsync("123"), "ReplaceOrder", nestedJsonProperty: "result", ignoreProperties: [ ]);
            await tester.ValidateAsync(CreateClient(useUpdatedDeserialization), client => client.SpotApi.CancelOrdersAsync(["123", "456"]), "CancelOrders", nestedJsonProperty: "result", ignoreProperties: [ ], skipResponseValidation: true);
            await tester.ValidateAsync(CreateClient(useUpdatedDeserialization), client => client.SpotApi.CancelAllOrdersAsync(), "CancelAllOrders", nestedJsonProperty: "result", ignoreProperties: [ ]);
            await tester.ValidateAsync(CreateClient(useUpdatedDeserialization), client => client.SpotApi.CancelAllOrdersAfterAsync(TimeSpan.Zero), "CancelAllOrdersAfter", nestedJsonProperty: "result", ignoreProperties: [ ]);
        }
    }
}
