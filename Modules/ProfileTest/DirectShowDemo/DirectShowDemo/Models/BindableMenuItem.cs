using MVVMUtilities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DirectShowDemo.Models
{
    class BindableMenuItem : BindAbleBases, IMenuItem
    {
        private string _menuName;
        public string MenuName { get { return _menuName; } set { _menuName = value; onPropertyChanged(this, "MenuName"); } }
        public string MenuImage { get; set; }
        public string MenuImagePressed { get; set; }
        public string MenuData { get; set; }
        public MyDelegateCommond<string> MenuCommand { get; set; }
        public Style MenuStyle { get; set; }
        public bool MenuEnabled { get; set; }
        public bool MenuChecked { get; set; }
        public bool MenuVisibility { get; set; }
    }
}
