//using System.Collections.Generic;
//using Kraken.Net.Objects.Models;
//using Kraken.Net.Objects.Sockets;

//namespace Kraken.Net.Converters
//{
//    internal static class StreamOrderBookConverter
//    {
//        public static KrakenSocketUpdate<KrakenStreamOrderBook>? Convert(JArray arr)
//        {
//            var result = new KrakenSocketUpdate<KrakenStreamOrderBook> {ChannelId = (int) arr[0]};

//            var orderBook = new KrakenStreamOrderBook();
//            if (arr.Count == 4)
//            {
//                var innerObject = arr[1];
//                if (innerObject == null)
//                    return null;

//                if (innerObject["as"] != null)
//                {
//                    // snapshot
//                    orderBook.Asks = innerObject["as"]!.ToObject<IEnumerable<KrakenStreamOrderBookEntry>>()!;
//                    orderBook.Bids = innerObject["bs"]!.ToObject<IEnumerable<KrakenStreamOrderBookEntry>>()!;
//                    orderBook.Snapshot = true;
//                }
//                else if (innerObject["a"] != null)
//                {
//                    // Only asks
//                    orderBook.Asks = innerObject["a"]!.ToObject<IEnumerable<KrakenStreamOrderBookEntry>>()!;
//                }
//                else
//                {
//                    // Only bids
//                    orderBook.Bids = innerObject["b"]!.ToObject<IEnumerable<KrakenStreamOrderBookEntry>>()!;
//                }

//                if (innerObject["c"] != null)
//                    orderBook.Checksum = innerObject["c"]!.Value<uint>();
                
//                result.ChannelName = arr[2].ToString();
//                result.Symbol = arr[3].ToString();
//            }
//            else
//            {
//                orderBook.Asks = arr[1]["a"]!.ToObject<IEnumerable<KrakenStreamOrderBookEntry>>()!;
//                orderBook.Bids = arr[2]["b"]!.ToObject<IEnumerable<KrakenStreamOrderBookEntry>>()!;
//                orderBook.Checksum = arr[2]["c"]!.Value<uint>();
//                result.ChannelName = arr[3].ToString();
//                result.Symbol = arr[4].ToString();
//            }

//            result.Data = orderBook;
//            return result;
//        }
//    }
//}
