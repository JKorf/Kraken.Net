using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageConverters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using Kraken.Net.Objects.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Kraken.Net.Clients.MessageHandlers
{
    internal class KrakenRestSpotMessageHandler : JsonRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;

        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(KrakenExchange._serializerContext);

        public KrakenRestSpotMessageHandler(ErrorMapping errorMapping)
        {
            _errorMapping = errorMapping;
        }

        public override Error? CheckDeserializedResponse<T>(HttpResponseHeaders responseHeaders, T result)
        {
            if (result is not KrakenResult krakenResult)
                return null;

            if (krakenResult.Error?.Length > 0)
            {
                var error = krakenResult.Error.First();
                var split = error.Split(':');
                if (split.Length > 1)
                {
                    var category = split[0];
                    var message = krakenResult.Error.Length > 1 ? string.Join(", ", krakenResult.Error.Select(x => string.Join(": ", x.Split(':').Skip(1)))) : string.Join(": ", split.Skip(1));
                    return new ServerError(category, _errorMapping.GetErrorInfo(category, message));
                }

                return new ServerError(error, _errorMapping.GetErrorInfo(error, null));
            }

            return null;
        }

        public override async ValueTask<Error> ParseErrorResponse(int httpStatusCode, object? state, HttpResponseHeaders responseHeaders, Stream responseStream)
        {
            var (parseError, document) = await GetJsonDocument(responseStream, state).ConfigureAwait(false);
            if (parseError != null)
                return parseError;

            var label = document!.RootElement.TryGetProperty("label", out var codeProp) ? codeProp.GetString() : null;
            if(label == null)
                return new ServerError(ErrorInfo.Unknown);

            var msg = document!.RootElement.TryGetProperty("message", out var msgProp) ? msgProp.GetString() : null;
            return new ServerError(label!, _errorMapping.GetErrorInfo(label!, msg));
        }
    }
}
