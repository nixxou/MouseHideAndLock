using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbroken.LaunchBox.Plugins.Data;
using Unbroken.LaunchBox.Plugins;

namespace MouseHideAndLock
{
	static class PluginUtils
	{

		/// <summary>
		/// Get a list of (emulator, emulator platform) tuples for a given platform.
		/// If the default emulator ID is specified, the resulting list will include the default emulator at index 0.
		/// </summary>
		/// <param name="platform"></param>
		/// <param name="defaultEmulatorId"></param>
		/// <returns>A list of (IEmulator, IEmulatorPlatform) tuples.</returns>
		public static List<(IEmulator, IEmulatorPlatform)> GetPlatformEmulators(string platform, string defaultEmulatorId = null)
		{
			List<(IEmulator, IEmulatorPlatform)> emulators = new List<(IEmulator, IEmulatorPlatform)>();

			foreach (var emulator in PluginHelper.DataManager.GetAllEmulators())
			{
				foreach (var emulatorPlatform in emulator.GetAllEmulatorPlatforms())
				{
					if (string.Equals(emulatorPlatform.Platform, platform))
					{
						if (string.Equals(emulator.Id, defaultEmulatorId))
						{
							if (!emulators.Select(emu => emu.Item1.Id).Contains(defaultEmulatorId) || emulatorPlatform.IsDefault)
							{
								emulators.Insert(0, (emulator, emulatorPlatform));
							}
							else
							{
								emulators.Add((emulator, emulatorPlatform));
							}
						}
						else
						{
							emulators.Add((emulator, emulatorPlatform));
						}
					}
				}
			}

			return emulators;
		}

		/// <summary>
		/// Get the emulator title. If the emulator is RetroArch, also gets the core in use
		/// </summary>
		/// <param name="emulator"></param>
		/// <param name="emulatorPlatform"></param>
		/// <returns>The emulator title, plus the core where applicable.</returns>
		public static string GetEmulatorTitle(IEmulator emulator, IEmulatorPlatform emulatorPlatform)
		{
			string title = emulator.Title;

			if (string.Equals(emulator.Title, "Retroarch", StringComparison.InvariantCultureIgnoreCase))
			{
				try
				{
					string corePath = emulatorPlatform.CommandLine.Split(new[] { ' ' })[1].Trim(new[] { '"' });
					string core = Path.GetFileNameWithoutExtension(Path.GetFileName(corePath));
					title = string.Format("{0} ({1})", title, core);
				}
				catch (Exception)
				{
				}
			}

			return title;
		}
	}
}
