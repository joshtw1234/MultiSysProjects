using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CmediaSDKTestApp.BaseModels
{
    public class MenuItem : IMenuItem
    {
        public string MenuName { get; set; }
        public string MenuImage { get; set; }
        public string MenuImagePressed { get; set; }
        public string MenuData { get; set; }
        public Style MenuStyle { get; set; }
        public bool MenuEnabled { get; set; }
        public bool MenuChecked { get; set; }
        public bool MenuVisibility { get; set; }
        public MyCommond<string> MenuCommand { get; set; }
    }

    public class MyCommond<T> : ICommand
    {
        private Action<T> _action;
        private bool _canExecute;

        public MyCommond(Action<T> action)
        {
            _action = action;
            _canExecute = true;
        }

        public MyCommond(Action<T> action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter)
        {
            _action((T)parameter);
        }
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
