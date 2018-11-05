using CmediaSDKTestApp.BaseModels;
using System;

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
            return rev;
        }
    }
}
