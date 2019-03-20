using CommonUILib.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace AudioDemoModule.Structures
{
    abstract class BaseViewItem : BindableBase, IViewItem
    {
        public virtual string MenuName { get; set; }
        public virtual string MenuImage { get; set; }
        public virtual string MenuImagePressed { get; set; }
        public virtual string MenuData { get; set; }
        public virtual DelegateCommand<string> MenuCommand { get; set; }
        public virtual Style MenuStyle { get; set; }
        public virtual bool MenuEnabled { get; set; }
        public virtual bool MenuChecked { get; set; }
        //public virtual bool MenuVisibility { get; set; }
        private bool _menuVisibility;
        public virtual bool MenuVisibility
        {
            get => _menuVisibility;
            set
            {
                SetProperty(ref _menuVisibility, value);
            }
        }
    }
}