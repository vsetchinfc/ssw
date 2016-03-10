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
using System.Reflection;
using System.Windows.Forms;

namespace SSW.UI.GlobalInputHooks
{
    /// <summary>
    /// This class allows you to tap keyboard and mouse and / or to detect their activity even when an 
    /// application runes in background or does not have any user interface at all. This class raises 
    /// common .NET events with KeyEventArgs and MouseEventArgs so you can easily retrive any information you need.
    /// </summary>
    /// <remarks>
    /// 	created by - Georgi
    /// 	created on - 22.05.2004 13:08:01
    /// </remarks>
    public class MouseHook : InputHook
    {
        #region Mouse Hooks
        #region Mouse Envent Handlers
        public event MouseEventHandler OnMouseActivity;
        #endregion Mouse Envent Handlers

        #region Mouse Variables and Constants
        //values from Winuser.h in Microsoft SDK.
        public const int WH_MOUSE_LL = 14;	//mouse hook constant
        static int hMouseHook = 0; //Declare mouse hook handle as int.
        HookProc HookProcedure; //Declare MouseHookProcedure as HookProc type.
        #endregion Mouse Variables and Constants

        #region public class POINT
        //Declare wrapper managed POINT class.
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }
        #endregion public class POINT

        #region public class MouseHookStruct
        //Declare wrapper managed MouseHookStruct class.
        [StructLayout(LayoutKind.Sequential)]
        public class HookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }
        #endregion public class MouseHookStruct

        #region Mouse Hook Constants
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDBLCLK = 0x209;
        #endregion Mouse Hook Constants

        public override void InstallHook()
        {
            // install Mouse hook 
            if (hMouseHook == 0)
            {
                // Create an instance of HookProc.
                HookProcedure = new HookProc(GlobalHookProc);
                IntPtr hInstance = LoadLibrary("User32");
                hMouseHook = SetWindowsHookEx(WH_MOUSE_LL,
                    HookProcedure,hInstance
                    /*Marshal.GetHINSTANCE(
                        Assembly.GetExecutingAssembly().GetModules()[0])*/,
                    0);

                //If SetWindowsHookEx fails.
                if (hMouseHook == 0)
                {
                    RemoveHook();
                    throw new Exception("SetWindowsHookEx failed.");
                }
            }
        }
        public override void RemoveHook()
        {
            bool retMouse = true;

            if (hMouseHook != 0)
            {
                retMouse = UnhookWindowsHookEx(hMouseHook);
                hMouseHook = 0;
            }

            //If UnhookWindowsHookEx fails.
            if (!retMouse) throw new Exception("UnhookWindowsHookEx failed.");
        }
        private int GlobalHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            // if ok and someone listens to our events
            if ((nCode >= 0) && (OnMouseActivity != null))
            {

                MouseButtons button = MouseButtons.None;
                switch (wParam)
                {
                    case WM_LBUTTONDOWN:
                        //case WM_LBUTTONUP: 
                        //case WM_LBUTTONDBLCLK: 
                        button = MouseButtons.Left;
                        break;
                    case WM_RBUTTONDOWN:
                        //case WM_RBUTTONUP: 
                        //case WM_RBUTTONDBLCLK: 
                        button = MouseButtons.Right;
                        break;
                    case WM_MBUTTONDOWN:
                        button = MouseButtons.Middle;
                        break;
                }
                int clickCount = 0;
                if (button != MouseButtons.None)
                    if (wParam == WM_LBUTTONDBLCLK || wParam == WM_RBUTTONDBLCLK) clickCount = 2;
                    else clickCount = 1;

                //Marshall the data from callback.
                HookStruct MyMouseHookStruct = (HookStruct)Marshal.PtrToStructure(lParam, typeof(HookStruct));
                MouseEventArgs e = new MouseEventArgs(
                                                    button,
                                                    clickCount,
                                                    MyMouseHookStruct.pt.x,
                                                    MyMouseHookStruct.pt.y,
                                                    0);
                OnMouseActivity(this, e);
            }
            return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }
        #endregion Mouse Hooks
    }
}
