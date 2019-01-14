using System.Collections.ObjectModel;
using MyStandardDLL.MVVMUtility;
using WPFAudioTest.Models;

namespace WPFAudioTest.ViewModels
{
    class MainWindowViewModel : IDebugViewModel
    {
        IMainWindowModel _model;
        public MainWindowViewModel(IMainWindowModel model)
        {
            _model = model;
            DisplayMenuItem = _model.GetDisplayMenuItem;
            CommonButtons = _model.GetCommonButtons;
        }

        public IViewItem DisplayMenuItem { get; set; }
        public ObservableCollection<IViewItem> CommonButtons { get; set; }
    }
}
