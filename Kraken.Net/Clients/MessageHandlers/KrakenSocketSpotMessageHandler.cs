using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using Kraken.Net;
using System;
using System.Linq;
using System.Text.Json;

namespace Kraken.Net.Clients.MessageHandlers
{
    internal class KrakenSocketSpotMessageHandler : JsonSocketMessageHandler
    {
        private static readonly HashSet<string> _channelsWithoutSymbol =
        [
            "heartbeat",
            "status",
            "instrument",
            "executions",
            "balances"
        ];

        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(KrakenExchange._serializerContext);

        protected override MessageEvaluator[] MessageEvaluators { get; } = [

             new MessageEvaluator {
                Priority = 1,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("req_id"),
                ],
                IdentifyMessageCallback = x => x.FieldValue("req_id")!
            },

             new MessageEvaluator {
                Priority = 2,
                Fields = [
                    new PropertyFieldReference("channel") { Constraint = x => !_channelsWithoutSymbol.Contains(x!) },
                    new PropertyFieldReference("method"),
                    new PropertyFieldReference("symbol") { Depth = 3 },
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("channel")}{x.FieldValue("method")}-{x.FieldValue("symbol")}"
            },

             new MessageEvaluator {
                Priority = 2,
                Fields = [
                    new PropertyFieldReference("channel") { Constraint = x => !_channelsWithoutSymbol.Contains(x!) },
                    new PropertyFieldReference("symbol") { Depth = 3 },
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("channel")}-{x.FieldValue("symbol")}"
            },

             new MessageEvaluator {
                Priority = 4,
                Fields = [
                    new PropertyFieldReference("channel") { Constraint = x => x!.Equals("balances", StringComparison.Ordinal) },
                    new PropertyFieldReference("type") { Constraint = x => x!.Equals("snapshot", StringComparison.Ordinal) },
                ],
                StaticIdentifier = "balancessnapshot"
            },

             new MessageEvaluator {
                Priority = 5,
                Fields = [
                    new PropertyFieldReference("channel") { Constraint = x => x!.Equals("balances", StringComparison.Ordinal) },
                    new PropertyFieldReference("type") { Constraint = x => !x!.Equals("snapshot", StringComparison.Ordinal) },
                ],
                StaticIdentifier = "balances"
            },

             new MessageEvaluator {
                Priority = 6,
                Fields = [
                    new PropertyFieldReference("channel"),
                    new PropertyFieldReference("method"),
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("channel")}{x.FieldValue("method")}"
            },

             new MessageEvaluator {
                Priority = 7,
                Fields = [
                    new PropertyFieldReference("channel") { Constraint = x => _channelsWithoutSymbol.Contains(x!) },
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("channel")}"
            },
        ];
    }
}
