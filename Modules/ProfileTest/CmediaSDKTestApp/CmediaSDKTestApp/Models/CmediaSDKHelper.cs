using CmediaSDKTestApp.BaseModels;
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

        public static async Task<OMENREVData> GetJackDeviceDataAsync(CMI_DeviceType deviceType, string apiName)
        {
            return await Task.Run(() => 
            {
                return CmediaSDKService.Instance.GetJackDeviceData(deviceType, apiName);
            });
        }

        public static async Task<OMENREVData> SetJackDeviceDataAsync(CMI_DeviceType deviceType, string apiName, byte[] setValue)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKService.Instance.SetJackDeviceData(deviceType, apiName, setValue);
            });
        }
    }
}
