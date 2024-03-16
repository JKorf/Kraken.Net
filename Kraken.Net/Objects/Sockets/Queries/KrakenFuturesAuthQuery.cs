using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Sockets.Queries
{
    internal class KrakenFuturesAuthQuery : Query<KrakenChallengeResponse>
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }

        public KrakenFuturesAuthQuery(string apiKey) : base(new KrakenChallengeRequest { ApiKey = apiKey, Event = "challenge" }, false)
        {
            ListenerIdentifiers = new HashSet<string>() { "challenge" };
        }

        public override CallResult<KrakenChallengeResponse> HandleMessage(SocketConnection connection, DataEvent<KrakenChallengeResponse> message)
        {
            // TODO test error?
            var authProvider = (KrakenFuturesAuthenticationProvider)connection.ApiClient.AuthenticationProvider!;
            var sign = authProvider.AuthenticateWebsocketChallenge(message.Data.Message);
            connection.Properties["OriginalChallenge"] = message.Data.Message;
            connection.Properties["SignedChallenge"] = sign;
            return new CallResult<KrakenChallengeResponse>(message.Data, message.OriginalData, null);
        }
    }
}
