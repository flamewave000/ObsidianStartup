using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace Obsidian_Startup
{
	public static class ObsidianUtil
	{
		public static readonly string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "obsidian", "obsidian.json");
		public static readonly string execPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Obsidian", "Obsidian.exe");
		public static ObsidianConfig? LoadVaults() => JsonSerializer.Deserialize<ObsidianConfig>(File.ReadAllText(configPath));
		public static void SaveVaults(ObsidianConfig vaults) => File.WriteAllText(configPath, JsonSerializer.Serialize(vaults));
		public static void Launch() => Process.Start(new ProcessStartInfo() { UseShellExecute = true, FileName = execPath });
		public static void OpenVault(string name) => Process.Start(new ProcessStartInfo() { UseShellExecute = true, FileName = "obsidian://open-vault?vault=" + Uri.EscapeDataString(Path.GetFileName(name)) });
	}
	public class ObsidianConfig
	{
		public Dictionary<string, ObsidianVault> vaults { get; set; } = new();
	}
	public class ObsidianVault
	{
		public string path { get; set; } = "";
		public ulong ts { get; set; }
		public bool open { get; set; }
	}
}
