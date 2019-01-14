using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStandardDLL.MVVMUtility;

namespace WPFAudioTest.Models
{
    interface IMainWindowModel
    {
        IViewItem GetDisplayMenuItem { get; }
        ObservableCollection<IViewItem> GetCommonButtons { get; }

        void InitializeAudio();
    }
}
