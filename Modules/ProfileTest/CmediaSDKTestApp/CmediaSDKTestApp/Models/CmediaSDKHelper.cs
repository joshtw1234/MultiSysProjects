using CmediaSDKTestApp.BaseModels;
using System;
using System.Threading.Tasks;

namespace CmediaSDKTestApp.Models
{
    class CmediaSDKHelper
    {
        public static async Task<int> InitializeSDKAsync(IMenuItem displayMessage)
        {
            return await Task.Run(() =>
             {
                 return CmediaSDKService.Instance.Initialize(displayMessage);
 
             });
        }

        public static async Task<int> UnInitializeSDKAsync()
        {
            return await Task.Run(() =>
            {
                return CmediaSDKService.Instance.Unitialize();
            });
        }

        public static async Task<OMENREVData> GetSetJackDeviceDataAsync(CMI_DataFlow deviceType, CMI_DriverRW readWrite, string apiName, byte[] setValue = null)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKService.Instance.GetSetJackDeviceData(deviceType, readWrite, apiName, setValue);
            });
        }

        public static int RegisterSDKCallbackFunction(CmediaSDKCallback callBack)
        {
            return CmediaSDKService.Instance.RegisterSDKCallBackFunction(callBack);
        }
    }
}
