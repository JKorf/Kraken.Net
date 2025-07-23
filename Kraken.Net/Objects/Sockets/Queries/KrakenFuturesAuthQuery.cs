using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;

namespace Kraken.Net.Objects.Sockets.Queries
{
    internal class KrakenFuturesAuthQuery : Query<KrakenChallengeResponse>
    {
        public KrakenFuturesAuthQuery(string apiKey) : base(new KrakenChallengeRequest { ApiKey = apiKey, Event = "challenge" }, false)
        {
            MessageMatcher = MessageMatcher.Create<KrakenChallengeResponse>(["challenge", "alert"], HandleMessage);
        }

        public CallResult<KrakenChallengeResponse> HandleMessage(SocketConnection connection, DataEvent<KrakenChallengeResponse> message)
        {
            if (message.Data.Event == "alert")
                return new CallResult<KrakenChallengeResponse>(default, message.OriginalData, new ServerError(message.Data.Message));

            var authProvider = (KrakenFuturesAuthenticationProvider)connection.ApiClient.AuthenticationProvider!;
            var sign = authProvider.AuthenticateWebsocketChallenge(message.Data.Message);
            connection.Properties["OriginalChallenge"] = message.Data.Message;
            connection.Properties["SignedChallenge"] = sign;
            return new CallResult<KrakenChallengeResponse>(message.Data, message.OriginalData, null);
        }
    }
}
