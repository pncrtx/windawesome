﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Windawesome
{
	public class CpuMonitorWidget : IFixedWidthWidget
	{
		private readonly PerformanceCounter counter;
		private Label label;
		private readonly Timer updateTimer;
		private int left, right;
		private bool isLeft;
		private readonly string prefix;
		private readonly string postfix;

		public CpuMonitorWidget(string prefix = "CPU:", string postfix = "%", int updateTime = 1000)
		{
			updateTimer = new Timer { Interval = updateTime };
			updateTimer.Tick += OnTimerTick;

			this.prefix = prefix;
			this.postfix = postfix;

			counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
		}

		private void OnTimerTick(object sender, System.EventArgs e)
		{
			label.Text = prefix + counter.NextValue().ToString("00") + postfix;
		}

		#region IWidget Members

		void IWidget.StaticInitializeWidget(Windawesome windawesome, Config config)
		{
		}

		void IWidget.InitializeWidget(Bar bar)
		{
			label = bar.CreateLabel(prefix + counter.NextValue().ToString("00") + postfix, 0);
			label.TextAlign = ContentAlignment.MiddleCenter;
		}

		IEnumerable<Control> IWidget.GetControls(int left, int right)
		{
			isLeft = right == -1;

			RepositionControls(left, right);

			return new[] { label };
		}

		public void RepositionControls(int left, int right)
		{
			if (isLeft)
			{
				this.left = left;
				label.Location = new Point(left, 0);
				this.right = label.Right;
			}
			else
			{
				this.right = right;
				label.Location = new Point(right - label.Width, 0);
				this.left = label.Left;
			}
		}

		int IWidget.GetLeft()
		{
			return left;
		}

		int IWidget.GetRight()
		{
			return right;
		}

		void IWidget.WidgetShown()
		{
			updateTimer.Start();
		}

		void IWidget.WidgetHidden()
		{
			updateTimer.Stop();
		}

		void IWidget.StaticDispose()
		{
		}

		void IWidget.Dispose()
		{
		}

		void IWidget.Refresh()
		{
		}

		#endregion
	}
}
