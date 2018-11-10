using System.Collections.ObjectModel;
using MVVMUtilities.Common;

namespace DirectShowDemo.Models
{
    interface IMainWindowModel
    {
        IMenuItem GetDisplayMenuItem { get; }
        ObservableCollection<IMenuItem> GetCommonButtons { get; }
    }
}
