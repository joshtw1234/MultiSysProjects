using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace ShareUtilityLib
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
