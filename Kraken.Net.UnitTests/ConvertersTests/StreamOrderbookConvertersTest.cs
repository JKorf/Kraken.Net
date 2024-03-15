using System.Linq;
using Kraken.Net.Converters;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Kraken.Net.UnitTests.ConvertersTests.StreamOrderbookConvertersTests
{
    [TestFixture]
    public class StreamOrderbookConvertersTests
    {
        private JArray _fiveElements;

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

            this._fiveElements = JArray.Parse(fiveElementsString);
        }

        [Test]
        public void Event_Should_ParseCountOfFour()
        {
            var testObj = StreamOrderBookConverter.Convert(this._fiveElements!);

            Assert.ReferenceEquals(2, testObj!.Data.Asks.Count());
            Assert.ReferenceEquals(1, testObj.Data.Bids.Count());
            Assert.ReferenceEquals(1234, testObj.ChannelId);
            Assert.ReferenceEquals("book-10", testObj.ChannelName);
            Assert.ReferenceEquals("XBT/USD", testObj.Symbol);

            Assert.ReferenceEquals("0.40100000", testObj.Data.Asks.ElementAt(1).RawQuantity);
            Assert.ReferenceEquals("5542.50000", testObj.Data.Asks.ElementAt(1).RawPrice);
        }
    }
}