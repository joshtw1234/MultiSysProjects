using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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
        GetDirectXVer,//This not work in multi thread environment
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

    class BaseCmediaSDK
    {
        public const int CMI_BUFFER_SIZE = 1024;

        public static OMENREVData OMEN_PropertyControl(ZazuRWData rwData)
        {
            // Allocate a Cmedia standard Array.
            byte[] devBvalue = new byte[CMI_BUFFER_SIZE];
            if (rwData.ReadWrite == CMI_DriverRW.Write)
            {
                devBvalue = rwData.WriteData;
            }
            // Allocate a memory buffer (that can be accessed and modified by unmanaged code)
            // to store values from the devBvalue array.
            IntPtr pdevValue = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * CMI_BUFFER_SIZE);

            // Copy values from devBvalue to this buffer (i.e. pdevValue).
            Marshal.Copy(devBvalue, 0, pdevValue, devBvalue.Length);

            // Allocate a GCHandle in order to allocate an IntPtr
            // that stores the memory address of pdevValue.
            GCHandle gchDevValue = GCHandle.Alloc(pdevValue, GCHandleType.Pinned);
            // Use GCHandle.AddrOfPinnedObject() to obtain a pointer 
            // to a pointer to the byte array of pdevValue.
            // It is devValue that will be passed to the API.
            IntPtr devValue = gchDevValue.AddrOfPinnedObject();

            byte[] devExtraBvalue = new byte[CMI_BUFFER_SIZE];
            IntPtr pdevExtraVlue = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * CMI_BUFFER_SIZE);
            Marshal.Copy(devExtraBvalue, 0, pdevExtraVlue, devExtraBvalue.Length);
            GCHandle gch = GCHandle.Alloc(pdevExtraVlue, GCHandleType.Pinned);
            IntPtr devExtraValue = gch.AddrOfPinnedObject();

            // Call the CMI_PropertyControl() API.
            // The CMI_PropertyControl() API will not
            // change the value of devValue. 
            int revCode = NativeMethods.CMI_PropertyControl(rwData.JackInfo.m_devInfo, rwData.RenderPropertyName.ToString(), devValue, devExtraValue, rwData.ReadWrite);
            string revString = $"\nCMI_PropertyControl [{rwData.RenderPropertyName}] {rwData.ReadWrite} return {revCode}";
            string revData = string.Empty;
            if (0 == revCode)
            {
                // We must now find a way to dereference the memory address
                // contained inside devValue.

                // Declare an array (of one single value) of IntPtr.
                IntPtr[] revPtr = new IntPtr[1];
                // Copy the value contained inside devValue
                // to revPtr.
                Marshal.Copy(devValue, revPtr, 0, 1);

                // Allocate a new byte array to be filled with 
                // values from the array pointed to by revPtr[0]
                byte[] NewByteArray = new byte[BaseCmediaSDK.CMI_BUFFER_SIZE];

                // Copy the byte array values pointed to by revPtr[0]
                // to NewByteArray.
                Marshal.Copy(revPtr[0], NewByteArray, 0, NewByteArray.Length);
                revData = System.Text.Encoding.UTF8.GetString(NewByteArray).Replace('\0', ' ').Trim();
                revString = $"\nCMI_PropertyControl [{rwData.RenderPropertyName} {rwData.ReadWrite}] return {revCode} Get Data [{revData}]";
            }
            gchDevValue.Free();
            gch.Free();
            OMENREVData rev = new OMENREVData() { RevCode = revCode, RevValue = revData, RevMessage = revString };
            return rev;
        }
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
        public CmediaRenderFunctionPoint RenderPropertyName;
        public CmediaCaptureFunctionPoint CapturePropertyName;
        public CMI_DriverRW ReadWrite;
        public byte[] WriteData;
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
