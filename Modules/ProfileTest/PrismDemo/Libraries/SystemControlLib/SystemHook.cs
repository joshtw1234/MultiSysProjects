using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        private List<IntPtr> _notificationHandles;
        private WinMessageCallback _onWinMessage;
        private DeviceChangeCallBack _onDeviceCallback;
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
                _notificationHandles = new List<IntPtr>();
                SetDeviceNotification(new BroadcastDeviceinterface()
                { DeviceType = WM_DeviceType.DBT_DEVTYP_DEVICEINTERFACE, ClassGuid = new Guid(SystemControlLibConsts.GUID_DEVINTERFACE_USB_DEVICE) });
                //SetDeviceNotification(new BroadcastDeviceinterface()
                //{ DeviceType = WM_DeviceType.DBT_DEVTYP_DEVICEINTERFACE, ClassGuid = new Guid(SystemControlLibConsts.KSCATEGORY_AUDIO) });
                return true;
            }
            return false;
        }

        public void RegisterWindowMessageCallback(WinMessageCallback _callBack)
        {
            _onWinMessage += _callBack;
            
        }

        public void RegisterDeviceChangeCallBack(DeviceChangeCallBack _callBack)
        {
            _onDeviceCallback += _callBack;
        }

        /// <summary>
        /// Registers a window to receive notifications when USB devices are plugged or unplugged.
        /// </summary>
        private void SetDeviceNotification(BroadcastDeviceinterface devInterface)
        {
            devInterface.Size = Marshal.SizeOf(devInterface);
            IntPtr filterPtr = Marshal.AllocHGlobal(devInterface.Size);
            Marshal.StructureToPtr(devInterface, filterPtr, true);

            var _notificationHandle = NativeImportMethods.RegisterDeviceNotification(_winHandle.Handle, filterPtr, 0);
            _notificationHandles.Add(_notificationHandle);

            Marshal.FreeHGlobal(filterPtr);
        }

        /// <summary>
        /// Unregisters the window for USB device notifications
        /// </summary>
        public void RemoveDeviceNotification()
        {
            foreach (var handel in _notificationHandles)
            {
                if (handel != null)
                {
                    NativeImportMethods.UnregisterDeviceNotification(handel);
                }
            }
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
                    break;
                case WinProc_Message.WM_KEYDOWN:
                    _onWinMessage?.Invoke(new WinMessage() { Message = (WinProc_Message)msg, WParam = wParam, LParam = lParam, IsHandled = handled });
                    break;
                case WinProc_Message.WM_KEYUP:
                case WinProc_Message.WM_SYSKEYDOWN:
                case WinProc_Message.WM_SYSKEYUP:
                    _onWinMessage?.Invoke(new WinMessage() { Message = (WinProc_Message)msg, WParam = wParam, LParam = lParam, IsHandled = handled });
                    break;
                // For power event
                case WinProc_Message.WM_POWERBROADCAST:
                // For user session change event
                case WinProc_Message.WM_WTSSESSION_CHANGE:
                
                // For device detection event
                case WinProc_Message.WM_DEVICECHANGE:
                    switch ((WM_DeviceChange)wParam)
                    {
                        case WM_DeviceChange.DBT_DEVICEARRIVAL:
                        case WM_DeviceChange.DBT_DEVICEREMOVECOMPLETE:
                            _onDeviceCallback?.Invoke(new DeviceMessage() { Message = (WM_DeviceChange)wParam, LParam = lParam, IsHandled = handled });
                            break;
                    }
                    break;
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
