using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Converters;
using Kraken.Net.Objects.Models;
using Kraken.Net.Objects.Models.Socket;
using Kraken.Net.Objects.Sockets;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Net.Objects.Sockets.Converters
{
    internal class KrakenSpotStreamConverter : SocketConverter
    {
        private static Dictionary<string, Type> _eventTypeMapping = new Dictionary<string, Type>
        {
            { "systemStatus", typeof(KrakenStreamSystemStatus) },
            { "heartbeat", typeof(KrakenEvent) },
        };

        private static Dictionary<string, Type> _arrayTypeMapping = new Dictionary<string, Type>
        {
            { "ticker", typeof(KrakenSocketUpdate<KrakenStreamTick>) },
            { "ohlc-1", typeof(KrakenSocketUpdate<KrakenStreamKline>) },
            { "ohlc-5", typeof(KrakenSocketUpdate<KrakenStreamKline>) },
            { "ohlc-15", typeof(KrakenSocketUpdate<KrakenStreamKline>) },
            { "ohlc-30", typeof(KrakenSocketUpdate<KrakenStreamKline>) },
            { "ohlc-60", typeof(KrakenSocketUpdate<KrakenStreamKline>) },
            { "ohlc-240", typeof(KrakenSocketUpdate<KrakenStreamKline>) },
            { "ohlc-1440", typeof(KrakenSocketUpdate<KrakenStreamKline>) },
            { "ohlc-10080", typeof(KrakenSocketUpdate<KrakenStreamKline>) },
            { "ohlc-21600", typeof(KrakenSocketUpdate<KrakenStreamKline>) },
            { "trade", typeof(KrakenSocketUpdate<IEnumerable<KrakenTrade>>) },
            { "spread", typeof(KrakenSocketUpdate<KrakenStreamSpread>) },
            { "book-10", typeof(KrakenSocketUpdate<KrakenStreamOrderBook>) },
            { "book-25", typeof(KrakenSocketUpdate<KrakenStreamOrderBook>) },
            { "book-100", typeof(KrakenSocketUpdate<KrakenStreamOrderBook>) },
            { "book-500", typeof(KrakenSocketUpdate<KrakenStreamOrderBook>) },
            { "book-1000", typeof(KrakenSocketUpdate<KrakenStreamOrderBook>) },
            { "ownTrades", typeof(KrakenAuthSocketUpdate<IEnumerable<Dictionary<string, KrakenStreamUserTrade>>>) },
            { "openOrders", typeof(KrakenAuthSocketUpdate<IEnumerable<Dictionary<string, KrakenStreamOrder>>>) },
        };

        public override MessageInterpreterPipeline InterpreterPipeline { get; } = new MessageInterpreterPipeline
        {
            //PostInspectCallbacks = new List<object>
            //{
            //    new PostInspectCallback
            //    {
            //        TypeFields = new List<TypeField> { new TypeField("reqid") },
            //        Callback = GetDeserializationTypeQueryResponse
            //    },new PostInspectCallback
            //    {
            //        TypeFields = new List<TypeField> { new TypeField("event") },
            //        Callback = GetDeserializationTypeEvent
            //    },
            //    //new PostInspectArrayCallback
            //    //{
            //    //    TypeFields = new List<int> { 3, 4 },
            //    //    Callback = GetDeserializationTypeBook
            //    //},
            //    //new PostInspectArrayCallback
            //    //{
            //    //    TypeFields = new List<int> { 2, 3 },
            //    //    Callback = GetDeserializationTypeArray
            //    //},
            //    //new PostInspectArrayCallback
            //    //{
            //    //    TypeFields = new List<int> { 1, 2 },
            //    //    Callback = GetDeserializationTypeArray2
            //    //}
            //},
            ObjectInitializer = InstantiateMessageObject,
            GetIdentity = GetIdentity,
        };

        private static string GetIdentity(IMessageAccessor accessor)
        {
            var id = accessor.GetStringValue("reqid");
            if (id != null)
                return id;

            var evnt = accessor.GetStringValue("event");
            if (evnt != null)
                return evnt;

            var arr4 = accessor.GetArrayStringValue(null, 4);
            var arr3 = accessor.GetArrayStringValue(null, 3);
            if (arr4 != null)
                return arr3.ToLowerInvariant() + "-" + arr4.ToLowerInvariant();

            var arr2 = accessor.GetArrayStringValue(null, 2);
            if (arr2 != null)
                return arr2.ToLowerInvariant() + "-" + arr3.ToLowerInvariant();

            return accessor.GetArrayStringValue(null, 1).ToLowerInvariant() + "-" + arr2.ToLowerInvariant();
        }

        private static PostInspectResult GetDeserializationTypeQueryResponse(IMessageAccessor accessor, Dictionary<string, Type> processors)
        {
            if (!processors.TryGetValue(accessor.GetStringValue("reqid"), out var type))
            {
                // Probably shouldn't be exception
                throw new Exception("Unknown update type");
            }

            return new PostInspectResult { Type = type, Identifier = accessor.GetStringValue("reqid") };
        }

        private static PostInspectResult GetDeserializationTypeEvent(IMessageAccessor accessor, Dictionary<string, Type> processors)
        {
            var streamId = accessor.GetStringValue("event")!;
            if (_eventTypeMapping.TryGetValue(streamId, out var streamIdMapping))
                return new PostInspectResult { Type = streamIdMapping, Identifier = streamId.ToLowerInvariant() };

            return new PostInspectResult();
        }

        //private static PostInspectResult GetDeserializationTypeArray(IMessageAccessor accessor, Dictionary<string, Type> processors)
        //{
        //    var streamId = idValues[2]!;
        //    if (_arrayTypeMapping.TryGetValue(streamId, out var streamIdMapping))
        //        return new PostInspectResult { Type = streamIdMapping, Identifier = streamId.ToLowerInvariant() + "-" + idValues[3].ToLowerInvariant() };

        //    return new PostInspectResult();
        //}

        //private static PostInspectResult GetDeserializationTypeArray2(IMessageAccessor accessor, Dictionary<string, Type> processors)
        //{
        //    var streamId = idValues[1]!;
        //    if (_arrayTypeMapping.TryGetValue(streamId, out var streamIdMapping))
        //        return new PostInspectResult { Type = streamIdMapping, Identifier = streamId.ToLowerInvariant() };

        //    return new PostInspectResult();
        //}

        //private static PostInspectResult GetDeserializationTypeBook(IMessageAccessor accessor, Dictionary<string, Type> processors)
        //{
        //    return new PostInspectResult { Type = typeof(KrakenSocketUpdate<KrakenStreamOrderBook>), Identifier = idValues[3].ToLowerInvariant() + "-" + idValues[4].ToLowerInvariant() };
        //}

        private new static BaseParsedMessage InstantiateMessageObject(JToken token, Type type)
        {
            if (type == typeof(KrakenSocketUpdate<KrakenStreamOrderBook>))
                return new ParsedMessage<KrakenSocketUpdate<KrakenStreamOrderBook>>(StreamOrderBookConverter.Convert((JArray)token));
            else
                return SocketConverter.InstantiateMessageObject(token, type);
        }
    }
}