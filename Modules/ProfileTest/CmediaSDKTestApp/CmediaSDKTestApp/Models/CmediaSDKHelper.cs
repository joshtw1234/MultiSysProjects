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
        public static async Task<OMENREVData> GetJackDeviceDataAsync(OMENClientData omenData)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKService.Instance.GetSetJackDeviceData(CMI_DriverRW.Read, omenData.ApiName, omenData.WriteValue);
            });
        }

        public static async Task<OMENREVData> SetJackDeviceDataAsync(OMENClientData omenData)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKService.Instance.GetSetJackDeviceData(CMI_DriverRW.Write, omenData.ApiName, omenData.WriteValue);
            });
        }

        public static int RegisterSDKCallbackFunction(CmediaSDKCallback callBack)
        {
            return CmediaSDKService.Instance.RegisterSDKCallBackFunction(callBack);
        }
    }
}
