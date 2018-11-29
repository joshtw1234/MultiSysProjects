using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MYAudioSDK.MYSDK.Structures
{
    class MYStructures
    {
    }

    public enum OMENVolumeChannel
    {
        Master = -1,
        FrontLeft,
        FrontRight
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void OMENSDKCallback(int type, int id, int componentType, ulong eventId);

    public struct OMENReturnValue
    {
        public int RevCode { get; set; }
        public string RevValue { get; set; }
        public string RevMessage { get; set; }
        public string RevExtraValue { get; set; }
    }


    public struct VolumeChannelSturcture
    {
        public OMENVolumeChannel ChannelIndex { get; set; }
        public float ChannelValue { get; set; }
    }

    public class BaseVolumeControlStructure
    {
        public int IsMuted { get; set; }
        public List<VolumeChannelSturcture> ChannelValues { get; set; }
    }

    public class VolumeControlStructure : BaseVolumeControlStructure
    {
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double StepValue { get; set; }
        public double ScalarValue { get; set; }
    }

    enum OMENDataFlow
    {
        Render,
        Capture
    }
}
