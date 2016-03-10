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
using System.Windows.Forms;
using System.Drawing;

namespace SSW.UI.Tray
{
    class TrayIcon
    {
        #region Variables
		private TrayIcon notifyIcon = null;
        private TrayIconAnimator trayIconAnimator = null;

        private Icon defaultIcon = null;
        private Icon mouseDisabledIcon = null;
        private ContextMenu menu = null;
        #endregion Variables

        #region Constants
        private const string DEFAULT_ICON = @"res\TIMER.ICO";
        private const string DISABLED_ICON = @"res\TIMER_DISABLE.ICO";
        private const string RUN_ICON = @"res\TIMER_RUN.ICO";
        #endregion Constants

        #region Functions
        public TrayIcon()
        {
            InitTrayIcon();
        }
        ~TrayIcon()
        {
            notifyIcon.Dispose();
        }
        private void InitTrayIcon()
        {
            try
            {
                notifyIcon = new NotifyIcon();
                trayIconAnimator = new TrayIconAnimator(notifyIcon);

                defaultIcon = new Icon(DEFAULT_ICON);
                mouseDisabledIcon = new Icon(DISABLED_ICON);

                trayIconAnimator.DefaultIcon = defaultIcon;
                trayIconAnimator.AddIcon(new Icon(RUN_ICON));
                trayIconAnimator.AddIcon(defaultIcon);

                notifyIcon.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion Functions

        #region Properties
        public void SetTrayDefaultIcon()
        {
            trayIconAnimator.DefaultIcon = defaultIcon;
        }
        public void SetTrayDisabledIcon()
        {
            trayIconAnimator.DefaultIcon = mouseDisabledIcon;
        }
        public string BaloonToolTip
        {
            set
            {
                notifyIcon.ShowBalloonTip(0, "Stopwatch", value, ToolTipIcon.Info);
            }
        }
        public string Text
        {
            set
            {
                notifyIcon.Text = value;
            }
        }
        public ContextMenu Menu
        {
            set
            {
                menu = value;
                notifyIcon.ContextMenuStrip = menu.Menu;
                menu.ValueUpdated += new SSW.Watcher.ValueUpdatedHandler(WatcherValueUpdated);
            }
        }

        void WatcherValueUpdated(string value)
        {
            this.BaloonToolTip = value;
        }
        #endregion Properties
    }
}
