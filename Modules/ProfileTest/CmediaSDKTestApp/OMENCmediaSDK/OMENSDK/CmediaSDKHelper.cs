using OMENCmediaSDK.CmediaSDK;
using System;
using System.Collections.Generic;

namespace OMENCmediaSDK.OMENSDK
{
    /// <summary>
    /// Internal Class for OMEN logic
    /// </summary>
    class CmediaSDKHelper
    {
        private static CmediaSDKHelper _instance;
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static CmediaSDKHelper Instance
        {
            get
            {
                return _instance ?? (_instance = new CmediaSDKHelper());
            }
        }

        /// <summary>
        /// Private constructor.
        /// Please use instance to get functions
        /// </summary>
        private CmediaSDKHelper() { }

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
            return CmediaSDKService.Instance.Initialize();
        }

        public int UnInitializeSDK()
        {
            return CmediaSDKService.Instance.Unitialize();
        }

        public VolumeControlStructure GetVolumeControl(OMENDataFlow renderCapture)
        {
            CmediaDataFlow cmediaDataFlow = CmediaDataFlow.eRender;
            if (renderCapture == OMENDataFlow.Capture)
            {
                cmediaDataFlow = CmediaDataFlow.eCapture;
            }
            VolumeControlStructure volumeControl = new VolumeControlStructure();
            var rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new ClientData() { ApiName = CmediaAPIFunctionPoint.GetMaxVol.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.MaxValue = double.Parse(rev.RevValue);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new ClientData() { ApiName = CmediaAPIFunctionPoint.GetMinVol.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.MinValue = double.Parse(rev.RevValue);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new ClientData() { ApiName = CmediaAPIFunctionPoint.VolumeScalarControl.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.ScalarValue = double.Parse(rev.RevValue);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new ClientData() { ApiName = CmediaAPIFunctionPoint.GetVolStep.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.StepValue = double.Parse(rev.RevValue);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new ClientData() { ApiName = CmediaAPIFunctionPoint.MuteControl.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.IsMuted = int.Parse(rev.RevValue);
            //Get Channel data
            volumeControl.ChannelValues = new System.Collections.Generic.List<VolumeChannelSturcture>();

            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new ClientData() { ApiName = CmediaAPIFunctionPoint.VolumeControl.ToString(), SetValue = null, SetExtraValue = CmediaVolumeChannel.Master });
            if (rev.RevCode != 0) return null;
            VolumeChannelSturcture channel = new VolumeChannelSturcture() { ChannelValue = float.Parse(rev.RevValue), ChannelIndex = OMENVolumeChannel.Master };
            volumeControl.ChannelValues.Add(channel);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new ClientData() { ApiName = CmediaAPIFunctionPoint.VolumeControl.ToString(), SetValue = null, SetExtraValue = CmediaVolumeChannel.FrontLeft });
            if (rev.RevCode != 0) return null;
            channel = new VolumeChannelSturcture() { ChannelValue = float.Parse(rev.RevValue), ChannelIndex = OMENVolumeChannel.FrontLeft };
            volumeControl.ChannelValues.Add(channel);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new ClientData() { ApiName = CmediaAPIFunctionPoint.VolumeControl.ToString(), SetValue = null, SetExtraValue = CmediaVolumeChannel.FrontRight });
            if (rev.RevCode != 0) return null;
            channel = new VolumeChannelSturcture() { ChannelValue = float.Parse(rev.RevValue), ChannelIndex = OMENVolumeChannel.FrontRight };
            volumeControl.ChannelValues.Add(channel);

            return volumeControl;
        }

        public bool SetVolumeScalarControl(OMENDataFlow renderCapture, List<VolumeChannelSturcture> volumeData)
        {
            CmediaDataFlow cmediaDataFlow = CmediaDataFlow.eRender;
            if (renderCapture == OMENDataFlow.Capture)
            {
                cmediaDataFlow = CmediaDataFlow.eCapture;
            }
            bool rev = false;
            ReturnValue revData;
            foreach (var channle in volumeData)
            {
                revData = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Write, new ClientData() { ApiName = CmediaAPIFunctionPoint.VolumeScalarControl.ToString(), SetValue = channle.ChannelValue, SetExtraValue = channle.ChannelIndex });
            }
            return rev;
        }

        public bool SetMuteControl(OMENDataFlow renderCapture, int isMute)
        {
            CmediaDataFlow cmediaDataFlow = CmediaDataFlow.eRender;
            if (renderCapture == OMENDataFlow.Capture)
            {
                cmediaDataFlow = CmediaDataFlow.eCapture;
            }
            bool rev = false;
            ReturnValue revData;
            revData = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Write, new ClientData() { ApiName = CmediaAPIFunctionPoint.MuteControl.ToString(), SetValue = isMute });
            if (revData.RevCode != 0) return rev;
            return rev;
        }

        private CmediaSDKCallback _cmediaSDKCallback;
        public int RegisterSDKCallBackFunction(OMENSDKCallback callBack)
        {
            _cmediaSDKCallback = new CmediaSDKCallback(callBack);
            return CmediaSDKService.Instance.RegisterSDKCallBackFunction(_cmediaSDKCallback);
        }
    }
}
