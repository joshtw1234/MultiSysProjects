using Microsoft.Win32;
using System;
using System.Windows.Interop;
using SystemControlLib.Enums;

namespace SystemControlLib
{
    public class SystemHook
    {
        private static SystemHook _instance = null;
        private HwndSource _winHandle;
        private WinMessageCallback _onWinMessage;
        public static SystemHook Instence
        {
            get
            {
                return _instance ?? (_instance = new SystemHook());
            }
        }

        public bool Initialize(HwndSource windowHandle)
        {
            if (_winHandle == null)
            {
                _winHandle = windowHandle;
                _winHandle.AddHook(WndProc);
                return true;
            }
            return false;
        }

        public void RegisterWindowMessageCallback(WinMessageCallback _callBack)
        {
            _onWinMessage += _callBack;
            
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch ((WinProc_Message)msg)
            {
                // For power event
                case WinProc_Message.WM_POWERBROADCAST:
                // For user session change event
                case WinProc_Message.WM_WTSSESSION_CHANGE:
                
                // For device detection event
                case WinProc_Message.WM_DEVICECHANGE:
                case WinProc_Message.WM_QUIT:
                    _onWinMessage?.Invoke(new WinMessage() { Message = (WinProc_Message)msg, WParam = wParam, LParam = lParam, IsHandled = handled });
                    break;
                case WinProc_Message.WM_QUERYENDSESSION:
                    _onWinMessage?.Invoke(new WinMessage() { Message = (WinProc_Message)msg, WParam = wParam, LParam = lParam, IsHandled = handled });
                    handled = true;
                    break;
                // For user log off event
                case WinProc_Message.WM_ENDSESSION:
                    _onWinMessage?.Invoke(new WinMessage() { Message = (WinProc_Message)msg, WParam = wParam, LParam = lParam, IsHandled = handled });
                    handled = false;
                    break;
            }
            return IntPtr.Zero;
        }
    }

    public delegate void WinMessageCallback(WinMessage winMessage);

    public struct WinMessage
    {
        public WinProc_Message Message;
        public IntPtr WParam;
        public IntPtr LParam;
        public bool IsHandled;
    }

}
