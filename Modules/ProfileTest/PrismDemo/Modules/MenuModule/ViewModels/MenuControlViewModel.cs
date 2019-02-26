using CommonUILib.Enums;
using CommonUILib.Interfaces;
using CommonUILib.Models;
using MenuModule.Interfaces;
using Microsoft.Practices.ServiceLocation;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace MenuModule.ViewModels
{
    class MenuControlViewModel
    {
        IMenuControlModel _model;

        public ObservableCollection<IViewItem> MenuButtonCollection { get; set; }
        public MenuControlViewModel(IMenuControlModel model)
        {
            _model = model;
            MenuButtonCollection = GetMenuButtons();
        }

        private ObservableCollection<IViewItem> GetMenuButtons()
        {
            return new ObservableCollection<IViewItem>()
            {
                new ViewItem()
                {
                    MenuName = "Audio Demo",
                    MenuStyle = Application.Current.Resources["BaseToggleButtonStyle"] as Style,
                    MenuCommand = CommonUILib.CommonUIHelper.Instance.NavigateToCommand,
                    MenuData = Module.AudioDemoControl.ToString()
                },
                new ViewItem()
                {
                    MenuName = "HID Demo",
                    MenuStyle = Application.Current.Resources["BaseToggleButtonStyle"] as Style,
                    MenuCommand = CommonUILib.CommonUIHelper.Instance.NavigateToCommand,
                    MenuData = Module.HIDDemoControl.ToString()
                },
            };
        }
    }
}
