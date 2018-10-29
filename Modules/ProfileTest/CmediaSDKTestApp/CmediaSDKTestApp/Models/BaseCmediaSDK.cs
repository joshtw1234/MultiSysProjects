using System.Runtime.InteropServices;

namespace CmediaSDKTestApp.Models
{
    class BaseCmediaSDK
    {
        [DllImport("osConfLib.dll")]
        protected static extern int ConfLibInit();
    }
}
