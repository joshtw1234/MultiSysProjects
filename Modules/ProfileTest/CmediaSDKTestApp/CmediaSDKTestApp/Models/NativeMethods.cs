using CmediaSDKTestApp.BaseModels;
using System;
using System.Runtime.InteropServices;

namespace CmediaSDKTestApp.Models
{
    class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        #region SDK Import functions
        const string _cmediaDllPath = @"osConfLib.dll";

        [DllImport(_cmediaDllPath, EntryPoint = "ConfLibInit")]
        public static extern int CMI_ConfLibInit();

        [DllImport(_cmediaDllPath, EntryPoint = "ConfLibUnInit")]
        public static extern int CMI_ConfLibUnInit();

        [DllImport(_cmediaDllPath, EntryPoint = "CreateDeviceList")]
        public static extern int CMI_CreateDeviceList();

        [DllImport(_cmediaDllPath, EntryPoint = "GetDeviceCount")]
        public static extern int CMI_GetDeviceCount(CmediaDataFlow type, ref uint DevCount);

        [DllImport(_cmediaDllPath, EntryPoint = "GetDeviceById")]
        public static extern int CMI_GetDeviceById(CmediaDataFlow type, int id, out CmediaDeviceInfo deviceInfo);

        [DllImport(_cmediaDllPath, EntryPoint = "PropertyControl", CharSet= CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        public static extern int CMI_PropertyControl(CmediaDeviceInfo info, string propertyName, IntPtr value, IntPtr extraData, CmediaDriverReadWrite driverRW);

        [DllImport(_cmediaDllPath, EntryPoint = "RegisterCallbackFunction", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CMI_RegisterCallbackFunction([MarshalAs(UnmanagedType.FunctionPtr)] CmediaSDKCallback callbackPointer, IntPtr wndHandle);
        #endregion
    }
   
    struct OMENClientData
    {
        public string ApiName { get; set; }
        public byte[] WriteValue { get; set; }
        public byte[] WriteExtraValue { get; set; }
    }

    class OMENREVData
    {
        public int RevCode { get; set; }
        public string RevValue { get; set; }
        public string RevMessage { get; set; }
        public string RevExtraValue { get; set; }
    }
}
