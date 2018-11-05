﻿using CmediaSDKTestApp.BaseModels;
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
        public MyCommond<string> MenuCommand { get; set; }
        public Style MenuStyle { get; set; }
        public bool MenuEnabled { get; set; }
        public bool MenuChecked { get; set; }
        public bool MenuVisibility { get; set; }
    }
}