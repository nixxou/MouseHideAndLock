using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbroken.LaunchBox.Plugins;

using System.Windows.Forms;



namespace MouseHideAndLock
{
	public class MHLConfig : ISystemMenuItemPlugin
	{

		public string Caption
		{
			get
			{
				return "MouseHide Configuration";
			}
		}

		public System.Drawing.Image IconImage
		{
			get
			{
				return SystemIcons.Exclamation.ToBitmap();
			}
		}

		public bool ShowInLaunchBox
		{
			get
			{
				return true;
			}
		}


		public bool ShowInBigBox
		{
			get
			{
				return false;
			}
		}


		public bool AllowInBigBoxWhenLocked
		{
			get
			{
				return false;
			}
		}

		public void OnSelected()
		{
			var x = new FormConfig();
			x.ShowDialog();	

		}
	}
}
