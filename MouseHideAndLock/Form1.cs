using CursorAutoHider;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MouseHideAndLock
{
	public partial class Form1 : Form
	{
		private IKeyboardMouseEvents g;

		Timer m_timer;
		Timer m_checkProcessTimer = new Timer();
		Point? m_lastMousePosition;
		DateTime? m_lastTime;
		volatile bool m_mouseClicked;

		int m_distanceThreshold = 5;
		int m_timeThresholdS = 2;
		string m_watchedApplication = "";

		static Rectangle workingRectangle = Screen.PrimaryScreen.Bounds;



		public Form1()
		{
			InitializeComponent();

			Screen s = Screen.FromPoint(Cursor.Position);
			if (s.Primary == false)
			{
				Cursor.Position = GoBack(Cursor.Position);
			}

			HideMouse();
			m_checkProcessTimer.Interval = 1000;
			m_checkProcessTimer.Tick += CheckProcessTimer_Tick;
			m_checkProcessTimer.Start();

			g = Hook.GlobalEvents();

			g.MouseDownExt += GlobalHookMouseDownExt;

			g.MouseMoveExt += G_MouseMoveExt;
			g.KeyDown += G_KeyDown;
			g.KeyUp += G_KeyUp;

			this.FormClosed += Form1_FormClosed;


		}

		private static Point GoBack(Point cursor)
		{
			if (cursor.X <= 3) cursor.X = 5;
			if (cursor.Y <= 3) cursor.Y = 5;
			if (cursor.X >= workingRectangle.Width - 3) cursor.X = workingRectangle.Width - 5;
			if (cursor.Y >= workingRectangle.Height - 3) cursor.Y = workingRectangle.Height - 5;


			return cursor;
		}


		private void G_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
			{
				shift = false;
			}
		}

		bool shift;
		bool allowedChange = false;

		private void G_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
			{
				shift = true;
				Cursor.Clip = new Rectangle();
			}
		}

		private void G_MouseMoveExt(object sender, MouseEventExtArgs e)
		{

			Screen s = Screen.FromPoint(Cursor.Position);
			if (s.Primary == false)
			{
				if (shift || allowedChange)
				{
					allowedChange = true;
				}
			}
			else
			{
				if (shift == false || allowedChange) Cursor.Clip = Screen.PrimaryScreen.Bounds;
				allowedChange = false;
			}

		}

		private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
		{
			//m_logControl.Items.Add("Mouse clicked: " + e.Clicked);
			m_mouseClicked |= e.Clicked;
		}

		private void CheckProcessTimer_Tick(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(m_watchedApplication) || Process.GetProcessesByName(m_watchedApplication).Length > 0)
			{
				if (m_timer == null)
				{
					m_lastTime = null;
					m_lastMousePosition = null;

					m_timer = new Timer();
					m_timer.Interval = 300;
					m_timer.Tick += Timer_Tick;
					m_timer.Start();
				}
			}
			else if (m_timer != null)
			{
				m_timer.Stop();
				m_timer = null;
				ShowMouse();

				m_lastTime = null;
				m_lastMousePosition = null;
			}
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (m_checkProcessTimer != null)
				m_checkProcessTimer.Stop();

			if (m_timer != null)
				m_timer.Stop();

			g.MouseDownExt -= GlobalHookMouseDownExt;

			g.MouseMoveExt -= G_MouseMoveExt;
			g.KeyUp -= G_KeyUp;
			g.KeyDown -= G_KeyDown;


			CursorsManager.Instance.RestoreCursors();
			g.Dispose();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// Minimise le formulaire
			this.WindowState = FormWindowState.Minimized;
			// Cache le formulaire de la barre des tâches
			this.ShowInTaskbar = false;
			// Définit l'icône pour le contrôle NotifyIcon
			//notifyIcon1.Icon = Properties.Resources.YourIcon;
			// Affiche un message lorsque l'utilisateur passe la souris sur l'icône
			//notifyIcon1.Text = "Your app name";
		}

		private void ShowMouse()
		{
			CursorsManager.Instance.RestoreCursors();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			var mousePosition = System.Windows.Forms.Cursor.Position;
			var now = DateTime.Now;

			if (m_lastMousePosition.HasValue)
			{
				double xdiff = (mousePosition.X - m_lastMousePosition.Value.X);
				double ydiff = (mousePosition.Y - m_lastMousePosition.Value.Y);
				double distSqr = xdiff * xdiff + ydiff * ydiff;

				if ((now - m_lastTime.Value).TotalSeconds > m_timeThresholdS)
				{
					if (!m_mouseClicked && distSqr < m_distanceThreshold)
						HideMouse();
					else
						ShowMouse();

					m_mouseClicked = false;
					m_lastTime = null;
					m_lastMousePosition = null;
				}
				else if (distSqr > m_distanceThreshold)
					ShowMouse();
			}
			else
			{
				m_lastTime = now;
				m_lastMousePosition = mousePosition;
			}
		}

		private void HideMouse()
		{
			CursorsManager.Instance.HideCursors();
		}

		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{

		}
	}
}
