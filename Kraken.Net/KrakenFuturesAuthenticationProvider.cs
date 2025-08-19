using CryptoExchange.Net.Clients;
using Kraken.Net.Objects;
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

        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration request)
        {
            if (!request.Authenticated)
                return;

            request.Headers.Add("APIKey", _credentials.Key);

            var queryString = request.GetQueryString();
            var body = request.ParameterPosition == HttpMethodParameterPosition.InBody ? request.BodyParameters.ToFormData() : string.Empty;
            var signData = $"{queryString}{body}{request.Path.Replace("/derivatives", "")}";

            var hash = SignSHA256Bytes(signData);
            using HMACSHA512 hMACSHA = new HMACSHA512(_hmacSecret);
            byte[] buff = hMACSHA.ComputeHash(hash);
            var signature = BytesToBase64String(buff);
            request.Headers.Add("Authent", signature);
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