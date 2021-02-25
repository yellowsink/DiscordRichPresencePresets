using System;
using System.Collections.Generic;

namespace DiscordRichPresencePresets.Shared
{
	public class Options
	{
		public string                               ClientId          { get; set; } = "810097912901402654";
		public bool                                 AutoSave          { get; set; } = false;
		public string                               DefaultCollection { get; set; } = "default";
		public bool                                 MinifiedJson      { get; set; } = true;
		public SaveLocations                        SaveLocation      { get; set; } = 0;
		public string                               CustomSavePath    { get; set; } = null;
		public Dictionary<string, OptionsExtension> Extensions        { get; set; } = new();
	}

	public abstract class OptionsExtension
	{
		public Type Type => GetType();
	}

	public enum SaveLocations
	{
		Appdata   = 0,
		Portable  = 1,
		Documents = 2,
		Custom    = 3
	}
}