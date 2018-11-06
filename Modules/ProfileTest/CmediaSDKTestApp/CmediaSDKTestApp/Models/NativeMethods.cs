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
        public static extern int CMI_GetDeviceCount(CMI_DeviceType type, ref uint DevCount);

        [DllImport(_cmediaDllPath, EntryPoint = "GetDeviceById")]
        public static extern int CMI_GetDeviceById(CMI_DeviceType type, int id, out CMI_DEVICEINFO deviceInfo);

        [DllImport(_cmediaDllPath, EntryPoint = "PropertyControl", CharSet= CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        public static extern int CMI_PropertyControl(CMI_DEVICEINFO info, string propertyName, IntPtr value, IntPtr extraData, CMI_DriverRW driverRW);

        [DllImport(_cmediaDllPath, EntryPoint = "RegisterCallbackFunction", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CMI_RegisterCallbackFunction([MarshalAs(UnmanagedType.FunctionPtr)] CmediaSDKCallback callbackPointer, IntPtr wndHandle);
        #endregion
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void CmediaSDKCallback(int type, int id, int componentType, ulong eventId);

    enum CmediaRenderFunctionPoint
    {
        #region CMI Device
        DefaultDeviceControl,//With write will set device to default
        AmplifierControl,
        EndpointEnableControl,//Get or Set device states, 1 is enable, 0 is disable
        EXControl,//EX switch device.
        GetSupportFeature,
        GetDeviceFriendlyName,
        GetDeviceID,
        GetExtraInfo,
        GetSymbolicLink,
        GetAudioCodecName,
        GetFirmwareVer,
        GetDriverVer,
        //GetDirectXVer,//This will took long time then others.
        #endregion

        #region Volume Control
        VolumeControl,
        MuteControl,
        VolumeScalarControl,//set -1 get master volume scalar,you can get other channel volume scalar
        GetMaxVol,//The unit of Value is dB. 
        GetMinVol,//The unit of Value is dB. 
        GetVolStep,
        #endregion

        #region EQ & EM
        Enable_EQ_GFX,
        EQ_Slider1,
        EQ_Slider2,
        EQ_Slider3,
        EQ_Slider4,
        EQ_Slider5,
        EQ_Slider6,
        EQ_Slider7,
        EQ_Slider8,
        EQ_Slider9,
        EQ_Slider10,
        Enable_RFX_GFX,
        RFX_ENVIRONMENT,
        RFX_ROOMSIZE,
        #endregion

        #region Mic features
        Enable_KEYSHIFT_GFX,
        KEYSHIFT_LEVEL,
        Enable_VOCALCANCEL_GFX,
        VOCALCANCEL_LEVEL,
        #endregion

        DRIVER_PROPERTY_VIRTUAL_SURROUND_CONTROL,
    }

    enum CmediaCaptureFunctionPoint
    {
        #region MIC device.
        Enable_MICECHO,
        MICECHO_Level,
        Enable_MAGICVOICE,
        MagicVoice_Selection,
        #endregion

        #region AA Volume Control
        AAVolumeControl,
        AAMuteControl,
        AAVolumeScalarControl,
        GetAAMaxVol,
        GetAAMinVol,
        GetAAVolStep,
        #endregion

    }

    class OMENREVData
    {
        public int RevCode;
        public string RevValue;
        public string RevMessage;
    }

    class ZazuRWData
    {
        public CMI_JackDeviceInfo JackInfo;
        public CMI_DeviceType DeviceType;
        public CmediaRenderFunctionPoint RenderPropertyName;
        public CmediaCaptureFunctionPoint CapturePropertyName;
        public CMI_DriverRW ReadWrite;
        public byte[] WriteData;
    }

    #region 7.1 Surround
    enum HPSurroundCommand
    {
        XEAR_SURR_HP_ENABLE,
        XEAR_SURR_HP_ROOM,
        XEAR_SURR_HP_MODE
    }
    enum HPSurroundValueType
    {
        ValueType_LONG = 0,
        ValueType_FLOAT = 1
    };
    enum HPSurround
    {
        KSPROPERTY_VIRTUALSURROUND_GETGUID = 1,
        KSPROPERTY_VIRTUALSURROUND_GETNUMOFPARAMELEMENT = 2,
        KSPROPERTY_VIRTUALSURROUND_GETPARAMRANGE = 3,
        KSPROPERTY_VIRTUALSURROUND_PARAMSVALUE = 4
    };

    struct REGISTER_OPERATION
    {
        public int Operation;
        public int Feature;
        public int ValueType;
    };
    #endregion

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
