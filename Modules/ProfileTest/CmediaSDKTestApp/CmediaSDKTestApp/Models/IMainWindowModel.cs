using System.Collections.ObjectModel;
using CmediaSDKTestApp.BaseModels;

namespace CmediaSDKTestApp.Models
{
    interface IMainWindowModel
    {
        ObservableCollection<IMenuItem> GetPageButtons { get; }
        ObservableCollection<IMenuItem> GetCommonButtons { get; }
        ObservableCollection<BasePageContentViewModel> GetContentPages { get; }
    }
}
