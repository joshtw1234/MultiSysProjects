using CommonUILib.Interfaces;
using CommonUILib.Models;
using System;
using SystemControlLib.Structures;

namespace WPFTestPrism7.AppGUIModules.ViewModels.Features
{
    class SysHookClientControlViewModel
    {
        bool isButtonClick = false;
        public IViewItem MessageBox { get; set; }
        public IViewItem HookButton { get; set; }
        public SysHookClientControlViewModel()
        {
            MessageBox = new DebugViewItem() { MenuName = "Josh" };
            HookButton = new DebugViewItem() { MenuName = "Remove Hook", MenuCommand = new Prism.Commands.DelegateCommand<string>(OnButtonClick) };
            SystemControlLib.SystemHook.Instence.RegisterDeviceChangeCallBack(OnDeviceChangeCallback);
            SystemControlLib.SystemHook.Instence.RegisterWindowMessageCallback(OnWindowsMessageCallBack);
            SystemControlLib.SystemHook.Instence.RegisterLowLevelMessageCallBack(OnLowLeverMsgCallBack);
        }

        private void OnButtonClick(string obj)
        {
            isButtonClick = !isButtonClick;
            string hookButton = "Setup Hook";
            if (isButtonClick)
            {
                SystemControlLib.SystemHook.Instence.RemoveLowLevelHook();
            }
            else
            {
                hookButton = "Remove Hook";
                SystemControlLib.SystemHook.Instence.SetLowLevelHook();
            }
            HookButton.MenuName = hookButton;
        }

        private void OnLowLeverMsgCallBack(WinMessage winMessage)
        {
            if (winMessage.IsHandled)
            {
                MessageBox.MenuName += $"\n [{winMessage.Message}] [{winMessage.WParam}] [{winMessage.LParam}]";
            }
            else
            {
                MessageBox.MenuName += $"\n Less Zero [{winMessage.Message}] [{winMessage.WParam}] [{winMessage.LParam}]";
            }
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
