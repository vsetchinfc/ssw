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

using SSW.Properties;
using SSW.UI;
using SSW.UI.Tray;
using SSW.Watcher;

namespace SSW.UI
{
    public class UIManager
    {
        public UIManager()
        {
            InitWatcher();
            InitTrayIcon();
            InitUIInputControllers();
        }

        #region UI Input Controllers
        private KeyboardController keyController = null;
        private MouseController mouseController = null;
        MouseControlsSettings mouseSettings = null;

        private void InitUIInputControllers()
        {
            keyController = new KeyboardController(watcher);
            mouseController = new MouseController(watcher);

            mouseSettings = MouseControlsSettings.Default;
            mouseSettings.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SettingsPropertyChanged);
        }

        void SettingsPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ControlsEnabled")
            {
                if (mouseSettings.ControlsEnabled)
                {
                    trayIcon.BaloonToolTip = "Enabled Mouse Controls";
                    trayIcon.SetTrayDefaultIcon();
                }
                else
                {
                    trayIcon.BaloonToolTip = "Disabled Mouse Controls";
                    trayIcon.SetTrayDisabledIcon();
                }
            }
        }
        #endregion  UI Input Controllers

        #region Tray Icon
        TrayIcon trayIcon = null;
        ContextMenu contextMenu = null;

        private void InitTrayIcon()
        {
            trayIcon = new TrayIcon();
            contextMenu = new ContextMenu(watcher);
            trayIcon.Menu = contextMenu;
        }
        #endregion Tray Icon

        #region Watcher
        IWatcher watcher = null;

        private void InitWatcher()
        {
            watcher = new StopWatch();
        }
        #endregion Watcher
    }
}
