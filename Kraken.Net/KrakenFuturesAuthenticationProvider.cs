using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Kraken.Net.Clients.FuturesApi;
using Kraken.Net.Interfaces.Clients.FuturesApi;
using Kraken.Net.Objects;
using Kraken.Net.Objects.Sockets.Queries;
using System.Security.Cryptography;
using System.Text;

namespace Kraken.Net
{
    internal class KrakenFuturesAuthenticationProvider : AuthenticationProvider
    {
        private readonly byte[] _hmacSecret;

        public string GetApiKey() => _credentials.Key;
        public override ApiCredentialsType[] SupportedCredentialTypes => [ApiCredentialsType.Hmac];

        public KrakenFuturesAuthenticationProvider(ApiCredentials credentials, INonceProvider? nonceProvider) : base(credentials)
        {
            try
            {
                _hmacSecret = Convert.FromBase64String(credentials.Secret);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Provided secret invalid, not in base64 format", ex);
            }
        }

        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration request)
        {
            if (!request.Authenticated)
                return;

            request.Headers ??= new Dictionary<string, string>();
            request.Headers.Add("APIKey", _credentials.Key);

            var queryString = request.GetQueryString();
            var body = (request.ParameterPosition == HttpMethodParameterPosition.InBody && request.BodyParameters?.Count > 0) ? request.BodyParameters.ToFormData() : string.Empty;
            var signData = $"{queryString}{body}{request.Path.Replace("/derivatives", "")}";

            var hash = SignSHA256Bytes(signData);
            using HMACSHA512 hMACSHA = new HMACSHA512(_hmacSecret);
            byte[] buff = hMACSHA.ComputeHash(hash);
            var signature = BytesToBase64String(buff);
            request.Headers.Add("Authent", signature);
        }

        public override Query? GetAuthenticationQuery(SocketApiClient apiClient, SocketConnection connection, Dictionary<string, object?>? context = null)
        {
            if (apiClient is not KrakenSocketClientFuturesApi)
                return null;

            return new KrakenFuturesAuthQuery(ApiKey);
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