using System.Windows.Controls;

namespace HIDDemo.Views
{
    /// <summary>
    /// Interaction logic for HIDDemoControl.xaml
    /// </summary>
    public partial class HIDDemoControl : UserControl
    {
        public HIDDemoControl()
        {
            InitializeComponent();
            Models.HIDDemoControlModel hidModel = new Models.HIDDemoControlModel();
            ViewModels.HIDDemoControlViewModel viewModel = new ViewModels.HIDDemoControlViewModel(hidModel);
            this.DataContext = viewModel;
        }
    }
}
