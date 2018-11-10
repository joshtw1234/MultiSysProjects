using CmediaSDKTestApp.BaseModels;
using System.Windows;

namespace CmediaSDKTestApp.Models
{
    class BindAbleMenuItem : BindAbleBases, IMenuItem
    {
        private string _menuName;
        public string MenuName { get { return _menuName; } set { _menuName = value; onPropertyChanged(this, "MenuName"); } }
        public string MenuImage { get; set; }
        public string MenuImagePressed { get; set; }
        public string MenuData { get; set; }
        public MyDelegateCommond<string> MenuCommand { get; set; }
        public Style MenuStyle { get; set; }
        public bool MenuEnabled { get; set; }
        private bool _menuChecked;
        public bool MenuChecked { get { return _menuChecked; } set { _menuChecked = value; onPropertyChanged(this, "MenuChecked"); } }
        public bool MenuVisibility { get; set; }
    }
}
