using MYAudioSDK.MYSDK.Enums;

namespace MYAudioSDK.MYSDK.Vendors
{
    interface IBaseVendor
    {
        MyErrorCode Initialize();
        MyErrorCode UnInitialize();
        MyErrorCode GetVolumeControl(OMENDataFlow renderCapture);
        MyErrorCode SetVolumeControl(OMENDataFlow renderCapture);
    }
}
