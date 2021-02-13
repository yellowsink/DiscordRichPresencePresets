using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DiscordRichPresencePresets
{
	public static class PresetDataManager
	{
		public static void SavePresetCollection(this IEnumerable<Presence> presences, string presetCollectionName,
		                                        int                        active)
		{
			var json = JsonSerializer.Serialize(new PresetCollection
			{
				Presences = presences.ToArray(),
				Active    = active
			});
			var appData     = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var appDataRoot = Path.Combine(appData,     "Cain Atkinson/Discord Rich Presence Presets");
			var saveFolder  = Path.Combine(appDataRoot, "Saved Preset Collections");
			var fileName    = Path.Combine(saveFolder,  presetCollectionName + ".json");

			Directory.CreateDirectory(saveFolder);

			if (File.Exists(fileName)) File.Delete(fileName);
			var sw = File.CreateText(fileName);
			sw.Write(json);
			sw.Dispose();
		}

		public static List<Presence> LoadPresetCollection(string presetCollectionName, out int active)
		{
			var appData     = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var appDataRoot = Path.Combine(appData,     "Cain Atkinson/Discord Rich Presence Presets");
			var saveFolder  = Path.Combine(appDataRoot, "Saved Preset Collections");
			var fileName    = Path.Combine(saveFolder,  presetCollectionName + ".json");

			try
			{
				var json             = File.ReadAllText(fileName);
				var presetCollection = JsonSerializer.Deserialize<PresetCollection>(json);
				// ReSharper disable once PossibleNullReferenceException
				active = presetCollection.Active;
				return presetCollection.Presences.ToList();
			}
			catch (Exception ex)
			{
				throw ex;
				active = 0;
				return new List<Presence>();
			}
		}

		public static string[] GetPresetCollections()
		{
			var appData     = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var appDataRoot = Path.Combine(appData,     "Cain Atkinson/Discord Rich Presence Presets");
			var saveFolder  = Path.Combine(appDataRoot, "Saved Preset Collections");

			return new DirectoryInfo(saveFolder).EnumerateFiles().Select(f => f.Name.Split('.')[0]).ToArray();
		}
	}

	public class PresetCollection
	{
		public int        Active    { get; set; }
		public Presence[] Presences { get; set; }
	}
}