using System;
using System.Windows.Interop;
using SystemControlLib.CallBacks;
using SystemControlLib.Enums;
using SystemControlLib.Structures;

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
                case WinProc_Message.WM_MOUSEHOVER:
                case WinProc_Message.WM_MOUSELEAVE:
                case WinProc_Message.WM_MOUSEMOVE:
                case WinProc_Message.WM_MOUSEWHEEL:
                    //In Client Area
                case WinProc_Message.WM_LBUTTONDBLCLK:
                case WinProc_Message.WM_LBUTTONDOWN:
                case WinProc_Message.WM_LBUTTONUP:
                case WinProc_Message.WM_RBUTTONDBLCLK:
                case WinProc_Message.WM_RBUTTONDOWN:
                case WinProc_Message.WM_RBUTTONUP:
                    //In None Client Area
                case WinProc_Message.WM_NCLBUTTONDBLCLK:
                case WinProc_Message.WM_NCLBUTTONDOWN:
                case WinProc_Message.WM_NCLBUTTONUP:
                case WinProc_Message.WM_NCRBUTTONDBLCLK:
                case WinProc_Message.WM_NCRBUTTONDOWN:
                case WinProc_Message.WM_NCRBUTTONUP:
                    //Keyboard
                case WinProc_Message.WM_IME_KEYDOWN:
                case WinProc_Message.WM_IME_KEYUP:
                case WinProc_Message.WM_KEYDOWN:
                case WinProc_Message.WM_KEYUP:
                case WinProc_Message.WM_SYSKEYDOWN:
                case WinProc_Message.WM_SYSKEYUP:
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

    
}
