using CryptoExchange.Net.Converters.SystemTextJson;
namespace Kraken.Net.Objects.Models
{
	/// <summary>
	/// Order info
	/// </summary>
    [SerializationModel]
	public record KrakenWithdraw
	{
		/// <summary>
		/// Reference id
		/// </summary>
		[JsonPropertyName("refid")]
		public string ReferenceId { get; set; } = string.Empty;
	}
}
