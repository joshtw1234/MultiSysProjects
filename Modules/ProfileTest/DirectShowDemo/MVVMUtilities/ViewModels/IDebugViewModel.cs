using MVVMUtilities.Common;
using System.Collections.ObjectModel;

namespace MVVMUtilities.ViewModels
{
    interface IDebugViewModel
    {
        IMenuItem DisplayMenuItem { get; set; }
        ObservableCollection<IMenuItem> CommonButtons { get; set; }
    }
}
