using CmediaSDKTestApp.BaseModels;
using CmediaSDKTestApp.Models;
using System.Collections.ObjectModel;

namespace CmediaSDKTestApp.ViewModels
{
    class MainWindowViewModel
    {
        IMainWindowModel _model;

        public ObservableCollection<IMenuItem> PageButtons { get; set; }
        public ObservableCollection<IMenuItem> CommonButtons { get; set; }
        public ObservableCollection<IMenuItem> ContentPages { get; set; }

        public MainWindowViewModel(IMainWindowModel model)
        {
            _model = model;
            PageButtons = _model.GetPageButtons;
            CommonButtons = _model.GetCommonButtons;
            ContentPages = _model.GetContentPages;

            _model.ModelInitialize();
        }
    }
}
