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
                return CmediaSDKService.Instance.GetSetJackDeviceData(CmediaDriverReadWrite.Read, omenData);
            });
        }

        public static async Task<OMENREVData> SetJackDeviceDataAsync(OMENClientData omenData)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKService.Instance.GetSetJackDeviceData(CmediaDriverReadWrite.Write, omenData);
            });
        }

        public static async Task<OMENREVData> GetSurroundAsync(HPSurroundCommand hpcommand)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKService.Instance.GetSetSurroundData(CmediaDriverReadWrite.Read, hpcommand);
            });
        }

        public static async Task<OMENREVData> SetSurroundAsync(HPSurroundCommand hpcommand)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKService.Instance.GetSetSurroundData(CmediaDriverReadWrite.Write, hpcommand);
            });
        }

        public static void RegisterSDKCallbackFunction(CmediaSDKCallback callBack)
        {
            //Return value is useless.
            CmediaSDKService.Instance.RegisterSDKCallBackFunction(callBack);
        }
    }
}
