using CommonUILib.Interfaces;
using CommonUILib.Models;
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
            StartEntireProgress();
        }

        void StartEntireProgress()
        {
            Task.Factory.StartNew(() =>
            {
                while (TextProgress.MenuVisibility)
                {
                    for (int i = 0; i <= 100; i += 10)
                    {
                        Thread.Sleep(100);
                        ViewProgressBar.MenuName = i.ToString();
                    }
                }
            });
        }
    }
}
