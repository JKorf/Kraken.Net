using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;

namespace Kraken.Net.Objects.Sockets.Queries
{
    internal class KrakenFuturesAuthQuery : Query<KrakenChallengeResponse>
    {
        public KrakenFuturesAuthQuery(string apiKey) : base(new KrakenChallengeRequest { ApiKey = apiKey, Event = "challenge" }, false)
        {
            MessageMatcher = MessageMatcher.Create<KrakenChallengeResponse>(["challenge", "alert"], HandleMessage);
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<KrakenChallengeResponse>(["challenge", "alert"], HandleMessage);
        }

        public CallResult<KrakenChallengeResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenChallengeResponse message)
        {
            if (message.Event == "alert")
                return new CallResult<KrakenChallengeResponse>(default, originalData, new ServerError(ErrorInfo.Unknown with { Message = message.Message }));

            var authProvider = (KrakenFuturesAuthenticationProvider)connection.ApiClient.AuthenticationProvider!;
            var sign = authProvider.AuthenticateWebsocketChallenge(message.Message);
            connection.Properties["OriginalChallenge"] = message.Message;
            connection.Properties["SignedChallenge"] = sign;
            return new CallResult<KrakenChallengeResponse>(message, originalData, null);
        }
    }
}
