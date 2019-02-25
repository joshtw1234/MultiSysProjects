using CommonUILib.Enums;
using CommonUILib.Interfaces;
using CommonUILib.Models;
using MenuModule.Interfaces;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace MenuModule.ViewModels
{
    class MenuControlViewModel
    {
        IMenuControlModel _model;
        /// <summary>
        /// The second level menu item command.
        /// </summary>
        private DelegateCommand<string> _menuButtonClickEvent = null;

        /// <summary>
        /// DelegateCommand which executes when a second level menu item is clicked .
        /// </summary>
        public DelegateCommand<string> MenuButtonClickEvent
            => _menuButtonClickEvent ?? (_menuButtonClickEvent = new DelegateCommand<string>(OnMenuButtonClick));

        private void OnMenuButtonClick(string obj)
        {
            throw new NotImplementedException();
        }

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
                    MenuCommand = MenuButtonClickEvent,
                    MenuData = Module.Audio.ToString()
                },
                new ViewItem()
                {
                    MenuName = "HID Demo",
                    MenuStyle = Application.Current.Resources["BaseToggleButtonStyle"] as Style,
                    MenuCommand = MenuButtonClickEvent,
                    MenuData = Module.HID.ToString()
                },
            };
        }
    }
}
