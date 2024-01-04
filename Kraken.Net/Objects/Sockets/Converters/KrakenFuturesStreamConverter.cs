//using CryptoExchange.Net.Converters;
//using CryptoExchange.Net.Interfaces;
//using CryptoExchange.Net.Objects.Sockets;
//using Kraken.Net.Objects.Models.Socket.Futures;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Kraken.Net.Objects.Sockets.Converters
//{
//    internal class KrakenFuturesStreamConverter : SocketConverter
//    {
//        private static Dictionary<string, Type> _eventTypeMapping = new Dictionary<string, Type>
//        {
//            { "info", typeof(KrakenEvent) },
//            { "challenge", typeof(KrakenChallengeResponse) },
//        };

//        private static Dictionary<string, Type> _feedTypeMapping = new Dictionary<string, Type>
//        {
//            { "heartbeat", typeof(KrakenFuturesHeartbeatUpdate) },
//            { "ticker", typeof(KrakenFuturesTickerUpdate) },
//            { "ticker_lite", typeof(KrakenFuturesMiniTickerUpdate) },
//            { "trade_snapshot", typeof(KrakenFuturesTradesSnapshotUpdate) },
//            { "trade", typeof(KrakenFuturesTradeUpdate) },
//            { "book", typeof(KrakenFuturesBookUpdate) },
//            { "book_snapshot", typeof(KrakenFuturesBookSnapshotUpdate) },
//            { "open_orders_verbose_snapshot", typeof(KrakenFuturesOpenOrdersSnapshotUpdate) },
//            { "open_orders_verbose", typeof(KrakenFuturesOpenOrdersUpdate) },
//            { "open_orders_snapshot", typeof(KrakenFuturesOpenOrdersSnapshotUpdate) },
//            { "open_orders", typeof(KrakenFuturesOpenOrdersUpdate) },
//            { "account_log_snapshot", typeof(KrakenFuturesAccountLogsSnapshotUpdate) },
//            { "account_log", typeof(KrakenFuturesAccountLogsUpdate) },
//            { "open_positions", typeof(KrakenFuturesOpenPositionUpdate) },
//            { "balances", typeof(KrakenFuturesBalancesUpdate) },
//            { "balances_snapshot", typeof(KrakenFuturesBalancesUpdate) },
//            { "fills_snapshot", typeof(KrakenFuturesUserTradesUpdate) },
//            { "fills", typeof(KrakenFuturesUserTradesUpdate) },
//            { "notifications_auth", typeof(KrakenFuturesNotificationUpdate) },
//        };

//        public override MessageInterpreterPipeline InterpreterPipeline { get; } = new MessageInterpreterPipeline
//        {
//            PostInspectCallbacks = new List<object>
//            {
//                new PostInspectCallback
//                {
//                    TypeFields = new List<TypeField> 
//                    {
//                        new TypeField("event", false),
//                        new TypeField("feed", false),
//                        new TypeField("product_ids", false),
//                        new TypeField("product_id", false)
//                    },
//                    Callback = GetDeserializationTypeEvent
//                }
//            },
//            ObjectInitializer = InstantiateMessageObject
//        };

//        private static PostInspectResult GetDeserializationTypeEvent(IMessageAccessor accessor, Dictionary<string, Type> processors)
//        {
//            var evnt = accessor.GetStringValue("event")?.ToLowerInvariant();
//            var feed = accessor.GetStringValue("feed")?.ToLowerInvariant();
//            var productIds = accessor.GetStringValue("product_ids")?.ToLowerInvariant();
//            var productId = accessor.GetStringValue("product_id")?.ToLowerInvariant();
//            var identifier = GetIdentifier(evnt, feed, (productIds ?? productId));
//            if (evnt != null && feed != null)
//            {
//                if (processors.TryGetValue(identifier, out var type))
//                    return new PostInspectResult { Type = type, Identifier = identifier };
//            }

//            if (feed != null)
//            {
//                if (_feedTypeMapping.TryGetValue(feed, out var streamIdMapping))
//                    return new PostInspectResult { Type = streamIdMapping, Identifier = identifier };
//            }

//            if (evnt != null)
//            {
//                if (_eventTypeMapping.TryGetValue(evnt, out var streamIdMapping))
//                    return new PostInspectResult { Type = streamIdMapping, Identifier = identifier };
//            }

//            return new PostInspectResult();
//        }

//        private static string GetIdentifier(string? evnt, string? feed, string? productId)
//        {
//            var sb = new StringBuilder();
//            if (evnt != null)
//            {
//                sb.Append(evnt);
//                sb.Append('-');
//            }
//            if (feed != null)
//            {
//                sb.Append(feed);
//                sb.Append('-');
//            }
//            if (productId != null)
//            {
//                sb.Append(productId);
//            }

//            return sb.ToString().Trim('-');
//        }
//    }
//}