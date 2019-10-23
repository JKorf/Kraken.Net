using CryptoExchange.Net;
using Kraken.Net.UnitTests.TestImplementations;
using Kucoin.Net.UnitTests.TestImplementations;
using NUnit.Framework;

namespace Kraken.Net.UnitTests
{
    [TestFixture]
    public class KrakenSocketClientTests
    {
        [Test]
        public void Subscribe_Should_SucceedIfAckResponse()
        {
            // arrange
            var socket = new TestSocket();
            socket.CanConnect = true;
            var client = TestHelpers.CreateSocketClient(socket);

            // act
            var subTask = client.SubscribeToTickerUpdatesAsync("XBT/EUR", test => { });
            socket.InvokeMessage($"{{\"channelID\": 1, \"status\": \"subscribed\", \"reqid\":\"{BaseClient.LastId}\"}}");
            var subResult = subTask.Result;

            // assert
            Assert.IsTrue(subResult.Success);
        }

        [Test]
        public void Subscribe_Should_FailIfNotAckResponse()
        {
            // arrange
            var socket = new TestSocket();
            socket.CanConnect = true;
            var client = TestHelpers.CreateSocketClient(socket);

            // act
            var subTask = client.SubscribeToTickerUpdatesAsync("XBT/EUR", test => { });
            socket.InvokeMessage($"{{\"channelID\": 1, \"status\": \"error\", \"errormessage\": \"Failed to sub\", \"reqid\":\"{BaseClient.LastId}\"}}");
            var subResult = subTask.Result;

            // assert
            Assert.IsFalse(subResult.Success);
            Assert.IsTrue(subResult.Error.Message.Contains("Failed to sub"));
        }
    }
}
