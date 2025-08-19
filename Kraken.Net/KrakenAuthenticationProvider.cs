using CryptoExchange.Net.Clients;
using Kraken.Net.Objects;
using System.Security.Cryptography;
using System.Text;

namespace Kraken.Net
{
    internal class KrakenAuthenticationProvider : AuthenticationProvider
    {
        private static readonly IMessageSerializer _serializer = new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(KrakenExchange._serializerContext));
        private readonly INonceProvider _nonceProvider;
        private readonly byte[] _hmacSecret;

        public KrakenAuthenticationProvider(ApiCredentials credentials, INonceProvider? nonceProvider) : base(credentials)
        {
            if (credentials.CredentialType != ApiCredentialsType.Hmac)
                throw new Exception("Only Hmac authentication is supported");

            _nonceProvider = nonceProvider ?? new KrakenNonceProvider();
            _hmacSecret = Convert.FromBase64String(credentials.Secret);
        }

        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration request)
        {
            if (!request.Authenticated)
                return;

            request.Headers.Add("API-Key", _credentials.Key);
            var nonce = _nonceProvider.GetNonce();
            var parameters = request.GetPositionParameters();
            parameters.Add("nonce", nonce);

            var body = request.ParameterPosition == HttpMethodParameterPosition.InUri ? string.Empty : request.BodyFormat == RequestBodyFormat.Json ? GetSerializedBody(_serializer, parameters) : request.BodyParameters.ToFormData();
            var queryString = request.GetQueryString();
            var parameterString = nonce + body + queryString;
           
            var pathBytes = Encoding.UTF8.GetBytes(request.Path);
            var allBytes = pathBytes.Concat(SignSHA256Bytes(parameterString)).ToArray();

            string signature;
            using (var hmac = new HMACSHA512(_hmacSecret))
                signature = Convert.ToBase64String(hmac.ComputeHash(allBytes));
            request.Headers.Add("API-Sign", signature);

            request.SetBodyContent(body);
            request.SetQueryString(queryString);
        }
    }
}
