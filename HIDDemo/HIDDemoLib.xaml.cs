using HIDDemo.Views;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using UtilityUILib;

namespace HIDDemo
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class HIDDemoLib : UserControl, IModuleInterface
    {
        public HIDDemoLib()
        {
            InitializeComponent();
        }

        public string interfaceVersion => throw new NotImplementedException();

        public string moduleName
        {
            get
            {
                return "HIDDemo";
            }
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
            //throw new NotImplementedException();
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
