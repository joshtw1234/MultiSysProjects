using MYAudioSDK.CAudioSDK.CallBacks;
using MYAudioSDK.CAudioSDK.Enums;
using MYAudioSDK.CAudioSDK.Structures;
using System;
using System.Runtime.InteropServices;

namespace MYAudioSDK.CAudioSDK
{
    class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        #region Cmedia SDK Import functions
        const string _caudioDllPath = @"osConfLib.dll";

        [DllImport(_caudioDllPath, EntryPoint = "ConfLibInit")]
        public static extern int CMI_ConfLibInit();

        [DllImport(_caudioDllPath, EntryPoint = "ConfLibUnInit")]
        public static extern int CMI_ConfLibUnInit();

        [DllImport(_caudioDllPath, EntryPoint = "CreateDeviceList")]
        public static extern int CMI_CreateDeviceList();

        [DllImport(_caudioDllPath, EntryPoint = "GetDeviceCount")]
        public static extern int CMI_GetDeviceCount(CAudioDataFlow type, ref uint DevCount);

        [DllImport(_caudioDllPath, EntryPoint = "GetDeviceById")]
        public static extern int CMI_GetDeviceById(CAudioDataFlow type, int id, out CAudioDeviceInfo deviceInfo);

        [DllImport(_caudioDllPath, EntryPoint = "PropertyControl", CharSet= CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        public static extern int CMI_PropertyControl(CAudioDeviceInfo info, string propertyName, IntPtr value, IntPtr extraData, CAudioDriverReadWrite driverRW);

        [DllImport(_caudioDllPath, EntryPoint = "RegisterCallbackFunction", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CMI_RegisterCallbackFunction([MarshalAs(UnmanagedType.FunctionPtr)] CAudioSDKCallback callbackPointer, IntPtr wndHandle);
        #endregion
    }
}
