using OMENCmediaSDK.CmediaSDK;
using System.Collections.Generic;

namespace OMENCmediaSDK.OMENSDK
{
    class OMENStructures
    {
    }

    public struct VolumeChannelSturcture
    {
        public VolumeChannel ChannelIndex { get; set; }
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
