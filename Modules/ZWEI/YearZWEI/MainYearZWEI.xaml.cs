using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace YearZWEI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MainYearZWEI : UserControl, UtilityUILib.IModuleInterface
    {
        public MainYearZWEI()
        {
            InitializeComponent();
        }

        public string interfaceVersion => throw new NotImplementedException();

        public string moduleName
        {
            get { return "YearZWEI"; }
        }

        public BitmapImage getModuleLogoImage()
        {
            throw new NotImplementedException();
        }

        public BitmapImage getModuleLogoImage2()
        {
            throw new NotImplementedException();
        }

        public int hide()
        {
            throw new NotImplementedException();
        }

        public void initialize()
        {
            //throw new NotImplementedException();
        }

        public bool isPlatformSupported()
        {
            return true;
        }

        public int show()
        {
            throw new NotImplementedException();
        }

        public void uninitialize()
        {
            throw new NotImplementedException();
        }
    }
}
