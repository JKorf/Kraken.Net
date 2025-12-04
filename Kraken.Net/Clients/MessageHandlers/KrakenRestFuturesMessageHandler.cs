using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageConverters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using Kraken.Net.Objects.Internal;
using Kraken.Net.Objects.Models.Futures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Kraken.Net.Clients.MessageHandlers
{
    internal class KrakenRestFuturesMessageHandler : JsonRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;

        public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(KrakenExchange._serializerContext);

        public KrakenRestFuturesMessageHandler(ErrorMapping errorMapping)
        {
            _errorMapping = errorMapping;
        }

        public override Error? CheckDeserializedResponse<T>(HttpResponseHeaders responseHeaders, T result)
        {
            if (result is not KrakenFuturesResult krakenResult)
                return null;

            if (krakenResult.Error?.Length > 0)
            {
                var krakenError = krakenResult.Errors!.First();
                return new ServerError(krakenError.Code, _errorMapping.GetErrorInfo(krakenError.Code.ToString(), krakenError.Message));
            }

            if (krakenResult.Error?.Length > 0)
            {
                return new ServerError(krakenResult.Error, _errorMapping.GetErrorInfo(krakenResult.Error, null));
            }

            return null;
        }

        public override async ValueTask<Error> ParseErrorResponse(int httpStatusCode, HttpResponseHeaders responseHeaders, Stream responseStream)
        {
            var (parseError, document) = await GetJsonDocument(responseStream).ConfigureAwait(false);
            if (parseError != null)
                return parseError;

            KrakenFuturesResult result;
            try
            {
                result = document!.Deserialize<KrakenFuturesResult>(SerializerOptions.WithConverters(KrakenExchange._serializerContext))!;
            }
            catch(Exception ex)
            {
                return new DeserializeError(ex.Message, ex);
            }

            if (result.Errors?.Any() == true)
            {
                var krakenError = result.Errors.First();
                return new ServerError(krakenError.Code, _errorMapping.GetErrorInfo(krakenError.Code.ToString(), krakenError.Message));
            }

            if (result.Error?.Length > 0)
                return new ServerError(result.Error, _errorMapping.GetErrorInfo(result.Error.ToString(), null));

            return new ServerError(ErrorInfo.Unknown);
        }
    }
}
