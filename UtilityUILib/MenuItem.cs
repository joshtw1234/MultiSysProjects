using System;
using System.Windows;
using System.Windows.Input;

namespace UtilityUILib
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

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action((T)parameter);
        }
    }

}
