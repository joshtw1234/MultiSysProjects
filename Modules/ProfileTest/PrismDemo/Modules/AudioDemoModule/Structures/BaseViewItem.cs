using CommonUILib.Interfaces;
using Prism.Commands;
using System.ComponentModel;
using System.Windows;

namespace AudioDemoModule.Structures
{
    abstract class BaseViewItem : IViewItem, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            { PropertyChanged(sender, new PropertyChangedEventArgs(propertyName)); }
        }
        #endregion

        public virtual string MenuName { get; set; }
        public virtual string MenuImage { get; set; }
        public virtual string MenuImagePressed { get; set; }
        public virtual string MenuData { get; set; }
        public virtual DelegateCommand<string> MenuCommand { get; set; }
        public virtual Style MenuStyle { get; set; }
        public virtual bool MenuEnabled { get; set; }
        public virtual bool MenuChecked { get; set; }
        public virtual bool MenuVisibility { get; set; }
    }
}