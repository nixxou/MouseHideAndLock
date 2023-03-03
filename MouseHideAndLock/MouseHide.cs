using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CursorAutoHider;
using Gma.System.MouseKeyHook;

namespace MouseHideAndLock
{
	public class MouseHide
	{

		private IKeyboardMouseEvents g;
		Rectangle workingRectangle = Screen.PrimaryScreen.Bounds;

		Timer m_timer;
		Timer m_checkProcessTimer = new Timer();
		Point? m_lastMousePosition;
		DateTime? m_lastTime;
		volatile bool m_mouseClicked;

		int m_distanceThreshold = 5;
		int m_timeThresholdS = 3;

		bool active = false;

		public MouseHide()
		{

			m_checkProcessTimer.Interval = 1000;
			m_checkProcessTimer.Tick += CheckProcessTimer_Tick;
			m_checkProcessTimer.Start();

			g = Hook.GlobalEvents();

			g.MouseDownExt += GlobalHookMouseDownExt;

			g.MouseMoveExt += G_MouseMoveExt;
			g.KeyDown += G_KeyDown;
			g.KeyUp += G_KeyUp;



		}


		private void G_KeyUp(object sender, KeyEventArgs e)
		{
			
			if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
			{
				shift = false;
			}
		}

		bool shift;

		private void G_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
			{
				shift = true;
			}
		}

		private void G_MouseMoveExt(object sender, MouseEventExtArgs e)
		{
			MessageBox.Show("Move");
			Point ModdedCursorPos = Cursor.Position;
			if (e.Y >= -10 && e.Y < 0) ModdedCursorPos.Y = 0;
			if (e.X >= -10 && e.X < 0) ModdedCursorPos.X = 0;
			if (e.X >= workingRectangle.Width && e.X <= workingRectangle.Width + 10) ModdedCursorPos.X = workingRectangle.Width - 2;
			if (e.Y >= workingRectangle.Height && e.Y <= workingRectangle.Height + 10) ModdedCursorPos.Y = workingRectangle.Height - 2;

			Screen s = Screen.FromPoint(ModdedCursorPos);
			if (s.Primary)
			{
				//  this.Text = e.X + ":" + e.Y + ":" + e.Location;
				if (e.Y < 0 && !shift && !Control.IsKeyLocked(Keys.Scroll))
				{
					Cursor.Position = new Point(e.X, 0);
					e.Handled = true;
				}

				if (e.X < 0 && !shift && !Control.IsKeyLocked(Keys.Scroll))
				{
					Cursor.Position = new Point(0, e.Y);
					e.Handled = true;
				}

				if (e.X >= workingRectangle.Width && !shift && !Control.IsKeyLocked(Keys.Scroll))
				{
					Cursor.Position = new Point(workingRectangle.Width - 5, e.Y);
					e.Handled = true;
				}

				if (e.Y >= workingRectangle.Height && !shift && !Control.IsKeyLocked(Keys.Scroll))
				{
					Cursor.Position = new Point(e.X, workingRectangle.Height - 5);
					e.Handled = true;
				}


			}

		}

		private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
		{
			//m_logControl.Items.Add("Mouse clicked: " + e.Clicked);
			m_mouseClicked |= e.Clicked;
		}

		private void CheckProcessTimer_Tick(object sender, EventArgs e)
		{
			//if (string.IsNullOrEmpty(m_watchedApplication) || Process.GetProcessesByName(m_watchedApplication).Length > 0)
			//{
				if (m_timer == null)
				{
					m_lastTime = null;
					m_lastMousePosition = null;

					m_timer = new Timer();
					m_timer.Interval = 300;
					m_timer.Tick += Timer_Tick;
					m_timer.Start();
				}
			//}
			/*
			else if (m_timer != null)
			{
				m_timer.Stop();
				m_timer = null;
				ShowMouse();

				m_lastTime = null;
				m_lastMousePosition = null;
			}
			*/
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




	}
}
