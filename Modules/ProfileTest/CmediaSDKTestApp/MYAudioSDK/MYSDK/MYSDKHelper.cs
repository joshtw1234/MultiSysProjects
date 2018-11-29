using MYAudioSDK.CAudioSDK;
using MYAudioSDK.CAudioSDK.CallBacks;
using MYAudioSDK.CAudioSDK.Enums;
using MYAudioSDK.CAudioSDK.Structures;
using MYAudioSDK.MYSDK.Structures;
using System.Collections.Generic;

namespace MYAudioSDK.MYSDK
{
    /// <summary>
    /// Internal Class for OMEN logic
    /// </summary>
    class MYSDKHelper
    {
        private static MYSDKHelper _instance;
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static MYSDKHelper Instance
        {
            get
            {
                return _instance ?? (_instance = new MYSDKHelper());
            }
        }

        /// <summary>
        /// Private constructor.
        /// Please use instance to get functions
        /// </summary>
        private MYSDKHelper() { }

        //OMENREVData GetSurroundAsync(HPSurroundCommand hpcommand)
        //{
        //    return CmediaSDKService.Instance.GetSetSurroundData(CmediaDriverReadWrite.Read, hpcommand);
        //}

        //OMENREVData SetSurroundAsync(HPSurroundCommand hpcommand)
        //{
        //    return CmediaSDKService.Instance.GetSetSurroundData(CmediaDriverReadWrite.Write, hpcommand);
        //}

        public int InitializeSDK()
        {
            return CAudioSDKService.Instance.Initialize();
        }

        public int UnInitializeSDK()
        {
            return CAudioSDKService.Instance.Unitialize();
        }

        public VolumeControlStructure GetVolumeControl(OMENDataFlow renderCapture)
        {
            CAudioDataFlow cmediaDataFlow = CAudioDataFlow.eRender;
            if (renderCapture == OMENDataFlow.Capture)
            {
                cmediaDataFlow = CAudioDataFlow.eCapture;
            }
            VolumeControlStructure volumeControl = new VolumeControlStructure();
            var rev = CAudioSDKService.Instance.ConfigureJackDeviceData(cmediaDataFlow, CAudioDriverReadWrite.Read, new CAudioClientData() { ApiName = CAudioAPIFunctionPoint.GetMaxVol.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.MaxValue = double.Parse(rev.RevValue);
            rev = CAudioSDKService.Instance.ConfigureJackDeviceData(cmediaDataFlow, CAudioDriverReadWrite.Read, new CAudioClientData() { ApiName = CAudioAPIFunctionPoint.GetMinVol.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.MinValue = double.Parse(rev.RevValue);
            rev = CAudioSDKService.Instance.ConfigureJackDeviceData(cmediaDataFlow, CAudioDriverReadWrite.Read, new CAudioClientData() { ApiName = CAudioAPIFunctionPoint.VolumeScalarControl.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.ScalarValue = double.Parse(rev.RevValue);
            rev = CAudioSDKService.Instance.ConfigureJackDeviceData(cmediaDataFlow, CAudioDriverReadWrite.Read, new CAudioClientData() { ApiName = CAudioAPIFunctionPoint.GetVolStep.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.StepValue = double.Parse(rev.RevValue);
            rev = CAudioSDKService.Instance.ConfigureJackDeviceData(cmediaDataFlow, CAudioDriverReadWrite.Read, new CAudioClientData() { ApiName = CAudioAPIFunctionPoint.MuteControl.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.IsMuted = int.Parse(rev.RevValue);
            //Get Channel data
            volumeControl.ChannelValues = new System.Collections.Generic.List<VolumeChannelSturcture>();

            rev = CAudioSDKService.Instance.ConfigureJackDeviceData(cmediaDataFlow, CAudioDriverReadWrite.Read, new CAudioClientData() { ApiName = CAudioAPIFunctionPoint.VolumeControl.ToString(), SetValue = null, SetExtraValue = CAudioVolumeChannel.Master });
            if (rev.RevCode != 0) return null;
            VolumeChannelSturcture channel = new VolumeChannelSturcture() { ChannelValue = float.Parse(rev.RevValue), ChannelIndex = OMENVolumeChannel.Master };
            volumeControl.ChannelValues.Add(channel);
            rev = CAudioSDKService.Instance.ConfigureJackDeviceData(cmediaDataFlow, CAudioDriverReadWrite.Read, new CAudioClientData() { ApiName = CAudioAPIFunctionPoint.VolumeControl.ToString(), SetValue = null, SetExtraValue = CAudioVolumeChannel.FrontLeft });
            if (rev.RevCode != 0) return null;
            channel = new VolumeChannelSturcture() { ChannelValue = float.Parse(rev.RevValue), ChannelIndex = OMENVolumeChannel.FrontLeft };
            volumeControl.ChannelValues.Add(channel);
            rev = CAudioSDKService.Instance.ConfigureJackDeviceData(cmediaDataFlow, CAudioDriverReadWrite.Read, new CAudioClientData() { ApiName = CAudioAPIFunctionPoint.VolumeControl.ToString(), SetValue = null, SetExtraValue = CAudioVolumeChannel.FrontRight });
            if (rev.RevCode != 0) return null;
            channel = new VolumeChannelSturcture() { ChannelValue = float.Parse(rev.RevValue), ChannelIndex = OMENVolumeChannel.FrontRight };
            volumeControl.ChannelValues.Add(channel);

            return volumeControl;
        }

        public bool SetVolumeScalarControl(OMENDataFlow renderCapture, List<VolumeChannelSturcture> volumeData)
        {
            CAudioDataFlow cmediaDataFlow = CAudioDataFlow.eRender;
            if (renderCapture == OMENDataFlow.Capture)
            {
                cmediaDataFlow = CAudioDataFlow.eCapture;
            }
            bool rev = false;
            ReturnValue revData;
            foreach (var channle in volumeData)
            {
                revData = CAudioSDKService.Instance.ConfigureJackDeviceData(
                    cmediaDataFlow, 
                    CAudioDriverReadWrite.Write, 
                    new CAudioClientData() {
                        ApiName = CAudioAPIFunctionPoint.VolumeScalarControl.ToString(),
                        SetValue = channle.ChannelValue,
                        SetExtraValue = (CAudioVolumeChannel)channle.ChannelIndex });
            }
            return rev;
        }

        public bool SetMuteControl(OMENDataFlow renderCapture, int isMute)
        {
            CAudioDataFlow cmediaDataFlow = CAudioDataFlow.eRender;
            if (renderCapture == OMENDataFlow.Capture)
            {
                cmediaDataFlow = CAudioDataFlow.eCapture;
            }
            bool rev = false;
            ReturnValue revData;
            revData = CAudioSDKService.Instance.ConfigureJackDeviceData(cmediaDataFlow, CAudioDriverReadWrite.Write, new CAudioClientData() { ApiName = CAudioAPIFunctionPoint.MuteControl.ToString(), SetValue = isMute });
            if (revData.RevCode != 0) return rev;
            return rev;
        }

        public OMENReturnValue GetCmediaInfo()
        {
            OMENReturnValue revData = new OMENReturnValue();
            var rev = CAudioSDKService.Instance.ConfigureJackDeviceData(CAudioDataFlow.eRender, CAudioDriverReadWrite.Read,
                new CAudioClientData() { ApiName = CAudioAPIFunctionPoint.GetDeviceFriendlyName.ToString() });
            revData.RevCode = rev.RevCode;
            revData.RevValue= rev.RevValue;
            revData.RevExtraValue = rev.RevExtraValue;
            revData.RevMessage = $"{rev.RevValue}|";
            rev = CAudioSDKService.Instance.ConfigureJackDeviceData(CAudioDataFlow.eRender, CAudioDriverReadWrite.Read,
                new CAudioClientData() { ApiName = CAudioAPIFunctionPoint.GetDriverVer.ToString() });
            revData.RevMessage += $"{rev.RevValue}|";
            rev = CAudioSDKService.Instance.ConfigureJackDeviceData(CAudioDataFlow.eRender, CAudioDriverReadWrite.Read,
               new CAudioClientData() { ApiName = CAudioAPIFunctionPoint.GetFirmwareVer.ToString() });
            revData.RevMessage += $"{rev.RevValue}|";
            return revData;
        }

        private CAudioSDKCallback _cmediaSDKCallback;
        public int RegisterSDKCallBackFunction(OMENSDKCallback callBack)
        {
            _cmediaSDKCallback = new CAudioSDKCallback(callBack);
            return CAudioSDKService.Instance.RegisterSDKCallBackFunction(_cmediaSDKCallback);
        }
    }
}
