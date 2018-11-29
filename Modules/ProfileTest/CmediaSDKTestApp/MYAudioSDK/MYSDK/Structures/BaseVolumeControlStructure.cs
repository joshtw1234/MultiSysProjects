using System.Collections.Generic;

namespace MYAudioSDK.MYSDK.Structures
{
    public class BaseVolumeControlStructure
    {
        public int IsMuted { get; set; }
        public List<VolumeChannelSturcture> ChannelValues { get; set; }
    }
}
