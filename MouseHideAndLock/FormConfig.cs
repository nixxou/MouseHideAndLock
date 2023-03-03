using CursorAutoHider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Assemblies;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace MouseHideAndLock
{
	public partial class FormConfig : Form
	{
		public FormConfig()
		{
			InitializeComponent();
		}

		private void buttonSave_Click(object sender, EventArgs e)
		{
			Config.SaveListBoxContentsToIniFile(checkedListBox1);
			Close();
		}

		private void FormConfig_Load(object sender, EventArgs e)
		{
			Config.LoadListFromIniFile();

			checkedListBox1.Items.Clear();
			//var listeEmulateurs = PluginHelper.DataManager.GetAllEmulators().Select(emu => emu.Title).ToArray();
			var listePlateform = PluginHelper.DataManager.GetAllPlatforms().Select(plat => plat.Name).ToArray();


			List<(IEmulator, IEmulatorPlatform)> emulators = new List<(IEmulator, IEmulatorPlatform)>();

			foreach (var emulator in PluginHelper.DataManager.GetAllEmulators())
			{
				foreach (var emulatorPlatform in emulator.GetAllEmulatorPlatforms())
				{
					if (listePlateform.Contains(emulatorPlatform.Platform))
					{
						string txtAffiche = emulator.Title + " " + emulatorPlatform.Platform;
						string cle = emulator.Title + "||" + emulatorPlatform.Platform;
						bool isChecked = false;
						if (Config.emulatorsWithMouseHideActivated.ContainsKey(cle))
						{
							isChecked = Config.emulatorsWithMouseHideActivated[cle];
						}
						checkedListBox1.Items.Add(txtAffiche, isChecked);

					}
				}
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var z = new Form1();
			z.ShowDialog();
		}
	}
}
