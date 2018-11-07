using CmediaSDKTestApp.BaseModels;
using System.Threading.Tasks;

namespace CmediaSDKTestApp.Models
{
    /// <summary>
    /// The Client API for OMEN
    /// </summary>
    class ZazuHelper
    {
        public static async Task<int> InitializeSDKAsync(IMenuItem displayMessage)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.InitializeSDK(displayMessage);

            });
        }

        public static async Task<int> UnInitializeSDKAsync()
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.UnInitializeSDK();
            });
        }

        public static async Task<OMENVolumeControlStructure> GetVolumeControl(OMENDataFlow renderCapture)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.GetVolumeControl(renderCapture);
            });
        }

        public static void RegisterSDKCallbackFunction(CmediaSDKCallback callBack)
        {
            //Return value is useless.
            CmediaSDKService.Instance.RegisterSDKCallBackFunction(callBack);
        }
    }
}
