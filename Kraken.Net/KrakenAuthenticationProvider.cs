using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;

namespace Kraken.Net
{
    internal class KrakenAuthenticationProvider: AuthenticationProvider
    {
        private static readonly object nonceLock = new object();
        private static long lastNonce;
        internal static string Nonce
        {
            get
            {
                lock (nonceLock)
                {
                    var nonce = DateTime.UtcNow.Ticks;
                    if (nonce == lastNonce)
                        nonce += 1;

                    lastNonce = nonce;
                    return lastNonce.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private readonly HMACSHA512 encryptor;

        public KrakenAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
            if(credentials.Secret == null)
                throw new ArgumentException("ApiKey/Secret needed");

            encryptor = new HMACSHA512(Convert.FromBase64String(credentials.Secret.GetString()));
        }

        public override Dictionary<string, object> AddAuthenticationToParameters(string uri, HttpMethod method, Dictionary<string, object> parameters, bool signed, PostParameters postParameterPosition, ArrayParametersSerialization arraySerialization)
        {
            if (!signed)
                return parameters;

            parameters.Add("nonce", Nonce);
            return parameters;
        }

        public override Dictionary<string, string> AddAuthenticationToHeaders(string uri, HttpMethod method, Dictionary<string, object> parameters, bool signed, PostParameters postParameterPosition, ArrayParametersSerialization arraySerialization)
        {
            if(!signed)
                return new Dictionary<string, string>();

            if (Credentials.Key == null)
                throw new ArgumentException("ApiKey/Secret needed");

            var nonce = parameters.Single(n => n.Key == "nonce").Value;
            var paramList = parameters.OrderBy(o => o.Key != "nonce");
            var pars = string.Join("&", paramList.Select(p => $"{p.Key}={p.Value}"));

            var result = new Dictionary<string, string> {{"API-Key", Credentials.Key.GetString()}};
            var np = nonce + pars;
            byte[] nonceParamsBytes;
            using (var sha = SHA256.Create())
                nonceParamsBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(np));
            var pathBytes = Encoding.UTF8.GetBytes(uri.Split(new[] { ".com" }, StringSplitOptions.None)[1]);
            var allBytes = pathBytes.Concat(nonceParamsBytes).ToArray();
            var sign = encryptor.ComputeHash(allBytes);

            result.Add("API-Sign", Convert.ToBase64String(sign));
            return result;
        }
    }
}
