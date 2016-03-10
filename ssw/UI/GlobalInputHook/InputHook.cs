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
using System.Runtime.InteropServices;

namespace SSW.UI.GlobalInputHooks
{
    public abstract class InputHook : object
    {
        #region Common Functions
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        #region Import Hook Functions
        //Import for SetWindowsHookEx function.
        //Use this function to install a hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
            IntPtr hInstance, int threadId);

        //Import for UnhookWindowsHookEx.
        //Call this function to uninstall the hook.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
             CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //Import for CallNextHookEx.
        //Use this function to pass the hook information to next hook procedure in chain.
        [DllImport("user32.dll", CharSet = CharSet.Auto,
             CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode,
            Int32 wParam, IntPtr lParam);

        /// <summary>
		/// Loads the library.
		/// </summary>
		/// <param name="lpFileName">Name of the library</param>
		/// <returns>A handle to the library</returns>
		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string lpFileName);
        #endregion Import Hook Functions
        #endregion Common Functions

        public InputHook()
        {
        }

        public abstract void InstallHook();
        public abstract void RemoveHook();
    }
}
