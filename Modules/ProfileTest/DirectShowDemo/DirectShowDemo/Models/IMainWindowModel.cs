using System.Collections.ObjectModel;
using MVVMUtilities.Common;

namespace DirectShowDemo.Models
{
    interface IMainWindowModel
    {
        IViewItem GetDisplayMenuItem { get; }
        ObservableCollection<IViewItem> GetCommonButtons { get; }

        void ModuleInitialize();
    }
}
