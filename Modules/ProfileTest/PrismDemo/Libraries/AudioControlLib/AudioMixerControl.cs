using AudioControlLib.AudioMixer;
using AudioControlLib.AudioMixer.Enums;

namespace AudioControlLib
{
    public class AudioMixerControl
    {
        public uint GetmixerDevsNums()
        {
            return MixerAPIs.mixerGetNumDevs();
        }

        public MMResult GetMixerDevsCap()
        {
            return MMResult.MMSYSERR_ERROR; //MixerAPIs.mixerGetDevCaps()
        }
    }
}
