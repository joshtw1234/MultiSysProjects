using System.Windows.Controls;
using System.Windows.Media.Imaging;
using UtilityUILib;

namespace HIDHeadSet
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class HIDHeadSet : UserControl, IModuleInterface
    {
        public HIDHeadSet()
        {
            InitializeComponent();
        }

        public string interfaceVersion => throw new System.NotImplementedException();

        public string moduleName
        {
            get
            {
                return "HIDHeadSet";
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
            throw new System.NotImplementedException();
        }

        public void initialize()
        {
            //throw new System.NotImplementedException();
        }

        public bool isPlatformSupported()
        {
            //throw new System.NotImplementedException();
            return true;
        }

        public int show()
        {
            throw new System.NotImplementedException();
        }

        public void uninitialize()
        {
            throw new System.NotImplementedException();
        }
    }
}
