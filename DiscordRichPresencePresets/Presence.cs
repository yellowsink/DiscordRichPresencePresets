using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DiscordRichPresencePresets
{
#pragma warning disable IDE0079 // Remove unnecessary suppression
	[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
	[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
	[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
	public class Presence
	{
		public string               Data1          { get; set; }
		public string               Data2          { get; set; }
		public string               BigImage       { get; set; }
		public string               BigImageText   { get; set; }
		public string               SmallImage     { get; set; }
		public string               SmallImageText { get; set; }
		public List<PresenceButton> Buttons        { get; set; } = new();
	}

#pragma warning disable IDE0079 // Remove unnecessary suppression
	[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
	public class PresenceButton
	{
		public string Text { get; set; }
		public string Url  { get; set; }
	}
}