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
        public static extern int CMI_GetDeviceCount(CmediaDataFlow type, ref uint DevCount);

        [DllImport(_caudioDllPath, EntryPoint = "GetDeviceById")]
        public static extern int CMI_GetDeviceById(CmediaDataFlow type, int id, out CmediaDeviceInfo deviceInfo);

        [DllImport(_caudioDllPath, EntryPoint = "PropertyControl", CharSet= CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        public static extern int CMI_PropertyControl(CmediaDeviceInfo info, string propertyName, IntPtr value, IntPtr extraData, CmediaDriverReadWrite driverRW);

        [DllImport(_caudioDllPath, EntryPoint = "RegisterCallbackFunction", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CMI_RegisterCallbackFunction([MarshalAs(UnmanagedType.FunctionPtr)] CmediaSDKCallback callbackPointer, IntPtr wndHandle);
        #endregion
    }
}
