using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace DesktopAppLowLevelKeyboardHook
{
    public class LowLevelKeyboardListener
    {
        private const int WH_KEYBOARD_LL = 13;

        #region Win32API
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        #endregion

        //Invoke when user presses a key
        public event Action<RawKey> OnKeyDown;

        //Invoke when user releases a key
        public event Action<RawKey> OnKeyUp;

        //public event EventHandler<KeyPressedArgs> OnKeyPressed; 

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private LowLevelKeyboardProc handleKeyboardProc;
        private IntPtr _hookID = IntPtr.Zero;

        public LowLevelKeyboardListener()
        {
            handleKeyboardProc = HookCallback;
        }

        public void StartHookKeyboard()
        {
            _hookID = SetHook(handleKeyboardProc);
        }

        public void StopHookKeyboard()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private IntPtr SetHook(LowLevelKeyboardProc _handleKeyboardProc)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, _handleKeyboardProc, IntPtr.Zero, 0);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0) return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);

            var args = (KeyboardArgs)lParam;
            var state = (RawKeyState)wParam;
            var key = (RawKey)args;

            if (state == RawKeyState.KeyDown || state == RawKeyState.SysKeyDown)
            {
                if(OnKeyDown != null) { OnKeyDown(key); }
            }
            else
            {
                if(OnKeyUp != null) { OnKeyUp(key); }
            }
            //if (nCode >= 0)
            //{
            //    int vkCode = Marshal.ReadInt32(lParam);
            //   // OnKeyPressed?.Invoke(this, new KeyPressedArgs((ConsoleKey)vkCode));
            //    OnKeyPressed?.Invoke(this, new KeyPressedArgs(""+wParam.ToString()));
            //}

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

    }

    //public class KeyPressedArgs : EventArgs
    //{
    //    //public ConsoleKey KeyPressed { get; private set; }

    //    //public KeyPressedArgs(ConsoleKey key)
    //    //{
    //    //    KeyPressed = key;
    //    //}

    //    public string KeyPressed { get; private set; }

    //    public KeyPressedArgs(string key)
    //    {
    //        KeyPressed = key;
    //    }
    //}
}