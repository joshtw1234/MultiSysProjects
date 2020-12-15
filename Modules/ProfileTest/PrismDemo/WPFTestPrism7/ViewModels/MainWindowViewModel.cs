using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;

namespace WPFTestPrism7.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand<EventArgs> WindowSourceInitializedEvent { get; set; }
        public MainWindowViewModel(Models.Interfaces.IMainWindowsModel model)
        {
            WindowSourceInitializedEvent = new DelegateCommand<EventArgs>(OnWindowSourceInitialized);
        }

        private void OnWindowSourceInitialized(EventArgs obj)
        {
            SystemControlLib.SystemHook.Instence.Initialize(System.Windows.Interop.HwndSource.FromHwnd(new System.Windows.Interop.WindowInteropHelper(Application.Current.MainWindow).Handle));
            SystemControlLib.SystemHook.Instence.SetLowLevelHook();
        }
    }
}
