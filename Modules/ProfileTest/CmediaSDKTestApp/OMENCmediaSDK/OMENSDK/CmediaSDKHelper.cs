using OMENCmediaSDK.CmediaSDK;
using System.Collections.Generic;

namespace OMENCmediaSDK.OMENSDK
{
    /// <summary>
    /// Internal Class for OMEN logic
    /// </summary>
    class CmediaSDKHelper
    {
        //OMENREVData GetSurroundAsync(HPSurroundCommand hpcommand)
        //{
        //    return CmediaSDKService.Instance.GetSetSurroundData(CmediaDriverReadWrite.Read, hpcommand);
        //}

        //OMENREVData SetSurroundAsync(HPSurroundCommand hpcommand)
        //{
        //    return CmediaSDKService.Instance.GetSetSurroundData(CmediaDriverReadWrite.Write, hpcommand);
        //}

        public static int InitializeSDK()
        {
            return CmediaSDKService.Instance.Initialize();
        }

        public static int UnInitializeSDK()
        {
            return CmediaSDKService.Instance.Unitialize();
        }

        public static VolumeControlStructure GetVolumeControl(OMENDataFlow renderCapture)
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
            
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new ClientData() { ApiName = CmediaAPIFunctionPoint.VolumeControl.ToString(), SetValue = null, SetExtraValue = VolumeChannel.Master });
            if (rev.RevCode != 0) return null;
            VolumeChannelSturcture channel = new VolumeChannelSturcture() { ChannelValue = float.Parse(rev.RevValue), ChannelIndex = VolumeChannel.Master };
            volumeControl.ChannelValues.Add(channel);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new ClientData() { ApiName = CmediaAPIFunctionPoint.VolumeControl.ToString(), SetValue = null, SetExtraValue = VolumeChannel.FrontLeft });
            if (rev.RevCode != 0) return null;
            channel = new VolumeChannelSturcture() { ChannelValue = float.Parse(rev.RevValue), ChannelIndex = VolumeChannel.FrontLeft };
            volumeControl.ChannelValues.Add(channel);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new ClientData() { ApiName = CmediaAPIFunctionPoint.VolumeControl.ToString(), SetValue = null, SetExtraValue = VolumeChannel.FrontRight });
            if (rev.RevCode != 0) return null;
            channel = new VolumeChannelSturcture() { ChannelValue = float.Parse(rev.RevValue), ChannelIndex = VolumeChannel.FrontRight };
            volumeControl.ChannelValues.Add(channel);

            return volumeControl;
        }

        public static bool SetVolumeScalarControl(OMENDataFlow renderCapture, List<VolumeChannelSturcture> volumeData)
        {
            CmediaDataFlow cmediaDataFlow = CmediaDataFlow.eRender;
            if (renderCapture == OMENDataFlow.Capture)
            {
                cmediaDataFlow = CmediaDataFlow.eCapture;
            }
            bool rev = false;
            ReturnValue revData;
            foreach(var channle in volumeData)
            {
                revData = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Write, new ClientData() { ApiName = CmediaAPIFunctionPoint.VolumeScalarControl.ToString(), SetValue = channle.ChannelValue, SetExtraValue = channle.ChannelIndex });
            }
            return rev;
        }

        public static bool SetMuteControl(OMENDataFlow renderCapture, int isMute)
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

    }
}
