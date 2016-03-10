/*
   SSW - The Open-Source Simple Stopwatch Performance Testing Tool
   Copyright (C) 2006-2008 Vlad Setchin <v_setchin@yahoo.com.au>

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace SSW.UI.Tray
{
    class TrayIconAnimator
    {
        #region Variables
        NotifyIcon notifyIcon = null;
        Icon defaultIcon = null;
        Timer timer = null;
        ArrayList iconArray = null;
        int iconArrayIndex = 0;
        #endregion Variables

        #region Constructor
        public TrayIconAnimator(NotifyIcon notifyIcon)
		{
			this.notifyIcon = notifyIcon;
			timer = new Timer();
			
			timer.Interval = 150;
			timer.Tick += new EventHandler(TimerTick);
			iconArray = new ArrayList();
			
			//notifyIcon.DoubleClick += new EventHandler(NotifyIconDoubleClick);
		}
		#endregion Constructor

        #region Timer
        void TimerTick(object sender, System.EventArgs e)
        {
            if (iconArrayIndex == iconArray.Count)
            {
                iconArrayIndex = 0;
            }
            notifyIcon.Icon = (Icon)iconArray[iconArrayIndex];
            iconArrayIndex++;
        }
        #endregion Timer

        #region Functions
        public void AddIcon(Icon icon)
        {
            iconArray.Add(icon);
        }
        public void StartAnimation()
        {
            timer.Start();
        }
        public void StopAnimation()
        {
            timer.Stop();
            notifyIcon.Icon = defaultIcon;
        }
        #endregion Functions

        #region Properties
        public Icon DefaultIcon
		{
			set
			{
				defaultIcon = value;
				notifyIcon.Icon = defaultIcon;
			}
			get
			{
				return defaultIcon;
			}
		}
		#endregion Properties
    }
}
