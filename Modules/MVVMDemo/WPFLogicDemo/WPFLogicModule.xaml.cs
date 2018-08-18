using System.Windows.Controls;
using System.Windows.Media.Imaging;
using UtilityUILib;

namespace WPFLogicDemo
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class WPFLogicModule : UserControl, IModuleInterface
    {
        public WPFLogicModule()
        {
            InitializeComponent();
        }

        #region Module Interface
        public string interfaceVersion => throw new System.NotImplementedException();

        public string moduleName
        {
            get
            {
                return "WPFLogic";
            }
        }

        public BitmapImage getModuleLogoImage()
        {
            throw new System.NotImplementedException();
        }

        public BitmapImage getModuleLogoImage2()
        {
            throw new System.NotImplementedException();
        }

        public int hide()
        {
            return 0;
        }

        public void initialize()
        {
            
        }

        public bool isPlatformSupported()
        {
            return true;
        }

        public int show()
        {
            return 0;
        }

        public void uninitialize()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
