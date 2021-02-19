using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DiscordRichPresencePresets.Shared
{
	public static class PresetDataManager
	{
		public static void SavePresetCollection(this IEnumerable<Presence> presences, string presetCollectionName,
		                                        int active, bool minify, SaveLocations location, string path = null)
		{
			var json = JsonSerializer.Serialize(new PresetCollection
			{
				Presences = presences.ToArray(),
				Active    = active
			}, new JsonSerializerOptions
			{
				WriteIndented = !minify
			});
			var appDataRoot = BuildSavePath(location, path);
			var saveFolder  = Path.Combine(appDataRoot, "Saved Preset Collections");
			var fileName    = Path.Combine(saveFolder,  presetCollectionName + ".json");

			Directory.CreateDirectory(saveFolder);

			if (File.Exists(fileName)) File.Delete(fileName);
			var sw = File.CreateText(fileName);
			sw.Write(json);
			sw.Dispose();
		}

		public static List<Presence> LoadPresetCollection(string        presetCollectionName, out int active,
		                                                  SaveLocations location,             string  path = null)
		{
			var appDataRoot = BuildSavePath(location, path);
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

		public static string[] GetPresetCollections(SaveLocations location, string path = null)
		{
			var appDataRoot = BuildSavePath(location, path);
			var saveFolder  = Path.Combine(appDataRoot, "Saved Preset Collections");

			return Directory.Exists(saveFolder)
				       ? new DirectoryInfo(saveFolder).EnumerateFiles().Select(f => f.Name.Split('.')[0]).ToArray()
				       : Array.Empty<string>();
		}

		public static void SaveOptions(Options options, bool minify, SaveLocations location)
		{
			if (location == SaveLocations.Custom)
				location = SaveLocations.Portable;

			var appDataRoot = BuildSavePath(location);
			var filePath    = Path.Combine(appDataRoot, "settings.json");
			var json        = JsonSerializer.Serialize(options, new JsonSerializerOptions {WriteIndented = !minify});

			Directory.CreateDirectory(appDataRoot);
			File.WriteAllText(filePath, json);
		}

		public static Options LoadOptions()
		{
			// try all possible paths
			var appdata   = BuildSavePath(SaveLocations.Appdata);
			var portable  = BuildSavePath(SaveLocations.Portable);
			var documents = BuildSavePath(SaveLocations.Documents);
			var filePath  = Path.Combine(appdata, "settings.json");

			var current = SaveLocations.Appdata;
			tryLoad:
			if (File.Exists(filePath)) return JsonSerializer.Deserialize<Options>(File.ReadAllText(filePath));
			// ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
			switch (current)
			{
				case SaveLocations.Appdata:
					filePath = Path.Combine(portable, "settings.json");
					current  = SaveLocations.Portable;
					goto tryLoad;
				case SaveLocations.Portable:
					filePath = Path.Combine(documents, "settings.json");
					current  = SaveLocations.Documents;
					goto tryLoad;
				case SaveLocations.Documents:
					return new();
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static string BuildSavePath(SaveLocations location, string customDir = null)
		{
			var relativePath = "Cain Atkinson/Discord Rich Presence Presets";
			return location switch
			{
				SaveLocations.Appdata =>
					Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), relativePath),
				SaveLocations.Portable =>
					Path.Combine(Environment.CurrentDirectory, "data"),
				SaveLocations.Documents =>
					Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), relativePath),
				SaveLocations.Custom =>
					customDir ?? throw new ArgumentException("Custom dir was null", nameof(customDir)),
				_ => throw new ArgumentOutOfRangeException(nameof(location), location, null)
			};
		}
	}

	public class PresetCollection
	{
		public int        Active    { get; set; }
		public Presence[] Presences { get; set; }
	}
}