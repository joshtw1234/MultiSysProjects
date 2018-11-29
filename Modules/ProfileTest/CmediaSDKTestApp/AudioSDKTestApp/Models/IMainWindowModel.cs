using System.Collections.ObjectModel;
using AudioSDKTestApp.BaseModels;

namespace AudioSDKTestApp.Models
{
    interface IMainWindowModel
    {
        ObservableCollection<IMenuItem> GetPageButtons { get; }
        ObservableCollection<IMenuItem> GetCommonButtons { get; }
        ObservableCollection<IMenuItem> GetContentPages { get; }

        void ModelInitialize();
    }
}
