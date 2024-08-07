﻿using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Kraken.Net.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Kraken.Net
{
    internal class KrakenFuturesAuthenticationProvider : AuthenticationProvider
    {
        private readonly INonceProvider _nonceProvider;
        private readonly byte[] _hmacSecret;

        public string GetApiKey() => _credentials.Key;

        public KrakenFuturesAuthenticationProvider(ApiCredentials credentials, INonceProvider? nonceProvider) : base(credentials)
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

            headers ??= new Dictionary<string, string>();
            headers.Add("APIKey", _credentials.Key);

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

            var message = parameters.CreateParamString(false, arraySerialization) + uri.AbsolutePath.Replace("/derivatives", "");
            using var hash256 = SHA256.Create();
            var hash = hash256.ComputeHash(Encoding.UTF8.GetBytes(message));

            using HMACSHA512 hMACSHA = new HMACSHA512(_hmacSecret);
            byte[] buff = hMACSHA.ComputeHash(hash);
            var sign = BytesToBase64String(buff);
            headers.Add("Authent", sign);
        }

        public string AuthenticateWebsocketChallenge(string challenge)
        {
            using var hash256 = SHA256.Create();
            var hash = hash256.ComputeHash(Encoding.UTF8.GetBytes(challenge));
            using HMACSHA512 hMACSHA = new HMACSHA512(_hmacSecret);
            byte[] buff = hMACSHA.ComputeHash(hash);
            return BytesToBase64String(buff);
        }
    }
}