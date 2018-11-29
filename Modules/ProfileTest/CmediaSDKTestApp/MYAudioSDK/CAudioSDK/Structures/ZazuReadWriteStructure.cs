using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace MYAudioSDK.CAudioSDK.Structures
{
    struct CAudioDeviceInfo
    {
        public int id;
        public CmediaJackType JackType;
        public CmediaDataFlow DataFlow;
        public CmediaDeviceState DeviceState;
    }

    class CAudioJackDeviceInfo
    {
        public CAudioDeviceInfo m_devInfo;       // reference to DEVICEINFO
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

    enum CmediaVolumeChannel
    {
        Master = -1,
        FrontLeft,
        FrontRight
    }
    #endregion

    #region Cmedia Function Point
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    delegate void CAudioSDKCallback(int type, int id, int componentType, ulong eventId);

    #endregion

    #region Custom structure

    class ZazuReadWriteStructure
    {
        public CAudioJackDeviceInfo JackInfo { get; set; }
        public string ApiPropertyName { get; set; }
        public CmediaDriverReadWrite ReadWrite { get; set; }
        public byte[] WriteData { get; set; }
        public bool IsWriteExtra { get; set; }
        public byte[] WriteExtraData { get; set; }
    }

    struct ReturnValue
    {
        public int RevCode { get; set; }
        public string RevValue { get; set; }
        public string RevMessage { get; set; }
        public string RevExtraValue { get; set; }
    }

    struct ClientData
    {
        const string BuildInName = "System";
        public string ApiName { get; set; }
        public object SetValue { get; set; }
        public object SetExtraValue { get; set; }
        public byte[] SetValueToByteArray()
        {
            if (SetValue == null)
            {
                return null;
            }
            return GetObjectBytes(SetValue);
        }

        public byte[] SetExtraValueToByteArray()
        {
            if (SetExtraValue == null)
            {
                return null;
            }
            return GetObjectBytes(SetExtraValue);
        }

        private byte[] GetObjectBytes(object objData)
        {
            if (objData.GetType().Namespace.Equals(BuildInName))
            {
                //Build in types
                if (objData is int)
                {
                    return BitConverter.GetBytes((int)objData);
                }
                if (objData is float)
                {
                    return BitConverter.GetBytes((float)objData);
                }
                if (objData is double)
                {
                    return BitConverter.GetBytes((double)objData);
                }
                if (objData is bool)
                {
                    return BitConverter.GetBytes((bool)objData);
                }
            }
            if (objData is CmediaVolumeChannel)
            {
                return BitConverter.GetBytes((int)objData);
            }
            return ObjectToByteArray(objData);
        }

        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null) return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
    #endregion
}
