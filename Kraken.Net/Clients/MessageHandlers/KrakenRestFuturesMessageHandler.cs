using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using CryptoExchange.Net.Objects.Errors;
using Kraken.Net.Objects.Models.Futures;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http.Headers;

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

#if NET5_0_OR_GREATER
        [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL3050:RequiresUnreferencedCode", Justification = "JsonSerializerOptions provided here has TypeInfoResolver set")]
        [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2026:RequiresUnreferencedCode", Justification = "JsonSerializerOptions provided here has TypeInfoResolver set")]
#endif
        public override async ValueTask<Error> ParseErrorResponse(int httpStatusCode, HttpResponseHeaders responseHeaders, Stream responseStream)
        {
            var (parseError, document) = await GetJsonDocument(responseStream).ConfigureAwait(false);
            if (parseError != null)
                return parseError;

            KrakenFuturesResult result;
            try
            {
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning disable IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
                result = document!.Deserialize<KrakenFuturesResult>(SerializerOptions.WithConverters(KrakenExchange._serializerContext))!;
#pragma warning restore IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
            }
            catch (Exception ex)
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
