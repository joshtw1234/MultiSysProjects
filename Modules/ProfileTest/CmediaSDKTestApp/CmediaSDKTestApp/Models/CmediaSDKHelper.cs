using CmediaSDKTestApp.BaseModels;
using System;
using System.Runtime.InteropServices;

namespace CmediaSDKTestApp.Models
{
    sealed class CmediaSDKHelper
    {
        CmediaSDKCallback _cmediaSDKCallback;
        CMI_JackDeviceInfo _cmediaJackInfoRender, _cmediajackInfoCapture;

        private IMenuItem DisplayMessage;
        private void OnCmediaSDKCallback(int type, int id, int componentType, ulong eventId)
        {
            //throw new NotImplementedException();
        }

        private static CmediaSDKHelper _instance;
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static CmediaSDKHelper Instance
        {
            get
            {
                return _instance ?? (_instance = new CmediaSDKHelper());
            }
        }

        /// <summary>
        /// Private constructor.
        /// Please use instance to get functions
        /// </summary>
        private CmediaSDKHelper() { }

        const int CMI_BUFFER_SIZE = 1024;

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

            byte[] devExtraBvalue = new byte[1];
            IntPtr pdevExtraVlue = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * CMI_BUFFER_SIZE);
            Marshal.Copy(devExtraBvalue, 0, pdevExtraVlue, devExtraBvalue.Length);
            GCHandle gch = GCHandle.Alloc(pdevExtraVlue, GCHandleType.Pinned);
            IntPtr devExtraValue = gch.AddrOfPinnedObject();

            // Call the CMI_PropertyControl() API.
            // The CMI_PropertyControl() API will not
            // change the value of devValue. 
            string APIName = string.Empty;
            switch(rwData.DeviceType)
            {
                case CMI_DeviceType.Render:
                    APIName = rwData.RenderPropertyName.ToString();
                    break;
                case CMI_DeviceType.Capture:
                    APIName = rwData.CapturePropertyName.ToString();
                    break;
            }
            int revCode = NativeMethods.CMI_PropertyControl(rwData.JackInfo.m_devInfo, APIName, devValue, devExtraValue, rwData.ReadWrite);
            string revString = $"\nCMI_PropertyControl [{APIName}] {rwData.ReadWrite} return {revCode}";
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
                byte[] NewByteArray = new byte[CMI_BUFFER_SIZE];

                // Copy the byte array values pointed to by revPtr[0]
                // to NewByteArray.
                Marshal.Copy(revPtr[0], NewByteArray, 0, NewByteArray.Length);
                revData = System.Text.Encoding.UTF8.GetString(NewByteArray).Replace('\0', ' ').Trim();
                revString = $"\nCMI_PropertyControl [{APIName} {rwData.ReadWrite}] return {revCode} Get Data [{revData}]";
            }
            gchDevValue.Free();
            gch.Free();
            OMENREVData rev = new OMENREVData() { RevCode = revCode, RevValue = revData, RevMessage = revString };
            return rev;
        }

        private CMI_JackDeviceInfo GetJackDevice(CMI_DeviceType deviceType)
        {
            string msg = "Found Device ";
            uint devCount = 0;
            CMI_DEVICEINFO devInfo;
            CMI_JackDeviceInfo jackDeviceInfo = new CMI_JackDeviceInfo();
            int rev = NativeMethods.CMI_GetDeviceCount(CMI_DeviceType.Render, ref devCount);
            if (0 == devCount)
            {
                msg = $"{deviceType} Device not found!!";
                return null;
            }
            else
            {
                for (int i = 0; i < devCount; i++)
                {
                    rev = NativeMethods.CMI_GetDeviceById(CMI_DeviceType.Render, i, out devInfo);
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

        private OMENREVData GetJackDeviceInfo(CMI_DeviceType deviceType, CMI_JackDeviceInfo jackDevice)
        {
            ZazuRWData rwData = null;
            OMENREVData rev = null;
            switch (deviceType)
            {
                case CMI_DeviceType.Render:
                    for (CmediaRenderFunctionPoint i = CmediaRenderFunctionPoint.DefaultDeviceControl; i <= CmediaRenderFunctionPoint.VOCALCANCEL_LEVEL; i++)
                    {
                        rwData = new ZazuRWData() { JackInfo = jackDevice, DeviceType = deviceType, RenderPropertyName = i, ReadWrite = CMI_DriverRW.Read, WriteData = null };
                        rev = OMEN_PropertyControl(rwData);
                        DisplayMessage.MenuName += $"{rev.RevMessage}";
                    }
                    break;
                case CMI_DeviceType.Capture:
                    for (CmediaCaptureFunctionPoint i = CmediaCaptureFunctionPoint.Enable_MICECHO; i <= CmediaCaptureFunctionPoint.GetAAVolStep; i++)
                    {
                        rwData = new ZazuRWData() { JackInfo = jackDevice, DeviceType = deviceType, CapturePropertyName = i, ReadWrite = CMI_DriverRW.Read, WriteData = null };
                        rev = OMEN_PropertyControl(rwData);
                        DisplayMessage.MenuName += $"{rev.RevMessage}";
                    }
                    break;
            }
            return rev;
        }

        public int Initialize(IMenuItem logMessag)
        {
            DisplayMessage = logMessag;
            int rev = NativeMethods.CMI_ConfLibInit();
            rev = NativeMethods.CMI_CreateDeviceList();
            //Get Render device
            _cmediaJackInfoRender = GetJackDevice(CMI_DeviceType.Render);
            //Get Capture device
            _cmediajackInfoCapture = GetJackDevice(CMI_DeviceType.Capture);
            //Register SDK callback
            _cmediaSDKCallback = OnCmediaSDKCallback;
            rev = NativeMethods.CMI_RegisterCallbackFunction(_cmediaSDKCallback, IntPtr.Zero);
            DisplayMessage.MenuName += $"\nRegisterCallback return {rev}";
            var revData = GetJackDeviceInfo(CMI_DeviceType.Render, _cmediaJackInfoRender);
            revData = GetJackDeviceInfo(CMI_DeviceType.Capture, _cmediajackInfoCapture);
            return rev;
        }
    }
}
