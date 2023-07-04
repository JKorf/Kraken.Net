using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Kraken.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Kraken.Net
{
    internal class KrakenFuturesAuthenticationProvider : AuthenticationProvider
    {
        private readonly INonceProvider _nonceProvider;
        private readonly byte[] _hmacSecret;

        public KrakenFuturesAuthenticationProvider(ApiCredentials credentials, INonceProvider? nonceProvider) : base(credentials)
        {
            if (credentials.CredentialType != ApiCredentialsType.Hmac)
                throw new Exception("Only Hmac authentication is supported");

            _nonceProvider = nonceProvider ?? new KrakenNonceProvider();
            _hmacSecret = Convert.FromBase64String(credentials.Secret!.GetString());
        }

        public override void AuthenticateRequest(RestApiClient apiClient, Uri uri, HttpMethod method, Dictionary<string, object> providedParameters, bool auth, ArrayParametersSerialization arraySerialization, HttpMethodParameterPosition parameterPosition, out SortedDictionary<string, object> uriParameters, out SortedDictionary<string, object> bodyParameters, out Dictionary<string, string> headers)
        {
            uriParameters = parameterPosition == HttpMethodParameterPosition.InUri ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            bodyParameters = parameterPosition == HttpMethodParameterPosition.InBody ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            headers = new Dictionary<string, string>();

            if (!auth)
                return;

            var parameters = parameterPosition == HttpMethodParameterPosition.InUri ? uriParameters : bodyParameters;

            headers.Add("APIKey", _credentials.Key!.GetString());
            var message = providedParameters.CreateParamString(false, arraySerialization) + uri.AbsolutePath.Replace("/derivatives", "");
            using var hash256 = SHA256.Create();
            var hash = hash256.ComputeHash(Encoding.UTF8.GetBytes(message));

            using HMACSHA512 hMACSHA = new HMACSHA512(_hmacSecret);
            byte[] buff = hMACSHA.ComputeHash(hash);
            var sign = BytesToBase64String(buff);
            headers.Add("Authent", sign);
        }
    }
}