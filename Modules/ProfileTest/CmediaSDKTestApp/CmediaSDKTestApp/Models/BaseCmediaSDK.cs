using System;
using System.Runtime.InteropServices;

namespace CmediaSDKTestApp.Models
{
    class BaseCmediaSDK
    {
#if CMEDIA
        CMI_Init = (LPConfLibInit) GetProcAddress(hAudioDriver, "ConfLibInit");
        CMI_GetDeviceById = (LPGetDeviceById) GetProcAddress(hAudioDriver, "GetDeviceById");
        CMI_PropertyControl = (LPPropertyControl) GetProcAddress(hAudioDriver, "PropertyControl");
        CMI_RegisterCallbackFunction = (LPRegisterCallbackFunction) GetProcAddress(hAudioDriver, "RegisterCallbackFunction");
        CMI_GetDeviceCount = (LPGetDeviceCount) GetProcAddress(hAudioDriver, "GetDeviceCount");
        CMI_CreateDeviceList = (LPCreateDeviceList) GetProcAddress(hAudioDriver, "CreateDeviceList");
        CMI_ConfLibUnInit = (LPConfLibUnInit) GetProcAddress(hAudioDriver, "ConfLibUnInit");
        CMI_ShowAsioPanel = (LPShowAsioPanel) GetProcAddress(hAudioDriver, "ShowAsioPanel");

        CMI_PlayTestSound = (LPPlayTestSound) GetProcAddress(hAudioDriver, "PlayTestSound");
        CMI_StopTestSound = (LPStopTestSound) GetProcAddress(hAudioDriver, "StopTestSound");
        CMI_CloseTestSound = (LPCloseTestSound) GetProcAddress(hAudioDriver, "CloseTestSound");
#endif
        const string _cmediaDllPath = @"osConfLib.dll";

        [DllImport(_cmediaDllPath, EntryPoint = "ConfLibInit")]
        public static extern int CMI_ConfLibInit();

        [DllImport(_cmediaDllPath, EntryPoint = "ConfLibUnInit")]
        public static extern int CMI_ConfLibUnInit();

        [DllImport(_cmediaDllPath, EntryPoint = "CreateDeviceList")]
        public static extern int CMI_CreateDeviceList();

        [DllImport(_cmediaDllPath, EntryPoint = "GetDeviceCount")]
        public static extern int CMI_GetDeviceCount(DeviceType type, ref uint DevCount);

        [DllImport(_cmediaDllPath, EntryPoint = "GetDeviceById")]
        public static extern int CMI_GetDeviceById(int type, int id, out DEVICEINFO deviceInfo);

        [DllImport(_cmediaDllPath, EntryPoint = "PropertyControl")]
        public static extern int CMI_PropertyControl(DEVICEINFO info, [MarshalAs(UnmanagedType.LPWStr)]string propertyName, ref IntPtr value, ref IntPtr extraData, byte RorW);
    }

    struct DEVICEINFO
    {
        int id;
        JackType JackType;
        DataFlow DataFlow;
        DeviceState DeviceState;
    }

    enum DeviceState
    {
        UnknowState = 0,
        Active,
        Nopresent,
        Disabled,
        Unplugged
    }

    enum JackType
    {
        UnknowJack = 0,
        JackSpeaker,
        JackMicrophone,
        JackLine,
        JackSPDIF,
        JackAux,
        JackCDPlayer,
        JackHeadphone,
        JackStereoMix,
        JackHDMI,
        JackDesktopMicrophone,
        JackSPDIFInRCA,
        JackSpeakerQuarter
    }

    enum DataFlow
    {
        eRender,
        eCapture,
        eAll,
        DATAFLOW_enum_count
    }

    enum DeviceType
    {
        Render = 0,
        Capture
    }
}
