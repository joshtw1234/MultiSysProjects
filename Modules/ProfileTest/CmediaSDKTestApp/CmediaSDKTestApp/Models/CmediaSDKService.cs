﻿using CmediaSDKTestApp.BaseModels;
using System;
using System.Runtime.InteropServices;

namespace CmediaSDKTestApp.Models
{
    sealed class CmediaSDKService : IDisposable
    {
        private CmediaSDKCallback _cmediaSDKCallback;
        private CMI_JackDeviceInfo _cmediaJackInfoRender;
        private CMI_JackDeviceInfo _cmediajackInfoCapture;

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

        private OMENREVData OMEN_PropertyControl(ZazuRWData rwData)
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
            if (rwData.ReadWrite == CMI_DriverRW.Write || rwData.IsWriteExtra)
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
            OMENREVData rev = new OMENREVData() { RevCode = revCode, RevValue = revData, RevMessage = revString };
            return rev;
        }

        private CMI_JackDeviceInfo GetJackDevice(CMI_DataFlow deviceType)
        {
            string msg = "Found Device ";
            uint devCount = 0;
            CMI_DEVICEINFO devInfo;
            CMI_JackDeviceInfo jackDeviceInfo = new CMI_JackDeviceInfo();
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
                        case CMI_DeviceState.Active:
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

        private OMENREVData GetJackDeviceInfoDemo(CMI_DataFlow deviceType, CMI_JackDeviceInfo jackDevice)
        {
            ZazuRWData rwData = null;
            OMENREVData rev = null;
            switch (deviceType)
            {
                case CMI_DataFlow.eRender:
                    for (CmediaRenderFunctionPoint i = CmediaRenderFunctionPoint.DefaultDeviceControl; i <= CmediaRenderFunctionPoint.VOICECLARITY_NOISESUPP_LEVEL; i++)
                    {
                        rwData = new ZazuRWData() { JackInfo = jackDevice, ApiPropertyName = i.ToString(), ReadWrite = CMI_DriverRW.Read };
                        rev = OMEN_PropertyControl(rwData);
                        DisplayMessage.MenuName += $"{rev.RevMessage}";
                    }
                    break;
                case CMI_DataFlow.eCapture:
                    for (CmediaCaptureFunctionPoint i = CmediaCaptureFunctionPoint.Enable_MICECHO; i <= CmediaCaptureFunctionPoint.GetAAVolStep; i++)
                    {
                        rwData = new ZazuRWData() { JackInfo = jackDevice, ApiPropertyName = i.ToString(), ReadWrite = CMI_DriverRW.Read };
                        rev = OMEN_PropertyControl(rwData);
                        DisplayMessage.MenuName += $"{rev.RevMessage}";
                    }
                    break;
            }
            return rev;
        }

        public OMENREVData GetSetJackDeviceData(CMI_DriverRW readWrite, OMENClientData clientData)
        {
            OMENREVData revData = new OMENREVData() { RevCode = -1, RevMessage = $"API Name [{clientData.ApiName}] not Correct!" };
            CMI_DataFlow deviceType = CMI_DataFlow.eRender;
            CmediaRenderFunctionPoint renderAPI;
            CmediaCaptureFunctionPoint captureAPI;
            if (Enum.TryParse(clientData.ApiName, out renderAPI))
            {
                deviceType = CMI_DataFlow.eRender;
            }
            else if (Enum.TryParse(clientData.ApiName, out captureAPI))
            {
                deviceType = CMI_DataFlow.eCapture;
            }
            else
            {
                return revData;
            }
            if (readWrite == CMI_DriverRW.Write && 
                clientData.WriteValue == null && clientData.WriteExtraValue == null)
            {
                revData.RevMessage = "SetValue Can't be Null";
                return revData;
            }
            
            ZazuRWData rwData = null;
            CMI_JackDeviceInfo jackInfo = null;
            switch (deviceType)
            {
                case CMI_DataFlow.eRender:
                    jackInfo = _cmediaJackInfoRender;
                    break;
                case CMI_DataFlow.eCapture:
                    jackInfo = _cmediajackInfoCapture;
                    break;
            }
            rwData = new ZazuRWData() { JackInfo = jackInfo, ApiPropertyName = clientData.ApiName, ReadWrite = readWrite, WriteData = clientData.WriteValue, WriteExtraData = clientData.WriteExtraValue };
            revData = OMEN_PropertyControl(rwData);
            DisplayMessage.MenuName += $"{revData.RevMessage}";
            return revData;
        }

        public OMENREVData GetSetSurround(CMI_DriverRW readWrite, HPSurroundCommand hpCommand)
        {
            OMENREVData revData = new OMENREVData() { RevCode = -1, RevMessage = $"[{hpCommand}] not Correct!" };
            ZazuRWData rwData = null;
            REGISTER_OPERATION regop = new REGISTER_OPERATION();
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
            rwData = new ZazuRWData() { JackInfo = _cmediaJackInfoRender, ApiPropertyName = CmediaRenderFunctionPoint.VirtualSurroundEffectControl.ToString(),
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
            _cmediaJackInfoRender = GetJackDevice(CMI_DataFlow.eRender);
            if (_cmediaJackInfoRender == null) return -1;
            //Get Capture device
            _cmediajackInfoCapture = GetJackDevice(CMI_DataFlow.eCapture);
            if (_cmediajackInfoCapture == null) return -1;
            var revData = GetJackDeviceInfoDemo(CMI_DataFlow.eRender, _cmediaJackInfoRender);
            DisplayMessage.MenuName += $"\nSDK Initialize return {rev}";
#if false
             
            revData = GetJackDeviceInfoDemo(CMI_DataFlow.eCapture, _cmediajackInfoCapture);

            GetJackDeviceData(CMI_DeviceType.Render, CmediaRenderFunctionPoint.DefaultDeviceControl.ToString());
            GetJackDeviceData(CMI_DeviceType.Capture, CmediaCaptureFunctionPoint.MagicVoice_Selection.ToString());
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
