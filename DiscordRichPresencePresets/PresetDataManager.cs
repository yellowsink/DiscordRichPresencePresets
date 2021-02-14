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
		                                        int                        active,    bool   minify)
		{
			var json = JsonSerializer.Serialize(new PresetCollection
			{
				Presences = presences.ToArray(),
				Active    = active
			}, new JsonSerializerOptions
			{
				WriteIndented = !minify
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
			catch (Exception)
			{
				active = 0;
				return new List<Presence>();
			}
		}

		public static string[] GetPresetCollections()
		{
			var appData     = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var appDataRoot = Path.Combine(appData,     "Cain Atkinson/Discord Rich Presence Presets");
			var saveFolder  = Path.Combine(appDataRoot, "Saved Preset Collections");

			return Directory.Exists(saveFolder)
				       ? new DirectoryInfo(saveFolder).EnumerateFiles().Select(f => f.Name.Split('.')[0]).ToArray()
				       : Array.Empty<string>();
		}

		public static void SaveOptions(Options options, bool minify)
		{
			var appData     = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var appDataRoot = Path.Combine(appData,     "Cain Atkinson/Discord Rich Presence Presets");
			var filePath    = Path.Combine(appDataRoot, "settings.json");
			var json        = JsonSerializer.Serialize(options, new JsonSerializerOptions {WriteIndented = !minify});

			Directory.CreateDirectory(appDataRoot);
			File.WriteAllText(filePath, json);
		}

		public static Options LoadOptions()
		{
			var appData     = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var appDataRoot = Path.Combine(appData,     "Cain Atkinson/Discord Rich Presence Presets");
			var filePath    = Path.Combine(appDataRoot, "settings.json");

			return File.Exists(filePath)
				       ? JsonSerializer.Deserialize<Options>(File.ReadAllText(filePath))
				       : new();
		}
	}

	public class PresetCollection
	{
		public int        Active    { get; set; }
		public Presence[] Presences { get; set; }
	}
}