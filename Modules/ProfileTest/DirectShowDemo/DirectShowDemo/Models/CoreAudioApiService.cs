using System;
using System.Runtime.InteropServices;

namespace DirectShowDemo.Models
{
    class CoreAudioApiService
    {
        //[DllImport("CoreAudioApisDll.dll", EntryPoint = "nCoreAudioApisDll", CharSet = CharSet.Auto)]
        //internal static extern int nCoreAudioApisDll;
        [DllImport("CoreAudioApisDll.dll", EntryPoint = "fnCoreAudioApisDll", CharSet = CharSet.Auto)]
        internal static extern int fnCoreAudioApisDll();
    }
}
