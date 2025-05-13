using Kraken.Net.UnitTests.TestImplementations;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Text.Json;
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
            var id = JsonDocument.Parse(socket.LastSendMessage!).RootElement.GetProperty("req_id").GetInt16();
            socket.InvokeMessage("{\"method\": \"subscribe\", \"result\": {\"channel\": \"ticker\", \"snapshot\": true, \"symbol\": \"XBT/EUR\" }, \"success\": true, \"time_in\": \"2023-09-25T09:04:31.742599Z\", \"time_out\": \"2023-09-25T09:04:31.742648Z\", \"req_id\": " + id + "}");
            var subResult = subTask.Result;

            // assert
            Assert.That(subResult.Success);
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
            var id = JsonDocument.Parse(socket.LastSendMessage!).RootElement.GetProperty("req_id").GetInt16();
            socket.InvokeMessage($"{{\"error\":\"Currency pair not in ISO 4217-A3 format DSF\",\"method\":\"subscribe\",\"req_id\":5,\"success\":false,\"symbol\":\"DSF\",\"time_in\":\"2024-10-11T09:49:47.814408Z\",\"time_out\":\"2024-10-11T09:49:47.814465Z\", \"req_id\": {id} }}");
            var subResult = subTask.Result;

            // assert
            ClassicAssert.IsFalse(subResult.Success);
            Assert.That(subResult.Error!.Message.Contains("Currency pair not in ISO 4217-A3 format"));
        }
    }
}
