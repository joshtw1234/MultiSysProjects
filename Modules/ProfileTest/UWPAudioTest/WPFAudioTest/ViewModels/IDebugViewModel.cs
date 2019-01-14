using MyStandardDLL.MVVMUtility;
using System.Collections.ObjectModel;

namespace WPFAudioTest.ViewModels
{
    public interface IDebugViewModel
    {
        IViewItem DisplayMenuItem { get; set; }
        ObservableCollection<IViewItem> CommonButtons { get; set; }
    }
}
