using CmediaSDKTestApp.BaseModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace CmediaSDKTestApp.Models
{
    class BasePageContentModel : BindAbleBases, IMenuItem
    {
        public string MenuName { get; set; }
        public string MenuImage { get; set; }
        public string MenuImagePressed { get; set; }
        public string MenuData { get; set; }
        public MyCommond<string> MenuCommand { get; set; }
        public Style MenuStyle { get; set; }
        public bool MenuEnabled { get; set; }
        public bool MenuChecked { get; set; }
        private bool _menuVisibility;
        public bool MenuVisibility { get { return _menuVisibility; } set { _menuVisibility = value; onPropertyChanged(this, "MenuVisibility"); } }
    }

    class MicPageContentModel : BasePageContentModel
    {
        public IMenuItem DisplayText { get; set; }
        public ObservableCollection<HorzSliderControlModel> SliderControls { get; set; }
    }
}
