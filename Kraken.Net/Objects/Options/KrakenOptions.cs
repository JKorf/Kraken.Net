using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Options
{
    /// <summary>
    /// Kraken options
    /// </summary>
    public class KrakenOptions
    {
        /// <summary>
        /// Rest client options
        /// </summary>
        public KrakenRestOptions Rest { get; set; } = new KrakenRestOptions();

        /// <summary>
        /// Socket client options
        /// </summary>
        public KrakenSocketOptions Socket { get; set; } = new KrakenSocketOptions();

        /// <summary>
        /// Trade environment. Contains info about URL's to use to connect to the API. Use `KrakenEnvironment` to swap environment, for example `Environment = KrakenEnvironment.Live`
        /// </summary>
        public KrakenEnvironment? Environment { get; set; }

        /// <summary>
        /// The api credentials used for signing requests.
        /// </summary>
        public ApiCredentials? ApiCredentials { get; set; }

        /// <summary>
        /// The DI service lifetime for the IKrakenSocketClient
        /// </summary>
        public ServiceLifetime? SocketClientLifeTime { get; set; }
    }
}
