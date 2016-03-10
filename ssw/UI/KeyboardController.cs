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
using SSW.UI.GlobalInputHooks;
using SSW.Watcher;

namespace SSW.UI
{
    /// <summary>
    /// Class for controlling watcher using Keyboard
    /// </summary>
    public class KeyboardController
    {
        KeyboardControlsSettings keyboardSettings = KeyboardControlsSettings.Default;
        MouseControlsSettings mouseSettings = MouseControlsSettings.Default;

        private IWatcher watcher = null;

        // TODO: Change to use Context Menu when implementing Shortcuts for Opening Options and Results Forms
        public KeyboardController(IWatcher watcher)
        {
            this.watcher = watcher;
            
            InitKeyboardHook();
        }

        ~KeyboardController()
        {
            keyboardHook.RemoveHook();
        }

        #region Keyboard Hook
        private KeyboardHook keyboardHook = null;

        private void InitKeyboardHook()
        {
            keyboardHook = new KeyboardHook();
            try
            {
                keyboardHook.InstallHook();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            keyboardHook.KeyUp += new System.Windows.Forms.KeyEventHandler(HookKeyUp);
        }

        void HookKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (keyboardSettings.StartStopKey == e.KeyCode
                && keyboardSettings.StartStopAltKey == e.Alt
                && keyboardSettings.StartStopControlKey == e.Control
                && keyboardSettings.StartStopShiftKey == e.Shift)
            {
                watcher.StartStop();
            }
            else if (keyboardSettings.ResetKey == e.KeyCode
                && keyboardSettings.ResetControlKey == e.Control
                && keyboardSettings.ResetShiftKey == e.Shift
                && keyboardSettings.ResetAltKey == e.Alt)
            {
                watcher.Reset();
            }
            else if (keyboardSettings.EnableDisableMouseControlsKey == e.KeyCode
              && keyboardSettings.EnableDisableMouseControlsControlKey == e.Control
              && keyboardSettings.EnableDisableMouseControlsShiftKey == e.Shift
              && keyboardSettings.EnableDisableMouseControlsAltKey == e.Alt)
            {
                mouseSettings.ControlsEnabled = !mouseSettings.ControlsEnabled;
            }
        }
        #endregion Keyboard Hook
    }
}
