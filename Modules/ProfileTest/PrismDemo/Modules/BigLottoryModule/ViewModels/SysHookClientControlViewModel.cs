using System;
using System.Runtime.InteropServices;
using CentralModule.Interface;
using SystemControlLib.Structures;

namespace CentralModule.ViewModels
{
    public class SysHookClientControlViewModel
    {
        public SysHookClientControlViewModel(IBigLottoryControlModel model)
        {
            SystemControlLib.SystemHook.Instence.RegisterDeviceChangeCallBack(OnDeviceChangeCallBack);
        }

        private void OnDeviceChangeCallBack(DeviceMessage devMessage)
        {
            var audioGUID = new Guid(SystemControlLib.SystemControlLibConsts.KSCATEGORY_AUDIO);
            DEV_BROADCAST_HDR devHDR = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(devMessage.LParam, typeof(DEV_BROADCAST_HDR));
            switch (devMessage.Message)
            {
                case SystemControlLib.Enums.WM_DeviceChange.DBT_DEVICEARRIVAL:
                    switch(devHDR.dbch_DeviceType)
                    {
                        case SystemControlLib.Enums.WM_DeviceType.DBT_DEVTYP_DEVICEINTERFACE:
                            DEV_BROADCAST_DEVICEINTERFACE devInterFace = (DEV_BROADCAST_DEVICEINTERFACE)Marshal.PtrToStructure(devMessage.LParam, typeof(DEV_BROADCAST_DEVICEINTERFACE));
                            
                            if (devInterFace.dbcc_classguid.Equals(audioGUID))
                            {
                              
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case SystemControlLib.Enums.WM_DeviceChange.DBT_DEVICEREMOVECOMPLETE:
                    switch (devHDR.dbch_DeviceType)
                    {
                        case SystemControlLib.Enums.WM_DeviceType.DBT_DEVTYP_DEVICEINTERFACE:
                            DEV_BROADCAST_DEVICEINTERFACE devInterFace = (DEV_BROADCAST_DEVICEINTERFACE)Marshal.PtrToStructure(devMessage.LParam, typeof(DEV_BROADCAST_DEVICEINTERFACE));
                            if (devInterFace.dbcc_classguid.Equals(audioGUID))
                            {

                            }
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }
    }
}
