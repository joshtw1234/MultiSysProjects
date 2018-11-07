﻿using System.Runtime.InteropServices;

namespace CmediaSDKTestApp.BaseModels
{
    class ZazuReadWriteStructure
    {
        public CmediaJackDeviceInfo JackInfo { get; set; }
        public string ApiPropertyName { get; set; }
        public CmediaDriverReadWrite ReadWrite { get; set; }
        public byte[] WriteData { get; set; }
        public bool IsWriteExtra { get; set; }
        public byte[] WriteExtraData { get; set; }
    }

    struct CmediaDeviceInfo
    {
        public int id;
        public CmediaJackType JackType;
        public CmediaDataFlow DataFlow;
        public CmediaDeviceState DeviceState;
    }

    class CmediaJackDeviceInfo
    {
        public CmediaDeviceInfo m_devInfo;       // reference to DEVICEINFO
        // function attributes
        public int m_dwCMediaDSP0 { get; set; }      // CMedia DSP function tables
        public int m_dwThirdPartyDSP0 { get; set; }    // Third-Party DSP function tables
        public int m_dwExtraStreamFunc { get; set; }   // Extra stream function tables
    }

    #region 7.1 Surround
    enum HPSurroundCommand
    {
        XEAR_SURR_HP_ENABLE = -1,
        XEAR_SURR_HP_ROOM,
        XEAR_SURR_HP_MODE,
        XEAR_SURR_HP_TOP
    }
    enum HPSurroundValueType
    {
        ValueType_LONG = 0,
        ValueType_FLOAT = 1
    }
    enum HPSurroundFunction
    {
        KSPROPERTY_VIRTUALSURROUND_GETGUID = 1,
        KSPROPERTY_VIRTUALSURROUND_GETNUMOFPARAMELEMENT = 2,
        KSPROPERTY_VIRTUALSURROUND_GETPARAMRANGE = 3,
        KSPROPERTY_VIRTUALSURROUND_PARAMSVALUE = 4
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct CmediaRegisterOperation
    {
        public HPSurroundFunction Operation;
        public HPSurroundCommand Feature;
        public HPSurroundValueType ValueType;
        public byte[] ToBytes()
        {
            byte[] bytes = new byte[Marshal.SizeOf(typeof(CmediaRegisterOperation))];
            GCHandle pinStructure = GCHandle.Alloc(this, GCHandleType.Pinned);
            try
            {
                Marshal.Copy(pinStructure.AddrOfPinnedObject(), bytes, 0, bytes.Length);
                return bytes;
            }
            finally
            {
                pinStructure.Free();
            }
        }
    }
    #endregion

    #region Cmedia Enums
    enum CmediaDeviceState
    {
        UnknowState = 0,
        Active,
        Nopresent,
        Disabled,
        Unplugged
    }

    enum CmediaJackType
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

    enum CmediaDataFlow
    {
        eRender,
        eCapture,
        eAll,
        DATAFLOW_enum_count
    }

    enum CmediaDriverReadWrite
    {
        Read,
        Write
    }
    #endregion

    #region Cmedia Function Point
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

        #region 7.1 surround
        VirtualSurroundEffectControl,
        #endregion

        #region Bass & HWEQ
        Enable_BM_GFX,
        BM_Slider_Frequency,
        BM_Slider_BassLevel,
        BM_LargeSpeaker,
        SpeakerDelay_C,
        SpeakerDelay_S,
        SpeakerDelay_B,
        HWEQBassControl,
        HWEQTrebleControl,
        #endregion

        #region Smart Bass & Volume
        Enable_ADAPTIVEVOLUME_GFX,
        ADAPTIVEVOLUME_LEVEL,
        ADAPTIVEVOLUME_MODE,
        Enable_VIRTUALBASS_LFX,
        VIRTUALBASS_Level,
        VIRTUALBASS_CutOffFrequency,
        VIRTUALBASS_Mode,
        Enable_AUDIOBRILLIANT_LFX,
        AUDIOBRILLIANT_LEVEL,
        Enable_VOICECLARITY_LFX,
        VOICECLARITY_LEVEL,
        VOICECLARITY_NOISESUPP_LEVEL,
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

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void CmediaSDKCallback(int type, int id, int componentType, ulong eventId);
    #endregion
}
