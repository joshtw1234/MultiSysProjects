using CommonUILib.Interfaces;
using CommonUILib.Models;
using Prism.Commands;
using PrismDemo.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PrismDemo.ViewModels
{
    class MainWindowViewModel : IProgressBarControlViewModel
    {
        IMainWindowModel _model;
        public IViewItem TextProgress { get; set; }
        public IViewItem ViewProgressBar { get; set; }
        private DelegateCommand<RoutedEventArgs> _mainWindowLoadedEvent;
        public DelegateCommand<RoutedEventArgs> MainWindowLoadedEvent => _mainWindowLoadedEvent ?? (_mainWindowLoadedEvent = new DelegateCommand<RoutedEventArgs>(OnMainWindowLoaded));

        private void OnMainWindowLoaded(RoutedEventArgs obj)
        {
            var resu = StartEntireProgress();
        }

        public MainWindowViewModel(IMainWindowModel model)
        {
            _model = model;
            TextProgress = new DebugViewItem()
            {
                MenuName = "Please wait processing....",
                MenuStyle = Application.Current.Resources["BaseTextBoxStyle"] as Style,
                MenuVisibility = true
            };

            ViewProgressBar = new ProgressBarViewItem()
            {
                MenuName = "50",
                MenuMinValue = "0",
                MenuMaxValue = "100",
                MenuStyle = Application.Current.Resources["CustomProgressBar"] as Style,
            };
            
        }
        private async Task StartEntireProgress()
        {
            await Task.Factory.StartNew(() =>
            {
                while (TextProgress.MenuVisibility)
                {
                    for (int i = 0; i <= 100; i += 10)
                    {
                        Thread.Sleep(10);
                        ViewProgressBar.MenuName = i.ToString();
                    }
                }
            });
        }

    }
}
