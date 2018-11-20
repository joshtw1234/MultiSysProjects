using System.ComponentModel;
using System.Windows;

namespace MVVMUtilities.Common
{
    /// <summary>
    /// This is the implement class of IViewItem
    /// </summary>
    public class ViewItem : IViewItem
    {
        public string MenuName { get; set; }
        public string MenuImage { get; set; }
        public string MenuImagePressed { get; set; }
        public string MenuData { get; set; }
        public Style MenuStyle { get; set; }
        public bool MenuEnabled { get; set; }
        public bool MenuChecked { get; set; }
        public bool MenuVisibility { get; set; }
        public MyDelegateCommond<string> MenuCommand { get; set; }
    }

    public class BindAbleBases : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            { PropertyChanged(sender, new PropertyChangedEventArgs(propertyName)); }
        }
        #endregion
    }

}
