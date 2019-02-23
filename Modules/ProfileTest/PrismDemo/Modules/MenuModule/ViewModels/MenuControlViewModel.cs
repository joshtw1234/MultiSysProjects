using CommonUILib.Interfaces;
using CommonUILib.Models;
using MenuModule.Interfaces;
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
                    MenuStyle = Application.Current.Resources["BaseToggleButtonStyle"] as Style
                }
            };
        }
    }
}
