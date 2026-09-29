using Kraken.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kraken.Net.Objects.Models
{
    /// <summary>
    /// Wallet accounts page
    /// </summary>
    public record KrakenWalletAccountPage
    {
        /// <summary>
        /// ["<c>accounts</c>"]
        /// </summary>
        [JsonPropertyName("accounts")]
        public KrakenWalletAccount[] Accounts { get; set; } = [];
    }

    /// <summary>
    /// Wallet account
    /// </summary>
    public record KrakenWalletAccount
    {
        /// <summary>
        /// ["<c>account_id</c>"] Account id
        /// </summary>
        [JsonPropertyName("account_id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>status</c>"] Account status
        /// </summary>
        [JsonPropertyName("status")]
        public AccountStatus Status { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Account type
        /// </summary>
        [JsonPropertyName("type")]
        public AccountWalletType Type { get; set; }
        /// <summary>
        /// ["<c>name</c>"] Account name
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        /// <summary>
        /// ["<c>flags</c>"] Flags
        /// </summary>
        [JsonPropertyName("flags")]
        public KrakenWalletAccountFlags Flags { get; set; } = default!;
    }

    /// <summary>
    /// Wallet account flags
    /// </summary>
    public record KrakenWalletAccountFlags
    {
        /// <summary>
        /// ["<c>user_defined</c>"] Whether wallet account was created by the user
        /// </summary>
        [JsonPropertyName("user_defined")]
        public bool UserDefined { get; set; }
        /// <summary>
        /// ["<c>active</c>"] Account is active
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }
}
