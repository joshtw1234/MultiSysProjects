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

        [DllImport(_cmediaDllPath, EntryPoint = "PropertyControl", CharSet= CharSet.Auto, SetLastError = true)]
        public static extern int CMI_PropertyControl(CMI_DEVICEINFO info, string propertyName, IntPtr value, IntPtr extraData, CMI_DriverRW driverRW);
        #endregion


    }
    enum CMI_FunctinoPoint
    {
        #region CMI Device
        DefaultDeviceControl,
        GetDeviceFriendlyName,
        GetDeviceID,
        GetExtraInfo,
        GetFirmwareVer,
        GetDriverVer,
        GetDirectXVer,
        #endregion

        #region Mic features
        Enable_KEYSHIFT_GFX,
        KEYSHIFT_LEVEL,
        Enable_VOCALCANCEL_GFX,
        VOCALCANCEL_LEVEL,
        Enable_MICECHO,
        MICECHO_Level,
        Enable_MAGICVOICE,
        MagicVoice_Selection,
        #endregion
    }

    class BaseCmediaSDK
    {
        public const int CMI_BUFFER_SIZE = 1024;

        public static async Task<OMENREVData> OMEN_PropertyControl(ZazuRWData rwData)
        {
            return await Task.Run(() =>
            {
                // Allocate a Cmedia standard Array.
                byte[] devBvalue = new byte[CMI_BUFFER_SIZE];
                if (rwData.ReadWrite == CMI_DriverRW.Write)
                {
                    devBvalue = rwData.WriteData;
                }
                // Allocate a memory buffer (that can be accessed and modified by unmanaged code)
                // to store values from the devBvalue array.
                IntPtr pdevValue = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * devBvalue.Length);

                // Copy values from devBvalue to this buffer (i.e. pdevValue).
                Marshal.Copy(devBvalue, 0, pdevValue, devBvalue.Length);

                // Allocate a GCHandle in order to allocate an IntPtr
                // that stores the memory address of pdevValue.
                GCHandle gchDevValue = GCHandle.Alloc(pdevValue, GCHandleType.Pinned);
                // Use GCHandle.AddrOfPinnedObject() to obtain a pointer 
                // to a pointer to the byte array of pdevValue.
                // It is devValue that will be passed to the API.
                IntPtr devValue = gchDevValue.AddrOfPinnedObject();

                byte[] devExtraBvalue = new byte[BaseCmediaSDK.CMI_BUFFER_SIZE];
                IntPtr pdevExtraVlue = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * devExtraBvalue.Length);
                Marshal.Copy(devExtraBvalue, 0, pdevExtraVlue, devExtraBvalue.Length);
                GCHandle gch = GCHandle.Alloc(pdevExtraVlue, GCHandleType.Pinned);
                IntPtr devExtraValue = gch.AddrOfPinnedObject();

                // Call the CMI_PropertyControl() API.
                // The CMI_PropertyControl() API will not
                // change the value of devValue. 
                int revCode = NativeMethods.CMI_PropertyControl(rwData.JackInfo.m_devInfo, rwData.PropertyName.ToString(), devValue, devExtraValue, rwData.ReadWrite);
                string revString = $"\nCMI_PropertyControl {rwData.PropertyName} {rwData.ReadWrite} return {revCode}";
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
                    string revData = System.Text.Encoding.ASCII.GetString(NewByteArray).Replace('\0', ' ').Trim();
                    revString = $"\nCMI_PropertyControl {rwData.PropertyName} {rwData.ReadWrite} return {revCode} Get Data [{revData}]";
                }
                gchDevValue.Free();
                gch.Free();
                OMENREVData rev = new OMENREVData() { RevCode = revCode, RevMessage = revString };
                return rev;
            });

        }
    }

    class OMENREVData
    {
        public int RevCode;
        public string RevMessage;
    }

    class ZazuRWData
    {
        public CMI_JackDeviceInfo JackInfo;
        public CMI_FunctinoPoint PropertyName;
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
