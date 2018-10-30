using System;
using System.Runtime.InteropServices;

namespace CmediaSDKTestApp.Models
{
    class NativeMethods
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
        public static extern int CMI_GetDeviceCount(CMI_DeviceType type, ref uint DevCount);

        [DllImport(_cmediaDllPath, EntryPoint = "GetDeviceById")]
        public static extern int CMI_GetDeviceById(CMI_DeviceType type, int id, out CMI_DEVICEINFO deviceInfo);

        [DllImport(_cmediaDllPath, EntryPoint = "PropertyControl", CharSet= CharSet.Auto, SetLastError = true)]
        public static extern int CMI_PropertyControl(CMI_DEVICEINFO info, string propertyName, IntPtr value, IntPtr extraData, CMI_DriverRW driverRW);
        #endregion


    }

    class BaseCmediaSDK
    {
        public const string CMI_DefaultDeviceControl = "DefaultDeviceControl";
        #region Mic features
        public const int CMI_BUFFER_SIZE = 1024;
        public const string CMI_Enable_KEYSHIFT_GFX = "Enable_KEYSHIFT_GFX";
        public const string CMI_KEYSHIFT_LEVEL = "KEYSHIFT_LEVEL";
        public const string CMI_Enable_VOCALCANCEL_GFX = "Enable_VOCALCANCEL_GFX";
        public const string CMI_VOCALCANCEL_LEVEL = "VOCALCANCEL_LEVEL";
        public const string CMI_Enable_MICECHO = "Enable_MICECHO";
        public const string CMI_MICECHO_Level = "MICECHO_Level";
        public const string CMI_Enable_MAGICVOICE = "Enable_MAGICVOICE";
        public const string CMI_MagicVoice_Selection = "MagicVoice_Selection";
        #endregion
    }

    struct CMI_DEVICEINFO
    {
        public int id;
        public CMI_JackType JackType;
        public CMI_DataFlow DataFlow;
        public CMI_DeviceState DeviceState;
    }

    class CMI_JackDeviceInfo
    {
        public CMI_DEVICEINFO m_devInfo;       // reference to DEVICEINFO

        // function attributes
        public int m_dwCMediaDSP0 { get; set; }      // CMedia DSP function tables
        public int m_dwThirdPartyDSP0 { get; set; }    // Third-Party DSP function tables
        public int m_dwExtraStreamFunc { get; set; }   // Extra stream function tables
    }

    enum CMI_DeviceState
    {
        UnknowState = 0,
        Active,
        Nopresent,
        Disabled,
        Unplugged
    }

    enum CMI_JackType
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

    enum CMI_DataFlow
    {
        eRender,
        eCapture,
        eAll,
        DATAFLOW_enum_count
    }

    enum CMI_DeviceType
    {
        Render = 0,
        Capture
    }

    enum CMI_DriverRW
    {
        Read,
        Write
    }
}
