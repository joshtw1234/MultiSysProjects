using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HPWMITester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("HPWMILoader.dll", EntryPoint = "WMILoaderExport")]
        public static extern int WMILoaderExport();
        [DllImport("HPWMILoader.dll", EntryPoint = "WMIDemoFunc")]
        public static extern int WMIDemoFunc();
        public MainWindow()
        {
            InitializeComponent();
            var rev = WMILoaderExport();
            var rev2 = WMIDemoFunc();
        }
    }
}
