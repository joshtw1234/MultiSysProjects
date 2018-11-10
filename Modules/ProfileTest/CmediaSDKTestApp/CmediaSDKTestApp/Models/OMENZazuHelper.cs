using CmediaSDKTestApp.BaseModels;
using System.Threading.Tasks;

namespace CmediaSDKTestApp.Models
{
    /// <summary>
    /// The Client API for OMEN
    /// </summary>
    class OMENZazuHelper
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

        public static async Task<VolumeControlStructure> GetAudioVolumeControl()
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.GetVolumeControl(OMENDataFlow.Render);
            });
        }

        public static async Task<VolumeControlStructure> GetMicrophoneVolumeControl()
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.GetVolumeControl(OMENDataFlow.Capture);
            });
        }

        public static async Task<bool> SetAudioVolumeControl(BaseVolumeControlStructure audioData)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.SetVolumeControl(OMENDataFlow.Render, audioData);
            });
        }

        public static async Task<bool> SetMicrophoneVolumeControl(BaseVolumeControlStructure micData)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.SetVolumeControl(OMENDataFlow.Capture, micData);
            });
        }

        public static void RegisterSDKCallbackFunction(CmediaSDKCallback callBack)
        {
            //Return value is useless.
            CmediaSDKService.Instance.RegisterSDKCallBackFunction(callBack);
        }
    }
}
