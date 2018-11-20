using MVVMUtilities.Common;
using System.Collections.ObjectModel;

namespace MVVMUtilities.ViewModels
{
    public interface IDebugViewModel
    {
        IViewItem DisplayMenuItem { get; set; }
        ObservableCollection<IViewItem> CommonButtons { get; set; }
    }
}
