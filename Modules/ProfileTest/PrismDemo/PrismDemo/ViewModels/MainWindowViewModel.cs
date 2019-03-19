using Prism.Commands;
using PrismDemo.Interfaces;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace PrismDemo.ViewModels
{
    class MainWindowViewModel
    {
        IMainWindowModel _model;

        public MainWindowViewModel(IMainWindowModel model)
        {
            _model = model;
        }
    }
}
