using System.Collections.Generic;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Socket;
using Newtonsoft.Json.Linq;

namespace Kraken.Net.Converters
{
    internal static class StreamOrderBookConverter
    {
        public static KrakenSocketEvent<KrakenStreamOrderBook> Convert(JArray arr)
        {
            var result = new KrakenSocketEvent<KrakenStreamOrderBook> {ChannelId = (int) arr[0]};

            var orderBook = new KrakenStreamOrderBook();
            if (arr.Count == 4)
            {
                var innerObject = arr[1];
                if (innerObject["as"] != null)
                {
                    // snapshot
                    orderBook.Asks = innerObject["as"].ToObject<IEnumerable<KrakenStreamOrderBookEntry>>();
                    orderBook.Bids = innerObject["bs"].ToObject< IEnumerable<KrakenStreamOrderBookEntry>>();
                }
                else if (innerObject["a"] != null)
                {
                    // Only asks
                    orderBook.Asks = innerObject["a"].ToObject<IEnumerable<KrakenStreamOrderBookEntry>>();
                }
                else
                {
                    // Only bids
                    orderBook.Bids = innerObject["b"].ToObject<IEnumerable<KrakenStreamOrderBookEntry>>();
                }

                result.Topic = (string)arr[2];
                result.Symbol = (string)arr[3];
            }
            else
            {
                orderBook.Asks = arr[1]["a"].ToObject<IEnumerable<KrakenStreamOrderBookEntry>>();
                orderBook.Bids = arr[2]["b"].ToObject<IEnumerable<KrakenStreamOrderBookEntry>>();
                result.Topic = (string)arr[3];
                result.Symbol = (string)arr[4];
            }

            result.Data = orderBook;
            return result;
        }
    }
}
