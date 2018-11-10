using CmediaSDKTestApp.BaseModels;
using System;

namespace CmediaSDKTestApp.Models
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

        internal static int InitializeSDK(IMenuItem displayMessage)
        {
            return CmediaSDKService.Instance.Initialize(displayMessage);
        }

        internal static int UnInitializeSDK()
        {
            return CmediaSDKService.Instance.Unitialize();
        }

        internal static OMENVolumeControlStructure GetVolumeControl(OMENDataFlow renderCapture)
        {
            CmediaDataFlow cmediaDataFlow = CmediaDataFlow.eRender;
            if (renderCapture == OMENDataFlow.Capture)
            {
                cmediaDataFlow = CmediaDataFlow.eCapture;
            }
            OMENVolumeControlStructure volumeControl = new OMENVolumeControlStructure();
            var rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new OMENClientData() { ApiName = CmediaAPIFunctionPoint.GetMaxVol.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.MaxValue = double.Parse(rev.RevValue);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new OMENClientData() { ApiName = CmediaAPIFunctionPoint.GetMinVol.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.MinValue = double.Parse(rev.RevValue);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new OMENClientData() { ApiName = CmediaAPIFunctionPoint.VolumeScalarControl.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.ScalarValue = double.Parse(rev.RevValue);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new OMENClientData() { ApiName = CmediaAPIFunctionPoint.GetVolStep.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.StepValue = double.Parse(rev.RevValue);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new OMENClientData() { ApiName = CmediaAPIFunctionPoint.MuteControl.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.IsMuted = Convert.ToBoolean(int.Parse(rev.RevValue));
            //Get Channel data
            volumeControl.ChannelValues = new System.Collections.Generic.List<OMENChannelControlSturcture>();
            
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new OMENClientData() { ApiName = CmediaAPIFunctionPoint.VolumeControl.ToString(), SetValue = null, SetExtraValue = OMENChannel.Master });
            if (rev.RevCode != 0) return null;
            OMENChannelControlSturcture channel = new OMENChannelControlSturcture() { ChannelValue = double.Parse(rev.RevValue), ChannelIndex = OMENChannel.Master };
            volumeControl.ChannelValues.Add(channel);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new OMENClientData() { ApiName = CmediaAPIFunctionPoint.VolumeControl.ToString(), SetValue = null, SetExtraValue = OMENChannel.FrontLeft });
            if (rev.RevCode != 0) return null;
            channel = new OMENChannelControlSturcture() { ChannelValue = double.Parse(rev.RevValue), ChannelIndex = OMENChannel.FrontLeft };
            volumeControl.ChannelValues.Add(channel);
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new OMENClientData() { ApiName = CmediaAPIFunctionPoint.VolumeControl.ToString(), SetValue = null, SetExtraValue = OMENChannel.FrontRight });
            if (rev.RevCode != 0) return null;
            channel = new OMENChannelControlSturcture() { ChannelValue = double.Parse(rev.RevValue), ChannelIndex = OMENChannel.FrontRight };
            volumeControl.ChannelValues.Add(channel);

            return volumeControl;
        }

        internal static bool SetVolumeControl(OMENDataFlow renderCapture, OMENChannelControlSturcture volumeData)
        {
            bool rev = false;
            return rev;
        }

    }
}
