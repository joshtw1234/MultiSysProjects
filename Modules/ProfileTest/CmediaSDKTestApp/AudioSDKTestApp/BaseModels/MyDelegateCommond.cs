using System;
using System.Windows.Input;

namespace AudioSDKTestApp.BaseModels
{
    public class MyDelegateCommond<T> : ICommand
    {
        private Action<T> _action;
        private bool _canExecute;

        public MyDelegateCommond(Action<T> action)
        {
            _action = action;
            _canExecute = true;
        }

        public MyDelegateCommond(Action<T> action, bool canExecute)
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
}
