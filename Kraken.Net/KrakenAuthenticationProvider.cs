﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Kraken.Net.Objects;
using Newtonsoft.Json;

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
            _hmacSecret = Convert.FromBase64String(credentials.Secret);
        }

        public override void AuthenticateRequest(
            RestApiClient apiClient,
            Uri uri,
            HttpMethod method,
            ref IDictionary<string, object>? uriParameters,
            ref IDictionary<string, object>? bodyParameters,
            ref Dictionary<string, string>? headers,
            bool auth,
            ArrayParametersSerialization arraySerialization,
            HttpMethodParameterPosition parameterPosition,
            RequestBodyFormat requestBodyFormat)
        {
            if (!auth)
                return;

            IDictionary<string, object> parameters;
            if (parameterPosition == HttpMethodParameterPosition.InUri)
            {
                uriParameters ??= new Dictionary<string, object>();
                parameters = uriParameters;
            }
            else
            {
                bodyParameters ??= new Dictionary<string, object>();
                parameters = bodyParameters;
            }

            headers ??= new Dictionary<string, string>();
            headers.Add("API-Key", _credentials.Key);
            var nonce = _nonceProvider.GetNonce();
            parameters.Add("nonce", nonce);
            string np;
            if (uri.PathAndQuery == "/0/private/AddOrderBatch")
            {
                // Only endpoint using json body data atm
                np = nonce + JsonConvert.SerializeObject(parameters);
            }
            else
            {
                np = nonce + uri.SetParameters(parameters, arraySerialization).Query.Replace("?", "");
            }

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
            if (string.Equals(x, "nonce", StringComparison.Ordinal))
                return -1;
            if (string.Equals(y, "nonce", StringComparison.Ordinal))
                return 1;

            return x.CompareTo(y);
        }
    }
}
