using CryptoExchange.Net.Objects.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Net.Objects.Options
{
    /// <summary>
    /// Kraken options
    /// </summary>
    public class KrakenOptions : LibraryOptions<KrakenRestOptions, KrakenSocketOptions, ApiCredentials, KrakenEnvironment>
    {
    }
}
