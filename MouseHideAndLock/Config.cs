using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins.Data;
using Unbroken.LaunchBox.Plugins;
using System.Net.NetworkInformation;

namespace MouseHideAndLock
{
	

	public class Config
	{
		public static Dictionary<string, bool> emulatorsWithMouseHideActivated = new Dictionary<string, bool>();
		public static bool emulatorListInitialized = false;
		private static string _pluginPath = "";

		public static string GetPluginPath()
		{
			if (_pluginPath != "") return _pluginPath;
			string assemblyPath = Assembly.GetEntryAssembly().Location;
			string assemblyDirectory = Path.GetDirectoryName(assemblyPath);

			string launchBoxRootPath = Path.GetFullPath(Path.Combine(assemblyDirectory, @".."));
			string relativePluginPath = @"Plugins\MouseHideAndLock";
			_pluginPath = Path.Combine(launchBoxRootPath, relativePluginPath);
			return _pluginPath;
		}


		public static string GetConfigFile()
		{
			string assemblyPath = Assembly.GetEntryAssembly().Location;
			string assemblyFileName = Path.GetFileName(assemblyPath);
			string assemblyDirectory = Path.GetDirectoryName(assemblyPath);

			string launchBoxRootPath = Path.GetFullPath(Path.Combine(assemblyDirectory, @".."));
			string relativePluginPath = @"Plugins\MouseHideAndLock";

			string iniFilePath = Path.Combine(launchBoxRootPath, relativePluginPath, "config_mhl.ini");
			return iniFilePath;
		}





		public static void LoadListFromIniFile()
		{

			emulatorsWithMouseHideActivated.Clear();
			IniFile ini = new IniFile(GetConfigFile());

			var listePlateform = PluginHelper.DataManager.GetAllPlatforms().Select(plat => plat.Name).ToArray();
			List<(IEmulator, IEmulatorPlatform)> emulators = new List<(IEmulator, IEmulatorPlatform)>();

			foreach (var emulator in PluginHelper.DataManager.GetAllEmulators())
			{
				foreach (var emulatorPlatform in emulator.GetAllEmulatorPlatforms())
				{
					if (listePlateform.Contains(emulatorPlatform.Platform))
					{
						string key = emulator.Title + "||" + emulatorPlatform.Platform;
						bool val = bool.Parse(ini.Read("Config", key, "False"));
						emulatorsWithMouseHideActivated[key] = val;
					}
				}
			}
			emulatorListInitialized = true;
		}

		public static void SaveListBoxContentsToIniFile(CheckedListBox checkedListBox1)
		{

			var listePlateform = PluginHelper.DataManager.GetAllPlatforms().Select(plat => plat.Name).ToArray();
			Dictionary<string, bool> listeChecked = new Dictionary<string, bool>();

			List<(IEmulator, IEmulatorPlatform)> emulators = new List<(IEmulator, IEmulatorPlatform)>();
			int i = 0;
			foreach (var emulator in PluginHelper.DataManager.GetAllEmulators())
			{
				foreach (var emulatorPlatform in emulator.GetAllEmulatorPlatforms())
				{
					if (listePlateform.Contains(emulatorPlatform.Platform))
					{
						string keyEmulator = emulator.Title + "||" + emulatorPlatform.Platform;

						if (checkedListBox1.GetItemChecked(i))
						{
							listeChecked[keyEmulator] = true;
						}
						else
						{
							listeChecked[keyEmulator] = false;
						}

						i++;
					}
				}
			}


			IniFile ini = new IniFile(GetConfigFile());
			foreach (var emulElem in listeChecked)
			{
				ini.Write("Config", emulElem.Key, emulElem.Value.ToString());
			}
			Config.emulatorsWithMouseHideActivated = listeChecked;

		}
	}
}
