using System.Diagnostics.CodeAnalysis;

namespace DiscordRichPresencePresets
{
#pragma warning disable IDE0079 // Remove unnecessary suppression
	[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
	public class Presence
	{
		public string Data1          { get; set; }
		public string Data2          { get; set; }
		public string BigImage       { get; set; }
		public string BigImageText   { get; set; }
		public string SmallImage     { get; set; }
		public string SmallImageText { get; set; }
	}
}