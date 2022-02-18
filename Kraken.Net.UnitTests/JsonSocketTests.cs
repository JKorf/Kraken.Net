using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kraken.Net.Interfaces.Clients;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Models.Socket;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Kraken.Net.UnitTests
{
    internal class JsonSocketTests
    {
        [Test]
        public async Task ValidateSystemStatusUpdateStreamJson()
        {
            await TestFileToObject<KrakenStreamSystemStatus>(@"JsonResponses\Socket\SystemStatusUpdate.txt");
        }

        [Test]
        public async Task ValidateTickerUpdateStreamJson()
        {
            await TestFileToObject<KrakenSocketEvent<KrakenStreamTick>>(@"JsonResponses\Socket\TickerUpdate.txt");
        }

        [Test]
        public async Task ValidateKlineUpdateStreamJson()
        {
            await TestFileToObject<KrakenSocketEvent<KrakenStreamKline>>(@"JsonResponses\Socket\KlineUpdate.txt");
        }

        [Test]
        public async Task ValidateTradeUpdateStreamJson()
        {
            await TestFileToObject<KrakenSocketEvent<IEnumerable<KrakenTrade>>>(@"JsonResponses\Socket\TradeUpdate.txt");
        }

        [Test]
        public async Task ValidateSpreadUpdateStreamJson()
        {
            await TestFileToObject<KrakenSocketEvent<KrakenStreamSpread>>(@"JsonResponses\Socket\SpreadUpdate.txt");
        }

        [Test]
        public async Task ValidateOpenOrderUpdateStreamJson()
        {
            await TestFileToObject<List<Dictionary<string, KrakenStreamOrder>>>(@"JsonResponses\Socket\OpenOrdersUpdate.txt", new List<string> { "avg_price" });
        }

        [Test]
        public async Task ValidateUserTradeUpdateStreamJson()
        {
            await TestFileToObject<List<Dictionary<string, KrakenStreamUserTrade>>>(@"JsonResponses\Socket\OwnTradeUpdate.txt", new List<string> { "avg_price" });
        }

        private static async Task TestFileToObject<T>(string filePath, List<string> ignoreProperties = null)
        {
            var listener = new EnumValueTraceListener();
            Trace.Listeners.Add(listener);
            var path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string json;
            try
            {
                var file = File.OpenRead(Path.Combine(path, filePath));
                using var reader = new StreamReader(file);
                json = await reader.ReadToEndAsync();
            }
            catch (FileNotFoundException)
            {
                throw;
            }

            var result = JsonConvert.DeserializeObject<T>(json);
            JsonToObjectComparer<IKrakenSocketClient>.ProcessData("", result, json, ignoreProperties: new Dictionary<string, List<string>>
            {
                { "", ignoreProperties ?? new List<string>() }
            });
            Trace.Listeners.Remove(listener);
        }
    }

    internal class EnumValueTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            if (message.Contains("Cannot map"))
                throw new Exception("Enum value error: " + message);
        }

        public override void WriteLine(string message)
        {
            if (message.Contains("Cannot map"))
                throw new Exception("Enum value error: " + message);
        }
    }
}
