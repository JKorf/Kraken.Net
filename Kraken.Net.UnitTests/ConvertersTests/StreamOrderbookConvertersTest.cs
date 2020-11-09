using System.Linq;
using Kraken.Net.Converters;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Kraken.Net.UnitTests.ConvertersTests.StreamOrderbookConvertersTests
{
    [TestFixture]
    public class StreamOrderbookConvertersTests
    {
        private JArray fiveElements;

        [OneTimeSetUp]
        public void SetupData()
        {
            var fiveElementsString = @"
            [
                1234,
                {
                    ""a"": [
                    [
                        ""5541.30000"",
                        ""2.50700000"",
                        ""1534614248.456738""
                    ],
                    [
                        ""5542.50000"",
                        ""0.40100000"",
                        ""1534614248.456738""
                    ]
                    ]
                },
                {
                    ""b"": [
                    [
                        ""5541.30000"",
                        ""0.00000000"",
                        ""1534614335.345903""
                    ]
                    ],
                    ""c"": ""974942666""
                },
                ""book-10"",
                ""XBT/USD""
                ]
            ";

            this.fiveElements = JArray.Parse(fiveElementsString);
        }

        [Test]
        public void Event_Should_ParseCountOfFour()
        {
            var testObj = StreamOrderBookConverter.Convert(this.fiveElements);

            Assert.AreEqual(2, testObj.Data.Asks.Count());
            Assert.AreEqual(1, testObj.Data.Bids.Count());
            Assert.AreEqual(1234, testObj.ChannelId);
            Assert.AreEqual("book-10", testObj.Topic);
            Assert.AreEqual("XBT/USD", testObj.Symbol);

            Assert.AreEqual("0.40100000", testObj.Data.Asks.ElementAt(1).RawQuantity);
            Assert.AreEqual("5542.50000", testObj.Data.Asks.ElementAt(1).RawPrice);
        }
    }
}