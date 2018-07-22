using System.Windows.Controls;
using WPFMVVM.Models;
using WPFMVVM.ViewModels;

namespace WPFMVVM.Views
{
    /// <summary>
    /// Interaction logic for WPFMVVMControl.xaml
    /// </summary>
    public partial class WPFMVVMControl : UserControl
    {
        public WPFMVVMControl()
        {
            InitializeComponent();
            /*
             * Funny MVVM Step
             * 1- init Model
             * 2- init ViewModel
             * 3- Set to DataContext
             */
            WPFMVVMModel _model = new WPFMVVMModel();
            WPFMVVMViewModel viewModel = new WPFMVVMViewModel(_model);
            this.DataContext = viewModel;
        }
    }
}
