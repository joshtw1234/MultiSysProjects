using AudioControlLib.Enums;

namespace AudioControlLib.CallBacks
{
    /// <summary>
    /// Audio Volume Call back
    /// </summary>
    /// <param name="audioFlow"></param>
    /// <param name="value"></param>
    public delegate void AudioVolumePeekCallBack(AudioDataFlow audioFlow, float value);
    /// <summary>
    /// Audio Pulse Call Back
    /// </summary>
    /// <param name="highValue"></param>
    /// <param name="lowValue"></param>
    public delegate void AudioPulseCallBack(double highValue, double lowValue);
    /// <summary>
    /// Audio Volume Change call Back
    /// </summary>
    /// <param name="highValue"></param>
    /// <param name="lowValue"></param>
    public delegate void AudioVolumeChangeCallBack(Structures.AudioVolumeNotificationData cData);
    /// <summary>
    /// The Audio Device state change call back
    /// </summary>
    /// <param name="audioDeviceData"></param>
    public delegate void AudioDeviceStateChangeCallBack(Structures.AudioDeviceChangeData audioDeviceData);
}
