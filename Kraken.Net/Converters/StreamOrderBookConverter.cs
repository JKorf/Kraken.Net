using Kraken.Net.Objects;
using Kraken.Net.Objects.Socket;
using Newtonsoft.Json.Linq;

namespace Kraken.Net.Converters
{
    internal static class StreamOrderBookConverter
    {
        public static KrakenSocketEvent<KrakenStreamOrderBook> Convert(JArray arr)
        {
            var result = new KrakenSocketEvent<KrakenStreamOrderBook>();
            result.ChannelId = (int)arr[0];

            var orderBook = new KrakenStreamOrderBook();
            if (arr.Count == 4)
            {
                var innerObject = arr[1];
                if (innerObject["as"] != null)
                {
                    // snapshot
                    orderBook.Asks = innerObject["as"].ToObject<KrakenStreamOrderBookEntry[]>();
                    orderBook.Bids = innerObject["bs"].ToObject<KrakenStreamOrderBookEntry[]>();
                }
                else if (innerObject["a"] != null)
                {
                    // Only asks
                    orderBook.Asks = innerObject["a"].ToObject<KrakenStreamOrderBookEntry[]>();
                }
                else
                {
                    // Only bids
                    orderBook.Bids = innerObject["b"].ToObject<KrakenStreamOrderBookEntry[]>();
                }

                result.Topic = (string)arr[2];
                result.Market = (string)arr[3];
            }
            else
            {
                orderBook.Asks = arr[1]["a"].ToObject<KrakenStreamOrderBookEntry[]>();
                orderBook.Bids = arr[2]["b"].ToObject<KrakenStreamOrderBookEntry[]>();
                result.Topic = (string)arr[3];
                result.Market = (string)arr[4];
            }

            result.Data = orderBook;
            return result;
        }
    }
}
