using CmediaSDKTestApp.BaseModels;
using System;
using System.Runtime.InteropServices;

namespace CmediaSDKTestApp.Models
{
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

        private const int CMI_BUFFER_SIZE = 1024;

        private OMENREVData OMEN_PropertyControl(ZazuReadWriteData rwData)
        {
            // Allocate a Cmedia standard Array.
            byte[] devBvalue = new byte[CMI_BUFFER_SIZE];
            if (rwData.ReadWrite == CmediaDriverReadWrite.Write && rwData.WriteData != null)
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
            if ((rwData.ReadWrite == CmediaDriverReadWrite.Write || rwData.IsWriteExtra) && rwData.WriteExtraData != null)
            {
                devExtraBvalue = rwData.WriteExtraData;
            }
            IntPtr pdevExtraVlue = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * CMI_BUFFER_SIZE);
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
                byte[] NewByteArray = new byte[CMI_BUFFER_SIZE];

                // Copy the byte array values pointed to by revPtr[0]
                // to NewByteArray.
                Marshal.Copy(revPtr[0], NewByteArray, 0, NewByteArray.Length);
                revData = System.Text.Encoding.UTF8.GetString(NewByteArray).Replace('\0', ' ').Trim();
                revString = $"\nCMI_PropertyControl [{rwData.JackInfo.m_devInfo.DataFlow}] [{rwData.ApiPropertyName} {rwData.ReadWrite}] return {revCode} Get Data [{revData}]";

                IntPtr[] revExraPtr = new IntPtr[1];
                Marshal.Copy(devExtraValue, revExraPtr, 0, 1);
                byte[] NewExtraByteArray = new byte[CMI_BUFFER_SIZE];
                Marshal.Copy(revExraPtr[0], NewExtraByteArray, 0, NewExtraByteArray.Length);
                revExraData = System.Text.Encoding.UTF8.GetString(NewExtraByteArray).Replace('\0', ' ').Trim();
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

        private OMENREVData GetJackDeviceInfoDemo(CmediaDataFlow deviceType, CmediaJackDeviceInfo jackDevice)
        {
            ZazuReadWriteData rwData = null;
            OMENREVData rev = null;
            switch (deviceType)
            {
                case CmediaDataFlow.eRender:
                    for (CmediaRenderFunctionPoint i = CmediaRenderFunctionPoint.DefaultDeviceControl; i <= CmediaRenderFunctionPoint.VOICECLARITY_NOISESUPP_LEVEL; i++)
                    {
                        rwData = new ZazuReadWriteData() { JackInfo = jackDevice, ApiPropertyName = i.ToString(), ReadWrite = CmediaDriverReadWrite.Read };
                        rev = OMEN_PropertyControl(rwData);
                        DisplayMessage.MenuName += $"{rev.RevMessage}";
                    }
                    break;
                case CmediaDataFlow.eCapture:
                    for (CmediaCaptureFunctionPoint i = CmediaCaptureFunctionPoint.Enable_MICECHO; i <= CmediaCaptureFunctionPoint.GetAAVolStep; i++)
                    {
                        rwData = new ZazuReadWriteData() { JackInfo = jackDevice, ApiPropertyName = i.ToString(), ReadWrite = CmediaDriverReadWrite.Read };
                        rev = OMEN_PropertyControl(rwData);
                        DisplayMessage.MenuName += $"{rev.RevMessage}";
                    }
                    break;
            }
            return rev;
        }

        public OMENREVData GetSetJackDeviceData(CmediaDriverReadWrite readWrite, OMENClientData clientData)
        {
            OMENREVData revData = new OMENREVData() { RevCode = -1, RevMessage = $"API Name [{clientData.ApiName}] not Correct!" };
            CmediaDataFlow deviceType = CmediaDataFlow.eRender;
            CmediaRenderFunctionPoint renderAPI;
            CmediaCaptureFunctionPoint captureAPI;
            if (Enum.TryParse(clientData.ApiName, out renderAPI))
            {
                deviceType = CmediaDataFlow.eRender;
            }
            else if (Enum.TryParse(clientData.ApiName, out captureAPI))
            {
                deviceType = CmediaDataFlow.eCapture;
            }
            else
            {
                return revData;
            }
            if (readWrite == CmediaDriverReadWrite.Write && 
                clientData.WriteValue == null && clientData.WriteExtraValue == null)
            {
                revData.RevMessage = "SetValue Can't be Null";
                return revData;
            }
            
            ZazuReadWriteData rwData = null;
            CmediaJackDeviceInfo jackInfo = null;
            switch (deviceType)
            {
                case CmediaDataFlow.eRender:
                    jackInfo = _cmediaJackInfoRender;
                    break;
                case CmediaDataFlow.eCapture:
                    jackInfo = _cmediajackInfoCapture;
                    break;
            }
            rwData = new ZazuReadWriteData() { JackInfo = jackInfo, ApiPropertyName = clientData.ApiName, ReadWrite = readWrite, WriteData = clientData.WriteValue, WriteExtraData = clientData.WriteExtraValue };
            revData = OMEN_PropertyControl(rwData);
            DisplayMessage.MenuName += $"{revData.RevMessage}";
            return revData;
        }

        public OMENREVData GetSetSurround(CmediaDriverReadWrite readWrite, HPSurroundCommand hpCommand)
        {
            OMENREVData revData = new OMENREVData() { RevCode = -1, RevMessage = $"[{hpCommand}] not Correct!" };
            ZazuReadWriteData rwData = null;
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
            rwData = new ZazuReadWriteData() { JackInfo = _cmediaJackInfoRender, ApiPropertyName = CmediaRenderFunctionPoint.VirtualSurroundEffectControl.ToString(),
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
            var revData = GetJackDeviceInfoDemo(CmediaDataFlow.eRender, _cmediaJackInfoRender);
            revData = GetJackDeviceInfoDemo(CmediaDataFlow.eCapture, _cmediajackInfoCapture);
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
