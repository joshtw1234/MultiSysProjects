using AudioControlLib.Enums;

namespace AudioControlLib.CallBacks
{
    public delegate void AudioVolumeCallBack(AudioDataFlow audioFlow, float value);
    public delegate void AudioPluseCallBack(double highValue, double lowValue);
}
