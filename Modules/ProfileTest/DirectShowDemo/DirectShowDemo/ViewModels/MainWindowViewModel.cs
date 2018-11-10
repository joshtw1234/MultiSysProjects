using DirectShowDemo.Models;
using MVVMUtilities.Common;
using MVVMUtilities.ViewModels;
using System.Collections.ObjectModel;

namespace DirectShowDemo.ViewModels
{
    class MainWindowViewModel : IDebugViewModel
    {
        IMainWindowModel _model;

        public IMenuItem DisplayMenuItem { get; set; }
        public ObservableCollection<IMenuItem> CommonButtons { get; set; }
        public MainWindowViewModel(IMainWindowModel model)
        {
            _model = model;
            DisplayMenuItem = _model.GetDisplayMenuItem;
            CommonButtons = _model.GetCommonButtons;
        }
    }
}
