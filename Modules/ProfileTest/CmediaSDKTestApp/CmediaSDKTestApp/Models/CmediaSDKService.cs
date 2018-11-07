using CmediaSDKTestApp.BaseModels;
using System;
using System.Runtime.InteropServices;

namespace CmediaSDKTestApp.Models
{
    /// <summary>
    /// Implement Cmedia SDK sample code as service
    /// </summary>
    sealed class CmediaSDKService : IDisposable
    {
        private CmediaSDKCallback _cmediaSDKCallback;
        private CmediaJackDeviceInfo _cmediaJackInfoRender;
        private CmediaJackDeviceInfo _cmediajackInfoCapture;

        private IMenuItem DisplayMessage;
        private static CmediaSDKService _instance;
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static CmediaSDKService Instance
        {
            get
            {
                return _instance ?? (_instance = new CmediaSDKService());
            }
        }

        /// <summary>
        /// Private constructor.
        /// Please use instance to get functions
        /// </summary>
        private CmediaSDKService() { }

        private const int CMEDIABUFFERSIZE = 1024;

        private OMENREVData OMEN_PropertyControl(ZazuReadWriteStructure rwData)
        {
            // Allocate a Cmedia standard Array.
            byte[] devBvalue = new byte[CMEDIABUFFERSIZE];
            if (rwData.ReadWrite == CmediaDriverReadWrite.Write && rwData.WriteData != null)
            {
                devBvalue = rwData.WriteData;
            }
            // Allocate a memory buffer (that can be accessed and modified by unmanaged code)
            // to store values from the devBvalue array.
            IntPtr pdevValue = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * CMEDIABUFFERSIZE);

            // Copy values from devBvalue to this buffer (i.e. pdevValue).
            Marshal.Copy(devBvalue, 0, pdevValue, devBvalue.Length);

            // Allocate a GCHandle in order to allocate an IntPtr
            // that stores the memory address of pdevValue.
            GCHandle gchDevValue = GCHandle.Alloc(pdevValue, GCHandleType.Pinned);
            // Use GCHandle.AddrOfPinnedObject() to obtain a pointer 
            // to a pointer to the byte array of pdevValue.
            // It is devValue that will be passed to the API.
            IntPtr devValue = gchDevValue.AddrOfPinnedObject();

            byte[] devExtraBvalue = new byte[CMEDIABUFFERSIZE];
            if ((rwData.ReadWrite == CmediaDriverReadWrite.Write || rwData.IsWriteExtra) && rwData.WriteExtraData != null)
            {
                devExtraBvalue = rwData.WriteExtraData;
            }
            IntPtr pdevExtraVlue = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * CMEDIABUFFERSIZE);
            Marshal.Copy(devExtraBvalue, 0, pdevExtraVlue, devExtraBvalue.Length);
            GCHandle gch = GCHandle.Alloc(pdevExtraVlue, GCHandleType.Pinned);
            IntPtr devExtraValue = gch.AddrOfPinnedObject();

            // Call the CMI_PropertyControl() API.
            // The CMI_PropertyControl() API will not
            // change the value of devValue. 
            int revCode = NativeMethods.CMI_PropertyControl(rwData.JackInfo.m_devInfo, rwData.ApiPropertyName, devValue, devExtraValue, rwData.ReadWrite);
            string revString = $"\nCMI_PropertyControl [{rwData.JackInfo.m_devInfo.DataFlow}] [{rwData.ApiPropertyName}] {rwData.ReadWrite} return {revCode}";
            string revData = string.Empty, revExraData = string.Empty;
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
                byte[] NewByteArray = new byte[CMEDIABUFFERSIZE];

                // Copy the byte array values pointed to by revPtr[0]
                // to NewByteArray.
                Marshal.Copy(revPtr[0], NewByteArray, 0, NewByteArray.Length);
                revData = System.Text.Encoding.ASCII.GetString(NewByteArray).Replace("\0", "");
                revString = $"\nCMI_PropertyControl [{rwData.JackInfo.m_devInfo.DataFlow}] [{rwData.ApiPropertyName} {rwData.ReadWrite}] return {revCode} Get Data [{revData}]";

                IntPtr[] revExraPtr = new IntPtr[1];
                Marshal.Copy(devExtraValue, revExraPtr, 0, 1);
                byte[] NewExtraByteArray = new byte[CMEDIABUFFERSIZE];
                Marshal.Copy(revExraPtr[0], NewExtraByteArray, 0, NewExtraByteArray.Length);
                revExraData = System.Text.Encoding.ASCII.GetString(NewExtraByteArray).Replace("\0", "");
                revString += $" Extra [{revExraData}]";
            }
            gchDevValue.Free();
            gch.Free();
            OMENREVData rev = new OMENREVData() { RevCode = revCode, RevValue = revData, RevExtraValue = revExraData, RevMessage = revString };
            return rev;
        }

        private CmediaJackDeviceInfo GetJackDevice(CmediaDataFlow deviceType)
        {
            string msg = "Found Device ";
            uint devCount = 0;
            CmediaDeviceInfo devInfo;
            CmediaJackDeviceInfo jackDeviceInfo = new CmediaJackDeviceInfo();
            int rev = NativeMethods.CMI_GetDeviceCount(deviceType, ref devCount);
            if (0 == devCount)
            {
                msg = $"{deviceType} Device not found!!";
                return null;
            }
            else
            {
                for (int i = 0; i < devCount; i++)
                {
                    rev = NativeMethods.CMI_GetDeviceById(deviceType, i, out devInfo);
                    switch (devInfo.DeviceState)
                    {
                        case CmediaDeviceState.Active:
                            jackDeviceInfo.m_devInfo = devInfo;
                            msg += $"JackInfo {deviceType} JackType [{jackDeviceInfo.m_devInfo.JackType}]";
                            break;
                        default:
                            msg = $"Device State {devInfo.DeviceState}";
                            break;
                    }
                };

            }
            DisplayMessage.MenuName += $"\n{msg}";
            return jackDeviceInfo;
        }

        private OMENREVData GetJackDeviceInfoDemo(CmediaDriverReadWrite readWrite)
        {
            ZazuReadWriteStructure rwData = null;
            CmediaJackDeviceInfo jackDevice = _cmediaJackInfoRender;
            OMENREVData rev = new OMENREVData() { RevCode = -1, RevMessage = $"jackDevice type [{jackDevice.m_devInfo.DataFlow}]" };
            for(int i = 0; i < Enum.GetNames(typeof(CmediaAPIFunctionPoint)).Length; i++)
            {
                if (i > (int)CmediaAPIFunctionPoint.VOICECLARITY_NOISESUPP_LEVEL)
                {
                    jackDevice = _cmediajackInfoCapture;
                }
                switch(readWrite)
                {
                    case CmediaDriverReadWrite.Read:
                        rwData = new ZazuReadWriteStructure() { JackInfo = jackDevice, ApiPropertyName = ((CmediaAPIFunctionPoint)i).ToString(), ReadWrite = readWrite };
                        break;
                    case CmediaDriverReadWrite.Write:
                        //TODO add write logic for demo
                        break;
                }
                rev = OMEN_PropertyControl(rwData);
                DisplayMessage.MenuName += $"{rev.RevMessage}";
            }
            return rev;
        }

        public OMENREVData GetSetJackDeviceData(CmediaDataFlow dataFlow, CmediaDriverReadWrite readWrite, OMENClientData clientData)
        {
            OMENREVData revData = new OMENREVData() { RevCode = -1, RevMessage = $"API Name [{clientData.ApiName}] not Correct!" };
            CmediaAPIFunctionPoint sdkAPI;
            if (!Enum.TryParse(clientData.ApiName, out sdkAPI))
            {
                return revData;
            }
            if (readWrite == CmediaDriverReadWrite.Write && 
                clientData.SetValue == null && clientData.SetExtraValue == null)
            {
                revData.RevMessage = "SetValue Can't be Null";
                return revData;
            }
            
            ZazuReadWriteStructure rwData = null;
            CmediaJackDeviceInfo jackInfo = null;
            jackInfo = _cmediaJackInfoRender;
            if (dataFlow == CmediaDataFlow.eCapture || sdkAPI > CmediaAPIFunctionPoint.VOICECLARITY_NOISESUPP_LEVEL)
            {
                jackInfo = _cmediajackInfoCapture;
            }
            rwData = new ZazuReadWriteStructure() { JackInfo = jackInfo, ApiPropertyName = clientData.ApiName, ReadWrite = readWrite, WriteData = clientData.SetExtraValueToByteArray(), WriteExtraData = clientData.SetExtraValueToByteArray() };
            revData = OMEN_PropertyControl(rwData);
            DisplayMessage.MenuName += $"{revData.RevMessage}";
            return revData;
        }

        public OMENREVData GetSetSurroundData(CmediaDriverReadWrite readWrite, HPSurroundCommand hpCommand)
        {
            OMENREVData revData = new OMENREVData() { RevCode = -1, RevMessage = $"[{hpCommand}] not Correct!" };
            ZazuReadWriteStructure rwData = null;
            CmediaRegisterOperation regop = new CmediaRegisterOperation();
            switch (hpCommand)
            {
                case HPSurroundCommand.XEAR_SURR_HP_ENABLE:
                    regop.Operation = HPSurroundFunction.KSPROPERTY_VIRTUALSURROUND_PARAMSVALUE;
                    regop.Feature = HPSurroundCommand.XEAR_SURR_HP_ENABLE;
                    regop.ValueType = HPSurroundValueType.ValueType_LONG;
                    break;
                case HPSurroundCommand.XEAR_SURR_HP_MODE:
                    regop.Operation = HPSurroundFunction.KSPROPERTY_VIRTUALSURROUND_PARAMSVALUE;
                    regop.Feature = HPSurroundCommand.XEAR_SURR_HP_MODE;
                    regop.ValueType = HPSurroundValueType.ValueType_LONG;
                    break;
                case HPSurroundCommand.XEAR_SURR_HP_ROOM:
                    regop.Operation = HPSurroundFunction.KSPROPERTY_VIRTUALSURROUND_PARAMSVALUE;
                    regop.Feature = HPSurroundCommand.XEAR_SURR_HP_ROOM;
                    regop.ValueType = HPSurroundValueType.ValueType_LONG;
                    break;
            }
            rwData = new ZazuReadWriteStructure() { JackInfo = _cmediaJackInfoRender, ApiPropertyName = CmediaAPIFunctionPoint.VirtualSurroundEffectControl.ToString(),
                ReadWrite = readWrite, WriteData = null, IsWriteExtra = true, WriteExtraData = regop.ToBytes() };
            revData = OMEN_PropertyControl(rwData);
            DisplayMessage.MenuName += $"{revData.RevMessage}";
            return revData;
        }

        public int Initialize(IMenuItem logMessag)
        {
            DisplayMessage = logMessag;
            int rev = NativeMethods.CMI_ConfLibInit();
            rev = NativeMethods.CMI_CreateDeviceList();

            //Get Render device
            _cmediaJackInfoRender = GetJackDevice(CmediaDataFlow.eRender);
            if (_cmediaJackInfoRender == null) return -1;
            //Get Capture device
            _cmediajackInfoCapture = GetJackDevice(CmediaDataFlow.eCapture);
            if (_cmediajackInfoCapture == null) return -1;

            DisplayMessage.MenuName += $"\nSDK Initialize return {rev}";
#if DEMO
            var revData = GetJackDeviceInfoDemo(CmediaDriverReadWrite.Read);
#endif
            return rev;
        }

        public int Unitialize()
        {
           return NativeMethods.CMI_ConfLibUnInit();
        }

        public int RegisterSDKCallBackFunction(CmediaSDKCallback callBack)
        {
            _cmediaSDKCallback += callBack;
            return NativeMethods.CMI_RegisterCallbackFunction(_cmediaSDKCallback, IntPtr.Zero);
        }

        public void Dispose()
        {
            _cmediaSDKCallback = null;
            _cmediaJackInfoRender = null;
            _cmediajackInfoCapture = null;
        }
    }
}
