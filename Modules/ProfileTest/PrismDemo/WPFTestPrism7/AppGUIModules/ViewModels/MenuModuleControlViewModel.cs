using CommonUILib.Enums;
using CommonUILib.Interfaces;
using CommonUILib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFTestPrism7.AppGUIModules.Models.Interfaces;

namespace WPFTestPrism7.AppGUIModules.ViewModels
{
    public class MenuModuleControlViewModel
    {
        public MenuModuleControlViewModel(IMenuModuleControlModel model)
        {
            _model = model;
            MenuButtonCollection = GetMenuButtons();
        }
        IMenuModuleControlModel _model;

        public ObservableCollection<IViewItem> MenuButtonCollection { get; set; }

        private ObservableCollection<IViewItem> GetMenuButtons()
        {
            var menuButtons = new ObservableCollection<IViewItem>();
            for (int i = 0; i < Enum.GetNames(typeof(ModuleViewName)).Length; i++)
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
