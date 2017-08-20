using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WPFMVVM
{
    /// <summary>
    /// Interaction logic for WPFMVVMModule.xaml
    /// </summary>
    public partial class WPFMVVMModule : UserControl, UtilityUILib.IModuleInterface
    {
        public WPFMVVMModule()
        {
            InitializeComponent();
        }
        #region Module Interface
        public string interfaceVersion
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string moduleName
        {
            get
            {
                return "WPFMVVM";
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
            //throw new NotImplementedException();
            return 0;
        }

        public void initialize()
        {
            //throw new NotImplementedException();
            cntCrl.Content = new WPFMVVM.Views.WPFMVVMControl();
        }

        public bool isPlatformSupported()
        {
            //throw new NotImplementedException();
            return true;
        }

        public int show()
        {
            //throw new NotImplementedException();
            return 0;
        }

        public void uninitialize()
        {
            throw new NotImplementedException();
        }
        #endregion 
    }
}
