using OMENCmediaSDK.OMENSDK.Structures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OMENCmediaSDK.OMENSDK
{
    /// <summary>
    /// The Client API for OMEN
    /// </summary>
    public class OMENHeadsetHelper
    {
        public static async Task<int> InitializeSDKAsync()
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.Instance.InitializeSDK();
            });
        }

        public static async Task<int> UnInitializeSDKAsync()
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.Instance.UnInitializeSDK();
            });
        }

        public static async Task<VolumeControlStructure> GetAudioVolumeControl()
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.Instance.GetVolumeControl(OMENDataFlow.Render);
            });
        }

        public static async Task<VolumeControlStructure> GetMicrophoneVolumeControl()
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.Instance.GetVolumeControl(OMENDataFlow.Capture);
            });
        }

        public static async Task<bool> SetAudioVolumeScalarControl(List<VolumeChannelSturcture> audioData)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.Instance.SetVolumeScalarControl(OMENDataFlow.Render, audioData);
            });
        }

        public static async Task<bool> SetMicrophoneVolumeScalarControl(List<VolumeChannelSturcture> micData)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.Instance.SetVolumeScalarControl(OMENDataFlow.Capture, micData);
            });
        }

        public static async Task<bool> SetAudioMuteControl(int isMute)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.Instance.SetMuteControl(OMENDataFlow.Render, isMute);
            });
        }

        public static async Task<bool> SetMicrophoneMuteControl(int isMute)
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.Instance.SetMuteControl(OMENDataFlow.Capture, isMute);
            });
        }

        public static async Task<OMENReturnValue> GetOMENHeadsetInfo()
        {
            return await Task.Run(() =>
            {
                return CmediaSDKHelper.Instance.GetCmediaInfo();
            });
        }

        public static void RegisterSDKCallbackFunction(OMENSDKCallback callBack)
        {
            //Return value is useless.
            CmediaSDKHelper.Instance.RegisterSDKCallBackFunction(callBack);
        }
    }
}
