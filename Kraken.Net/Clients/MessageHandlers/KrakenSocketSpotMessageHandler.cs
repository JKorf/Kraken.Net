using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using Kraken.Net;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;
using System;
using System.Linq;
using System.Text.Json;

namespace Kraken.Net.Clients.MessageHandlers
{
    internal class KrakenSocketSpotMessageHandler : JsonSocketMessageHandler
    {
        //private static readonly HashSet<string> _channelsWithoutSymbol =
        //[
        //    "heartbeat",
        //    "status",
        //    "instrument",
        //    "executions",
        //    "balances"
        //];

        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(KrakenExchange._serializerContext);

        public KrakenSocketSpotMessageHandler()
        {
            AddTopicMapping<KrakenSocketUpdateV2<KrakenTickerUpdate[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<KrakenSocketUpdateV2<KrakenKlineUpdate[]>>(x => x.Data.First().Symbol + x.Data.First().KlineInterval);
            AddTopicMapping<KrakenSocketUpdateV2<KrakenTradeUpdate[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<KrakenSocketUpdateV2<KrakenBookUpdate[]>>(x => x.Data.First().Symbol);
            AddTopicMapping<KrakenSocketUpdateV2<KrakenIndividualBookUpdate[]>>(x => x.Data.First().Symbol);
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [

             new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("req_id"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("req_id")!
            },

             new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("channel").WithEqualContstraint("balances"),
                    new PropertyFieldReference("type").WithEqualContstraint("snapshot"),
                ],
                StaticIdentifier = "balancessnapshot"
            },

             new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("channel").WithEqualContstraint("balances"),
                    new PropertyFieldReference("type").WithNotEqualContstraint("snapshot"),
                ],
                StaticIdentifier = "balances"
            },

             new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("channel"),
                    new PropertyFieldReference("method"),
                ],
                TypeIdentifierCallback = x => $"{x.FieldValue("channel")}{x.FieldValue("method")}"
            },

             new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("channel"),
                ],
                TypeIdentifierCallback = x => $"{x.FieldValue("channel")}"
            },
        ];
    }
}
