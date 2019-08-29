using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Kraken.Net.UnitTests.TestImplementations;
using Kucoin.Net.UnitTests.TestImplementations;
using Newtonsoft.Json;
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
            var subTask = client.SubscribeToTickerUpdatesAsync("test", test => { });
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
            var subTask = client.SubscribeToTickerUpdatesAsync("test", test => { });
            socket.InvokeMessage($"{{\"channelID\": 1, \"status\": \"error\", \"errormessage\": \"Failed to sub\", \"reqid\":\"{BaseClient.LastId}\"}}");
            var subResult = subTask.Result;

            // assert
            Assert.IsFalse(subResult.Success);
            Assert.IsTrue(subResult.Error.Message.Contains("Failed to sub"));
        }
    }
}
