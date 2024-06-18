﻿using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Deposit address
    /// </summary>
    public record KrakenDepositAddress
    {
        /// <summary>
        /// The actual address
        /// </summary>
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// The expire time of the address
        /// </summary>
        [JsonProperty("expiretm"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// If the address has been used before
        /// </summary>
        [JsonProperty("new")]
        public bool IsNew { get; set; }
        /// <summary>
        /// Tag
        /// </summary>
        [JsonProperty("tag")]
        public string? Tag { get; set; }
        /// <summary>
        /// Memo
        /// </summary>
        [JsonProperty("memo")]
        public string? Memo { get; set; }
    }
}
