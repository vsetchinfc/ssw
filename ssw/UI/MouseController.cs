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
using SSW.Watcher;
using SSW.UI.GlobalInputHooks;

namespace SSW.UI
{
    /// <summary>
    /// Class for controlling watcher using Mouse
    /// </summary>
    public class MouseController
    {
        private MouseControlsSettings mouseSettings = MouseControlsSettings.Default;
        private MouseHook mouseHook = null;
        private IWatcher watcher = null;

        public MouseController(IWatcher watcher)
        {
            this.watcher = watcher;
            mouseHook = new MouseHook();
            mouseHook.InstallHook();
            mouseHook.OnMouseActivity += new System.Windows.Forms.MouseEventHandler(MouseActivity);
        }
        ~MouseController()
        {
            mouseHook.RemoveHook();
        }

        void MouseActivity(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Clicks > 0 && mouseSettings.ControlsEnabled)
            {
                if (e.Button == mouseSettings.StartStopMouseButton /*MouseButtons.Left*/ )
                {
                    watcher.StartStop();
                }
                else if (e.Button == mouseSettings.ResetMouseButton /*MouseButtons.Middle*/)
                {
                    watcher.Reset();
                }
            }
        }

        
    }
}
