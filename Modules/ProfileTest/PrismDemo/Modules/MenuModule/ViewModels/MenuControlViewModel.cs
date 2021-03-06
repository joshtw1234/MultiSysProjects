﻿using CommonUILib.Enums;
using CommonUILib.Interfaces;
using CommonUILib.Models;
using MenuModule.Interfaces;
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
            var menuButtons = new ObservableCollection<IViewItem>();
            for (int i = 0; i< Enum.GetNames(typeof(ModuleViewName)).Length; i++)
            {
                var item = new ViewItem()
                {
                    MenuName = ((ModuleViewName)i).ToString(),
                    MenuStyle = Application.Current.Resources["BaseToggleButtonStyle"] as Style,
                    MenuCommand = CommonUILib.CommonUIHelper.Instance.NavigateToCommand,
                    MenuData = ((ModuleViewName)i).ToString()
                };
                menuButtons.Add(item);
            }
            return menuButtons;
        }
    }
}
