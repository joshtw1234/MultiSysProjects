using CmediaSDKTestApp.BaseModels;
using System.Collections.Generic;
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

        public static async Task<bool> SetAudioVolumeScalarControl(List<VolumeChannelSturcture> audioData)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.SetVolumeScalarControl(OMENDataFlow.Render, audioData);
            });
        }

        public static async Task<bool> SetMicrophoneVolumeScalarControl(List<VolumeChannelSturcture> micData)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.SetVolumeScalarControl(OMENDataFlow.Capture, micData);
            });
        }

        public static async Task<bool> SetAudioMuteControl(int isMute)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.SetMuteControl(OMENDataFlow.Render, isMute);
            });
        }

        public static async Task<bool> SetMicrophoneMuteControl(int isMute)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.SetMuteControl(OMENDataFlow.Capture, isMute);
            });
        }

        public static void RegisterSDKCallbackFunction(CmediaSDKCallback callBack)
        {
            //Return value is useless.
            CmediaSDKService.Instance.RegisterSDKCallBackFunction(callBack);
        }
    }
}
