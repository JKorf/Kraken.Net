using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Socket;

namespace Kraken.Net.Clients.MessageHandlers
{
    internal class KrakenSocketSpotMessageHandler : JsonSocketMessageHandler
    {
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
                    new PropertyFieldReference("channel").WithEqualConstraint("balances"),
                    new PropertyFieldReference("type").WithEqualConstraint("snapshot"),
                ],
                StaticIdentifier = "balancessnapshot"
            },

             new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("channel").WithEqualConstraint("balances"),
                    new PropertyFieldReference("type").WithNotEqualConstraint("snapshot"),
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
