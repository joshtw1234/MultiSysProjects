using MYAudioSDK.MYSDK.Structures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MYAudioSDK.MYSDK
{
    /// <summary>
    /// The Client API for OMEN
    /// </summary>
    public class MYHeadsetHelper
    {
        public static async Task<int> InitializeSDKAsync()
        {
            return await Task.Run(() =>
            {
                return MYSDKHelper.Instance.InitializeSDK();
            });
        }

        public static async Task<int> UnInitializeSDKAsync()
        {
            return await Task.Run(() =>
            {
                return MYSDKHelper.Instance.UnInitializeSDK();
            });
        }

        public static async Task<VolumeControlStructure> GetAudioVolumeControl()
        {
            return await Task.Run(() =>
            {
                return MYSDKHelper.Instance.GetVolumeControl(OMENDataFlow.Render);
            });
        }

        public static async Task<VolumeControlStructure> GetMicrophoneVolumeControl()
        {
            return await Task.Run(() =>
            {
                return MYSDKHelper.Instance.GetVolumeControl(OMENDataFlow.Capture);
            });
        }

        public static async Task<bool> SetAudioVolumeScalarControl(List<VolumeChannelSturcture> audioData)
        {
            return await Task.Run(() =>
            {
                return MYSDKHelper.Instance.SetVolumeScalarControl(OMENDataFlow.Render, audioData);
            });
        }

        public static async Task<bool> SetMicrophoneVolumeScalarControl(List<VolumeChannelSturcture> micData)
        {
            return await Task.Run(() =>
            {
                return MYSDKHelper.Instance.SetVolumeScalarControl(OMENDataFlow.Capture, micData);
            });
        }

        public static async Task<bool> SetAudioMuteControl(int isMute)
        {
            return await Task.Run(() =>
            {
                return MYSDKHelper.Instance.SetMuteControl(OMENDataFlow.Render, isMute);
            });
        }

        public static async Task<bool> SetMicrophoneMuteControl(int isMute)
        {
            return await Task.Run(() =>
            {
                return MYSDKHelper.Instance.SetMuteControl(OMENDataFlow.Capture, isMute);
            });
        }

        public static async Task<OMENReturnValue> GetOMENHeadsetInfo()
        {
            return await Task.Run(() =>
            {
                return MYSDKHelper.Instance.GetCmediaInfo();
            });
        }

        public static void RegisterSDKCallbackFunction(OMENSDKCallback callBack)
        {
            //Return value is useless.
            MYSDKHelper.Instance.RegisterSDKCallBackFunction(callBack);
        }
    }
}
