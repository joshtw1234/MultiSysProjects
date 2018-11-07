using CmediaSDKTestApp.BaseModels;
using System;
using System.Threading.Tasks;

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
            rev = CmediaSDKService.Instance.GetSetJackDeviceData(cmediaDataFlow, CmediaDriverReadWrite.Read, new OMENClientData() { ApiName = CmediaAPIFunctionPoint.VolumeControl.ToString() });
            if (rev.RevCode != 0) return null;
            volumeControl.ChannelValue = new OMENChannelControlSturcture() { ChannelValue = double.Parse(rev.RevValue), ChannelIndex = string.IsNullOrEmpty(rev.RevExtraValue) ? 0.0 : double.Parse(rev.RevExtraValue) };

            return volumeControl;
        }

    }
}
