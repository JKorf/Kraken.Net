using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;

namespace Kraken.Net.Objects.Sockets.Queries
{
    internal class KrakenFuturesAuthQuery : Query<KrakenChallengeResponse>
    {
        private readonly KrakenFuturesAuthenticationProvider _authProvider;

        public KrakenFuturesAuthQuery(
            KrakenFuturesAuthenticationProvider authProvider,
            string apiKey) : base(new KrakenChallengeRequest { ApiKey = apiKey, Event = "challenge" }, false)
        {
            _authProvider = authProvider;
            MessageRouter = MessageRouter.CreateForQuery<KrakenChallengeResponse>(["challenge", "alert"], HandleMessage);
        }

        public CallResult<KrakenChallengeResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, KrakenChallengeResponse message)
        {
            if (message.Event == "alert")
                return CallResult.Fail<KrakenChallengeResponse>(new ServerError(ErrorInfo.Unknown with { Message = message.Message }), originalData);

            var sign = _authProvider.AuthenticateWebsocketChallenge(message.Message);
            connection.Properties["OriginalChallenge"] = message.Message;
            connection.Properties["SignedChallenge"] = sign;
            return CallResult.Ok(message, originalData);
        }
    }
}
