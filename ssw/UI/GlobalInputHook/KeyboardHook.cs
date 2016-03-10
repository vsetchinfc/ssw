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
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;

namespace SSW.UI.GlobalInputHooks
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyboardHook : InputHook
    {
        public KeyboardHook()
        {
        }

        #region Keyboard Hooks
		#region Keyboard Envent Handlers
		public event KeyEventHandler KeyDown;
		public event KeyPressEventHandler KeyPress;
		public event KeyEventHandler KeyUp;
		#endregion Keyboard Envent Handlers
		
		#region Keyboard Variables and Constants
		static int hKeyboardHook = 0; //Declare keyboard hook handle as int.
		public const int WH_KEYBOARD_LL = 13;	//keyboard hook constant	
		HookProc HookProcedure; //Declare KeyboardHookProcedure as HookProc type.
		#endregion Keyboard Variables and Constants
		
		#region public class KeyboardHookStruct
		//Declare wrapper managed KeyboardHookStruct class.
		[StructLayout(LayoutKind.Sequential)]
		public class HookStruct
		{
			public int vkCode;	//Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
			public int scanCode; // Specifies a hardware scan code for the key. 
			public int flags;  // Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
			public int time; // Specifies the time stamp for this message.
			public int dwExtraInfo; // Specifies extra information associated with the message. 
        }
		#endregion public class KeyboardHookStruct
		
		#region Keyboard Hook Constants
		private const int WM_KEYDOWN 		= 0x100;
		private const int WM_KEYUP 			= 0x101;
		private const int WM_SYSKEYDOWN 	= 0x104;
		private const int WM_SYSKEYUP 		= 0x105;
		#endregion Keyboard Hook Constants
		
		//The GetKeyboardState function copies the status of the 256 virtual keys to the specified buffer. 
		[DllImport("user32")] 
		public static extern int GetKeyboardState(byte[] pbKeyState);
		
		//The ToAscii function translates the specified virtual-key code and keyboard state to the corresponding character or characters. The function translates the code using the input language and physical keyboard layout identified by the keyboard layout handle.
		[DllImport("user32")] 
		public static extern int ToAscii(int uVirtKey, //[in] Specifies the virtual-key code to be translated. 
		                                 int uScanCode, // [in] Specifies the hardware scan code of the key to be translated. The high-order bit of this value is set if the key is up (not pressed). 
		                                 byte[] lpbKeyState, // [in] Pointer to a 256-byte array that contains the current keyboard state. Each element (byte) in the array contains the state of one key. If the high-order bit of a byte is set, the key is down (pressed). The low bit, if set, indicates that the key is toggled on. In this function, only the toggle bit of the CAPS LOCK key is relevant. The toggle state of the NUM LOCK and SCROLL LOCK keys is ignored.
		                                 byte[] lpwTransKey, // [out] Pointer to the buffer that receives the translated character or characters. 
		                                 int fuState); // [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise. 
		
		public override void InstallHook()
		{
			// install Keyboard hook 
			if(hKeyboardHook == 0)
			{
				HookProcedure = new HookProc(GlobalHookProc);
                IntPtr hInstance = LoadLibrary("User32");
				hKeyboardHook = SetWindowsHookEx( WH_KEYBOARD_LL,
                    HookProcedure, hInstance,0);

				//If SetWindowsHookEx fails.
				if(hKeyboardHook == 0 )	
                {
					int errorCode = Marshal.GetLastWin32Error();
                    //do cleanup
                    RemoveHook();
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
				}
			}
		}
		public override void RemoveHook()
        {
        	bool retKeyboard = true;
			
			if(hKeyboardHook != 0)
			{
				retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
				hKeyboardHook = 0;
			} 
			
			//If UnhookWindowsHookEx fails.
			if (!retKeyboard) throw new Exception("UnhookWindowsHookEx failed.");
        }
		private int GlobalHookProc(int nCode, Int32 wParam, IntPtr lParam)
		{
			// it was ok and someone listens to events
			if ((nCode >= 0) && (KeyDown!=null || KeyUp!=null || KeyPress!=null))
			{
				HookStruct MyKeyboardHookStruct = (HookStruct) Marshal.PtrToStructure(lParam, typeof(HookStruct));
				// raise KeyDown
				if ( KeyDown!=null && ( wParam ==WM_KEYDOWN || wParam==WM_SYSKEYDOWN ))
				{
					Keys keyData=(Keys)MyKeyboardHookStruct.vkCode;
                    keyData |= Control.ModifierKeys;
					KeyEventArgs e = new KeyEventArgs(keyData);
					KeyDown(this, e);
                    Debug.WriteLine(keyData.ToString());
				}
				
				// raise KeyPress
				if ( KeyPress != null &&  wParam == WM_KEYDOWN )
				{
					byte[] keyState = new byte[256];
					GetKeyboardState(keyState);

					byte[] inBuffer= new byte[2];
					if (ToAscii(MyKeyboardHookStruct.vkCode,
				            MyKeyboardHookStruct.scanCode,
				            keyState,
				            inBuffer,
				            MyKeyboardHookStruct.flags)==1) 
				            {
				            	KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
								KeyPress(this, e);
				            }
				}
				
				// raise KeyUp
				if ( KeyUp!=null && ( wParam == WM_KEYUP || wParam == WM_SYSKEYUP ))
				{
					Keys keyData=(Keys)MyKeyboardHookStruct.vkCode;
                    // add modifier keys
                    keyData |= Control.ModifierKeys;
					KeyEventArgs e = new KeyEventArgs(keyData);
					KeyUp(this, e);
				}

			}
			return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam); 
		}
		#endregion Keyboard Hooks
    }
}
