using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Kraken.Net.Clients;
using Kraken.Net.UnitTests.TestImplementations;
using Kucoin.Net.UnitTests.TestImplementations;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Kraken.Net.UnitTests
{
    [TestFixture]
    public class KrakenSocketClientTests
    {
        [Test]
        public async Task Subscribe_Should_SucceedIfAckResponse()
        {
            // arrange
            var socket = new TestSocket();
            socket.CanConnect = true;
            var client = TestHelpers.CreateSocketClient(socket);

            // act
            var subTask = client.SpotApi.SubscribeToTickerUpdatesAsync("XBT/EUR", test => { });
            await Task.Delay(10);
            var id = JToken.Parse(socket.LastSendMessage!)["reqid"];
            socket.InvokeMessage($"{{\"channelID\": 1, \"status\": \"subscribed\", \"reqid\":{id}}}");
            var subResult = subTask.Result;

            // assert
            Assert.IsTrue(subResult.Success);
        }

        [Test]
        public async Task Subscribe_Should_FailIfNotAckResponse()
        {
            // arrange
            var socket = new TestSocket();
            socket.CanConnect = true;
            var client = TestHelpers.CreateSocketClient(socket);

            // act
            var subTask = client.SpotApi.SubscribeToTickerUpdatesAsync("XBT/EUR", test => { });
            await Task.Delay(10);
            var id = JToken.Parse(socket.LastSendMessage!)["reqid"];
            socket.InvokeMessage($"{{\"channelID\": 1, \"status\": \"error\", \"errormessage\": \"Failed to sub\", \"reqid\":{id}}}");
            var subResult = subTask.Result;

            // assert
            Assert.IsFalse(subResult.Success);
            Assert.IsTrue(subResult.Error!.Message.Contains("Failed to sub"));
        }

        [Test]
        public void CheckSocketInterfaces()
        {
            var assembly = Assembly.GetAssembly(typeof(KrakenSocketClient));
            var clientInterfaces = assembly.GetTypes().Where(t => t.Name.StartsWith("IKrakenSocketClientSpot"));

            foreach (var clientInterface in clientInterfaces)
            {
                var implementation = assembly.GetTypes().Single(t => t.IsAssignableTo(clientInterface) && t != clientInterface);
                int methods = 0;
                foreach (var method in implementation.GetMethods().Where(m => m.ReturnType.IsAssignableTo(typeof(Task<CallResult<UpdateSubscription>>))))
                {
                    var interfaceMethod = clientInterface.GetMethod(method.Name, method.GetParameters().Select(p => p.ParameterType).ToArray());
                    Assert.NotNull(interfaceMethod, $"Missing interface for method {method.Name} in {implementation.Name} implementing interface {clientInterface.Name}");
                    methods++;
                }
                Debug.WriteLine($"{clientInterface.Name} {methods} methods validated");
            }
        }
    }
}
