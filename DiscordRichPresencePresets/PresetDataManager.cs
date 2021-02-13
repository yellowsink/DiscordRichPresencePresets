using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DiscordRichPresencePresets
{
	public static class PresetDataManager
	{
		public static void SavePresetCollection(this IEnumerable<Presence> presences, string presetCollectionName)
		{
			var json        = JsonSerializer.Serialize(presences);
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

		public static List<Presence> LoadPresetCollection(string presetCollectionName)
		{
			var appData     = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var appDataRoot = Path.Combine(appData,     "Cain Atkinson/Discord Rich Presence Presets");
			var saveFolder  = Path.Combine(appDataRoot, "Saved Preset Collections");
			var fileName    = Path.Combine(saveFolder,  presetCollectionName + ".json");

			try
			{
				var json = File.ReadAllText(fileName);
				return JsonSerializer.Deserialize<List<Presence>>(json);
			}
			catch (Exception)
			{
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
}