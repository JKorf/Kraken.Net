using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Kraken.Net.Objects;

namespace Kraken.Net
{
    internal class KrakenAuthenticationProvider: AuthenticationProvider
    {
        private readonly INonceProvider _nonceProvider;
        private readonly byte[] _hmacSecret;

        public KrakenAuthenticationProvider(ApiCredentials credentials, INonceProvider? nonceProvider) : base(credentials)
        {
            if (credentials.CredentialType != ApiCredentialsType.Hmac)
                throw new Exception("Only Hmac authentication is supported");

            _nonceProvider = nonceProvider ?? new KrakenNonceProvider();
            _hmacSecret = Convert.FromBase64String(credentials.Secret!.GetString());
        }

        public override void AuthenticateRequest(RestApiClient apiClient, Uri uri, HttpMethod method, Dictionary<string, object> providedParameters, bool auth, ArrayParametersSerialization arraySerialization, HttpMethodParameterPosition parameterPosition, out SortedDictionary<string, object> uriParameters, out SortedDictionary<string, object> bodyParameters, out Dictionary<string, string> headers)
        {
            uriParameters = parameterPosition == HttpMethodParameterPosition.InUri ? new SortedDictionary<string, object>(providedParameters, new KrakenParameterComparer()) : new SortedDictionary<string, object>();
            bodyParameters = parameterPosition == HttpMethodParameterPosition.InBody ? new SortedDictionary<string, object>(providedParameters, new KrakenParameterComparer()) : new SortedDictionary<string, object>();
            headers = new Dictionary<string, string>();

            if (!auth)
                return;

            var parameters = parameterPosition == HttpMethodParameterPosition.InUri ? uriParameters : bodyParameters;

            headers.Add("API-Key", _credentials.Key!.GetString());
            var nonce = _nonceProvider.GetNonce();
            parameters.Add("nonce", nonce);
            var np = nonce + uri.SetParameters(parameters, arraySerialization).Query.Replace("?", "");

            var pathBytes = Encoding.UTF8.GetBytes(uri.AbsolutePath);
            var allBytes = pathBytes.Concat(SignSHA256Bytes(np)).ToArray();

            string sign;
            using (var hmac = new HMACSHA512(_hmacSecret))
                sign = Convert.ToBase64String(hmac.ComputeHash(allBytes));

            headers.Add("API-Sign", sign);
        }
    }

    internal class KrakenParameterComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == "nonce")
                return -1;
            if (y == "nonce")
                return 1;

            return x.CompareTo(y);
        }
    }
}
