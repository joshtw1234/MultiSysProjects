using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace UtilityUILib
{
    public interface IModuleInterface
    {
        string interfaceVersion { get; }
        string moduleName { get; }

        BitmapImage getModuleLogoImage();
        BitmapImage getModuleLogoImage2();
        int hide();
        void initialize();
        bool isPlatformSupported();
        int show();
        void uninitialize();
    }
}
