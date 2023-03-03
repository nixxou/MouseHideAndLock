using CursorAutoHider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;
using Gma.System.MouseKeyHook;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MouseHideAndLock
{
	public class MHLGameLaunch : IGameLaunchingPlugin
	{
		Form1 myForm;
		public void OnAfterGameLaunched(IGame game, IAdditionalApplication app, IEmulator emulator)
		{

			
			if (!Config.emulatorListInitialized) Config.LoadListFromIniFile();
			string plateform = game.Platform;
			string emul = emulator.Title;
			string key = emulator.Title + "||" + game.Platform;

			

			if (Config.emulatorsWithMouseHideActivated.ContainsKey(key))
			{
				if (Config.emulatorsWithMouseHideActivated[key])
				{
					Thread.Sleep(2000);
					//CursorsManager.Instance.HideCursors();
					myForm = new Form1();
					System.Windows.Forms.Application.Run(myForm);

				}
			}
			
		}


		public void OnBeforeGameLaunching(IGame game, IAdditionalApplication app, IEmulator emulator)
		{
			
		}

		public void OnGameExited()
		{
			myForm.Close();
			//CursorsManager.Instance.RestoreCursors();

		}



	}

}


