using DirectShowDemo.Models;
using MVVMUtilities.Common;
using MVVMUtilities.ViewModels;
using System.Collections.ObjectModel;

namespace DirectShowDemo.ViewModels
{
    class MainWindowViewModel : IDebugViewModel
    {
        IMainWindowModel _model;

        public IViewItem DisplayMenuItem { get; set; }
        public ObservableCollection<IViewItem> CommonButtons { get; set; }
        public MainWindowViewModel(IMainWindowModel model)
        {
            _model = model;
            DisplayMenuItem = _model.GetDisplayMenuItem;
            CommonButtons = _model.GetCommonButtons;
            _model.ModuleInitialize();
        }
    }
}
