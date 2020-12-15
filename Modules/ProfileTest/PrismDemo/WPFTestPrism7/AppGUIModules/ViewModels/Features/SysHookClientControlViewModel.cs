using CommonUILib.Interfaces;
using CommonUILib.Models;
using System.Windows;
using System.Windows.Interop;
using SystemControlLib.Structures;

namespace WPFTestPrism7.AppGUIModules.ViewModels.Features
{
    class SysHookClientControlViewModel
    {
        public IViewItem MessageBox { get; set; }
        public SysHookClientControlViewModel()
        {
            MessageBox = new DebugViewItem() { MenuName = "Josh" };
            SystemControlLib.SystemHook.Instence.RegisterDeviceChangeCallBack(OnDeviceChangeCallback);
            SystemControlLib.SystemHook.Instence.RegisterWindowMessageCallback(OnWindowsMessageCallBack);
        }

        private void OnWindowsMessageCallBack(WinMessage winMessage)
        {
            MessageBox.MenuName += $"\n [{winMessage.Message}] [{winMessage.WParam}] [{winMessage.LParam}]";
        }

        private void OnDeviceChangeCallback(DeviceMessage devMessage)
        {
            MessageBox.MenuName += $"\n [{devMessage.Message}] [{devMessage.LParam}]";
        }
    }
}
