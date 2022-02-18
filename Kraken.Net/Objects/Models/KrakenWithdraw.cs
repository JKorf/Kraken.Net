using Newtonsoft.Json;

namespace Kraken.Net.Objects.Models
{
	/// <summary>
	/// Order info
	/// </summary>
	public class KrakenWithdraw
	{
		/// <summary>
		/// Reference id
		/// </summary>
		[JsonProperty("refid")]
		public string ReferenceId { get; set; } = string.Empty;
	}
}